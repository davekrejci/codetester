import { createHelpers } from 'vuex-map-fields';
import client from 'api-client'
import { QuestionTypes } from '@/util/questionTypes.js';

const { getField, updateField } = createHelpers({
  getterType: 'getField',
  mutationType: 'updateField',
});

export default {
  namespaced: true,
  state: {
    selectedQuestionType: "",
    selectedTags: [],
    multiChoice: {
      questionText: "",
      answers: [{
          answerText: "",
          isCorrect: true,
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
      state.multiChoice.answers[payload.index].answerText = payload.value;
    },
    setMultiChoiceQuestionText(state, questionText) {
      state.multiChoice.questionText = questionText;
    },
    setSelectedQuestionType(state, selectedQuestionType) {
      state.selectedQuestionType = selectedQuestionType;
    },
    addMultiChoiceAnswer(state) {
      state.multiChoice.answers.push({
        answerText: "",
        isCorrect: false,
      });
    },
    setSelectedTags(state, selectedTags) {
      state.selectedTags = selectedTags;
    },
    addSelectedTag(state, tagText) {
      state.selectedTags.push({
        tagText: tagText
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
  actions: {
    async createQuestion ({state}) {
      let question = {};
      question.questionType = state.selectedQuestionType;
      question.tags = state.selectedTags;
      if(question.questionType == QuestionTypes.MultiChoice){
        question.questionText = state.multiChoice.questionText;
        question.answers = state.multiChoice.answers;
      }
      if(question.questionType == QuestionTypes.FillInCode){
        question.codeDescription = state.fillInCode.codeDescription;
        question.code = state.fillInCode.content;
        question.fillCount = state.fillInCode.fillInCount;
        let fillInCodeBlocks = [];
        let doc = state.fillInCode.cm.getDoc();
        let widgets = doc.getAllMarks();
        widgets.forEach((widget) => {
          let range = widget.find();
          let content = doc.getRange(range.from, range.to);
          let fromIndex = doc.indexFromPos(range.from);
          let toIndex = doc.indexFromPos(range.to);
          fillInCodeBlocks.push({content: content, startPosition: fromIndex, endPosition: toIndex});
        })
        question.fillInCodeBlocks = fillInCodeBlocks;
      }
      console.log(question);
      try{
        await client.createQuestion(question);
      } catch (error) {
        return Promise.reject(error);
      }
    },
  },
  getters: {
    getField
  }
}