<template>
  <v-container fluid class="">
    <div v-if="exam != null">
      <v-breadcrumbs :items="breadcrumbs" class="pa-0 pb-4 pl-1" ></v-breadcrumbs>
      <h1 class="ml-1 mb-6">{{ this.exam.name }}</h1>
      <v-form ref="createExamForm">
        <v-text-field
          outlined
          disabled
          :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
          label="Název testu"
          v-model="exam.name"
          :rules="[rules.required]"
        >
        </v-text-field>

        <v-row>
          <!-- Course selector -->
          <v-col class="py-0">
            <v-text-field
              v-model="exam.semester.course.courseCode"
              disabled
              label="Předmět"
              outlined
            >
            </v-text-field>
          </v-col>
          <!-- Semester selector -->
          <v-col class="py-0">
            <v-text-field
              :value="
                exam.semester.year +
                ' - ' +
                exam.semester.semesterType.displayText
              "
              label="Semestr"
              disabled
              outlined
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-text-field
          outlined
          disabled
          :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
          label="Status"
          v-model="exam.status"
        >
          <template v-slot:prepend-inner>
            <div>
              <v-avatar
                left
                size="10"
                class="mr-1"
                :color="statusIndicatorColor(exam.status)"
              >
              </v-avatar>
            </div>
          </template>
        </v-text-field>

        <v-row>
          <v-col class="py-0">
            <v-datetime-picker
              label="Začátek testu"
              :disabled="editingDisabled"
              v-model="exam.startDate"
              :textFieldProps="{
                outlined: true,
				rules: editingDisabled ? [] : [rules.required, rules.minimumStartTime]
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
              v-model="exam.endDate"
              :disabled="editingDisabled"
              :textFieldProps="{
                outlined: true,
				rules: editingDisabled ? [] : [rules.required, rules.minimumEndTime]
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

        <v-card outlined class="rounded mb-1">
          <v-tabs v-model="tab" center-active>
            <v-tab>Otázky</v-tab>
            <v-tab>Výsledky</v-tab>
          </v-tabs>
        </v-card>
        <v-card outlined class="ma-0 rounded">
          <v-tabs-items v-model="tab">
            <v-tab-item> 
              <!-- Question selector -->
              <exam-questions
                ref="examQuestions"
                v-on:selectedQuestionsChanged="updateSelectedQuestions"
                :currentQuestions="exam.questions"
                :editingDisabled="editingDisabled"
              ></exam-questions>
            </v-tab-item>
            <v-tab-item>
              <exam-results
                :examInstances="exam.examInstances"
              ></exam-results>
            </v-tab-item>
          </v-tabs-items>
        </v-card>

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
              @click="showDeleteDialog = true"
              :loading="loading"
              :disabled="loading"
              color="error"
              outlined
              class="mr-4 mb-2"
            >
              Smazat <v-icon right dark> mdi-trash-can-outline </v-icon>
            </v-btn>
          
          <!-- <v-card class="text-center pa-4">
            <v-icon color="error" x-large>mdi-alert-circle-outline</v-icon>
            <v-card-title class="text-h5">
            </v-card-title>
            <v-card-text
              >Opravdu si přejete test smazat? Tato akce je
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
              <v-btn color="error" class="mx-2" outlined @click="deleteExam"> Ano </v-btn>
              <v-spacer></v-spacer>
            </v-card-actions>
          </v-card> -->
          <v-btn
            color="primary"
            :loading="loading"
            :disabled="editingDisabled"
            depressed
            class="mr-4 mb-2"
            @click="updateExam"
          >
            Uložit <v-icon right dark> mdi-plus-circle-outline </v-icon>
          </v-btn>
        </div>
      </v-form>
    </div>

    <default-confirmation-dialog
      color="error"
      icon="mdi-alert-circle-outline"
      confirmationButtonText="Smazat"
      :show="showDeleteDialog"
      :confirmAction="deleteExam"
      @close-dialog="showDeleteDialog = false"
    >
      <template v-slot:title>
        Smazat test?
      </template>
      <template v-slot:text>
        Opravdu si přejete test smazat? Tato akce je nevratná.
      </template>
    </default-confirmation-dialog>

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
import ExamResults from "@/components/ExamResults.vue";
import DefaultConfirmationDialog from '@/components/DefaultConfirmationDialog.vue';
import moment from "moment";

moment.locale("cs");

export default {
  components: { DefaultSnackbar, DefaultConfirmationDialog, ExamQuestions, ExamResults },
  name: "Exam",
  data() {
    return {
      tab: null,
      currentDatetime: new Date(),
      searchTags: "",
      searchCourses: "",
      searchSemesters: "",
      error: null,
      hasSaved: false,
      showDeleteDialog: false,
      loading: false,
      exam: null,
      rules: {
        required: (value) => !!value || "Povinné.",
        minimumStartTime: (value) =>
          new Date(value) > this.currentDatetime ||
          "Zvolený čas musí být větší než současný čas",
        minimumEndTime: (value) =>
          new Date(value) > this.exam.startDate ||
          "Konec testu nemůže být dřív než začátek",
      },
    };
  },
  methods: {
    validate() {
      return this.$refs.createExamForm.validate();
    },
    statusIndicatorColor(status) {
      switch (status) {
        case "planned":
          return "primary";
        case "open":
          return "warning";
        case "closed":
          return "success";
        default:
          return "grey";
      }
    },
    getCurrentTime() {
      const today = new Date();
      this.currentDatetime = today;
    },
    updateSelectedQuestions(questions) {
      this.exam.questions = questions;
    },
    alterDateTimeFormat(datetime) {
      return moment.utc(datetime).toDate();
    },
    async fetchExam() {
      this.error = this.exam = null;
      this.loading = true;
      try {
        // load from store if data is already there
        this.exam = this.$store.getters.getExamById(this.$route.params.id);
        if (this.exam) {
          console.log("retrieved data from store");
          this.loading = false;
        } else {
          // fetch exams from api otherwise and then retrieve from store
          console.log(
            "fetching exams from api and retrieving from store now ..."
          );
          await this.$store.dispatch("fetchExams");
          this.exam = this.$store.getters.getExamById(this.$route.params.id);
        }
      } catch (error) {
        console.log(error);
        this.error = error;
      }
      this.exam.startDate = this.alterDateTimeFormat(this.exam.startDate);
      this.exam.endDate = this.alterDateTimeFormat(this.exam.endDate);

      this.loading = false;
    },
    async deleteExam() {
      this.error = null;
      this.loading = true;
      try {
        await api.deleteExam(this.$route.params.id);
        this.$router.push({
          name: "Exams",
        });
      } catch (error) {
        this.error = error;
      }
      this.loading = false;
    },
    async updateExam() {
      let isFormValid = this.validate();
      console.log(isFormValid);
      if (!isFormValid) return;
      console.log("got past validity check");
      this.hasSaved = false;
      this.loading = true;
      try {
        console.log("trying api ...");
        let questionIds = this.exam.questions.map((question) => question.id);
        let examUpdateDto = {
          name: this.exam.name,
          startDate: this.exam.startDate,
          endDate: this.exam.endDate,
          semesterId: this.exam.semester.id,
          questionIds: questionIds,
          tags: this.exam.tags,
        };
        console.log(examUpdateDto);
        console.log(JSON.stringify(examUpdateDto));
        await api.updateExam(this.$route.params.id, examUpdateDto);
        console.log("api called");
        this.hasSaved = true;
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
    status() {
      if (this.exam.startDate < Date.now()) return "naplanovany";
      if (this.exam.startDate > Date.now() && this.exam.endDate > Date.now())
        return "probihajici";
      return "hotovy";
    },
    editingDisabled() {
      return this.exam.status != "planned";
    },
    currentDateTime() {
      return new Date();
    },
    breadcrumbs() {
      return [
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
          text: this.exam.name,
          disabled: true,
        },
      ];
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
          text: "Test byl uložen",
          show: true,
        };
      }
      return {
        show: false,
      };
    },
    tags() {
      return this.$store.state.tags;
    },
    selectedTags() {
      if (this.exam != null) {
        return this.exam.tags;
      }
      return null;
    },
  },
  created() {
    this.fetchExam().then(() => {
      this.$watch("exam.tags", (val, prev) => {
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
      });
    });
    // if no tags in store, fetch tags
    if (this.$store.state.tags.length == 0) {
      this.$store.dispatch("fetchTags");
    }
    // update current time
    setInterval(this.getCurrentTime, 1000);
  },
};
</script>

<style>
.noBottomBorder {
  border-bottom: none !important;
}
</style>