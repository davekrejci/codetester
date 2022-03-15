<template>
  <div>
    <v-navigation-drawer class="pl-4" v-model="drawer" app clipped floating>
      <v-list-item class="px-2">
        <v-img
          src="../assets/svg/logo/symbol_logo.svg"
          max-height="25"
          max-width="60"
          contain
        ></v-img>
        <v-img
          src="../assets/svg/logo/text_logo.svg"
          max-height="100"
          max-width="150"
          contain
        ></v-img>
      </v-list-item>
      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="text-h6"> Otázky </v-list-item-title>
          <v-card flat color="transparent" justify="center" align="center">
            <v-chip-group mandatory active-class="" column>
              <v-chip
                v-for="question in questions"
                :key="question.number"
                class="short"
                label
                @click="$vuetify.goTo(`#question-${question.number}`)"
                :color="question.answered ? 'primary' : ''"
              >
                <span class="text-center">
                  {{ question.number }}
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
      <time-remaining-tracker class="mx-auto"></time-remaining-tracker>
      <v-spacer></v-spacer>
      <v-btn flat small primary>Odevzdat</v-btn>
    </v-app-bar>
    <v-main>
      <v-container fluid class="pa-0">
        <template v-for="question in questions">
          <v-card
            :key="`question-${question.number}`"
            :id="`question-${question.number}`"
            class="ma-0 pa-4"
            color="transparent"
            flat
          >
            <v-list-item three-line>
              <v-list-item-content>
                <div class="primary--text text-overline mb-4">
                  Otázka {{ question.number }}
                </div>
                <v-list-item-title class="mb-4">
                  {{ question.question }}
                </v-list-item-title>
                <fill-blank-code-editor
                  v-if="question.type == 'fill-in-code'"
                  :content="question.answers"
                ></fill-blank-code-editor>
                <v-card
                  class="question-card"
                  v-if="question.type == 'multi-choice'"
                  color="transparent"
                  outlined
                >
                  <v-list dense class="pa-0" color="transparent">
                    <v-list-item-group multiple @change="checkQuestionIsDone($event, question.number)">
                      <template v-for="(answer, i) in question.answers">
                        <v-list-item
                          :key="`question-${question.number}-answer-${answer.id}`"
                          :value="answer.id"
                          active-class="primary--text text--accent-4"
                        >
                          <template v-slot:default="{ active }">
                            <v-list-item-action>
                              <v-checkbox
                                :input-value="active"
                                color="primary"
                              ></v-checkbox>
                            </v-list-item-action>
                            <v-list-item-content>
                              <v-list-item-title
                                v-text="answer.text"
                              ></v-list-item-title>
                            </v-list-item-content>
                          </template>
                        </v-list-item>
                        <v-divider
                          v-if="i < question.answers.length - 1"
                          :key="`question-${question.number}-answer-divider-${i}`"
                        ></v-divider>
                      </template>
                    </v-list-item-group>
                  </v-list>
                </v-card>
              </v-list-item-content>
            </v-list-item>
          </v-card>
          <v-divider
            class="primary--text"
            :key="`question-divider-${question.number}`"
          ></v-divider>
        </template>
        <v-card class="my-2 pa-6" color="transparent" flat>
          <v-card-actions>
            <v-btn x-large text class="primary px-4">Odevzdat</v-btn>
          </v-card-actions>
        </v-card>
      </v-container>
    </v-main>
  </div>
</template>

<script>
import FillBlankCodeEditor from "@/components/FillBlankCodeEditor.vue";
import TimeRemainingTracker from "@/components/TimeRemainingTracker.vue";
export default {
  name: "ExamInstance",
  components: {
    FillBlankCodeEditor,
    TimeRemainingTracker,
  },
  methods: {
    checkQuestionIsDone(answers, questionNumber){
      if(answers.length > 0){
        this.questions[--questionNumber].answered = true;
      } else {
        this.questions[--questionNumber].answered = false;
      }
    }
  },
  data() {
    return {
      drawer: true,
      questions: [
        {
          number: 1,
          type: "multi-choice",
          question: "Co je to třída?",
          answers: [
            {
              id: "9821390",
              text: "Předpis, který definuje strukturu objektu.",
            },
            {
              id: "2981312",
              text: "Skupina, která obsahuje různorodé typy objektů.",
            },
            { id: "87126312", text: "Reálná věc." },
            {
              id: "982173721",
              text: "Reprezentace konkrétní věci reálného světa.",
            },
          ],
          answered: false
        },
        {
          number: 2,
          type: "multi-choice",
          question: "Kdy je vykonána první část hlavičky cyklu for?",
          answers: [
            { id: "9221390", text: "Při vstupu do cyklu." },
            { id: "2991312", text: "Před každým průběhem cyklu." },
            { id: "87126352", text: "Pouze po prvním průběhu cyklu." },
            { id: "982173021", text: "Po každém průběhu cyklu." },
            { id: "982143821", text: "Pouze po posledním průběhu cyklu." },
          ],
          answered: false
        },
        {
          number: 3,
          type: "fill-in-code",
          question: "Doplnte kod",
          answers: `public boolean addPerson(Person personToAdd){\n\tif(personToAdd == null) return false;\n\tfor (int i = 0; i < persons.length; i++){\n\t\tif({"widget_id":"98231", "length":16}){\n\t\tpersons[i] = personToAdd;\n\t\treturn true;\n\t\t}\n\t}\n\treturn false;\n}`,
          answered: false
        },
        { number: 4, type: "", question: "", answers: "question 4 Content" },
        { number: 5, type: "", question: "", answers: "question 5 Content" },
        { number: 6, type: "", question: "", answers: "question 6 Content" },
        { number: 7, type: "", question: "", answers: "question 7 Content" },
        { number: 8, type: "", question: "", answers: "question 8 Content" },
        { number: 9, type: "", question: "", answers: "question 9 Content" },
        { number: 10, type: "", question: "", answers: "question 10 Content" },
        // { number: 11, type: "", question: "", answers: "question 10 Content" },
        // { number: 12, type: "", question: "", answers: "question 10 Content" },
        // { number: 13, type: "", question: "", answers: "question 10 Content" },
        // { number: 14, type: "", question: "", answers: "question 10 Content" },
        // { number: 15, type: "", question: "", answers: "question 10 Content" },
        // { number: 16, type: "", question: "", answers: "question 10 Content" },
        // { number: 17, type: "", question: "", answers: "question 10 Content" },
        // { number: 18, type: "", question: "", answers: "question 10 Content" },
        // { number: 19, type: "", question: "", answers: "question 10 Content" },
        // { number: 20, type: "", question: "", answers: "question 10 Content" },
        // { number: 21, type: "", question: "", answers: "question 10 Content" },
        // { number: 22, type: "", question: "", answers: "question 10 Content" },
        // { number: 23, type: "", question: "", answers: "question 10 Content" },
        // { number: 24, type: "", question: "", answers: "question 10 Content" },
        // { number: 25, type: "", question: "", answers: "question 10 Content" },
        // { number: 26, type: "", question: "", answers: "question 10 Content" },
        // { number: 27, type: "", question: "", answers: "question 10 Content" },
        // { number: 28, type: "", question: "", answers: "question 10 Content" },
        // { number: 29, type: "", question: "", answers: "question 10 Content" },
        // { number: 30, type: "", question: "", answers: "question 10 Content" },
      ],
    };
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
.v-application .transparent.question-card {
  border-color: rgba(0, 0, 0, 0.12) !important;
}
</style>