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
              <v-btn class="mx-2" depressed fab disabled small color="primary">
                <v-icon dark> mdi-cloud-upload </v-icon>
              </v-btn>
            </div>
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
        no-data-text="Žádné data"
        item-key="question"
        v-model="selected"
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
    <default-snackbar :type="snackbar.type" :text="snackbar.text" v-on:close-snackbar="error = null"></default-snackbar>
  </div>
</template>

<script>
import api from "api-client";
import DefaultSnackbar from '@/components/DefaultSnackbar.vue';

export default {
  components: { DefaultSnackbar },
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