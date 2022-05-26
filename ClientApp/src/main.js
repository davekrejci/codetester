import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import vuetify from './plugins/vuetify';
import Notifications from 'vue-notification'
import DatetimePicker from 'vuetify-datetime-picker'


Vue.config.productionTip = false
Vue.use(DatetimePicker);
Vue.use(Notifications);

new Vue({
  router,
  store,
  vuetify,
  render: h => h(App)
}).$mount('#app')
