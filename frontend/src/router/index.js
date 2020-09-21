import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Dashboard',
    component: () => import('../views/Dashboard.vue')

  },
  {
    path: '/settings',
    name: 'Settings',
    component: () => import('../views/Settings.vue')
  },
  {
    path: '/assignement-detail/:id',
    name: 'AssignementDetail',
    component: () => import('../views/AssignementDetail.vue')
  }
]

const router = new VueRouter({
  routes
})

export default router
