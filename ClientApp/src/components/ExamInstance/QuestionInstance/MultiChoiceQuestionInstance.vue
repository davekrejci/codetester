<template>
  <div>
    <div class="mb-8" v-html="question.questionText"></div>
    <v-card
      class="question-card"
      color="transparent"
      outlined
    >
      <v-list dense class="pa-0" color="transparent">
        <v-list-item-group
          v-model="selectedAnswers"
          multiple
        >
          <template v-for="(answer, i) in question.answers">
            <v-list-item
              :key="`question-${question.id}-answer-${answer.id}`"
              :value="answer.id"
              @change="setAnswer(question, answer)"
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
                  <v-list-item-title v-text="answer.answerText"></v-list-item-title>
                </v-list-item-content>
              </template>
            </v-list-item>
            <v-divider
              v-if="i < question.answers.length - 1"
              :key="`question-${question.id}-answer-divider-${i}`"
            ></v-divider>
          </template>
        </v-list-item-group>
      </v-list>
    </v-card>
  </div>
</template>

<script>
export default {
    name: "MultiChoiceQuestionInstance",
    props: {
        question: {
            type: Object,
            required: true,
        },
    },
    data() {
      return {
        selectedAnswers: [],
      }
    },
    methods: {
      setAnswer(question, answer) {
        // this.$store.commit('examInstance/setAnswer',
        //   { 
        //     id: this.question.id, 
        //     isAnswered: this.isAnswered, 
        //     answer:{ selectedAnswers: this.selectedAnswers 
        //   } 
        // })

        // TODO: change isSelected on answer to true/false instead
        answer.isSelected = !answer.isSelected;
        question.isAnswered = this.isAnswered;
      }
    },
    computed: {
      isAnswered() {
        //return this.selectedAnswers.length > 0
        return this.question.answers.some(answer => answer.isSelected == true);
      }
    }
};
</script>

<style>
.v-application .transparent.question-card {
  border-color: rgba(0, 0, 0, 0.12) !important;
}
</style>