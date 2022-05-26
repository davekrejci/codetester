<template>
  <v-dialog v-model="showPreview" max-width="800" @click:outside="closePreview">
    <template v-slot:activator="{ on, attrs }">
      <v-btn :disabled="loading" color="primary" v-bind="attrs" v-on="on" depressed outlined class="mr-4 mb-2">
        Náhled
        <v-icon right dark> mdi-magnify </v-icon>
      </v-btn>
    </template>
    <v-card outline class="py-4 px-8">
      <v-card-actions>
      <div class="primary--text text-overline mb-4">Otázka X</div>
        <v-spacer></v-spacer>
        <v-btn
              icon
              color="primary"
              @click="setPreviewEditorValue"
              v-if="selectedQuestionType == 'fill-in-code'"
            >
              <v-icon>mdi-restore</v-icon>
            </v-btn>
            <v-btn
              icon
              color="primary"
              @click="showPreview = false"
            >
              <v-icon>mdi-close</v-icon>
            </v-btn>
      </v-card-actions>
      <div v-if="selectedQuestionType == 'multi-choice'">
        <div class="mb-4" v-html="questionText"></div>
        <v-card
          class="question-card"
          v-if="selectedQuestionType == 'multi-choice'"
          color="transparent"
          outlined
        >
          <v-list dense class="pa-0" color="transparent">
            <v-list-item-group multiple>
              <template v-for="(answer, i) in answers">
                <v-list-item
                  :key="answer[i]"
                  active-class="primary--text text--accent-4"
                >
                  <template v-slot:default="{ active }">
                    <v-list-item-action>
                      <v-checkbox
                        :input-value="active"
                        color="primary"
                      ></v-checkbox>
                    </v-list-item-action>
                    <v-list-item-content>
                      <v-list-item-title v-text="answer.answerText"></v-list-item-title>
                    </v-list-item-content>
                  </template>
                </v-list-item>
                <v-divider
                  v-if="i < answers.length - 1"
                  :key="`answer-divider-${i}`"
                ></v-divider>
              </template>
            </v-list-item-group>
          </v-list>
        </v-card>
      </div>
      <div v-if="selectedQuestionType == 'fill-in-code'">
        <div class="mb-4" v-html="codeDescription"></div>
        <textarea id="preview-editor"></textarea>
      </div>


    </v-card>
  </v-dialog>
</template>

<script>
import * as CodeMirror from "codemirror";
import Vue from "vue";
import vuetify from "@/plugins/vuetify";
import InputWidget from "@/components/InputWidget.vue";
import { shuffleArray } from "@/util/util.js";
import "codemirror/lib/codemirror.css";
import "codemirror/theme/dracula.css";
import "codemirror/theme/duotone-light.css";
import "codemirror/theme/nord.css";
import "codemirror/theme/eclipse.css";
import "codemirror/mode/clike/clike.js";
import "codemirror/addon/search/searchcursor";
import "codemirror/addon/display/autorefresh";
import { createHelpers, mapMultiRowFields } from "vuex-map-fields";

const WidgetComponentClass = Vue.extend(InputWidget);
const { mapFields } = createHelpers({
  getterType: "questionDesigner/getField",
  mutationType: "questionDesigner/updateField",
});

export default {
  name: "QuestionPreview",
  props: {
    loading: Boolean
  },
  data() {
    return {
      // data
      showPreview: false,
    };
  },
  computed: {
    ...mapFields([
      "selectedQuestionType",
      "multiChoice.questionText",
      "fillInCode.codeDescription",
      "fillInCode.content",
      "fillInCode.fillInCount",
    ]),
    ...mapFields({
      cmDesignerEditor: "fillInCode.cm",
    }),
    ...mapMultiRowFields("questionDesigner", ["multiChoice.answers"]),
  },
  methods: {
    // methods
    closePreview() {
      this.showPreview = false;
    },
    replaceShortcode(widgetREGEX, editor, loc) {
      const cursor = editor.getSearchCursor(widgetREGEX, loc, {
        multiline: "disable",
      });

      while (cursor.findNext()) {
        const markers = editor.findMarks(cursor.from(), cursor.to());
        if (markers.length === 0) {
          const from = cursor.from();
          const to = cursor.to();
          const match = editor.getRange(from, to);

          const parsedObj = JSON.parse(match);

          const widgetInstance = new WidgetComponentClass({
            propsData: {
              id: parsedObj.id,
              length: parsedObj.length,
            },
            vuetify,
          });
          widgetInstance.$mount();
          editor.markText(from, to, { replacedWith: widgetInstance.$el });
          editor.refresh();
        }
      }
    },
    /**
     * Test function to get editor content with '{widget...}' value instead of the widget contents
     * @param {Int} n - number of widgets to replace, leave out to replace all
     * @return {String} transformed content string
     * */
    getTransformedValue(n = -1) {
      let content = this.cmDesignerEditor.getValue("\n");
      let transformedContent = content;

      let doc = this.cmDesignerEditor.getDoc();
      let widgets = doc.getAllMarks();

      let charDiff = 0;

      // pick n random widgets based on selected fillInCount number
      if (n >= 0) {
        let shuffledWidgets = shuffleArray(widgets);
        widgets = shuffledWidgets.slice(0, n);
      }

      // sort randomy selected widgets by position in content so that the strings can be properly replaced
      let sortedWidgets = widgets.sort((a, b) =>
        doc.indexFromPos(a.find().from) > doc.indexFromPos(b.find().from)
          ? 1
          : doc.indexFromPos(b.find().from) > doc.indexFromPos(a.find().from)
          ? -1
          : 0
      );
      // replace each widget with shortcode
      sortedWidgets.forEach((widget) => {
        let range = widget.find();
        let fromIndex = doc.indexFromPos(range.from);
        let toIndex = doc.indexFromPos(range.to);
        let length = toIndex - fromIndex;
        let widgetString = `{"widget_id":"${widget.id}", "length":${length}}`;
        transformedContent = replaceStringRange(
          transformedContent,
          fromIndex + charDiff,
          toIndex + charDiff,
          widgetString
        );
        charDiff += widgetString.length - (toIndex - fromIndex);
      });
      return transformedContent;

      function replaceStringRange(string, from, to, replacement) {
        return (
          string.substr(0, from) +
          replacement +
          string.substr(to, string.length)
        );
      }
    },
    setPreviewEditorValue() {
      // set the preview editor content to transformed value
      let n = this.fillInCount;
      let transformedValue = this.getTransformedValue(n);
      this.cmPreviewEditor.setValue(transformedValue);
      // replace shortcodes from transformed value with fillin widgets
      var widgetREGEX = /\{"widget_id":"(\w+)", "length":(\w+)\}/;
      this.replaceShortcode(widgetREGEX, this.cmPreviewEditor, {
        line: 0,
        ch: 0,
      });
      this.cmPreviewEditor.refresh();
    },
  },
  watch: {
    showPreview(isVisible) {
      if (this.selectedQuestionType != 'fill-in-code') { return; }
      if (isVisible) {
        // need to wait for vue to change DOM so codemirror can find element to bind to
        this.$nextTick(() => {
          this.cmPreviewEditor = CodeMirror.fromTextArea(
            document.getElementById("preview-editor"),
            {
              autoRefresh: true,
              theme: this.$vuetify.theme.dark
                ? "nord"
                : "duotone-light",
              mode: "text/x-java",
              readOnly: "nocursor",
              defaultTextHeight: 32,
            }
          );
          this.setPreviewEditorValue();
        });
      } else {
        this.cmPreviewEditor.toTextArea();
      }
    },
  },
};
</script>

<style>
.v-application .transparent.question-card {
  border-color: rgba(0, 0, 0, 0.12) !important;
}
</style>