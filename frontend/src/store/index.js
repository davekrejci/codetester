import Vue from 'vue'
import Vuex from 'vuex'
import client from 'api-client'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    courses : {}
  },
  mutations: {
    setCourses (state, courses) {
      state.courses = courses
    }
  },
  actions: {
    fetchCourses ({ commit }) {
      return client
        .fetchCourses()
        .then(courses => commit('setCourses', courses))
    }
  },
  modules: {
  }
})
