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
            <router-link :to="{ name: 'CreateCourse' }">
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
          <span>Přidat Předmět</span>
        </v-tooltip>
        
      </v-card-title>
      <v-data-table
        :headers="headers"
        :items="courses"
        :items-per-page="15"
        :search="search"
        :loading="loading"
        loading-text="Načítání dat..."
        no-data-text="Žádné data"
        item-key="course"
        v-model="selected"
      >
        <template v-slot:[`item.actions`]="{ item }">
          <v-row
            align="center"
            justify="space-around"
            class="d-flex flex-nowrap"
          >
            <router-link
              :to="{ name: 'Course', params: { id: item.id } }"
              style="text-decoration: none; color: inherit"
            >
              <v-btn small plain icon color="primary" class="mx-1">
                <v-icon> mdi-magnify </v-icon>
              </v-btn>
            </router-link>
            <v-btn
              @click="showDeleteDialogForCourse(item)"
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
          >Opravdu si přejete smazat předmět {{ courseToDelete.courseCode }}? Tato akce je
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
            @click="deleteCourse(courseToDelete.id)"
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
  name: "Courses",
  components: { DefaultSnackbar },
  data() {
    return {
      search: "",
      loading: false,
      error: null,
      hasBeenDeleted: false,
      courseToDelete: {},
      showDeleteDialog: false,
      headers: [
        { text: "Id", value: "id" },
        { text: "Kod Předmětu", value: "courseCode" },
        { text: "Jméno Předmětu", value: "courseName" },
        { text: "Akce", value: "actions", sortable: false, width: "15" },
      ],
      selected: [],
      breadcrumbs: [
        {
          text: "Předměty",
          disabled: true,
          to: "Courses",
        },
      ],
    };
  },
  methods: {
    showDeleteDialogForCourse(course) {
      this.courseToDelete = course; 
      this.showDeleteDialog = true;
    },
    async deleteCourse(id) {
      this.hasBeenDeleted = false;
      this.courseToDelete = {};
      this.showDeleteDialog = false;
      this.error = null;
      this.loading = true;
      try {
        await api.deleteCourse(id);
        this.hasBeenDeleted = true;
      } catch (error) {
        this.error = error;
      }
      this.fetchCourses();
    },
    async fetchCourses() {
      this.loading = true;
      try{
        await this.$store.dispatch("fetchCourses")
      } catch(error){
        console.log(error);
        this.error = error;
      }
      this.loading = false;
    },
  },
  computed: {
    courses() {
      let courses = this.$store.state.courses;
      return courses;
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
          text: "Předmět byl smazán",
          show: true
        };
      }
      return {
        show: false
      }
    },
  },
  created() {
    this.fetchCourses();
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