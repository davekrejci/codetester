<template>
  <div v-if="hasData">
    <v-navigation-drawer class="pl-4" v-model="drawer" app clipped floating>
      <v-list-item class="px-2">
        <v-img
          src="@/assets/svg/logo/symbol_logo.svg"
          max-height="25"
          max-width="60"
          contain
        ></v-img>
        <v-img
          src="@/assets/svg/logo/text_logo.svg"
          max-height="100"
          max-width="150"
          contain
        ></v-img>
      </v-list-item>
      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="text-h6"> Otázky </v-list-item-title>
          <v-card flat color="transparent" justify="center" align="center">
            <v-chip-group active-class="" column>
              <v-chip
                v-for="(question, index) in examResult.questionInstances"
                :key="question.id"
                class="short font-weight-bold"
                label
                :color="getChipColor(question.state)"
                @click="$vuetify.goTo(`#question-${question.id}`)"
              >
                <span class="text-center">
                  {{ index + 1 }}
                </span>
              </v-chip>
            </v-chip-group>
          </v-card>
        </v-list-item-content>
      </v-list-item>
    </v-navigation-drawer>
    <v-app-bar dense app flat clipped-left>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
      <v-spacer></v-spacer>
      <v-chip outlined color="primary" class="font-weight-medium px-4">
        Výsledek: {{ percentageScore }}
      </v-chip>
      <v-spacer></v-spacer>
      <router-link to="/">
        <v-btn small color="primary">Zpět</v-btn>
      </router-link>
    </v-app-bar>
    <v-main>
      <v-container fluid class="pa-0">
        <v-card
            id="summary"
            class="ma-0 pa-4"
            color="transparent"
            flat
          >
            <v-list-item three-line>
              <v-list-item-content>
                  <h2>
                    {{ examResult.exam.semester.course.courseCode }} - {{ examResult.exam.name }}
                  </h2>
                  <!-- <p class="mb-0 caption"><v-chip small class=" my-1 font-weight-bold overline">Předmět</v-chip> : {{ examResult.exam.semester.course.courseCode }}</p>
                  <p class="my-0 caption"><v-chip small class=" my-1 font-weight-bold overline">Test</v-chip> : {{ examResult.exam.name }}</p>
                  <p class="my-0 caption"><v-chip small class=" my-1 font-weight-bold overline">Body</v-chip> : {{ examResult.score}} z {{ examResult.maxScore }}</p>
                  <p class="my-0 caption"><v-chip small class=" my-1 font-weight-bold overline">Výsledek</v-chip> : {{ percentageScore }}</p> -->
                  <v-simple-table class="mt-4 result-table">
                    <template v-slot:default>
                      <tbody>
                        <tr>
                          <th class="result-table-header font-weight-black">Body:</th>
                          <td>{{ examResult.score}} z {{ examResult.maxScore }}</td>
                        </tr>
                        <tr>
                          <th class="result-table-header font-weight-black">Výsledek:</th>
                          <td>{{ percentageScore }}</td>
                        </tr>
                      </tbody>
                    </template>
                  </v-simple-table>
                
              </v-list-item-content>
            </v-list-item>
          </v-card>
          <v-divider
            class="primary--text"
          ></v-divider>
        <template v-for="(question, index) in examResult.questionInstances">
          <v-card
            :key="`question-${question.id}`"
            :id="`question-${question.id}`"
            class="ma-0 pa-4"
            color="transparent"
            flat
          >
            <v-list-item three-line>
              <v-list-item-content>
                <div class="d-flex align-center mb-3">
                  <span class=" font-weight-bold mb-0">
                    Otázka {{ index + 1 }}
                  </span>
                </div>
                <div class="mb-6">
                  <v-chip :color="getChipColor(question.state)" small class="font-weight-bold">
                    {{getQuestionStateDescription(question.state)}}
                  </v-chip>
                  <v-chip small class="ml-1 font-weight-bold">
                    Body: {{ question.score }} z {{ question.maxScore }}
                  </v-chip>

                </div>
                <component
                  :is="parseType(question.questionType)"
                  :question="question"
                ></component>
              </v-list-item-content>
            </v-list-item>
          </v-card>
          <v-divider
            class="primary--text"
            :key="`question-divider-${index}`"
          ></v-divider>
        </template>
      </v-container>
    </v-main>
  </div>
  <div v-else-if="!hasData && loading">Načítání...</div>
  <div v-else>
    <v-card class="mx-auto mt-12 pa-4" width="500px">
      <p>
        Nastala chyba pri pokusu o načtení dat pro test. <br />
        Kontaktujte prosim administratora.
      </p>
    </v-card>
  </div>
</template>

<script>
import api from "api-client";
import { QuestionTypes } from "@/util/questionTypes.js";
import MultiChoiceQuestionResult from "@/components/ExamResult/QuestionResult/MultiChoiceQuestionResult.vue";
import FillInCodeQuestionResult from "@/components/ExamResult/QuestionResult/FillInCodeQuestionResult.vue";

export default {
    name: "ExamResult",
  components: {
    MultiChoiceQuestionResult,
    FillInCodeQuestionResult,
  },
  data() {
    return {
      drawer: true,
      error: null,
      loading: false,
      hasData: false,
      examResult: null
    };
  },
  methods: {
    // get a question type and return the component to render
    parseType(questionType) {
      switch (questionType) {
        case QuestionTypes.MultiChoice:
          return "MultiChoiceQuestionResult";
        case QuestionTypes.FillInCode:
          return "FillInCodeQuestionResult";
      }
    },
    getChipColor(state) {
      switch (state) {
        case "correct":
          return "success";
        case "incorrect":
          return "error";
        case "partiallycorrect":
          return "warning"
        default:
          return ""
      }
    },
    getQuestionStateDescription(state) {
      switch (state) {
        case "correct":
          return "Správně";
        case "incorrect":
          return "Špatně";
        case "partiallycorrect":
          return "Částečne správně"
        default:
          return ""
      }
    },
    async fetchExamResult() {
      this.error = this.exam = null;
      this.loading = true;
      try {
        // fetch
        this.examResult = await api.fetchExamResult(this.$route.params.id);
        if(this.examResult != null) {
          this.hasData = true;
        }
      } catch (error) {
        this.$notify({
          title: "Error",
          text: "Načtení testu se nepodařilo",
          type: "error",
        });
      }
      this.loading = false;
    },
    
  },
  computed: {
    percentageScore() {
      return ((this.examResult.score / this.examResult.maxScore) * 100).toFixed(2) + "%"
    }
  },
  created() {
    this.fetchExamResult();
  },
}
</script>
<style scoped>
.result-table {
  max-width: 300px;
  border: 2px solid rgba(0, 0, 0, 0.12);
}
.result-table-header {
  max-width: 40px;
  background-color: "";
  font-size: 14px !important;
}

</style>