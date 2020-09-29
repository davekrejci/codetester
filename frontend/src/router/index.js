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
    path: '/',
    component: () => import('../views/Dashboard.vue'),
    redirect: '/assignements',
    children: [
      {
        path: 'assignements',
        name: 'Assignements',
        component: () => import('../views/Assignements.vue')
    
      },
      {
        path: 'settings',
        name: 'Settings',
        component: () => import('../views/Settings.vue')
      },
      {
        path: 'assignement-detail/:id',
        name: 'AssignementDetail',
        component: () => import('../views/AssignementDetail.vue')
      },
    ]
  },
]

const router = new VueRouter({
  routes
})

export default router
