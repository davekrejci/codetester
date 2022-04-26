<template>
  <div>
    <!-- selected questions -->
    <v-card flat :outlined="hasOutline" class="px-4">
      <v-card-title>
        <v-spacer></v-spacer>
        <v-text-field
          v-model="searchSelected"
          append-icon="mdi-magnify"
          placeholder="Vyhledat"
          single-line
          hide-details
          filled
          rounded
          dense
          class="shrink mx-4"
        ></v-text-field>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              class="mx-2"
              depressed
              fab
              small
              :disabled="editingDisabled"
              color="primary"
              v-bind="attrs"
              v-on="on"
              @click.stop="dialog = true"
            >
              <v-icon dark> mdi-plus </v-icon>
            </v-btn>
          </template>
          <span>Přidat Otázku</span>
        </v-tooltip>
      </v-card-title>
      <v-data-table
        :headers="headers"
        dense
        :items="selectedQuestions"
        :items-per-page="15"
        :search="searchSelected"
        :loading="loading"
        loading-text="Načítání dat..."
        no-data-text="Žádné vybrané otázky"
        item-key="question"
        :custom-filter="customFilter"
      >
        <template v-slot:[`item.actions`]="{ item }">
          <v-row
            align="center"
            justify="space-around"
            class="d-flex flex-nowrap"
          >
            <router-link
              :to="{ name: 'Question', params: { id: item.id } }"
              style="text-decoration: none; color: inherit"
            >
              <v-btn small plain icon color="primary" class="mx-1">
                <v-icon> mdi-magnify </v-icon>
              </v-btn>
            </router-link>
            <v-btn
              @click="removeFromSelectedQuestions(item.id)"
              :disabled="editingDisabled"
              small
              icon
              plain
              color="error"
              class="mx-1"
            >
              <v-icon> mdi-minus </v-icon>
            </v-btn>
          </v-row>
        </template>
        <template v-slot:[`item.tags`]="{ item }">
          <div
            v-if="item.tags.length > 0"
            class="text-truncate"
            style="max-width: 400px"
          >
            <v-slide-group show-arrows="always">
              <v-chip small v-for="tag in item.tags" :key="tag.tagText" class="mx-1">
                {{ tag.tagText }}
              </v-chip>
            </v-slide-group>
          </div>
        </template>
      </v-data-table>
    </v-card>

    <!-- possible questions -->
    <v-dialog v-model="dialog" max-width="1000">
      <v-card class="pa-8">
        <v-card-title>
          <v-text-field
            v-model="search"
            append-icon="mdi-magnify"
            placeholder="Vyhledat"
            single-line
            hide-details
            filled
            rounded
            dense
            class="shrink mx-4"
          ></v-text-field>
        </v-card-title>
        <v-data-table
          :headers="headers"
          dense
          :items="nonSelectedQuestions"
          :items-per-page="15"
          :search="search"
          :loading="loading"
          loading-text="Načítání dat..."
          no-data-text="Žádné data"
          item-key="id"
          v-model="selected"
          show-select
          :custom-filter="customFilter"
        >
          <template v-slot:[`item.actions`]="{ item }">
            <v-row
              align="center"
              justify="space-around"
              class="d-flex flex-nowrap"
            >
              <router-link
                :to="{ name: 'Question', params: { id: item.id } }"
                style="text-decoration: none; color: inherit"
              >
                <v-btn small plain icon color="primary" class="mx-1">
                  <v-icon> mdi-magnify </v-icon>
                </v-btn>
              </router-link>
            </v-row>
          </template>
          <template v-slot:[`item.tags`]="{ item }">
            <div
              v-if="item.tags.length > 0"
              class="text-truncate"
              style="max-width: 400px"
            >
              <v-slide-group show-arrows="always">
                <v-chip
                  v-for="tag in item.tags"
                  small
                  :key="tag.tagText"
                  class="mx-1"
                >
                  {{ tag.tagText }}
                </v-chip>
              </v-slide-group>
            </div>
          </template>
        </v-data-table>
        <v-card-actions>
          <v-spacer></v-spacer>

          <v-btn color="error" outlined @click="dialog = false"> Zrušit </v-btn>
          <v-btn
            color="primary"
            depressed
            @click="addSelectedToSelectedQuestions()"
          >
            Přidat
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
export default {
  name: "ExamQuestions",
  props: {
    currentQuestions: {
      type: Array,
      default: () => new Array(), 
    },
    editingDisabled: {
      type: Boolean,
      default: false
    },
    hasOutline: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      search: "",
      searchSelected: "",
      dialog: false,
      loading: false,
      headers: [
        { text: "Id", value: "id" },
        { text: "Typ", value: "questionType" },
        { text: "Otázka", value: "questionText" },
        { text: "Tagy", value: "tags" },
        { text: "Akce", value: "actions", sortable: false },
      ],
      selected: [],
      selectedQuestions: this.initCurrentQuestions(),
    };
  },
  methods: {
    initCurrentQuestions() {
      let questions = this.currentQuestions;
      // change codeDescription to questionText attribute so that they can populate a single column
      questions.forEach((question) => {
        if (question.questionType == "fill-in-code") {
          question.questionText = question.codeDescription;
        }
      });
      return questions;
    },
    customFilter(value, search, item) {
      let tagTexts = item.tags.map((tag) => tag.tagText);
      return (
        (value != null &&
          search != null &&
          typeof value !== "boolean" &&
          value
            .toString()
            .toLocaleLowerCase()
            .indexOf(search.toLocaleLowerCase()) !== -1) ||
        tagTexts.some((tagText) => tagText.includes(search))
      );
    },
    async fetchQuestions() {
      this.loading = true;
      try {
        await this.$store.dispatch("fetchQuestions");
      } catch (error) {
        console.log(error);
        this.error = error;
      }
      this.loading = false;
    },
    addSelectedToSelectedQuestions() {
      this.selected.forEach((question) =>
        this.selectedQuestions.push(question)
      );
      this.selected = [];
      this.dialog = false;
    },
    removeFromSelectedQuestions(id) {
      this.selectedQuestions = this.selectedQuestions.filter(
        (question) => question.id != id
      );
    },
  },
  computed: {
    questions() {
      let questions = this.$store.state.questions;
      // change codeDescription to questionText attribute so that they can populate a single column
      questions.forEach((question) => {
        if (question.questionType == "fill-in-code") {
          question.questionText = question.codeDescription;
        }
      });
      return questions;
    },
    nonSelectedQuestions() {
      let selectedQuestionIds = this.selectedQuestions.map(
        (question) => question.id
      );
      return this.questions.filter(
        (question) => selectedQuestionIds.indexOf(question.id) === -1
      );
    },
  },
  watch: {
    // update parent component on selectedQuestions change
    selectedQuestions(val) {
      this.$emit("selectedQuestionsChanged", val);
    },
  },
  created() {
    if (this.questions.length == 0) {
      this.fetchQuestions();
    }
  },
};
</script>

<style>
</style>