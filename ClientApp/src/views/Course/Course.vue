<template>
  <v-container fluid class="">
    <div v-if="this.course != null">
      <v-breadcrumbs :items="breadcrumbs" class="pa-0 pb-4 pl-1"></v-breadcrumbs>
      <h1 class="ml-1 mb-6 mt-0">{{course.courseName}}</h1>
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
  </v-container>
</template>

<script>
import api from "api-client";
import DefaultConfirmationDialog from '@/components/DefaultConfirmationDialog.vue';
import CourseSemesters from '@/components/CourseSemesters.vue';

export default {
  name: "Course",
  components: {
    DefaultConfirmationDialog,
    CourseSemesters,
  },
  data() {
    return {
      showDeleteDialog: false,
      loading: false,
      course: null,
    };
  },
  methods: {
    async fetchCourse() {
      this.course = null;
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
      this.loading = true;
      try {
        await api.deleteCourse(this.$route.params.coursecode);
        this.$router.push({ name: "Courses" });
        this.$notify({
          title: "Úspěch",
          text: "Předmět byl smazán",
          type: "success",
        });
      } catch (error) {
        this.$notify({
          title: "Error",
          text: "Předmět se nepodařilo smazat",
          type: "error",
        });
      }
      this.loading = false;
    },
    handleSemesterDeleteError() {
      this.$notify({
          title: "Error",
          text: "Semestr se nepodařilo smazat",
          type: "error",
        });
    },
    handleSemesterDeleteSuccess() {
      this.$notify({
          title: "Úspěch",
          text: "Semestr byl smazán",
          type: "success",
        });
      this.fetchCourse();
    }
  },
  computed: {
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
          text: "Management",
          disabled: true,
        },
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