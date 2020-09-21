import Vue from 'vue';
import Vuetify from 'vuetify/lib';

Vue.use(Vuetify);

export default new Vuetify({
    theme: {
      dark: true,
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
