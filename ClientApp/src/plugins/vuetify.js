import Vue from 'vue';
import Vuetify from 'vuetify/lib';

const vuetify = new Vuetify({
  theme: {
    dark: false,
    options: {
      customProperties: true,
    },
      themes: {
        dark: {
          // primary: '#97c7f8',
          // secondary: '#9fa8e6',
          // accent: '#a2c8ff',
          // success: '#7cddaa',
          // warning: '#ffca28',
          // error: '#f89595'
          primary: '#488FEF',
            secondary: '#364FCC',
            accent: '#82B1FF',
            success: '#27D086',
            warning: '#FFC107',
            error: '#FB4444'
        },
        light: {
            primary: '#488FEF',
            secondary: '#364FCC',
            accent: '#82B1FF',
            success: '#27D086',
            warning: '#FFC107',
            error: '#FB4444'
        }
      },
    },
});

Vue.use(Vuetify);

export default vuetify;
