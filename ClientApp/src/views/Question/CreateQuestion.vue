<template>
  <v-container fluid class="">
    <v-breadcrumbs :items="breadcrumbs" class="pa-0 pb-4 pl-1"></v-breadcrumbs>
    <h1 class="ml-1 mb-6 mt-0">Vytvořit novou otázku</h1>
    <v-form ref="form">
      <!-- Question type selector   -->
      <v-select
        :items="questionTypes"
        item-text="displayText"
        item-value="value"
        v-model="selectedQuestionType"
        label="Typ Otázky"
        outlined
        class="shrink"
        :menu-props="{ bottom: true, offsetY: true }"
        :rules="[rules.required]"
      ></v-select>

      <!-- Question component based on type -->
      <transition name="slide-fade">
        <multi-choice
          v-if="selectedQuestionType == 'multi-choice'"
          class="mb-8"
          :rules="[rules.required]"
        ></multi-choice>
        <fill-in-code
          v-if="selectedQuestionType == 'fill-in-code'"
          class="mb-8"
        ></fill-in-code>
      </transition>

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
      <v-btn color="error" :disabled="loading" outlined class="mr-4 mb-2" @click="reset">
        Smazat <v-icon right dark> mdi-trash-can-outline </v-icon>
      </v-btn>
      <v-btn color="primary" :disabled="loading" outlined class="mr-4 mb-2" @click="aiDialog = true">
        AI <v-icon right dark> mdi-memory </v-icon>
      </v-btn>
      <question-preview :loading="loading"></question-preview>
      <v-btn color="primary" :loading="loading" depressed class="mr-4 mb-2" @click="createQuestion">
        Vytvořit <v-icon right dark> mdi-plus-circle-outline </v-icon>
      </v-btn>
    </v-form>
    <ai-dialog :aiDialog.sync="aiDialog"></ai-dialog>
    </v-container>
</template>

<script>
import MultiChoice from "@/components/QuestionDesigner/MultiChoice.vue";
import FillInCode from "@/components/QuestionDesigner/FillInCode.vue";
import QuestionPreview from "@/components/QuestionDesigner/QuestionPreview.vue";
import AiDialog from "@/components/QuestionDesigner/AIService/AiDialog.vue";


export default {
  name: "CreateQuestion",
  components: {
    MultiChoice,
    FillInCode,
    QuestionPreview,
    AiDialog,
  },
  data() {
    return {
      search: "",
      error: null,
      loading: false,
      hasSaved: false,
      aiDialog: false,
      questionTypes: [
      {
        displayText: "Multi-Choice",
        value: "multi-choice"
      }, 
      {
        displayText: "Fill-In-Code",
        value: "fill-in-code"
      }
      ],
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
          text: "Nová Otázka",
          disabled: true,
          to: { name: "CreateQuestion" }
        },
      ],
    };
  },
  methods: {
    validate() {
      this.$refs.form.validate();
    },
    reset() {
      this.$refs.form.reset();
    },
    async createQuestion() {
      this.hasSaved = false;
      this.loading = true;
      try {
        await this.$store.dispatch('questionDesigner/createQuestion');
        this.reset();
        this.$notify({
          title: "Úspěch",
          text: "Otázka byla vytvořena.",
          type: "success",
        });
      } catch (error) {
        this.$notify({
          title: "Error",
          text: "Otázku se nepodařilo vytvořit.",
          type: "error",
        });
      }
      this.loading = false;
      window.scrollTo(0,0);
    }
  },
  computed: {
    tags() {
      return this.$store.state.tags;
    },
    selectedQuestionType: {
      get() {
        return this.$store.state.questionDesigner.selectedQuestionType;
      },
      set(value) {
        this.$store.commit("questionDesigner/setSelectedQuestionType", value);
      },
    },
    selectedTags: {
      get() {
        return this.$store.state.questionDesigner.selectedTags;
      },
      set(value) {
        this.$store.commit("questionDesigner/setSelectedTags", value);
      },
    },
  },
  watch: {
    // Map newly entered tag string to tag object
      selectedTags(val, prev) {
            if (val.length === prev.length) return
            this.selectedTags = val.map(v => {
              if (typeof v === 'string') {
                v = {
                  tagText: v,
                }
                this.selectedTags.push(v)
              }

              return v
            })
          }
  },
  created() {
    this.$store.dispatch("fetchTags");
  },
};
</script>

<style>
.slide-fade-enter-active {
  transition: all 0.8s ease;
}
.slide-fade-leave-active {
  transition: all 0s ease;
}
.slide-fade-enter, .slide-fade-leave-to
/* .slide-fade-leave-active below version 2.1.8 */ {
  transform: translateX(10px);
  opacity: 0;
}
.v-snack__wrapper {
  max-width: none;
  min-width: 100%;
  margin: 0;
}
</style>