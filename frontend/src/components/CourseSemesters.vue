<template>
  <div>
    <v-card outlined flat class="pa-4">
      <v-card-title>
          <span>Semestry</span>
          <v-spacer></v-spacer>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <router-link :to="{ name: 'CreateSemester' }">
              <v-btn
                class="mx-2 text-right"
                depressed
                fab
                x-small
                color="primary"
                v-bind="attrs"
                v-on="on"
              >
                <v-icon dark> mdi-plus </v-icon>
              </v-btn>
            </router-link>
          </template>
          <span>Přidat Semestr</span>
        </v-tooltip>
      </v-card-title>
      <v-data-table
      dense
        :headers="headers"
        :items="semesters"
        :items-per-page="15"
        :search="search"
        :loading="loading"
        loading-text="Načítání dat..."
        no-data-text="Žádné data"
        item-key="semester"
        v-model="selected"
      >
        <template v-slot:[`item.actions`]="{ item }">
          <v-row
            align="center"
            justify="space-around"
            class="d-flex flex-nowrap"
          >
            <router-link
              :to="{ name: 'Semester', params: { id: item.id } }"
              style="text-decoration: none; color: inherit"
            >
              <v-btn small plain icon color="primary" class="mx-1">
                <v-icon> mdi-magnify </v-icon>
              </v-btn>
            </router-link>
            <v-btn
              @click="showDeleteDialogForSemester(item)"
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
          >Opravdu si přejete smazat <strong>{{ semesterToDelete.semesterType.displayText }}</strong> semestr <strong>{{ semesterToDelete.year }}</strong>? Tato akce
          je nevratná.</v-card-text
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
            @click="deleteSemester(semesterToDelete.id)"
          >
            Ano
          </v-btn>
          <v-spacer></v-spacer>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import api from "api-client";

export default {
  name: "CourseSemesters",
  props: {
    semesters: { type: Array, required: true },
  },
  data() {
    return {
      search: "",
      loading: false,
      semesterToDelete: {
        year: "",
        semesterType: {
          displayText: "",
          value: ""
        }
      },
      showDeleteDialog: false,
      headers: [
        { text: "Rok", value: "year" },
        { text: "Semestr", value: "semesterType.displayText" },
        { text: "Počet studentů", value: "studentCount" },
        { text: "Akce", value: "actions", sortable: false, width: "15" },
      ],
      selected: [],
    };
  },
  methods: {
    showDeleteDialogForSemester(semester) {
      this.semesterToDelete = semester;
      this.showDeleteDialog = true;
    },
    async deleteSemester(id) {
      this.semesterToDelete = {
        year: "",
        semesterType: {
          displayText: "",
          value: ""
        }
      };
      this.showDeleteDialog = false;
      this.loading = true;
      try {
        await api.deleteSemester(id);
        this.$emit('semester-delete-success');
      } catch (error) {
        this.$emit('semester-delete-error', error);
      }
      this.loading = false;
      // TODO: reload data
    },
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