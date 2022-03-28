import Vue from 'vue'
import VueRouter from 'vue-router'
import store from '@/store'
import { Role } from '@/util/role.js';

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
    component: () => import('@/views/ExamInstance.vue'),
    meta: { authorize: [] } 
  },
  {
    path: '/',
    component: () => import('@/views/Dashboard.vue'),
    redirect: '/myexams',
    children: [
      {
        path: 'myexams',
        name: 'MyExams',
        component: () => import('@/views/MyExams.vue'),
        meta: { authorize: [] } 
      },
      {
        path: 'settings',
        name: 'Settings',
        component: () => import('@/views/Settings.vue'),
        meta: { authorize: [] } 
      },
      {
        path: 'exams',
        name: 'Exams',
        component: () => import('@/views/Exam/Exams.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'exams/create',
        name: 'CreateExam',
        component: () => import('@/views/Exam/CreateExam.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'exams/:id',
        name: 'Exam',
        component: () => import('@/views/Exam/Exam.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'courses',
        name: 'Courses',
        component: () => import('@/views/Course/Courses.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'courses/create',
        name: 'CreateCourse',
        component: () => import('@/views/Course/CreateCourse.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'courses/:coursecode',
        name: 'Course',
        component: () => import('@/views/Course/Course.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'courses/:coursecode/create/semester',
        name: 'CreateSemester',
        component: () => import('@/views/Semester/CreateSemester.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'semesters/:id',
        name: 'Semester',
        component: () => import('@/views/Semester/Semester.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'questions',
        name: 'Questions',
        component: () => import('@/views/Question/Questions.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'questions/create',
        name: 'CreateQuestion',
        component: () => import('@/views/Question/CreateQuestion.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'questions/:id',
        name: 'Question',
        component: () => import('@/views/Question/Question.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'users',
        name: 'Users',
        component: () => import('@/views/User/Users.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
    ]
  },
  
  {
    path: '*',
    name: 'NotFound',
    component: () => import('@/views/404.vue'),
    meta: { authorize: [] }
  }
]

const router = new VueRouter({
  mode: 'history',
  routes
});

// Setup route guards
router.beforeEach((to, from, next) => {
  const { authorize } = to.meta;
  const loggedUser = store.getters.getLoggedUser;
  
  // redirect to login page if not logged in and trying to access a restricted page
  if (authorize && !loggedUser) {
    return next('/login');
  }
  // redirect to home page if user logged in but does not have required role
  if (authorize && authorize.length && !authorize.includes(loggedUser.role)) {
    return next('/');
  }
  // redirect to home page if logged in and tryin to access login page
  if (!authorize && loggedUser) {
    return next('/');
  }
  next();
})

export default router
