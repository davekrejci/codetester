<template>
  <div>
    <v-card flat class="pa-4">
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
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <router-link :to="{ name: 'QuestionDesigner' }">
              <v-btn
                class="mx-2"
                depressed
                fab
                small
                dark
                color="primary"
                v-bind="attrs"
                v-on="on"
              >
                <v-icon dark> mdi-plus </v-icon>
              </v-btn>
            </router-link>
          </template>
          <span>Přidat Otázku</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              class="mx-2"
              depressed
              fab
              small
              dark
              color="primary"
              v-bind="attrs"
              v-on="on"
            >
              <v-icon dark> mdi-cloud-upload </v-icon>
            </v-btn>
          </template>
          <span>Nahrát Otázky</span>
        </v-tooltip>
      </v-card-title>
      <v-data-table
        :headers="headers"
        :items="questions"
        :items-per-page="15"
        :search="search"
        :loading="loading"
        loading-text="Načítání dat..."
        item-key="question"
        v-model="selected"
      >
        <template v-slot:item.actions="{ item }">
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
            @click="showDeleteDialogForItem(item.id)"
              small
              icon
              plain
              color="error"
              class="mx-1"
            >
              <v-icon> mdi-delete </v-icon>
            </v-btn>
          </v-row>
        </template>
        <template v-slot:item.tags="{ item }">
          <div
            v-if="item.tags.length > 0"
            class="text-truncate"
            style="max-width: 400px"
          >
            <v-slide-group show-arrows="always">
              <v-chip v-for="tag in item.tags" :key="tag" class="mx-1">
                {{ tag }}
              </v-chip>
            </v-slide-group>
          </div>
        </template>
      </v-data-table>
    </v-card>

    <!-- Delete Dialog -->
            <v-dialog v-model="showDeleteDialog" max-width="400px">
              <v-card class="text-center pa-4">
                <v-icon color="error" x-large>mdi-alert-circle-outline</v-icon>
                <v-card-title class="text-h5">
                  <!-- <span class="mx-auto my-4"> Jste si jistý?</span> -->
                </v-card-title>
                <v-card-text
                  >Opravdu si přejete smazat otázku #{{ toDeleteId }}? Tato akce je
                  nevratná.</v-card-text
                >
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn
                    color="grey"
                    class="mx-2"
                    outlined
                    @click="showDeleteDialog = false"
                  >
                    Ne
                  </v-btn>
                  <v-btn
                    color="error"
                    class="mx-2"
                    outlined
                    @click="deleteQuestion(toDeleteId)"
                  >
                    Ano
                  </v-btn>
                  <v-spacer></v-spacer>
                </v-card-actions>
              </v-card>
            </v-dialog>

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
  </div>
</template>

<script>
import api from "api-client";

export default {
  name: "Questions",
  data() {
    return {
      search: "",
      loading: false,
      error: null,
      hasBeenDeleted: false,
      toDeleteId: null,
      showDeleteDialog: false,
      headers: [
        { text: "Id", value: "id" },
        { text: "Typ", value: "questionType" },
        { text: "Otázka", value: "questionText" },
        { text: "Tagy", value: "tags" },
        { text: "Akce", value: "actions", sortable: false },
      ],
      selected: [],
      breadcrumbs: [
        {
          text: "Otázky",
          disabled: true,
          to: "Questions",
        },
      ],
    };
  },
  methods: {
    showDeleteDialogForItem(id) {
      this.toDeleteId = id;
      this.showDeleteDialog = true;
    },
    async deleteQuestion(id) {
      this.hasBeenDeleted = false;
      this.toDeleteId = null;
      this.showDeleteDialog = false;
      this.error = null;
      this.loading = true;
      try {
        await api.deleteQuestion(id);
        this.hasBeenDeleted = true;
        //this.$router.push({ name: "Questions" });
      } catch (error) {
        this.error = error;
      }
      this.fetchQuestions();
    },
    async fetchQuestions() {
      this.loading = true;
      await this.$store.dispatch("fetchQuestions").catch((err) => {
        console.log(err);
        this.loading = false;
      });
      this.loading = false;
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
        // convert tag objects to array of strings so they are searchable in datatable
        let textOnlyTags = [];
        question.tags.forEach((tag) => textOnlyTags.push(tag.tagText));
        question.tags = textOnlyTags;
      });
      return questions;
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
      if (this.hasBeenDeleted) {
        return {
          show: true,
          icon: "mdi-check-circle",
          color: "success",
          title: "Úspěch",
          text: "Otázka byla smazána",
          timeout: 3000,
        };
      }
      return {
        show: false,
      };
    },
  },
  created() {
    this.fetchQuestions();
  },
};
</script>

<style>
.v-snack__wrapper {
  max-width: none;
  min-width: 100%;
  margin: 0;
}
</style>