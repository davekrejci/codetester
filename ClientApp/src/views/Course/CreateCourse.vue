<template>
  <v-container fluid class="">
    <v-breadcrumbs :items="breadcrumbs" class="pa-0 pb-4 pl-1"></v-breadcrumbs>
    <h1 class="ml-1 mb-6 mt-0">Vytvořit nový předmět</h1>
    <v-form ref="createCourseForm">
      <v-text-field
        outlined
        :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
        label="Kód předmětu"
        v-model="course.courseCode"
        :rules="[rules.required]"
      >
      </v-text-field>

      <v-text-field
        outlined
        :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
        label="Jméno předmetu"
        v-model="course.courseName"
        :rules="[rules.required]"
        class="mb-4"
      >
      </v-text-field>

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
        @click="createCourse"
      >
        Vytvořit <v-icon right dark> mdi-plus-circle-outline </v-icon>
      </v-btn>
    </v-form>
  </v-container>
</template>

<script>
import api from "api-client";

export default {
  name: "CreateCourse",
  data() {
    return {
      loading: false,
      course: {
        courseCode: "",
        courseName: "",
      },
      rules: {
        required: (value) => !!value || "Povinné.",
      },
      breadcrumbs: [
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
          text: "Nový Předmět",
          disabled: true,
          link: true,
          exact: true,
          to: { name: "CreateCourses" }
        },
      ],
    };
  },
  methods: {
    validate() {
      return this.$refs.createCourseForm.validate();
    },
    reset() {
      this.$refs.createCourseForm.reset();
    },
    async createCourse() {
      let isFormValid = this.validate();
      if (!isFormValid) return;
      this.loading = true;
      try {
        await api.createCourse(this.course);
        this.reset();
        this.$notify({
          title: "Úspěch",
          text: "Předmět byl vytvořen",
          type: "success",
        });
      } catch (error) {
        this.$notify({
          title: "Error",
          text: "Předmět se nepodařilo vytvořit",
          type: "error",
        });
      }
      this.loading = false;
      window.scrollTo(0, 0);
    },
  },
};
</script>

<style>
</style>