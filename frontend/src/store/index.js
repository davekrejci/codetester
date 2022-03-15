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
    exams: [],
    tags: [],
  },
  mutations: {
    setCourses (state, courses) {
      state.courses = courses
    },
    setExams (state, exams) {
      state.exams = exams
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
    fetchExams ({ commit }) {
      return client
        .fetchExams()
        .then(exams => commit('setExams', exams))
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
  getters: {
    getCoursesWithoutSemesters(state) {
      // eslint-disable-next-line
      return state.courses.map(({semesters, ...course}) => course);
    },
    getCourseByCourseCode: (state) => (coursecode) => {
      // eslint-disable-next-line
      return state.courses.find(course => course.courseCode == coursecode);
    },
    getSemester: (state) => (courseCode, year, semesterType) => {
      let course =  state.courses.find(course => course.courseCode == courseCode);
      console.log(course);
      let semester = course.semesters.find(semester => semester.year == year && semester.semesterType == semesterType)
      return semester;
    }
  },
  modules: {
    questionDesigner,
  }
})
