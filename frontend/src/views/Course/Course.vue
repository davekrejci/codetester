<template>
  <v-container class="px-12">
    <div v-if="this.course != null">
      <v-breadcrumbs :items="breadcrumbs" class="mb-4"></v-breadcrumbs>
      <v-form ref="form">
        <v-text-field
          outlined
          disabled
          :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
          label="Kód předmětu"
          v-model="course.courseCode"
          readonly
          color="grey"
        >
        </v-text-field>

        <v-text-field
          outlined
          :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
          label="Jméno předmetu"
          v-model="course.courseName"
          readonly
          disabled
          color="grey"
          class="mb-4"
        >
        </v-text-field>


        <course-semesters 
          class="mb-12"
          :semesters="semesters" 
          v-on:semester-delete-success="handleSemesterDeleteSuccess" 
          v-on:semester-delete-error="handleSemesterDeleteError"
        ></course-semesters>

        <!-- Action buttons -->
        <v-btn
          @click="showDeleteDialog = true"
          :loading="loading"
          :disabled="loading"
          color="error"
          outlined
          class="mr-4 mb-2"
        >
          Smazat <v-icon right dark> mdi-trash-can-outline </v-icon>
        </v-btn>
        

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
            Smazat předmět?
          </template>
          <template v-slot:text>
            Opravdu si přejete smazat předmět? Tato akce je nevratná.
          </template>
        </default-confirmation-dialog>


      </v-form>
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
import DefaultConfirmationDialog from '@/components/DefaultConfirmationDialog.vue';
import CourseSemesters from '@/components/CourseSemesters.vue';

export default {
  name: "Course",
  components: { 
    DefaultSnackbar,
    DefaultConfirmationDialog,
    CourseSemesters,
  },
  data() {
    return {
      semesterHasBeenDeleted: false,
      showDeleteDialog: false,
      loading: false,
      error: null,
      course: null,
    };
  },
  methods: {
    async fetchCourse() {
      this.error = this.course = null;
      this.hasSaved = false;
      this.loading = true;
      try {
        this.course = await api.fetchCourse(this.$route.params.coursecode);
      } catch (error) {
        this.$router.replace({
          name: "NotFound",
          params: { 0: this.$route.path },
        });
      }
      this.loading = false;
    },
    async deleteCourse() {
      this.error = null;
      this.loading = true;
      try {
        await api.deleteCourse(this.$route.params.coursecode);
        this.$router.push({ name: "Courses" });
      } catch (error) {
        this.error = error;
      }
      this.loading = false;
    },
    handleSemesterDeleteError(error) {
      this.semesterHasBeenDeleted = false;
      this.error = error;
    },
    handleSemesterDeleteSuccess() {
      this.semesterHasBeenDeleted = true;
      this.fetchCourse();
    }
  },
  computed: {
    snackbar() {
      if (this.error != null) {
        return {
          type: "error",
          text: this.error.toString(),
          show: true,
        };
      }
      if (this.semesterHasBeenDeleted) {
        return {
          type: "success",
          text: "Semestr byl smazán",
          show: true,
        };
      }
      return {
        show: false,
      };
    },
    semesters() {
      return this.course.semesters.map(semester => {
        let studentCount = semester.enrolledStudents.length;
        if(semester.semesterType === 'winter'){
          return {...semester, semesterType: { displayText:'zimní', value: 'winter' }, studentCount}
        }
        if(semester.semesterType === 'summer'){
          return {...semester, semesterType: { displayText:'letní', value: 'summer' }, studentCount}
        }
      });
    },
    breadcrumbs() {
      let currentCourseCode = this.course.courseCode || ""
      return [
        {
          text: "Předměty",
          disabled: false,
          link: true,
          exact: true,
          to: { name: "Courses" }
        },
        {
          text: currentCourseCode,
          disabled: true,
          link: true,
          exact: true,
          to: { 
            name: "Course",
            params: {
              id: this.$route.params.coursecode 
            }
          }
        },
      ];
    },
  },
  created() {
    this.fetchCourse();
  },
};
</script>

<style>
</style>