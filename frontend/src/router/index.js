import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

const routes = [
  {
    path: '/login',
    name: 'Login',
    component: () => import('../views/Login.vue')
  },
  {
    path: '/exam/:id',
    name: 'Exam',
    component: () => import('../views/Exam.vue')
  },
  {
    path: '/',
    component: () => import('../views/Dashboard.vue'),
    redirect: '/exams',
    children: [
      {
        path: 'exams',
        name: 'Exams',
        component: () => import('../views/Exams.vue')
    
      },
      {
        path: 'settings',
        name: 'Settings',
        component: () => import('../views/Settings.vue')
      },
      {
        path: 'exam-detail/:id',
        name: 'ExamDetail',
        component: () => import('../views/ExamDetail.vue')
      },
      {
        path: 'courses',
        name: 'Courses',
        component: () => import('../views/Courses.vue')
    
      },
      {
        path: 'course-detail/:id',
        name: 'CourseDetail',
        component: () => import('../views/CourseDetail.vue')
    
      },
      {
        path: 'questions',
        name: 'Questions',
        component: () => import('../views/Questions.vue')
    
      },
      {
        path: 'questiondesigner',
        name: 'QuestionDesigner',
        component: () => import('../views/QuestionDesigner.vue')
    
      },
      {
        path: 'users',
        name: 'Users',
        component: () => import('../views/Users.vue')
    
      },
      {
        path: 'examdesigner',
        name: 'Exam Designer',
        component: () => import('../views/ExamDesigner.vue')
    
      },
    ]
  },
  
  {
    path: '*',
    name: 'NotFound',
    component: () => import('../views/404.vue')
  }
]

const router = new VueRouter({
  //mode: 'history',
  routes
})

export default router
