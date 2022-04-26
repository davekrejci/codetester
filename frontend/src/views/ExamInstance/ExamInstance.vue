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
                v-for="(question, index) in questions"
                :key="question.id"
                class="short"
                label
                :color="question.isAnswered ? 'primary' : ''"
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

      <v-list-item>
        <v-chip label color="primary" small></v-chip>
        <v-list-item-content>
          <v-list-item-subtitle class="ml-2">
            Zodpovězené
          </v-list-item-subtitle>
        </v-list-item-content>
      </v-list-item>
      <v-list-item>
        <v-chip label small></v-chip>
        <v-list-item-content>
          <v-list-item-subtitle class="ml-2">
            Nezodpovězené
          </v-list-item-subtitle>
        </v-list-item-content>
      </v-list-item>
    </v-navigation-drawer>
    <v-app-bar dense app flat clipped-left>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
      <v-spacer></v-spacer>
      <time-remaining-tracker
        class="mx-auto"
        :end="examInfo.endDate"
        @duration-over="turnInExam()"
      ></time-remaining-tracker>
      <v-spacer></v-spacer>
      <v-btn @click="showTurnInConfirmationDialog = true" small color="primary">Odevzdat</v-btn>
    </v-app-bar>
    <v-main>
      <v-container fluid class="pa-0">
        <template v-for="(question, index) in questions">
          <v-card
            :key="`question-${question.id}`"
            :id="`question-${question.id}`"
            class="ma-0 pa-4"
            color="transparent"
            flat
          >
            <v-list-item three-line>
              <v-list-item-content>
                <div class="primary--text text-overline mb-4">
                  Otázka {{ index + 1 }}
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
        <v-card class="my-2 pa-6" color="transparent" flat>
          <v-card-actions>
            <v-btn
              color="primary"
              class="px-4"
              @click="showTurnInConfirmationDialog = true"
              >Odevzdat</v-btn
            >
          </v-card-actions>
        </v-card>
      </v-container>

      <!-- Turn In Dialog -->
      <default-confirmation-dialog
        color="primary"
        icon="mdi-check-circle-outline"
        confirmationButtonText="Odevzdat"
        :show="showTurnInConfirmationDialog"
        :confirmAction="turnInExam"
        @close-dialog="showTurnInConfirmationDialog = false"
      >
        <template v-slot:title> Odevzdat test? </template>
        <template v-slot:text>
          <p class="">
            Opravdu si přejete odevzdat test? Odpovědi již nebude možné změnit
            po odevzdání.
          </p>
          <div class="text-justify mb-2">
            <v-alert
              v-if="hasIncompleteQuestions"
              dense
              text
              border="left"
              type="warning"
              color="orange"
              class="mb-0"
            >
              Některé otázky nejsou zodpovězené.
            </v-alert>
          </div>
        </template>
      </default-confirmation-dialog>
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
import TimeRemainingTracker from "@/components/ExamInstance/TimeRemainingTracker.vue";
import MultiChoiceQuestionInstance from "@/components/ExamInstance/QuestionInstance/MultiChoiceQuestionInstance.vue";
import FillInCodeQuestionInstance from "@/components/ExamInstance/QuestionInstance/FillInCodeQuestionInstance.vue";
import { QuestionTypes } from "@/util/questionTypes.js";
import DefaultConfirmationDialog from "../../components/DefaultConfirmationDialog.vue";
export default {
  name: "ExamInstance",
  components: {
    MultiChoiceQuestionInstance,
    FillInCodeQuestionInstance,
    TimeRemainingTracker,
    DefaultConfirmationDialog,
  },
  data() {
    return {
      drawer: true,
      error: null,
      loading: false,
      showTurnInConfirmationDialog: false,
      warnBeforeReroute: true
    };
  },
  methods: {
    async turnInExam() {
      try{
        await this.$store.dispatch("examInstance/turnInExamInstance");
        this.warnBeforeReroute = false;
        this.$router.push({ name: 'ExamResult', params: { id: this.$route.params.id }});
      } catch (error) {
        this.error = error;
        console.log(error);
      }
      this.showTurnInConfirmationDialog = false;
    },
    // get a question type and return the component to render
    parseType(questionType) {
      switch (questionType) {
        case QuestionTypes.MultiChoice:
          return "MultiChoiceQuestionInstance";
        case QuestionTypes.FillInCode:
          return "FillInCodeQuestionInstance";
      }
    },
    async fetchExamInstance() {
      this.error = this.exam = null;
      this.loading = true;
      try {
        // do not request again if data is already in store
        // if(this.$store.state.examInstance.id != null) {
        //   return
        // }
        // otherwise request data from API
        await this.$store.dispatch("examInstance/fetchExamInstance", {
          id: this.$route.params.id,
        });
      } catch (error) {
        console.log(error);
        this.error = error;
        if (error.response.status === 400) {
          this.warnBeforeReroute = false;
            this.$router.push({ name: 'ExamResult', params: { id: this.$route.params.id }});
        }
      }
      this.loading = false;
    },
    preventNav(event) {
      if(this.warnBeforeReroute) {
        event.preventDefault();
        event.returnValue = "";
      }
    },
  },
  computed: {
    hasData() {
      return this.$store.state.examInstance.isLoaded;
    },
    questions() {
      return this.$store.state.examInstance.questions;
    },
    hasIncompleteQuestions() {
      return this.questions.some((question) => question.isAnswered === false);
    },
    examInfo() {
      return this.$store.state.examInstance.examInfo;
    },
  },
  created() {
    this.fetchExamInstance();
  },
  beforeMount() {
    window.addEventListener("beforeunload", this.preventNav);
  },
  beforeDestroy() {
    window.removeEventListener("beforeunload", this.preventNav);
  },
  beforeRouteLeave(to, from, next) {
    if(this.warnBeforeReroute) {
      if (!window.confirm("Odejít bez uložení?")) {
        return;
      }
    }
    next();
  },
};
</script>

<style>
.short {
  min-width: 40px;
  align-items: center;
  justify-content: center;
}
.v-chip.v-size--default.short {
  height: 35px;
}
</style>