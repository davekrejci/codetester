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
      exams.forEach((exam) => {
        //set semesterType displayText
        let semesterTypeWithDisplayText = {};
        semesterTypeWithDisplayText.value = exam.semester.semesterType;
        if (semesterTypeWithDisplayText.value == "winter") {
          semesterTypeWithDisplayText.displayText = "zimní";
        }
        if (semesterTypeWithDisplayText.value == "summer") {
          semesterTypeWithDisplayText.displayText = "letní";
        }
        exam.semester.semesterType = semesterTypeWithDisplayText;

        //set status displayText
        // if (exam.status == "planned") {
        //   exam.status = "naplánovaný";
        // }
      });
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
      return state.courses.find(course => course.courseCode == coursecode);
    },
    getExamById: (state) => (id) => {
      return state.exams.find(exam => exam.id == id);
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
