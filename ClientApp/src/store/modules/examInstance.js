//import { createHelpers } from 'vuex-map-fields';
import client from 'api-client'
//import { QuestionTypes } from '@/util/questionTypes.js';

// const { getField, updateField } = createHelpers({
//   getterType: 'getField',
//   mutationType: 'updateField',
// });

function initState() {
  return {
      isLoaded: false,
      id: null,
      examInfo: null,
      questions: null
  }
}

export default {
  namespaced: true,
  state: initState(),
  mutations: {
    setExamInstance(state, examInstance) {
      state.id = examInstance.id;
      state.examInfo = examInstance.exam;
      state.questions = examInstance.questionInstances;
      state.isLoaded = true;
    },
  },
  actions: {
    async fetchExamInstance ({ commit }, { id }) {
        return client
        .fetchExamInstance(id)
        .then(examInstance => commit('setExamInstance', examInstance))
    },
    async turnInExamInstance ({ state }) {
      let examTurnInDto = {
        id: state.id,
        questionInstances: state.questions
      }
       return client
       .turnInExamInstance(state.id, examTurnInDto);
    },
  },
  getters: {
    // getField
  }
}