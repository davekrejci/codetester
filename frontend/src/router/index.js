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
