import axios from 'axios'

axios.defaults.baseURL = process.env.VUE_APP_CODETESTER_API_URL;

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