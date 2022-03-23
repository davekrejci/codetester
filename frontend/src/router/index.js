import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

const routes = [
  {
    path: '/login',
    name: 'Login',
    component: () => import('@/views/Login.vue')
  },
  {
    path: '/examinstance/:id',
    name: 'ExamInstance',
    component: () => import('@/views/ExamInstance.vue')
  },
  {
    path: '/',
    component: () => import('@/views/Dashboard.vue'),
    redirect: '/myexams',
    children: [
      {
        path: 'myexams',
        name: 'MyExams',
        component: () => import('@/views/MyExams.vue')
    
      },
      {
        path: 'settings',
        name: 'Settings',
        component: () => import('@/views/Settings.vue')
      },
      {
        path: 'exams',
        name: 'Exams',
        component: () => import('@/views/Exam/Exams.vue')
    
      },
      {
        path: 'exams/create',
        name: 'CreateExam',
        component: () => import('@/views/Exam/CreateExam.vue')
      },
      {
        path: 'exams/:id',
        name: 'Exam',
        component: () => import('@/views/Exam/Exam.vue')
      },
      {
        path: 'courses',
        name: 'Courses',
        component: () => import('@/views/Course/Courses.vue')
      },
      {
        path: 'courses/create',
        name: 'CreateCourse',
        component: () => import('@/views/Course/CreateCourse.vue')
      },
      {
        path: 'courses/:coursecode',
        name: 'Course',
        component: () => import('@/views/Course/Course.vue')
    
      },
      {
        path: 'courses/:coursecode/create/semester',
        name: 'CreateSemester',
        component: () => import('@/views/Semester/CreateSemester.vue')
      },
      {
        path: 'semesters/:id',
        name: 'Semester',
        component: () => import('@/views/Semester/Semester.vue'),
      },
      {
        path: 'questions',
        name: 'Questions',
        component: () => import('@/views/Question/Questions.vue'),
    
      },
      {
        path: 'questions/create',
        name: 'CreateQuestion',
        component: () => import('@/views/Question/CreateQuestion.vue')
      },
      {
        path: 'questions/:id',
        name: 'Question',
        component: () => import('@/views/Question/Question.vue'),
    
      },
      {
        path: 'users',
        name: 'Users',
        component: () => import('@/views/User/Users.vue')
      },
    ]
  },
  
  {
    path: '*',
    name: 'NotFound',
    component: () => import('@/views/404.vue')
  }
]

const router = new VueRouter({
  mode: 'history',
  routes
})

export default router
