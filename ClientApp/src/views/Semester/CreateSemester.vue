<template>
  <v-container fluid class="">
    <div v-if="this.currentCourse">
      <v-breadcrumbs :items="breadcrumbs" class="pa-0 pb-4 pl-1"></v-breadcrumbs>
      <h1 class="ml-1 mb-6 mt-0">Vytvořit nový semestr</h1>
        <v-text-field
          outlined
          disabled
          :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
          label="Kód předmětu"
          v-model="currentCourse.courseCode"
        >
        </v-text-field>

        <v-text-field
          outlined
          disabled
          :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
          label="Jméno předmetu"
          v-model="currentCourse.courseName"
          class="mb-4"
        >
        </v-text-field>
      <v-form ref="createSemesterForm">

        <v-select
          outlined
          :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
          label="Rok"
          :items="possibleYears"
          v-model="semester.year"
          :rules="[rules.required]"
          class="mb-4"
        >
        </v-select>

        <v-select
          outlined
          :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
          label="Semestr"
          :items="possibleTypes"
          item-text="displayText"
          v-model="semester.semesterType"
          :rules="[rules.required]"
          class="mb-4"
        >
        </v-select>

        <!-- Action buttons -->
        <v-btn
          color="error"
          :disabled="loading"
          outlined
          class="mr-4 mb-2"
          @click="reset"
        >
          Smazat <v-icon right dark> mdi-trash-can-outline </v-icon>
        </v-btn>
        <v-btn
          color="primary"
          :loading="loading"
          depressed
          class="mr-4 mb-2"
          @click="createSemester"
        >
          Vytvořit <v-icon right dark> mdi-plus-circle-outline </v-icon>
        </v-btn>
      </v-form>
    </div>
    <div v-else>
      <not-found></not-found>
    </div>
  </v-container>
</template>

<script>
import api from "api-client";
import NotFound from "@/views/404.vue";

export default {
  components: { NotFound },
  name: "CreateSemester",
  data() {
    return {
      loading: false,
      semester: {
        courseCode: this.$route.params.coursecode,
        year: "",
        semesterType: "",
      },
      possibleTypes: [
        {
          displayText: "Zimní",
          value: "winter",
        },
        {
          displayText: "Letní",
          value: "summer",
        },
      ],
      currentCourse: null,
      rules: {
        required: (value) => !!value || "Povinné.",
      },
    };
  },
  methods: {
    validate() {
      return this.$refs.createSemesterForm.validate();
    },
    reset() {
      this.$refs.createSemesterForm.reset();
    },
    async createSemester() {
      let isFormValid = this.validate();
      if (!isFormValid) return;
      this.loading = true;
      try {
        let response = await api.createSemester(this.semester);
        let id = response.data.id;
        this.reset();
        this.$router.push({ name: "Semester", params: { id: id } });
        this.$notify({
          title: "Úspěch",
          text: "Semestr byl vytvořen",
          type: "success",
        });
      } catch (error) {
        if (error.response.status === 400) {
          this.$notify({
            title: "Error",
            text: "Tento semestr už existuje.",
            type: "error",
          });
        } else {
          this.$notify({
            title: "Error",
            text: "Semestr se nepodařilo vytvořit.",
            type: "error",
          });
        }
      }
      this.loading = false;
      window.scrollTo(0, 0);
    },
  },
  computed: {
    possibleYears() {
      let min = new Date().getFullYear();
      let max = min + 3;
      let years = [];
      for (var i = min; i <= max; i++) {
        years.push(i);
      }
      return years;
    },
    breadcrumbs() {
      let currentCourseCode = this.currentCourse.courseCode;
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
          to: { name: "Courses" },
        },
        {
          text: currentCourseCode,
          disabled: false,
          link: true,
          exact: true,
          to: {
            name: "Course",
            params: {
              id: this.$route.params.id,
            },
          },
        },
        {
          text: "Nový semestr",
          disabled: true,
        },
      ];
    },
  },
  mounted() {
    //check if courses are in store and retrieve them if not
    let courses = this.$store.state.courses;
    if (courses.length === 0) {
      this.$store.dispatch("fetchCourses").then(() => {
        this.currentCourse = this.$store.getters.getCourseByCourseCode(
          this.$route.params.coursecode
        );
      });
    } else {
      this.currentCourse = this.$store.getters.getCourseByCourseCode(
        this.$route.params.coursecode
      );
    }
  },
};
</script>

<style>
</style>