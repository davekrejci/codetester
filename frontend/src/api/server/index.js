import axios from 'axios'

export default {
    async fetchCourses() {
        // to implement
    },
    async fetchUsers() {
        // to implement
    },
    async fetchQuestions() {
        const url = process.env.VUE_APP_CODETESTER_API_URL + "/questions";
        const response = await axios.get(url);
        return response.data
    },
    async fetchQuestion(id) {
        const url = process.env.VUE_APP_CODETESTER_API_URL + "/questions/" + id;
        const response = await axios.get(url);
        return response.data
    },
    async editQuestion(id, question) {
        const url = process.env.VUE_APP_CODETESTER_API_URL + "/questions/" + id;
        const response = await axios.put(url, question);
        console.log(response)
    },
    async createQuestion(question) {
        const url = process.env.VUE_APP_CODETESTER_API_URL + "/questions";
        console.log(JSON.stringify(question));
        let response = await axios.post(url, question);
        console.log(response)
    },
    async deleteQuestion(id) {
        const url = process.env.VUE_APP_CODETESTER_API_URL + "/questions/" + id;
        let response = await axios.delete(url);
        console.log(response)
    },
    async fetchTags() {
        const url = process.env.VUE_APP_CODETESTER_API_URL + "/tags";
        const response = await axios.get(url);
        return response.data
    },
}