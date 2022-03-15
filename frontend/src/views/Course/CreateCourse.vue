<template>
  <v-container class="px-12">
    <v-breadcrumbs :items="breadcrumbs"></v-breadcrumbs>
    <h1 class="mb-8">Vytvořit nový předmět</h1>
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
    <default-snackbar :type="snackbar.type" :text="snackbar.text" v-on:close-snackbar="error = null"></default-snackbar>
  </v-container>
</template>

<script>
import api from "api-client";
import DefaultSnackbar from '@/components/DefaultSnackbar.vue';

export default {
  components: { DefaultSnackbar },
  name: "CreateCourse",
  data() {
    return {
      error: null,
      hasSaved: false,
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
      this.hasSaved = false;
      this.loading = true;
      try {
        await api.createCourse(this.course);
        this.hasSaved = true;
        this.reset();
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
          text: this.error.toString(),
          show: true
        };
      }
      if (this.hasSaved) {
        return {
          type: 'success',
          text: "Předmět byl vytvořen",
          show: true
        };
      }
      return {
        show: false
      }
    },
  }
};
</script>

<style>
</style>