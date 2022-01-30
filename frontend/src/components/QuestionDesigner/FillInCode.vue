<template>
  <div>
    <v-textarea
      outlined
      auto-grow
      :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
      rows="1"
      clearable
      clear-icon="mdi-close-circle"
      label="Popište kód"
      v-model="codeDescription"
      :rules="[rules.required]"
    ></v-textarea>

    <v-toolbar dense outlined elevation="0" class="rounded rounded-b-0">
      <v-tooltip top>
        <template v-slot:activator="{ on, attrs }">
          <v-btn
            icon
            v-bind="attrs"
            v-on="on"
            @click="autoFormatSelectionAll()"
          >
            <v-icon>mdi-format-align-left</v-icon>
          </v-btn>
        </template>
        <span>Formátovat</span>
      </v-tooltip>
      <v-tooltip top>
        <template v-slot:activator="{ on, attrs }">
          <v-btn icon v-bind="attrs" v-on="on" @click="setRangeFillable()">
            <v-icon>mdi-code-tags</v-icon>
          </v-btn>
        </template>
        <span>Vybrat označené (alt-s)</span>
      </v-tooltip>
      <v-tooltip top>
        <template v-slot:activator="{ on, attrs }">
          <v-btn icon v-bind="attrs" v-on="on" @click="setRandomFillable()">
            <v-icon>mdi-restore</v-icon>
          </v-btn>
        </template>
        <span>Vybrat náhodně</span>
      </v-tooltip>
      <v-tooltip top>
        <template v-slot:activator="{ on, attrs }">
          <v-text-field
            v-bind="attrs" v-on="on"
            v-model="numberToRandomize"
            hide-details
            single-line
            type="number"
            solo
            flat
            dense
            class="numberToRandomize"
            min="1"
            max="99"
            onkeydown="return false"
          />
        </template>
        <span>Počet náhodných výběrů</span>
      </v-tooltip>
      <v-spacer></v-spacer>
      <v-spacer></v-spacer>
      <v-select
        v-model="selectedLanguage"
        :items="languageOptions"
        item-text="name"
        solo
        flat
        dense
        disabled
        class="mt-6 languageSelect"
      ></v-select>
    </v-toolbar>
    <v-card
     flat
     outlined
     class="rounded-t-0"
    >
      <textarea v-model="content" id="editor"></textarea>
      <v-overlay :value="overlay" absolute>
        <v-progress-circular
          indeterminate
          size="64"
        ></v-progress-circular>
      </v-overlay>
    </v-card>
    <v-card flat class="mt-2 pa-8">
      <v-slider
        hint="Nastavte počet bloků, které student bude muset doplnit(náhodný výběr)"
        v-model="fillInCount"
        :max="this.widgets.length == 0 ? 1 : this.widgets.length"
        min="1"
        thumb-label
        ticks="always"
        tick-size="4"
        persistent-hint
      >
        <template v-slot:append>
          <v-chip class="primary">
            {{ fillInCount }}
          </v-chip>
        </template>
      </v-slider>
    </v-card>

    <v-btn @click="getTransformedValue">Get value</v-btn>
    <v-btn @click="getTokenizedContent">Get tokens value</v-btn>

    <v-menu
      v-model="showMenu"
      :position-x="x"
      :position-y="y"
      absolute
      offset-y
      content-class="elevation-1"
      transition="scale-transition"
    >
      <v-list dense class="pa-0">
        <v-list-item class="contextMenuItem" @click="setRangeFillable()">
          <v-list-item-icon class="mr-1">
            <v-icon small>mdi-code-tags</v-icon>
          </v-list-item-icon>
          <v-list-item-title>Vybrat označené</v-list-item-title>
        </v-list-item>
        <v-list-item class="contextMenuItem" @click="autoFormatSelectionAll()">
          <v-list-item-icon class="mr-1">
            <v-icon small>mdi-format-align-left</v-icon>
          </v-list-item-icon>
          <v-list-item-title>Formátovat</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
  </div>
</template>

<script>
import Vue from "vue";
import vuetify from "@/plugins/vuetify";
import FillableWidget from "@/components/QuestionDesigner/FillableWidget.vue";
import JavaLexer from "java-parser/src/lexer";
import { shuffleArray } from "@/util/util.js";
import * as CodeMirror from "codemirror";
import "codemirror-formatting";
import "codemirror/lib/codemirror.css";
import "codemirror/theme/dracula.css";
import "codemirror/theme/duotone-light.css";
import "codemirror/theme/material-palenight.css";
import "codemirror/theme/eclipse.css";
import "codemirror/mode/clike/clike.js";
import "codemirror/addon/search/searchcursor";
import "codemirror/addon/display/placeholder.js";
import "codemirror/addon/edit/closebrackets.js";
import "codemirror/addon/edit/matchbrackets.js";
import "codemirror/addon/hint/show-hint.js";
import "codemirror/addon/hint/show-hint.css";
import "codemirror/addon/hint/anyword-hint.js";

const WidgetComponentClass = Vue.extend(FillableWidget);

export default {
  name: "FillInCode",
  data() {
    return {
      codeDescription: "",
      content: `public class HelloWorld{ \n\tpublic static void main(String[] args){\n\t\tSystem.out.println("Hello, World!"); \n\t}\n}`,
      //content: '',
      rules: {
        required: (value) => !!value || "Povinné.",
      },
      showMenu: false,
      x: 0,
      y: 0,
      markerInstances: [],
      widgets: [],
      widgetIdCounter: 0,
      fillInCount: 1,
      languageOptions: [
        {
          name: "Java",
          lexer: JavaLexer,
          mode: "text/x-java",
        },
      ],
      selectedLanguage: {
        name: "Java",
        lexer: JavaLexer,
        mode: "text/x-java",
      },
      numberToRandomize: 1,
      overlay:false,
    };
  },
  mounted() {
    CodeMirror.commands.autocomplete = function (cm) {
      cm.showHint({ hint: CodeMirror.hint.anyword });
    };
    this.cm = CodeMirror.fromTextArea(document.getElementById("editor"), {
      lineNumbers: true,
      theme: this.$vuetify.theme.dark ? "material-palenight" : "duotone-light",
      mode: "text/x-java",
      extraKeys: {
        "Ctrl-Space": "autocomplete",
        "Alt-S": this.setRangeFillable,
      },
      autoCloseTags: true,
      lineWrapping: true,
      defaultTextHeight: 32,
      placeholder: "// Zadejte kod",
    });

    this.cm.on(
      "contextmenu",
      function (instance, event) {
        this.showMenu = false;
        this.x = event.clientX;
        this.y = event.clientY;
        this.$nextTick(() => {
          this.showMenu = true;
        });
        event.preventDefault();
      }.bind(this)
    );
  },
  methods: {
    /**
     * Switches the range in the editor with a FillableWidget component
     * @param {Object} range - the range to set fillable {from: Pos, to: Pos}, defaults to currently selected range
     * */
    setRangeFillable(event, range = this.getSelectedRange()) {
      //get range info
      const doc = this.cm.getDoc();
      const content = doc.getRange(range.from, range.to);
      const rangeLength = content.length;

      // Exit if nothing was selected
      if (rangeLength == 0) {
        return;
      }

      //add widget to data model
      const widget = {
        id: this.widgetIdCounter,
        content: content,
        length: rangeLength,
      };
      this.widgets.push(widget);
      this.widgetIdCounter++;

      //create widget component
      const widgetComponent = new WidgetComponentClass({
        propsData: {
          id: this.widgetIdCounter,
          length: rangeLength,
          content: content,
        },
        vuetify,
      });
      widgetComponent.$mount();
      const textMarker = this.cm.markText(range.from, range.to, {
        replacedWith: widgetComponent.$el,
      });

      // add markerInstance to widget so it can reference it to clear it when removing widget
      // WARNING: adding markerInstance to widgets causes a ' Converting circular structure to JSON' error when trying to stringify
      let lastWidget = this.widgets[this.widgets.length - 1];
      lastWidget.markerInstance = textMarker;

      //add listener for when widget emits remove event
      widgetComponent.$on("removeWidget", (id) => {
        this.widgets = this.widgets.filter((widget) => widget.id !== id);
        textMarker.clear();
      });

      this.cm.refresh;
      this.cm.setCursor(range.to);
    },
    /**
     * Retrieves the currently selected content in the editor
     * @returns {Object} {from: Pos, to: Pos}
     * */
    getSelectedRange() {
      return { from: this.cm.getCursor(true), to: this.cm.getCursor(false) };
    },
    /**
     * Function to autoformat the editor content
     * */
    autoFormatSelectionAll() {
      var cursorPos = this.cm.getCursor();
      this.cm.execCommand("selectAll");
      var range = this.getSelectedRange();
      this.cm.autoFormatRange(range.from, range.to);
      this.cm.setCursor(cursorPos);

      // since autoformat removes the widgets, remove them from the data model as well
      this.removeAllWidgets();
    },
    /**
     * Removes all widgets from the code editor and data model
     */
    removeAllWidgets() {
      let doc = this.cm.getDoc();
      let widgets = doc.getAllMarks();
      widgets.forEach((widget) => {
        widget.clear();
      });
      this.widgets = [];
      this.widgetIdCounter = 0;
      this.fillInCount = 1;
    },
    /**
     * Test function to get editor content with '{{widget}}' value instead of the widget contents
     * */
    getTransformedValue() {
      let content = this.cm.getValue("\n");
      console.log(content);

      let transformedContent = content;

      let doc = this.cm.getDoc();
      let widgets = doc.getAllMarks();

      let charDiff = 0;
      let sortedWidgets = widgets.sort((a, b) =>
        doc.indexFromPos(a.find().from) > doc.indexFromPos(b.find().from)
          ? 1
          : doc.indexFromPos(b.find().from) > doc.indexFromPos(a.find().from)
          ? -1
          : 0
      );
      sortedWidgets.forEach((widget) => {
        let range = widget.find();
        let fromIndex = doc.indexFromPos(range.from);
        let toIndex = doc.indexFromPos(range.to);
        transformedContent = replaceStringRange(
          transformedContent,
          fromIndex + charDiff,
          toIndex + charDiff,
          "{{widget}}"
        );
        charDiff += "{{widget}}".length - (toIndex - fromIndex);
      });
      console.log(transformedContent);

      function replaceStringRange(string, from, to, replacement) {
        return (
          string.substr(0, from) +
          replacement +
          string.substr(to, string.length)
        );
      }
    },
    /**
     * Tokenizes the code editor input
     * @returns {Object} {errors: [], groups: {}, tokens: []}
     */
    getTokenizedContent() {
      let content = this.cm.getValue("\n");
      const lexResult = JavaLexer.tokenize(content);
      return lexResult;
    },
    switchOverlay(){
      this.overlay = !this.overlay;
    },
    /**
     * Sets random tokens from the code editor content to be fillable
     */
    setRandomFillable() {
      // since method can take some time, add loading overlay
      this.switchOverlay();
      // need quick timeout for overlay to render
      setTimeout(() => {
        // reset current widgets
        this.removeAllWidgets();
        // get tokenized content
        let tokenizedContent = this.getTokenizedContent();
        if (tokenizedContent.errors.length != 0) {
          tokenizedContent.errors.forEach((error) => console.log(error));
        }
        let tokens = tokenizedContent.tokens;
        
        // TODO: allow filtering by token type
        // implement

        // pick random tokens
        let shuffledTokens = shuffleArray(tokens);
        let n = this.numberToRandomize;
        if (n > shuffledTokens.length) {
          n = shuffledTokens.length;
        }
        let selectedTokens = shuffledTokens.slice(0, n);
        selectedTokens.forEach((token) => {
          // map token positions to codemirror positions
          let range = {
            from: {
              line: token.startLine - 1,
              ch: token.startColumn - 1,
            },
            to: {
              line: token.endLine - 1,
              ch: token.endColumn,
            },
          };
          // set the token range to be a FillableWidget
          this.setRangeFillable(event, range);
        });
        this.switchOverlay();
      }, 100);
    },
  },
};
</script>

<style>
.CodeMirror {
  line-height: 32px;
  min-height: 200px;
  padding: 10px;
  height: auto;
}
.contextMenuItem:hover {
  cursor: pointer;
  background-color: var(--v-primary-lighten1);
}
.languageSelect {
  max-width: 100px;
}
.numberToRandomize {
  max-width: 60px;
}
</style>