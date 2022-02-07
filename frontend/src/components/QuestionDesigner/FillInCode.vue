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
        <span>Formátovat (alt-f)</span>
      </v-tooltip>
      <v-tooltip top>
        <template v-slot:activator="{ on, attrs }">
          <v-btn icon v-bind="attrs" v-on="on" @click="setRangeFillable()">
            <v-icon>mdi-pencil-box-multiple-outline</v-icon>
          </v-btn>
        </template>
        <span>Vybrat označené (alt-s)</span>
      </v-tooltip>
      <v-tooltip top>
        <template v-slot:activator="{ on, attrs }">
          <v-btn icon v-bind="attrs" v-on="on" @click="setAllFillable()">
            <v-icon>mdi-plus-box-multiple-outline</v-icon>
          </v-btn>
        </template>
        <span>Vybrat vše (alt-a)</span>
      </v-tooltip>
      <v-tooltip top>
        <template v-slot:activator="{ on, attrs }">
          <v-btn icon v-bind="attrs" v-on="on" @click="setRandomFillable()">
            <v-icon>mdi-star-box-multiple-outline</v-icon>
          </v-btn>
        </template>
        <span>Vybrat náhodně (alt-r)</span>
      </v-tooltip>
      <v-menu :close-on-content-click="false" :nudge-width="200" offset-x>
        <template v-slot:activator="{ on: onMenu }">
          <v-tooltip top>
            <template v-slot:activator="{ on: onTooltip }">
              <v-btn icon v-on="{ ...onMenu, ...onTooltip }">
                <v-icon>mdi-cog</v-icon>
              </v-btn>
            </template>
            <span>Nastavení</span>
          </v-tooltip>
        </template>

        <v-card>
          <v-list>
            <v-list-item>
              <v-list-item-action>
                <v-switch
                  v-model="allowedTokenTypes.separators.allowed"
                  color="primary"
                ></v-switch>
              </v-list-item-action>
              <v-list-item-title>Separátory</v-list-item-title>
              <v-list-item-subtitle>{ } ( ) [ ] ; , ...</v-list-item-subtitle>
            </v-list-item>

            <v-list-item>
              <v-list-item-action>
                <v-switch
                  v-model="allowedTokenTypes.operators.allowed"
                  color="primary"
                ></v-switch>
              </v-list-item-action>
              <v-list-item-title>Operátory</v-list-item-title>
              <v-list-item-subtitle>+ - = > ...</v-list-item-subtitle>
            </v-list-item>
            <v-list-item>
              <v-list-item-action>
                <v-switch
                  v-model="allowedTokenTypes.literals.allowed"
                  color="primary"
                ></v-switch>
              </v-list-item-action>
              <v-list-item-title>Literály</v-list-item-title>
              <v-list-item-subtitle>"Joe", 9.53 ...</v-list-item-subtitle>
            </v-list-item>
            <v-list-item>
              <v-list-item-action>
                <v-switch
                  v-model="allowedTokenTypes.identifiers.allowed"
                  color="primary"
                ></v-switch>
              </v-list-item-action>
              <v-list-item-title>Identifikátory</v-list-item-title>
              <v-list-item-subtitle>a, b, jmeno ...</v-list-item-subtitle>
            </v-list-item>
            <v-list-item>
              <v-list-item-action>
                <v-switch
                  v-model="allowedTokenTypes.keywords.allowed"
                  color="primary"
                ></v-switch>
              </v-list-item-action>
              <v-list-item-title>Klíčové slova</v-list-item-title>
              <v-list-item-subtitle>abstract, if, for ...</v-list-item-subtitle>
            </v-list-item>
          </v-list>
        </v-card>
      </v-menu>
      <v-tooltip top>
        <template v-slot:activator="{ on, attrs }">
          <v-text-field
            v-bind="attrs"
            v-on="on"
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
    <v-card flat outlined class="rounded-t-0">
      <textarea v-model="content" id="editor"></textarea>
      <v-overlay :value="loading" absolute>
        <v-progress-circular
          indeterminate
          size="64"
          width="7"
        ></v-progress-circular>
      </v-overlay>
    </v-card>
    <v-card v-show="widgets.length > 1" flat outlined class="mt-2 px-12 py-4">
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
import JavaLexer from "./Lexers/javalexer.js";
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
import { createHelpers } from "vuex-map-fields";
import { mapMutations } from "vuex";

const WidgetComponentClass = Vue.extend(FillableWidget);

const { mapFields, mapMultiRowFields } = createHelpers({
  getterType: "questionDesigner/getField",
  mutationType: "questionDesigner/updateField",
});

export default {
  name: "FillInCode",
  data() {
    return {
      rules: {
        required: (value) => !!value || "Povinné.",
      },
      showMenu: false,
      x: 0,
      y: 0,
      widgetIdCounter: 0,
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
      loading: false,
      allowedTokenTypes: {
        separators: {
          allowed: false,
          names: ["Separators"],
        },
        operators: {
          allowed: true,
          names: ["Operator"],
        },
        literals: {
          allowed: true,
          names: ["Literal"],
        },
        identifiers: {
          allowed: true,
          names: ["Identifier"],
        },
        keywords: {
          allowed: true,
          names: ["Keyword"],
        },
      },
    };
  },
  computed: {
    ...mapFields([
      "fillInCode.codeDescription", 
      "fillInCode.content", 
      "fillInCode.cm",
      "fillInCode.fillInCount"
      ]),
    ...mapMultiRowFields(["fillInCode.widgets"])
  },
  mounted() {
    // reset store widget data that might be left over after leaving and coming back 
    this.removeAllFillInCodeWidgets();

    // create codemirror instance
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
        "Alt-R": this.setRandomFillable,
        "Alt-A": this.setAllFillable,
        "Alt-F": this.autoFormatSelectionAll,
      },
      autoCloseTags: true,
      lineWrapping: true,
      defaultTextHeight: 32,
      placeholder: "// Zadejte kod",
    });

    // setup codemirror event listeners
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
    this.cm.on("change", (cm) => {
      this.content = cm.getValue();
    });
  },
  methods: {
    ...mapMutations('questionDesigner',["addFillInCodeWidget", "removeFillInCodeWidget", "removeAllFillInCodeWidgets"]),
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

      //add widget to data model
      const widget = {
        id: this.widgetIdCounter,
        content: content,
        length: rangeLength,
        markerInstance: textMarker
      };
      this.addFillInCodeWidget(widget);

      //add listener for when widget emits remove event
      widgetComponent.$on("removeWidget", (id) => {
        console.log("removing widget with id: " + id);
        this.removeFillInCodeWidget(id);
        textMarker.clear();
      });

      this.cm.refresh;
      this.cm.setCursor(range.to);
      this.widgetIdCounter++;
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
      this.removeAllFillInCodeWidgets();
      this.widgetIdCounter = 0;
      this.fillInCount = 1;
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
    /**
     * Filters a Token array to not include unwanted token types
     * @param {Array} tokens
     * @returns {Array} filtered tokens
     */
    filterTokenTypes(tokens) {
      console.log(tokens);
      let toFilter = [];
      for (const [key, value] of Object.entries(this.allowedTokenTypes)) {
        console.log(key);
        if (value.allowed == false) {
          toFilter.push(...value.names);
        }
      }
      console.log(toFilter);
      let filteredTokens = [];
      tokens.forEach((token) => {
        let hasFilteredCategory = false;
        // check specific token type name
        if (toFilter.includes(token.tokenType.name)) {
          hasFilteredCategory = true;
        }
        // check any general token category names
        token.tokenType.CATEGORIES.forEach((category) => {
          if (toFilter.includes(category.name)) {
            hasFilteredCategory = true;
          }
        });
        if (!hasFilteredCategory) {
          filteredTokens.push(token);
        }
      });
      console.log(filteredTokens);
      return filteredTokens;
    },
    switchLoading() {
      this.loading = !this.loading;
    },
    /**
     * Sets all tokens from the code editor content to be fillable
     */
    setAllFillable() {
      // since method can take some time, add loading overlay
      this.switchLoading();
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

        //filter unwanted token types
        tokens = this.filterTokenTypes(tokens);

        tokens.forEach((token) => {
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
        this.switchLoading();
      }, 100);
    },
    /**
     * Sets random tokens from the code editor content to be fillable
     */
    setRandomFillable() {
      // since method can take some time, add loading overlay
      this.switchLoading();
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

        //filter unwanted token types
        tokens = this.filterTokenTypes(tokens);

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
        this.switchLoading();
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