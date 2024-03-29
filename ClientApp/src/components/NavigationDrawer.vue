<template>
  <div>
    <v-app-bar app dense clipped-left flat>
      <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-img
        src="../assets/svg/logo/symbol_logo.svg"
        max-height="25"
        max-width="60"
        contain
      ></v-img>
      <v-img
        src="../assets/svg/logo/text_logo.svg"
        max-height="100"
        max-width="150"
        contain
      ></v-img>

      <v-spacer></v-spacer>

      <v-menu
        origin="top right"
        content-class="elevation-3 "
        offset-y
        bottom
        left
        nudge-bottom="4"
        nudge-left="2"
      >
        <template v-slot:activator="{ on }">
          <v-btn icon v-on="on">
            <v-badge
              bordered
              bottom
              color="success"
              dot
              offset-x="10"
              offset-y="18"
            >
              <v-list-item-avatar class="mx-0" color="primary">
                <span class="white--text font-weight-medium">{{
                  userInitials
                }}</span>
              </v-list-item-avatar>
            </v-badge>
          </v-btn>
        </template>

        <v-list>
          <v-list nav dense>
            <v-list-item>
              <v-badge
                bordered
                bottom
                color="success"
                dot
                offset-x="10"
                offset-y="18"
              >
                <v-list-item-avatar class="mx-0" color="primary">
                  <span class="white--text font-weight-medium">{{
                    userInitials
                  }}</span>
                </v-list-item-avatar>
              </v-badge>
              <v-list-item-content class="ml-4">
                <v-list-item-title class="font-weight-medium">{{
                  user.firstName + " " + user.lastName
                }}</v-list-item-title>
                <v-list-item-subtitle>{{ user.email }}</v-list-item-subtitle>
              </v-list-item-content>
            </v-list-item>
            <v-divider class="my-2"></v-divider>
            <v-list-item link to="/settings">
              <v-list-item-icon>
                <v-icon>mdi-cog</v-icon>
              </v-list-item-icon>
              <v-list-item-title>Nastavení</v-list-item-title>
            </v-list-item>
            <v-list-item @click="logout" link>
              <v-list-item-icon>
                <v-icon>mdi-logout-variant</v-icon>
              </v-list-item-icon>
              <v-list-item-title>Odhlásit</v-list-item-title>
            </v-list-item>
          </v-list>
        </v-list>
      </v-menu>
    </v-app-bar>
    <v-navigation-drawer
      v-model="drawer"
      class="pa-0"
      app
      clipped
      bottom
      :mini-variant="menuMini"
    >
      <v-layout column fill-height>
        <v-list nav dense>
          <v-list-item-group color="primary">
            <v-list-item
              v-for="item in items"
              :key="item.title"
              :to="item.to"
              link
            >
              <v-list-item-action>
                <v-icon>{{ item.icon }}</v-icon>
              </v-list-item-action>
              <v-list-item-content>
                <v-list-item-title>{{ item.title }}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </v-list-item-group>
        </v-list>
        <v-spacer></v-spacer>
        <v-list nav v-if="!isMobile">
          <v-list-item @click.stop="mini = !mini">
            <v-list-item-action>
                <v-icon v-if="!mini">mdi-chevron-left</v-icon>
                <v-icon v-if="mini">mdi-chevron-right</v-icon>
            </v-list-item-action>
            <v-list-item-content>
                <v-list-item-title></v-list-item-title>
              </v-list-item-content>
          </v-list-item>
        </v-list>
      </v-layout>
    </v-navigation-drawer>
  </div>
</template>

<script>
import store from "@/store";
import router from "@/router";

export default {
  name: "NavigationDrawer",
  data() {
    return {
      drawer: this.isMobile ? false : true,
      mini: false,
    };
  },
  methods: {
    async logout() {
      await store.dispatch("logout");
      router.push("/login");
    },
  },
  computed: {
    isMobile() {
      return this.$vuetify.breakpoint.mobile;
    },
    menuMini() {
      return this.mini && !this.isMobile;
    },
    user() {
      return store.state.user;
    },
    userInitials() {
      return this.user.firstName[0] + this.user.lastName[0];
    },
    items() {
      return store.getters.getUserNavigationItems;
    },
  },
  created() {
    if (!this.user) {
      store.dispatch("getUser");
    }
  },
};
</script>

<style>
.v-expansion-panel .transparent {
  background-color: rgba(0, 0, 0, 0) !important;
}
.v-expansion-panel-content__wrap {
  padding: 0;
}
</style>>


