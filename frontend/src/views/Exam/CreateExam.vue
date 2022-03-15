<template>
  <v-container class="px-12">
    <v-breadcrumbs :items="breadcrumbs"></v-breadcrumbs>
    <h1 class="mb-8">Vytvořit nový test</h1>
    <v-form ref="createExamForm">
      <v-text-field
        outlined
        :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
        label="Název testu"
        v-model="exam.name"
        :rules="[rules.required]"
      >
      </v-text-field>

      <v-row>
        <!-- Course selector -->
        <v-col>
          <v-autocomplete
            v-model="exam.course"
            :items="courses"
            item-text="courseCode"
            return-object
            :search-input.sync="searchCourses"
            hide-selected
            label="Předmět"
            persistent-hint
            outlined
            :rules="[rules.required]"
          >
            <template v-slot:no-data>
              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title>
                    Žádné výsledky pro "<strong>{{ searchCourses }}</strong
                    >".
                  </v-list-item-title>
                </v-list-item-content>
              </v-list-item>
            </template>
          </v-autocomplete>
        </v-col>
        <!-- Semester selector -->
        <v-col v-if="exam.course"> 
          <v-autocomplete
            v-model="exam.semesterId"
            :items="exam.course.semesters"
            item-text="`${data.item.year} - ${data.item.semesterType}`"
            item-value="id"
            :search-input.sync="searchSemesters"
            hide-selected
            label="Semestr"
            persistent-hint
            outlined
            :rules="[rules.required]"
          >
          <template slot="item" slot-scope="data">
            {{ data.item.year }} - {{ data.item.semesterType }}
          </template>
          <template slot="selection" slot-scope="data">
            {{ data.item.year }} - {{ data.item.semesterType }}
          </template>
            <template v-slot:no-data>
              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title>
                    Žádné výsledky pro "<strong>{{ searchSemesters }}</strong
                    >".
                  </v-list-item-title>
                </v-list-item-content>
              </v-list-item>
            </template>
          </v-autocomplete>
        </v-col>
      </v-row>

      <!-- Tag selector -->
      <v-combobox
        v-model="exam.tags"
        :items="tags"
        item-text="tagText"
        item-value="tagText"
        :search-input.sync="searchTags"
        hide-selected
        label="Štítky"
        multiple
        persistent-hint
        small-chips
        outlined
        deletable-chips
        clearable
      >
        <template v-slot:no-data>
          <v-list-item>
            <v-list-item-content>
              <v-list-item-title>
                Žádné výsledky pro "<strong>{{ searchTags }}</strong
                >". Stiskněte <kbd>enter</kbd> pro vytvoření nového štítku
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </template>
      </v-combobox>

      <!-- Action buttons -->
      <div class="mt-8">
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
          @click="createExam"
        >
          Vytvořit <v-icon right dark> mdi-plus-circle-outline </v-icon>
        </v-btn>
      </div>
    </v-form>
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
  name: "CreateExam",
  data() {
    return {
      searchTags: "",
      searchCourses: "",
      searchSemesters: "",
      error: null,
      hasSaved: false,
      loading: false,
      exam: {
        name: "",
        semesterId: null,
        tags: [],
      },
      rules: {
        required: (value) => !!value || "Povinné.",
      },
      breadcrumbs: [
        {
          text: "Testy",
          disabled: false,
          link: true,
          exact: true,
          to: { name: "Exams" },
        },
        {
          text: "Nový Test",
          disabled: true,
        },
      ],
    };
  },
  methods: {
    validate() {
      return this.$refs.createExamForm.validate();
    },
    reset() {
      this.$refs.createExamForm.reset();
    },
    async createExam() {
      let isFormValid = this.validate();
      if (!isFormValid) return;
      this.hasSaved = false;
      this.loading = true;
      try {
        await api.createExam(this.exam);
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
          show: true,
        };
      }
      if (this.hasSaved) {
        return {
          type: "success",
          text: "Test byl vytvořen",
          show: true,
        };
      }
      return {
        show: false,
      };
    },

    courses() {
      return this.$store.state.courses;
    },
    tags() {
      return this.$store.state.tags;
    },
    selectedTags() {
      return this.exam.tags;
    },
  },
  watch: {
    // Map newly entered tag string to tag object
    selectedTags(val, prev) {
      if (val.length === prev.length) return;
      this.exam.tags = val.map((v) => {
        if (typeof v === "string") {
          v = {
            tagText: v,
          };
          this.exam.tags.push(v);
        }

        return v;
      });
    },
  },
  created() {
    // if no tags in store, fetch tags
    if (this.$store.state.tags.length == 0) {
      this.$store.dispatch("fetchTags");
    }
    // if no courses in store, fetch courses
    if (this.$store.state.courses.length == 0) {
      this.$store.dispatch("fetchCourses");
    }
  },
};
</script>

<style>
</style>