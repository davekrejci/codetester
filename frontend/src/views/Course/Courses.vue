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
        dense
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
              :to="{ name: 'Course', params: { coursecode: item.courseCode } }"
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
    <default-confirmation-dialog
      color="error"
      icon="mdi-alert-circle-outline"
      confirmationButtonText="Smazat"
      :show="showDeleteDialog"
      :confirmAction="deleteCourse"
      @close-dialog="showDeleteDialog = false"
    >
      <template v-slot:title>
        Smazat {{ courseToDelete.courseCode }}?
      </template>
      <template v-slot:text>
        Opravdu si přejete smazat předmět {{ courseToDelete.courseCode }}? Tato akce je nevratná.
      </template>
    </default-confirmation-dialog>

    <default-snackbar :type="snackbar.type" :text="snackbar.text" v-on:close-snackbar="error = null"></default-snackbar>
    
  </div>
</template>

<script>
import api from "api-client";
import DefaultSnackbar from '@/components/DefaultSnackbar.vue';
import DefaultConfirmationDialog from '@/components/DefaultConfirmationDialog.vue';


export default {
  name: "Courses",
  components: { DefaultSnackbar, DefaultConfirmationDialog },
  data() {
    return {
      search: "",
      loading: false,
      error: null,
      hasBeenDeleted: false,
      courseToDelete: {},
      showDeleteDialog: false,
      headers: [
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
    async deleteCourse() {
      this.hasBeenDeleted = false;
      this.showDeleteDialog = false;
      this.error = null;
      this.loading = true;
      try {
        await api.deleteCourse(this.courseToDelete.courseCode);
        this.hasBeenDeleted = true;
      } catch (error) {
        this.error = error;
      }
      this.courseToDelete = {};
      this.fetchCourses();
    },
    async fetchCourses() {
      this.loading = true;
      try{
        await this.$store.dispatch("fetchCourses")
      } catch(error){
        console.error(error, error.stack);
        this.error = error;
      }
      this.loading = false;
    },
  },
  computed: {
    courses() {
      let courses = this.$store.getters.getCoursesWithoutSemesters;
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