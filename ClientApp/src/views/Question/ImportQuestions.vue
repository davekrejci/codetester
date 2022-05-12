<template>
  <v-container fluid class="">
    <v-breadcrumbs :items="breadcrumbs" class="pa-0 pb-4 pl-1"></v-breadcrumbs>
    <div class="mb-4">
      <h1 class="ml-1 mb-6 mt-0 d-inline">Nahrát otázky</h1>
      <div class="text-caption ml-1">
        <span>
          Formáty:
          <v-chip small class="ml-1 mb-1" color="orange" outlined>json</v-chip>
        </span>
      </div>
    </div>
    <v-file-input
      @change="selectFile"
      outlined
      color="primary"
      show-size
      truncate-length="27"
      accept=".json"
      label="Vyberte soubor"
      prepend-inner-icon="mdi-paperclip"
      prepend-icon=""
    ></v-file-input>
    <div v-if="questions">
      <v-card outlined class="mb-4">
        <v-data-table
          :headers="headers"
          :items="this.questions"
          :items-per-page="10"
        >
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
      <v-btn color="primary" depressed @click="importQuestions">Nahrát</v-btn>
    </div>
    <div v-if="fileType == 'not-supported'">
      <v-alert dense border="left" type="error">
        Nepodporovaný formát.
      </v-alert>
    </div>
    <default-snackbar
      :type="snackbar.type"
      :text="snackbar.text"
      v-on:close-snackbar="error = null"
    ></default-snackbar>
  </v-container>
</template>

<script>
import api from "api-client";
import DefaultSnackbar from "@/components/DefaultSnackbar.vue";

export default {
  components: { DefaultSnackbar },
  name: "ImportQuestions",
  data() {
    return {
      file: null,
      fileType: "",
      isValidFile: true,
      error: null,
      hasSaved: false,
      loading: false,
      questions: null,
      headers: [
        { text: "Typ", value: "questionType" },
        { text: "Text", value: "questionText" },
        { text: "Štítky", value: "tags" },
      ],
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
          to: { name: "Questions" },
        },
        {
          text: "Nahrát Otázky",
          disabled: true,
          link: true,
          exact: true,
          to: { name: "ImportQuestions" },
        },
      ],
    };
  },
  methods: {
    setQuestions(questions) {
      this.questions = questions;
    //   questions.forEach((questions) => {
    //     if (!this.isValidQuestion(questions)) {
    //       this.isValidFile = false;
    //     }
    //   });
    },
    // isValidQuestion(question) {
    //   let isValid = true;
    //   return isValid;
    // },
    selectFile(file) {
      this.questions = null;
      this.file = file;
      var ext = file.name.match(/.([^.]+)$/)[1];
      switch (ext) {
        case "json":
          this.fileType = "json";
          break;
        default:
          this.fileType = "not-supported";
      }
      if (this.fileType == "json") {
        var reader = new FileReader();
        var self = this;
        reader.readAsText(file);
        reader.onload = function (e) {
          // parse string to json
          self.questions = JSON.parse(e.target.result);
        };
      }
    },
    async importQuestions() {
      this.hasSaved = false;
      this.loading = true;
      try {
        await api.importQuestions(this.questions);
        this.hasSaved = true;
      } catch (error) {
        console.log(error);
        this.error = error;
      }
      this.loading = false;
      window.scrollTo(0, 0);
    },
  },
  computed: {
    snackbar() {
      if (this.error != null) {
        return {
          type: "error",
          text: this.error.response.data,
          show: true,
        };
      }
      if (this.hasSaved) {
        return {
          type: "success",
          text: "Otázky byly vytvořeny",
          show: true,
        };
      }
      return {
        show: false,
      };
    },
  },
};
</script>

<style>
</style>