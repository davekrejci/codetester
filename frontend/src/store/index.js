import Vue from 'vue'
import Vuex from 'vuex'
import client from 'api-client'
import questionDesigner from './modules/questionDesigner';
import { Role } from '@/util/role.js';

Vue.use(Vuex)

function initState() {
  return {
    user: JSON.parse(localStorage.getItem('user')),
    courses : [],
    users: [],
    questions: [],
    exams: [],
    tags: [],
    navigationItems: [
      { 
        title: "Moje Testy", 
        to: { name: "MyExams" }, 
        icon: "mdi-pencil",
        roles: [Role.Admin, Role.Teacher, Role.Student]
      },
      { 
        title: "Testy", 
        to: { name: "Exams" }, 
        icon: "mdi-format-list-bulleted",
        roles: [Role.Admin, Role.Teacher]
      },  
      { 
        title: "Předměty", 
        to: { name: "Courses" }, 
        icon: "mdi-book-open",
        roles: [Role.Admin, Role.Teacher]
      },
      { 
        title: "Otázky", 
        to: { name: "Questions" }, 
        icon: "mdi-help-circle",
        roles: [Role.Admin, Role.Teacher]
      },
      { 
        title: "Uživatelé", 
        to: { name: "Users" }, 
        icon: "mdi-account-multiple",
        roles: [Role.Admin, Role.Teacher]
      },
    ]
  }
}

export default new Vuex.Store({
  state: initState(),
  mutations: {
    setUser(state, user) {
      state.user = user
    },
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
    reset (state) {
      Object.assign(state, initState())
    }
    
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
    getUser ({ commit }) {
      const user = JSON.parse(localStorage.getItem('user'));
      commit('setUser', user);
    },
    login ({ commit }, userLoginDto ) {
      return client
          .loginUser(userLoginDto)
          .then(user => {
              localStorage.setItem("user", JSON.stringify(user));
              commit('setUser', user)
          })
    },
    logout ({ commit }) {
      localStorage.removeItem("user");
      commit('reset')
    }
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
    },
    getLoggedUser (state) {
      return state.user
    },
    getUserById: (state) => (id) => {
      return state.users.find(user => user.id == id);
    },
    isAuthenticated (state) {
      return !!state.user
    },
    getUserNavigationItems(state) {
      let userRole = state.user.role;
      let navigationItems = state.navigationItems;
      let userNavigationItems = [];
      navigationItems.forEach(item => {
        if (item.roles.includes(userRole)) {
          userNavigationItems.push(item);
        }
      });

      return userNavigationItems;
    }
    },
  modules: {
    questionDesigner,
  }
})
