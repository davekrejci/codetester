import Vue from 'vue'
import Vuex from 'vuex'
import client from 'api-client'
import questionDesigner from './modules/questionDesigner';

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    courses : [],
    users: [],
    questions: [],
    tags: [],
  },
  mutations: {
    setCourses (state, courses) {
      state.courses = courses
    },
    setUsers (state, users) {
      state.users = users
    },
    setQuestions (state, questions) {
      state.questions = questions
    },
    setTags (state, tags) {
      state.tags = tags
    },
    
  },
  actions: {
    fetchCourses ({ commit }) {
      return client
        .fetchCourses()
        .then(courses => commit('setCourses', courses))
    },
    fetchUsers ({ commit }) {
      return client
        .fetchUsers()
        .then(users => commit('setUsers', users))
    },
    fetchQuestions ({ commit }) {
      return client
        .fetchQuestions()
        .then(questions => commit('setQuestions', questions))
    },
    fetchTags ({ commit }) {
      return client
        .fetchTags()
        .then(tags => commit('setTags', tags))
    },
  },
  modules: {
    questionDesigner,
  }
})
