<template>
  <v-container class="px-12">
    <v-breadcrumbs :items="breadcrumbs"></v-breadcrumbs>
    <h1 class="mb-8">Vytvořit novou otázku</h1>
    <v-form ref="form">
      <!-- Question type selector   -->
      <v-select
        :items="questionTypes"
        v-model="selectedType"
        label="Typ Otázky"
        outlined
        class="shrink"
        :menu-props="{ bottom: true, offsetY: true }"
        :rules="[rules.required]"
      ></v-select>

      <!-- Question component based on type -->
      <transition name="slide-fade">
        <multi-choice
          v-if="selectedType == 'Multi-Choice'"
          class="mb-8"
          :rules="[rules.required]"
        ></multi-choice>
        <fill-in-code
          v-if="selectedType == 'Fill-In-Code'"
          class="mb-8"
        ></fill-in-code>
      </transition>

      <!-- Tag selector -->
      <v-combobox
        v-model="selectedTags"
        :items="tags"
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
      <v-btn color="error" outlined class="mr-4 mb-2" @click="reset">
        Smazat <v-icon right dark> mdi-trash-can-outline </v-icon>
      </v-btn>
      <v-btn color="primary" @click.stop="showPreview = true" depressed outlined class="mr-4 mb-2">
        Náhled
        <v-icon right dark> mdi-magnify </v-icon>
      </v-btn>
      <v-btn color="primary" depressed class="mr-4 mb-2">
        Vytvořit <v-icon right dark> mdi-plus-circle-outline </v-icon>
      </v-btn>
    </v-form>
    <question-preview
      :showPreview="this.showPreview"
      @closePreview="showPreview = false"
      ></question-preview>
  </v-container>
</template>

<script>
import MultiChoice from "@/components/QuestionDesigner/MultiChoice.vue";
import FillInCode from "@/components/QuestionDesigner/FillInCode.vue";
import QuestionPreview from '@/components/QuestionDesigner/QuestionPreview.vue';

export default {
  name: "QuestionDesigner",
  components: {
    MultiChoice,
    FillInCode,
    QuestionPreview
  },
  data() {
    return {
      search: "",
      selectedType: "",
      selectedTags: [],
      selectedDifficulty: "",
      difficultyLabels: ["Lehká", "Střední", "Těžká"],
      questionTypes: ["Multi-Choice", "Fill-In-Code"],
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
      showPreview: false,
    };
  },
  methods: {
    validate() {
      this.$refs.form.validate();
    },
    reset() {
      this.$refs.form.reset();
    },
  },
  computed: {
    tags() {
      return this.$store.state.tags;
    },
  },
  created() {
    this.$store.dispatch("fetchTags");
  },
};
</script>

<style scoped>
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
</style>