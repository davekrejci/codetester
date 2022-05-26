<template>
  <div>
    <div class="mb-4" v-html="question.questionText"></div>
    <v-card
      class="question-card"
      color="transparent"
      outlined
    >
      <v-list disabled dense class="pa-0" color="transparent">
        <v-list-item-group
          v-model="selectedAnswers"
          multiple
        >
          <template v-for="(answer, i) in question.answers">
            <v-list-item
              :key="`question-${question.id}-answer-${answer.id}`"
              :value="answer.id"
              :input-value="true"
              active-class="primary--text text--accent-4"
            >
              <template v-slot:default="{}">
                <v-list-item-action>
                  <v-icon color="success" v-if="answer.isCorrect && answer.isSelected">mdi-checkbox-outline</v-icon>
                  <v-icon color="warning" v-if="answer.isCorrect && !answer.isSelected">mdi-checkbox-outline</v-icon>
                  <v-icon color="error" v-if="!answer.isCorrect && answer.isSelected">mdi-close-box-outline</v-icon>
                  <v-icon color="grey" v-if="!answer.isCorrect && !answer.isSelected">mdi-close-box-outline</v-icon>
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
    name: "MultiChoiceQuestionResult",
    props: {
        question: {
            type: Object,
            required: true,
        },
    },
    data() {
      return {
        
      }
    },
    methods: {
    
    },
    computed: {
      selectedAnswers() {
          return this.question.answers
                .map((answer) => answer.isSelected == true ? answer.id : '')
                .filter(String)
      }
    }
};
</script>

<style>
.v-application .transparent.question-card {
  border-color: rgba(0, 0, 0, 0.12) !important;
}
</style>