<template>
  <v-container fluid>
    <v-breadcrumbs :items="breadcrumbs" class="pa-0 pb-4 pl-1"></v-breadcrumbs>
    <h1 class="ml-1 mb-6 mt-0">Otázky</h1>
    <v-card outlined class="pb-4">
      <v-card-title class="mb-4">
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
            <router-link :to="{ name: 'CreateQuestion' }">
              <v-btn
                class="mx-2"
                depressed
                fab
                small
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
            <div v-bind="attrs" v-on="on">
              <router-link :to="{ name: 'ImportQuestions' }">
                <v-btn class="mx-2" depressed fab small color="primary">
                  <v-icon dark> mdi-cloud-upload </v-icon>
                </v-btn>
              </router-link>
            </div>
          </template>
          <span>Nahrát Otázky</span>
        </v-tooltip>
      </v-card-title>
      <v-data-table
        dense
        :headers="headers"
        :items="questions"
        :items-per-page="15"
        :search="search"
        :loading="loading"
        loading-text="Načítání dat..."
        no-data-text="Žádné data"
        item-key="question"
        class="mx-4"
        v-model="selected"
        :custom-filter="customFilter"
      >
        <template v-slot:[`item.actions`]="{ item }">
          <v-row
            align="center"
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

    <!-- Delete Dialog -->
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
        Opravdu si přejete smazat otázku #{{ toDeleteId }}? Tato akce je nevratná.
      </template>
    </default-confirmation-dialog>
    <default-snackbar :type="snackbar.type" :text="snackbar.text" v-on:close-snackbar="error = null"></default-snackbar>
  </v-container>
</template>

<script>
import api from "api-client";
import DefaultSnackbar from '@/components/DefaultSnackbar.vue';
import DefaultConfirmationDialog from '@/components/DefaultConfirmationDialog.vue';

export default {
  components: { DefaultSnackbar, DefaultConfirmationDialog },
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
        { text: "Tagy", value: "tags" },
        { text: "Akce", value: "actions", sortable: false },
      ],
      selected: [],
      breadcrumbs: [
        {
          text: "Management",
          disabled: true,
        },
        {
          text: "Otázky",
          disabled: true,
          to: "Questions",
        },
      ],
    };
  },
  methods: {
    customFilter(value, search, item) {
      let tagTexts = item.tags.map(tag => tag.tagText);
      return (
        value != null &&
        search != null &&
        typeof value !== "boolean" &&
        value
          .toString()
          .toLocaleLowerCase()
          .indexOf(search.toLocaleLowerCase()) !== -1 || tagTexts.some(tagText => tagText.includes(search)
        )
      );
    },
    showDeleteDialogForItem(id) {
      this.toDeleteId = id;
      this.showDeleteDialog = true;
    },
    async deleteQuestion() {
      this.hasBeenDeleted = false;
      this.showDeleteDialog = false;
      this.error = null;
      this.loading = true;
      try {
        await api.deleteQuestion(this.toDeleteId);
        this.hasBeenDeleted = true;
        //this.$router.push({ name: "Questions" });
      } catch (error) {
        this.error = error;
      }
      this.toDeleteId = null;
      this.fetchQuestions();
    },
    async fetchQuestions() {
      this.loading = true;
      try{
        await this.$store.dispatch("fetchQuestions")
      } catch(error){
        console.log(error);
        this.error = error;
      }
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
      });
      return questions;
    },
    snackbar() {
      if (this.error != null) {
        return {
          type: "error",
          text: this.error.toString(),
          show: true
        };
      }
      if (this.hasBeenDeleted) {
        return {
          type: 'success',
          text: "Otázka byla smazána",
          show: true
        };
      }
      return {
        show: false
      }
    },
  },
  created() {
    this.fetchQuestions();
  },
};
</script>

<style>
</style>