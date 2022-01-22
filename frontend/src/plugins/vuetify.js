import Vue from 'vue';
import Vuetify from 'vuetify/lib';
import { TiptapVuetifyPlugin } from 'tiptap-vuetify'
import 'tiptap-vuetify/dist/main.css'

const vuetify = new Vuetify({
  theme: {
    dark: false,
    options: {
      customProperties: true,
    },
      themes: {
        dark: {
          primary: '#488FEF',
          secondary: '#364FCC',
          accent: '#82B1FF',
          success: '#27D086',
          warning: '#FADB39',
          error: '#FB4444'
        },
        light: {
            primary: '#488FEF',
            secondary: '#364FCC',
            accent: '#82B1FF',
            success: '#27D086',
            warning: '#FADB39',
            error: '#FB4444'
        }
      },
    },
});

Vue.use(Vuetify);
Vue.use(TiptapVuetifyPlugin, {
  vuetify,
  iconsGroup: 'mdi'
})

export default vuetify;
