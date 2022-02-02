import { createHelpers } from 'vuex-map-fields';

const { getField, updateField } = createHelpers({
  getterType: 'getField',
  mutationType: 'updateField',
});

export default {
  namespaced: true,
  state: {
    selectedQuestionType: "",
    multiChoice: {
      questionText: "",
      answers: [{
          text: "",
          isCorrect: true,
        },
        {
          text: "",
          isCorrect: false,
        },
        {
          text: "",
          isCorrect: false,
        },
        {
          text: "",
          isCorrect: false,
        },
      ],
    },
    fillInCode: {
      codeDescription: "",
      content: `public class HelloWorld{ \n\tpublic static void main(String[] args){\n\t\tSystem.out.println("Hello, World!"); \n\t}\n}`,
      fillInCount: 1,
      widgets: [],
      cm: {}
    },
  },
  mutations: {
    updateField,
    setMultiChoiceAnswerText(state, payload) {
      state.multiChoice.answers[payload.index].text = payload.value;
    },
    setMultiChoiceQuestionText(state, questionText) {
      state.multiChoice.questionText = questionText;
    },
    setSelectedQuestionType(state, selectedQuestionType) {
      state.selectedQuestionType = selectedQuestionType;
    },
    addMultiChoiceAnswer(state) {
      state.multiChoice.answers.push({
        text: "",
        isCorrect: false,
      });
    },
    removeMultiChoiceAnswer(state, index) {
      state.multiChoice.answers.splice(index, 1);

      //if only one answer remaining, it must be the correct answer
      if (state.multiChoice.answers.length < 2) {
        state.multiChoice.answers[0].isCorrect = true;
      }
    },
    toggleMultiChoiceAnswerCorrect(state, index) {
      state.multiChoice.answers[index].isCorrect = !state.multiChoice.answers[index].isCorrect;
    },
    addFillInCodeWidget(state, widget) {
      state.fillInCode.widgets.push(widget);
    },
    removeFillInCodeWidget(state, id) {
      state.fillInCode.widgets = state.fillInCode.widgets.filter((widget) => widget.id !== id);
    },
    removeAllFillInCodeWidgets(state) {
      state.fillInCode.widgets = [];
    }
  },

  getters: {
    getField
  }
}