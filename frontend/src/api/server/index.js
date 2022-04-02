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
      return Promise.reject(error);
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
        const response = await axios.post(url, semester);
        console.log(response)
    },
    async updateSemester(id, semesterUpdateDto) {
        const url = "semesters/" + id;
        const response = await axios.put(url, semesterUpdateDto);
        console.log(response)
    },

    // USERS
    async loginUser(userLoginDto) {
        const url = "users/login";
        const response = await axios.post(url, userLoginDto);
        return response.data
    },
    async fetchUsers() {
        const url = "users";
        const response = await axios.get(url);
        return response.data
    },
    async createUser(userCreateDto) {
        const url = "users";
        const response = await axios.post(url, userCreateDto);
        console.log(response)
    },
    async updateUser(id, userUpdateDto) {
        const url = "users/" + id;
        let response = await axios.put(url, userUpdateDto);
        console.log(response)
    },
    async deleteUser(id) {
        const url = "users/" + id;
        let response = await axios.delete(url);
        console.log(response)
    },
    // admin only route for changing any users password (eg. forgotten passwords)
    async resetUserPassword(id, userPasswordResetDto) {
        const url = "users/password/reset/" + id;
        const response = await axios.post(url, userPasswordResetDto);
        console.log(response)
    },
    // any user route for changing their own password
    async changeUserPassword(userPasswordChangeDto) {
        const url = "users/password/change";
        const response = await axios.post(url, userPasswordChangeDto);
        console.log(response)
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