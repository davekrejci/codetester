import axios from 'axios'

export default {
    async fetchCourses () {
        // to implement
    },
    async fetchUsers () {
        // to implement
    },
    async fetchQuestions () {
        try{
            const url = process.env.VUE_APP_CODETESTER_API_URL + "/questions";
            const response = await axios.get(url);
            return response.data
        }
        catch(error){
            console.log(error);
            return Promise.reject(error);
        }
    },
    async fetchQuestion (id) {
        try{
            const url = process.env.VUE_APP_CODETESTER_API_URL + "/questions/" + id;
            const response = await axios.get(url);
            return response.data
        }
        catch(error){
            console.log(error);
            return Promise.reject(error);
        }
    },
    async editQuestion(id, question) {
        try{
            const url = process.env.VUE_APP_CODETESTER_API_URL + "/questions/" + id;
            const response = await axios.put(url, question);
            console.log(response)
        }
        catch(error){
            console.log(error);
            return Promise.reject(error);
        }
    },
    async createQuestion(question) {
        try{
            const url = process.env.VUE_APP_CODETESTER_API_URL + "/questions";
            console.log(JSON.stringify(question));
            let response = await axios.post(url, question);
            console.log(response)
        }
        catch(error){
            console.log(error.response);
            return Promise.reject(error);
        }
    },
    async deleteQuestion(id) {
        try{
            const url = process.env.VUE_APP_CODETESTER_API_URL + "/questions/" + id;
            let response = await axios.delete(url);
            console.log(response)
        }
        catch(error){
            console.log(error);
            return Promise.reject(error);
        }
    },
    async fetchTags() {
        try{
            const url = process.env.VUE_APP_CODETESTER_API_URL + "/tags";
            const response = await axios.get(url);
            return response.data
        }
        catch(error){
            console.log(error);
            return Promise.reject(error);
        }
    },
}