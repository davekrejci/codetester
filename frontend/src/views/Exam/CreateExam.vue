<template>
  <v-container class="">
    <v-breadcrumbs :items="breadcrumbs" class="pa-0 pl-1 pb-4"></v-breadcrumbs>
    <h1 class="ml-1 mb-6">Vytvořit nový test</h1>
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
        <v-col  class="py-0">
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
        <v-col v-if="exam.course"  class="py-0">
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

      <v-row>
        <v-col class="py-0">
          <v-datetime-picker
            label="Začátek testu"
            v-model="exam.startTime"
            :textFieldProps="{
              outlined: true,
              rules: [rules.required, rules.minimumStartTime],
            }"
            :datePickerProps="{
              locale: 'cs-CZ',
            }"
            :timePickerProps="{
              format: '24hr',
            }"
          >
            <template slot="dateIcon">
              <v-icon>mdi-calendar</v-icon>
            </template>
            <template slot="timeIcon">
              <v-icon>mdi-clock-outline</v-icon>
            </template>
            <template slot="actions" slot-scope="{ parent }">
              <v-btn
                outlined
                color="error lighten-1"
                @click.native="parent.clearHandler"
                >Smazat</v-btn
              >
              <v-btn color="primary darken-1" @click="parent.okHandler"
                >Ok</v-btn
              >
            </template>
          </v-datetime-picker>
        </v-col>
        <v-col class="py-0">
          <v-datetime-picker
            label="Konec testu"
            v-model="exam.endTime"
            :disabled="!this.exam.startTime"
            :textFieldProps="{
              outlined: true,
              rules: [rules.required, rules.minimumEndTime],
            }"
            :datePickerProps="{
              locale: 'cs-CZ',
            }"
            :timePickerProps="{ format: '24hr' }"
          >
            <template slot="dateIcon">
              <v-icon>mdi-calendar</v-icon>
            </template>
            <template slot="timeIcon">
              <v-icon>mdi-clock-outline</v-icon>
            </template>
            <template slot="actions" slot-scope="{ parent }">
              <v-btn
                outlined
                color="error lighten-1"
                @click.native="parent.clearHandler"
                >Smazat</v-btn
              >
              <v-btn color="primary darken-1" @click="parent.okHandler"
                >Ok</v-btn
              >
            </template>
          </v-datetime-picker>
        </v-col>
      </v-row>

      <!-- Question selector -->
      <exam-questions
        ref="examQuestions"
        v-on:selectedQuestionsChanged="updateSelectedQuestions"
        hasOutline
      ></exam-questions>

      <!-- Tag selector -->
      <v-combobox
        class="mt-8"
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
import ExamQuestions from "@/components/ExamQuestions.vue";

export default {
  components: { DefaultSnackbar, ExamQuestions },
  name: "CreateExam",
  data() {
    return {
      currentDatetime: new Date(),
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
        startTime: null,
        endTime: null,
        questions: [],
      },
      rules: {
        required: (value) => !!value || "Povinné.",
        minimumStartTime: (value) =>
          new Date(value) > this.currentDatetime ||
          "Zvolený čas musí být větší než současný čas",
        minimumEndTime: (value) =>
          new Date(value) > this.exam.startTime ||
          "Konec testu nemůže být dřív než začátek",
      },
      breadcrumbs: [
        {
          text: "Management",
          disabled: true,
        },
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
      this.$refs.examQuestions.selectedQuestions = [];
    },
    // allowedStartDates(val) {
    //   let date = new Date(val).getDate();
    //   let today = new Date().getDate();
    //   return date >= today;
    // },
    // allowedStartHours(val) {
    //   let currentHours = new Date().getHours();
    //   return val >= currentHours;
    // },
    // allowedStartMinutes(val) {
    //   let currentMinute = new Date().getMinutes();
    //   return val >= currentMinute;
    // },
    // allowedEndHours(val) {
    //   let startHour = new Date(this.exam.startTime).getHours();
    //   return val >= startHour;
    // },
    // allowedEndMinutes(val) {
    //   let startMinute = new Date(this.exam.startTime).getMinutes();
    //   return val >= startMinute;
    // },
    // allowedEndDates(val) {
    //   let date = new Date(val).getDate();
    //   let today = new Date().getDate();
    //   return date >= today;
    // },
    getCurrentTime() {
      const today = new Date();
      this.currentDatetime = today;
    },
    updateSelectedQuestions(questions) {
      this.exam.questions = questions;
    },
    async createExam() {
      let isFormValid = this.validate();
      console.log(isFormValid);
      if (!isFormValid) return;
      console.log("got past validity check");
      this.hasSaved = false;
      this.loading = true;
      try {
      console.log("trying api ...");
        let questionIds = this.exam.questions.map(question => question.id);
        let examCreateDto = {
          name: this.exam.name,
          startDate: this.exam.startTime,
          endDate: this.exam.endTime,
          semesterId: this.exam.semesterId,
          questionIds: questionIds,
          tags: this.exam.tags
        }
        console.log(examCreateDto);
        console.log(JSON.stringify(examCreateDto));
        await api.createExam(examCreateDto);
        console.log("api called");
        this.hasSaved = true;
        this.reset();
      } catch (error) {
        console.log(error);
        this.error = error;
      }
      console.log("finalizing ...");
      this.loading = false;
      window.scrollTo(0, 0);
    },
  },
  computed: {
    currentDateTime() {
      return new Date();
    },
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
    // update current time
    setInterval(this.getCurrentTime, 1000);
  },
};
</script>

<style>
</style>