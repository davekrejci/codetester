<template>
  <div>
    <textarea v-model="content" :id="'editor_' + id"></textarea>
    <result-input-widget
      ref="widgets"
      v-for="widget in widgets"
      :key="widget.id"
      :length="widget.length"
      :id="widget.id"
      :answer="widget.answer"
      :correctAnswer="widget.correctAnswer"
      :isCorrect="widget.isCorrect"
    ></result-input-widget>
  </div>
</template>

<script>
import * as CodeMirror from "codemirror";
import ResultInputWidget from "./ResultInputWidget.vue";
import "codemirror/lib/codemirror.css";
import "codemirror/theme/duotone-light.css";
import "codemirror/theme/nord.css";
import "codemirror/mode/clike/clike.js";
import "codemirror/addon/search/searchcursor";
import "codemirror/addon/display/autorefresh";

export default {
  name: "FillBlankResultCodeEditor",
  components: {
    ResultInputWidget,
  },
  data() {
    return {
      widgets: [],
    };
  },
  props: {
    content: {
      default: "No content",
      type: String,
    },
    blocks: {
     type: Array
    },
    id: {
      default: 0,
      type: Number,
    },
  },
  mounted() {
    (this.cm = CodeMirror.fromTextArea(
      document.getElementById("editor_" + this.id),
      {
        theme: this.$vuetify.theme.dark
          ? "nord"
          : "duotone-light",
        mode: "text/x-java",
        readOnly: "nocursor",
        defaultTextHeight: 32,
      }
    )),
      (this.widgetREGEX = /\{"widget_id":"(\w+)", "length":(\w+)\}/);
    this.replaceShortcode(this.widgetREGEX, this.cm, { line: 0, ch: 0 });
  },
  unmounted() {
    this.cm.toTextArea();
  },
  methods: {
    // Method to replace the {widget..} shortcode received in the code with a result widget
    replaceShortcode(widgetREGEX, editor, loc) {
      const cursor = editor.getSearchCursor(widgetREGEX, loc, {
        multiline: "disable",
      });

          let index = 0;
      while (cursor.findNext()) {
        const markers = editor.findMarks(cursor.from(), cursor.to());
        if (markers.length === 0) {
          const from = cursor.from();
          const to = cursor.to();
          const match = editor.getRange(from, to);

          const parsedObj = JSON.parse(match);

        let matchingBlock = this.blocks.find(block => block.widgetId == parsedObj.widget_id);
          let widget = {
            id: parsedObj.widget_id,
            length: parsedObj.length,
            text: "",
            from: from,
            to: to,
            index: index,
            isFilled: false,
            answer: matchingBlock.answer,
            correctAnswer: matchingBlock.content,
            isCorrect: matchingBlock.isCorrect
          };
          this.widgets.push(widget);
          index++;
        }
      }
      // Wait for DOM to be update with new widget components before placing
      this.$nextTick(function () {
        this.widgets.forEach((widget) => {
          editor.markText(widget.from, widget.to, {
            replacedWith: this.$refs.widgets[widget.index].$el,
          });
          editor.refresh();
        });
      });
    },
  },
};
</script>

<style>
.CodeMirror {
  border-radius: 5px;
  line-height: 32px;
  padding: 10px;
  max-width: 800px;
  height: auto;
  border: 2px solid rgba(0,0,0,0.12);
}
</style>