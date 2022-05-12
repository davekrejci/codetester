<template>
  <v-container fluid class="">
    <v-breadcrumbs :items="breadcrumbs" class="pa-0 pb-4 pl-1"></v-breadcrumbs>
    <div class="mb-4">
      <h1 class="ml-1 mb-6 mt-0 d-inline">Nahrát uživatele</h1>
      <xlsx-workbook class="d-inline">
        <xlsx-sheet
          :collection="templateData"
          sheetName="Sheet1"
        />
        <xlsx-download disable-wrapper-click class="d-inline">
          <template #default="{ download }">
            <v-tooltip right>
              <template v-slot:activator="{ on }">
                <v-btn @click="download" small class="mb-4 ml-2" icon>
                  <v-icon v-on="on" class="">mdi-download-circle</v-icon>
                </v-btn>
              </template>
              <span>Stáhnout excel šablonu</span>
            </v-tooltip>
          </template>
        </xlsx-download>
      </xlsx-workbook>
      <div class="text-caption ml-1">
        <span>
          Formáty:
          <v-chip small class="mb-1" color="green" outlined>xlsx</v-chip>
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
      accept=".xls,.xlsx,.json"
      label="Vyberte soubor"
      prepend-inner-icon="mdi-paperclip"
      prepend-icon=""
    ></v-file-input>
    <xlsx-read :file="file" v-if="this.fileType == 'excel'">
      <xlsx-json @parsed="setUsers"> </xlsx-json>
    </xlsx-read>
    <div v-if="users">
      <v-card outlined class="mb-4">
        <v-data-table
          :headers="headers"
          :items="this.users"
          :items-per-page="10"
        >
        </v-data-table>
      </v-card>
      <v-btn color="primary" depressed @click="importUsers">Nahrát</v-btn>
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
import { XlsxRead, XlsxJson, XlsxSheet, XlsxWorkbook, XlsxDownload } from "vue-xlsx";
import DefaultSnackbar from "@/components/DefaultSnackbar.vue";

export default {
  components: { DefaultSnackbar, XlsxRead, XlsxJson, XlsxSheet, XlsxWorkbook, XlsxDownload },
  name: "ImportUsers",
  data() {
    return {
      file: null,
      fileType: "",
      isValidFile: true,
      error: null,
      hasSaved: false,
      loading: false,
      users: null,
      templateData: [
        { 
          "username": "",
          "email": "",
          "role": "",
          "password": "",
          "firstName": "" ,
          "lastName": "" ,
        } 
      ],
      headers: [
        { text: "Uživatelské jméno", value: "username" },
        { text: "Email", value: "email" },
        { text: "Jméno", value: "firstName" },
        { text: "Příjmení", value: "lastName" },
        { text: "Heslo", value: "password" },
        { text: "Role", value: "role" },
      ],
      breadcrumbs: [
        {
          text: "Management",
          disabled: true,
        },
        {
          text: "Uživatelé",
          disabled: false,
          link: true,
          exact: true,
          to: { name: "Users" },
        },
        {
          text: "Nahrát Uživatele",
          disabled: true,
          link: true,
          exact: true,
          to: { name: "ImportUsers" },
        },
      ],
    };
  },
  methods: {
    setUsers(users) {
      this.users = users;
      users.forEach((user) => {
        if (!this.isValidUser(user)) {
          this.isValidFile = false;
        }
      });
    },
    isValidUser(user) {
      let isValid = true;
      if (!user.username) isValid = false;
      if (!user.email) isValid = false;
      if (!user.password) isValid = false;
      if (!user.firstName) isValid = false;
      if (!user.lastName) isValid = false;
      if (!user.role) isValid = false;
      return isValid;
    },
    selectFile(file) {
      this.users = null;
      this.file = file;
      var ext = file.name.match(/.([^.]+)$/)[1];
      switch (ext) {
        case "xls":
          this.fileType = "excel";
          break;
        case "xlsx":
          this.fileType = "excel";
          break;
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
          self.users = JSON.parse(e.target.result);
        };
      }
    },
    async importUsers() {
      this.hasSaved = false;
      this.loading = true;
      try {
        await api.importUsers(this.users);
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
          text: "Uživatelé byli vytvořeni",
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