import courses from './data/courses'
const fetch = (mockData, time = 0) => {
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve(mockData)
    }, time)
  })
}
export default {
  fetchCourses () {
    return fetch(courses, 1000) // wait 1s before returning courses
  }
}