import axios from 'axios';
import store from '@/store';
import router from '@/router';

// Set base API URL
axios.defaults.baseURL = process.env.VUE_APP_CODETESTER_API_URL;

// Add authentication token to each request
axios.interceptors.request.use(
    config => {
        const user = JSON.parse(localStorage.getItem('user'));
        if (user && user.token)
            config.headers.authorization = `Bearer ${user.token}`;
        return config;
    },
    error => {
        return Promise.reject(error);
    }
);

// If authentication token expires logout user
axios.interceptors.response.use(undefined, function (error) {
    if (error) {
      const originalRequest = error.config;
      if (error.response.status === 401 && !originalRequest._retry) {
          originalRequest._retry = true;
          store.dispatch('logout');
          return router.push('/login');
      }
    }
  })

export default {
    // COURSES
    async fetchCourse(coursecode) {
        const url = "courses/" + coursecode;
        const response = await axios.get(url);
        return response.data
    },
    async fetchCourses() {
        const url = "courses";
        const response = await axios.get(url);
        return response.data
    },
    async createCourse(course) {
        const url = "courses";
        const response = await axios.post(url, course);
        console.log(response);
    },
    async deleteCourse(coursecode) {
        const url = "courses/" + coursecode;
        const response = await axios.delete(url);
        console.log(response);
    },

    // SEMESTERS
    async fetchSemester(id) {
        const url = "semesters/" + id;
        const response = await axios.get(url);
        return response.data;
    },
    async deleteSemester(id) {
        const url = "semesters/" + id;
        const response = await axios.delete(url);
        return response.data
    },
    async createSemester(semester) {
        const url = "semesters";
        console.log(JSON.stringify(semester));
        const response = await axios.post(url, semester);
        console.log(response)
    },

    // USERS
    async loginUser(userLoginDto) {
        const url = "users/login";
        const response = await axios.post(url, userLoginDto);
        return response.data
    },
    async fetchUsers() {
        // to implement
    },

    // QUESTIONS
    async fetchQuestions() {
        const url = "questions";
        const response = await axios.get(url);
        return response.data
    },
    async fetchQuestion(id) {
        const url = "questions/" + id;
        const response = await axios.get(url);
        return response.data
    },
    async editQuestion(id, question) {
        const url = "questions/" + id;
        const response = await axios.put(url, question);
        console.log(response)
    },
    async createQuestion(question) {
        const url = "questions";
        console.log(JSON.stringify(question));
        let response = await axios.post(url, question);
        console.log(response)
    },
    async deleteQuestion(id) {
        const url = "questions/" + id;
        let response = await axios.delete(url);
        console.log(response)
    },

    // EXAMS
    async fetchExams() {
        const url = "exams";
        const response = await axios.get(url);
        console.log(response.data);
        return response.data
    },
    async fetchExam(id) {
        const url = "exams/" + id;
        const response = await axios.get(url);
        return response.data
    },
    async createExam(exam) {
        const url = "exams";
        console.log(JSON.stringify(exam));
        let response = await axios.post(url, exam);
        console.log(response)
    },
    async updateExam(id, exam) {
        const url = "exams/" + id;
        console.log(JSON.stringify(exam));
        let response = await axios.put(url, exam);
        console.log(response)
    },
    async deleteExam(id) {
        const url = "exams/" + id;
        const response = await axios.delete(url);
        return response.data
    },

    // TAGS
    async fetchTags() {
        const url = "tags";
        const response = await axios.get(url);
        return response.data
    },
}