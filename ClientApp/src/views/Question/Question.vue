<template>
  <v-container fluid class="">
    <div v-if="this.question != null">
      <v-breadcrumbs :items="breadcrumbs" class="pa-0 pb-4 pl-1"></v-breadcrumbs>
      <h1 class="ml-1 mb-6">Otázka #{{question.id}}</h1>
      <v-form ref="form">
        <v-text-field
          :value="this.formattedQuestionType"
          label="Typ Otázky"
          outlined
          color="grey"
          readonly
        ></v-text-field>

        <!-- MultiChoice -->
        <div v-if="this.question.questionType === 'multi-choice'" class="mb-8">
          <rich-text-editor :value="this.question.questionText" :editable="false" class="mb-8"></rich-text-editor>

          <div
            v-for="(answer, index) in this.question.answers"
            :key="index"
            class="mb-2"
          >
            <v-text-field
              readonly
              outlined
              :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
              color="grey"
              :success="answer.isCorrect"
              class="answerField"
              :label="'Odpověď ' + (index + 1)"
              dense
              prepend-icon="mdi-drag"
              hide-details
              v-model="answer.answerText"
              :rules="[rules.required]"
            >
              <!-- <template v-slot:append>
                <v-tooltip bottom>
                  <template v-slot:activator="{ on, attrs }">
                    <v-btn
                      outlined
                      small
                      v-bind="attrs"
                      v-on="on"
                      @click="removeMultiChoiceAnswer(index)"
                      height="40px"
                      width="40px"
                      tile
                      class="answerButton leftButton"
                    >
                      <v-icon>mdi-close</v-icon>
                    </v-btn>
                  </template>
                  <span>Odstranit</span>
                </v-tooltip>
                <v-tooltip bottom>
                  <template v-slot:activator="{ on, attrs }">
                    <v-btn
                      outlined
                      small
                      :color="answer.isCorrect ? 'green' : ''"
                      v-bind="attrs"
                      v-on="on"
                      @click="toggleMultiChoiceAnswerCorrect(index)"
                      height="40px"
                      width="40px"
                      class="answerButton rounded-l-0"
                    >
                      <v-icon>mdi-check</v-icon>
                    </v-btn>
                  </template>
                  <span>Označit správně</span>
                </v-tooltip>
              </template> -->
            </v-text-field>
          </div>
          <!-- <v-row class="justify-end mt-6">
            <v-btn
              outlined
              color="primary"
              class="mr-8"
              @click="addMultiChoiceAnswer()"
              :disabled="this.question.answers.length >= 5"
            >
              <span>Přidat odpověď </span>
              <v-icon small>mdi-plus</v-icon>
            </v-btn>
          </v-row> -->
        </div>

        <!-- FillInCode -->
        <div v-if="this.question.questionType === 'fill-in-code'" class="mb-8">
          <rich-text-editor :value="this.question.codeDescription" :editable="false" class="mb-8"></rich-text-editor>

          <v-card flat outlined class="rounded">
            <textarea v-model="this.question.code" id="editor"></textarea>
            <v-overlay :value="false" absolute>
              <v-progress-circular
                indeterminate
                size="64"
                width="7"
              ></v-progress-circular>
            </v-overlay>
          </v-card>
          <v-card flat outlined class="mt-2 px-12 py-4">
            <v-slider
              hint="Počet bloků, které student bude muset doplnit(náhodný výběr)"
              v-model="question.fillCount"
              thumb-label
              :max="question.fillInCodeBlocks.length"
              ticks
              tick-size="4"
              persistent-hint
              readonly
            >
              <template v-slot:append>
                <v-chip class="primary">
                  {{ question.fillCount }}
                </v-chip>
              </template>
            </v-slider>
          </v-card>
        </div>

        <!-- Tag selector -->
        <v-combobox
          v-model="selectedTags"
          :items="tags"
          item-text="tagText"
          item-value="tagText"
          :search-input.sync="search"
          hide-selected
          label="Štítky"
          multiple
          persistent-hint
          small-chips
          outlined
          deletable-chips
          clearable
        >
          <template v-slot:no-data>
            <v-list-item>
              <v-list-item-content>
                <v-list-item-title>
                  Žádné výsledky pro "<strong>{{ search }}</strong
                  >". Stiskněte <kbd>enter</kbd> pro vytvoření nového štítku
                </v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </template>
        </v-combobox>

        <!-- Action buttons -->
        <!-- Delete Dialog -->
        <v-btn
          @click="showDeleteDialog = true"
          :loading="loading"
          :disabled="loading"
          color="error"
          outlined
          class="mr-4 mb-2"
        >
          Smazat <v-icon right dark> mdi-trash-can-outline </v-icon>
        </v-btn>
        <v-btn
          color="primary"
          depressed
          width="150px"
          :loading="loading"
          :disabled="loading"
          class="mr-4 mb-2"
          @click="editQuestion()"
        >
          Uložit <v-icon right dark> mdi-content-save </v-icon>
        </v-btn>
      </v-form>
    </div>
    <default-confirmation-dialog
      color="error"
      icon="mdi-alert-circle-outline"
      confirmationButtonText="Smazat"
      :show="showDeleteDialog"
      :confirmAction="deleteQuestion"
      @close-dialog="showDeleteDialog = false"
    >
      <template v-slot:title>
        Smazat otázku?
      </template>
      <template v-slot:text>
        Opravdu si přejete smazat otázku? Tato akce je nevratná.
      </template>
    </default-confirmation-dialog>
  </v-container>
</template>

<script>
import Vue from "vue";
import api from "api-client";
import * as CodeMirror from "codemirror";
import "codemirror-formatting";
import "codemirror/lib/codemirror.css";
import "codemirror/theme/nord.css";
import "codemirror/theme/duotone-light.css";
import "codemirror/mode/clike/clike.js";
import "codemirror/addon/search/searchcursor";
import "codemirror/addon/display/placeholder.js";
import "codemirror/addon/edit/closebrackets.js";
import "codemirror/addon/edit/matchbrackets.js";
import vuetify from "@/plugins/vuetify";
import FillableWidget from "@/components/QuestionDesigner/FillableWidget.vue";
import DefaultConfirmationDialog from '../../components/DefaultConfirmationDialog.vue';
import RichTextEditor from "@/components/RichTextEditor/RichTextEditor.vue";

const WidgetComponentClass = Vue.extend(FillableWidget);

export default {
  name: "Question",
  components: { DefaultConfirmationDialog, RichTextEditor },
  data() {
    return {
      loading: false,
      question: null,
      cm: null,
      showDeleteDialog: false,
      search: "",
      rules: {
        required: (value) => !!value || "Povinné.",
      },
      breadcrumbs: [
        {
          text: "Management",
          disabled: true,
        },
        {
          text: "Otázky",
          disabled: false,
          link: true,
          exact: true,
          to: { name: "Questions" }
        },
        {
          text: "Otázka #" + this.$route.params.id,
          disabled: true
        },
      ],
    };
  },
  methods: {
    validate() {
      this.$refs.form.validate();
    },
    async editQuestion() {
      this.loading = true;
      try {
        await api.editQuestion(this.$route.params.id, this.question);
        this.$notify({
          title: "Úspěch",
          text: "Změny byly uloženy.",
          type: "success",
        });
      } catch (error) {
        this.$notify({
          title: "Error",
          text: "Změny se nepodařilo uložit.",
          type: "error",
        });
      }
      this.loading = false;
    },
    async deleteQuestion() {
      this.showDeleteDialog = false;
      this.loading = true;
      try {
        await api.deleteQuestion(this.$route.params.id);
        this.$router.push({ name: "Questions" });
        this.$notify({
          title: "Úspěch",
          text: "Otázka byla smazána.",
          type: "success",
        });
      } catch (error) {
        this.$notify({
          title: "Error",
          text: "Otázku se nepodařilo smazat.",
          type: "error",
        });
      }
      this.loading = false;
    },
    async fetchQuestion() {
      this.question = null;
      this.loading = true;
      try {
        this.question = await api.fetchQuestion(this.$route.params.id);
        if (this.question && this.question.questionType === "fill-in-code") {
          this.initCodemirror();
        }
      } catch (error) {
        this.$router.replace({
          name: "NotFound",
          params: { 0: this.$route.path },
        });
      }
      this.loading = false;
    },
    removeMultiChoiceAnswer(index) {
      this.question.answers.splice(index, 1);

      //if only one answer remaining, it must be the correct answer
      if (this.question.answers.length < 2) {
        this.question.answers[0].isCorrect = true;
      }
    },
    toggleMultiChoiceAnswerCorrect(index) {
      this.question.answers[index].isCorrect =
        !this.question.answers[index].isCorrect;
    },
    addMultiChoiceAnswer() {
      this.question.answers.push({
        answerText: "",
        isCorrect: false,
      });
    },
    initCodemirror() {
      this.$nextTick(() => {
        this.cm = CodeMirror.fromTextArea(document.getElementById("editor"), {
          lineNumbers: true,
          theme: this.$vuetify.theme.dark
            ? "nord"
            : "duotone-light",
          mode: "text/x-java",
          autoCloseTags: true,
          lineWrapping: true,
          defaultTextHeight: 32,
          placeholder: "// Zadejte kod",
          readOnly: "nocursor",
        });

        let doc = this.cm.getDoc();
        this.question.fillInCodeBlocks.forEach((block) => {
          //create widget component
          const widgetComponent = new WidgetComponentClass({
            propsData: {
              id: block.id,
              length: block.endPosition - block.startPosition,
              content: block.content,
              deletable: false,
            },
            vuetify,
          });
          let from = doc.posFromIndex(block.startPosition);
          let to = doc.posFromIndex(block.endPosition);
          widgetComponent.$mount();
          this.cm.markText(from, to, {
            replacedWith: widgetComponent.$el,
          });
        });
      });
    },
  },
  computed: {
    formattedQuestionType() {
      if (this.question.questionType === "multi-choice") return "Multi-Choice";
      if (this.question.questionType === "fill-in-code") return "Fill-In-Code";

      return "";
    },
    tags() {
      return this.$store.state.tags;
    },
    selectedTags: {
      get() {
        if (this.question) {
          return this.question.tags;
        }
        return [];
      },
      set(value) {
        if (this.question) {
          this.question.tags = value;
        }
      },
    },
  },
  watch: {
    // Map newly entered tag string to tag object
    selectedTags(val, prev) {
      if (val.length === prev.length) return;
      this.selectedTags = val.map((v) => {
        if (typeof v === "string") {
          v = {
            tagText: v,
          };
          this.selectedTags.push(v);
        }

        return v;
      });
    },
  },
  mounted() {
    this.$store.dispatch("fetchTags");
    this.fetchQuestion();
  },
  created() {
    /** needed if <router-view> is not :keyed - (going for simplicity over performance atm, don't mind reloading component for each route)
      //watch the params of the route to fetch the data again
      this.$watch(
        () => this.$route.params,
        () => {
          this.fetchQuestion();
        },
        // fetch the data when the view is created and the data is
        // already being observed
        { immediate: true }
    );
     * */
  },
};
</script>

<style>
.answerButton {
  margin-top: -8px;
  border-color: #9e9e9e;
}
.leftButton {
  border-right: none;
}
.answerField > .v-input__control > .v-input__slot {
  padding-right: 0px !important;
}
.v-snack__wrapper {
  max-width: none;
  min-width: 100%;
  margin: 0;
}
.CodeMirror {
  line-height: 32px;
  min-height: 200px;
  padding: 10px;
  height: auto;
}
</style>