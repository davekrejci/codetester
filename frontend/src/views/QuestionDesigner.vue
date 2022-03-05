<template>
  <v-container class="px-12">
    <v-breadcrumbs :items="breadcrumbs"></v-breadcrumbs>
    <h1 class="mb-8">Vytvořit novou otázku</h1>
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
      <question-preview :loading="loading"></question-preview>
      <v-btn color="primary" :loading="loading" depressed class="mr-4 mb-2" @click="createQuestion">
        Vytvořit <v-icon right dark> mdi-plus-circle-outline </v-icon>
      </v-btn>
    </v-form>
    <div class="text-center">
      <v-snackbar
        :timeout="snackbar.timeout"
        :value="snackbar.show"
        absolute
        top
        :color="snackbar.color"
        middle
        tile
        multi-line
      >
        <v-layout align-center pr-4>
          <v-icon class="pr-3" dark large>{{ snackbar.icon }}</v-icon>
          <v-layout column>
            <div>
              <strong>{{ snackbar.title }}</strong>
            </div>
            <div>{{ snackbar.text }}</div>
          </v-layout>
          <v-btn v-if="snackbar.timeout === -1" icon @click="error = null">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-layout>
        <!-- <v-icon left> {{ snackbar.icon }} </v-icon> <strong>{{ snackbar.text }}</strong> -->
      </v-snackbar>
    </div>
  </v-container>
</template>

<script>
import MultiChoice from "@/components/QuestionDesigner/MultiChoice.vue";
import FillInCode from "@/components/QuestionDesigner/FillInCode.vue";
import QuestionPreview from "@/components/QuestionDesigner/QuestionPreview.vue";

export default {
  name: "QuestionDesigner",
  components: {
    MultiChoice,
    FillInCode,
    QuestionPreview,
  },
  data() {
    return {
      search: "",
      error: null,
      loading: false,
      hasSaved: false,
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
          text: "Otázky",
          disabled: false,
          to: "Questions",
        },
        {
          text: "Nová Otázka",
          disabled: true,
          to: "QuestionDesigner",
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
        this.hasSaved = true;
        this.reset();
      } catch (error) {
        console.log(error);
        this.error = error;
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
    snackbar() {
      if (this.error != null) {
        return {
          show: true,
          icon: "mdi-alert-circle",
          color: "error",
          title: "Error",
          text: this.error.toString(),
          timeout: -1,
        };
      }
      if (this.hasSaved) {
        return {
          show: true,
          icon: "mdi-check-circle",
          color: "success",
          title: "Úspěch",
          text: "Otázka byla vytvořena",
          timeout: 3000,
        };
      }
      return {
        show: false,
      };
    }
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