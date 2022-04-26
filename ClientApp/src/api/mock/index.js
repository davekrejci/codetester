import courses from './data/courses'
import users from './data/users'
import questions from './data/questions'
import tags from './data/tags';

export default {
  fetchCourses () {
    return fetch(courses, 1000)
  },
  fetchUsers () {
    return fetch(users, 1000)
  },
  fetchQuestions () {
    return fetch(questions, 1000)
  },
  fetchTags () {
    return fetch(tags, 1000)
  }
}

// Function to simulate loading times with setTimeout
const fetch = (mockData, time = 0) => {
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve(mockData)
    }, time)
  })
}