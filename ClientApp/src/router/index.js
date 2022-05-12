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
    component: () => import('@/views/ExamInstance/ExamInstance.vue'),
    meta: { authorize: [] } 
  },
  {
    path: '/examresult/:id',
    name: 'ExamResult',
    component: () => import('@/views/ExamResult/ExamResult.vue'),
    meta: { authorize: [] } 
  },
  {
    path: '/',
    component: () => import('@/views/Dashboard.vue'),
    redirect: '/exams',
    children: [
      {
        path: 'exams',
        name: 'MyExams',
        component: () => import('@/views/ExamInstance/MyExams.vue'),
        meta: { authorize: [] } 
      },
      {
        path: 'settings',
        name: 'Settings',
        component: () => import('@/views/Settings.vue'),
        meta: { authorize: [] } 
      },
      {
        path: 'management/exams',
        name: 'Exams',
        component: () => import('@/views/Exam/Exams.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'management/exams/create',
        name: 'CreateExam',
        component: () => import('@/views/Exam/CreateExam.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'management/exams/:id',
        name: 'Exam',
        component: () => import('@/views/Exam/Exam.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'management/courses',
        name: 'Courses',
        component: () => import('@/views/Course/Courses.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'management/courses/create',
        name: 'CreateCourse',
        component: () => import('@/views/Course/CreateCourse.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'management/courses/:coursecode',
        name: 'Course',
        component: () => import('@/views/Course/Course.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'management/courses/:coursecode/create/semester',
        name: 'CreateSemester',
        component: () => import('@/views/Semester/CreateSemester.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'management/semesters/:id',
        name: 'Semester',
        component: () => import('@/views/Semester/Semester.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'management/questions',
        name: 'Questions',
        component: () => import('@/views/Question/Questions.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'management/questions/create',
        name: 'CreateQuestion',
        component: () => import('@/views/Question/CreateQuestion.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'management/questions/import',
        name: 'ImportQuestions',
        component: () => import('@/views/Question/ImportQuestions.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'management/questions/:id',
        name: 'Question',
        component: () => import('@/views/Question/Question.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'management/user/:id',
        name: 'User',
        component: () => import('@/views/User/User.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'management/users',
        name: 'Users',
        component: () => import('@/views/User/Users.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'management/users/create',
        name: 'CreateUser',
        component: () => import('@/views/User/CreateUser.vue'),
        meta: { authorize: [Role.Admin, Role.Teacher] }
      },
      {
        path: 'management/users/import',
        name: 'ImportUsers',
        component: () => import('@/views/User/ImportUsers.vue'),
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
