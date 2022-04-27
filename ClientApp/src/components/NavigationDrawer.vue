<template>
  <v-navigation-drawer
    v-if="$vuetify.breakpoint.mdAndUp"
    :width="280"
    app
    permanent
    floating
  >
    <v-list class="mt-2">
      <v-list-item class="px-2">
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
      </v-list-item>
    </v-list>
    <v-card class="mx-2" elevation="0" outlined v-if="user">
      <v-expansion-panels focusable flat>
        <v-expansion-panel class="transparent pa-0">
          <v-expansion-panel-header color="transparent" class="py-0">
            <v-badge
              bordered
              bottom
              color="success"
              dot
              offset-x="10"
              offset-y="18"
            >
              <v-list-item-avatar class="mx-0" color="primary">
                <span class="white--text font-weight-medium">{{ userInitials }}</span>
              </v-list-item-avatar>
            </v-badge>
            <v-list-item-content class="ml-4">
              <v-list-item-title class="font-weight-medium">{{
                user.firstName + " " + user.lastName
              }}</v-list-item-title>
              <v-list-item-subtitle>{{ user.email }}</v-list-item-subtitle>
            </v-list-item-content>
          </v-expansion-panel-header>
          <v-expansion-panel-content color="transparent" class="pa-0">
            <v-list nav dense>
              <v-list-item @click="logout" link>
                <v-list-item-icon>
                  <v-icon>mdi-logout-variant</v-icon>
                </v-list-item-icon>
                <v-list-item-title>Odhlásit</v-list-item-title>
              </v-list-item>
              <v-list-item link to="/settings">
                <v-list-item-icon>
                  <v-icon>mdi-cog</v-icon>
                </v-list-item-icon>
                <v-list-item-title>Nastavení</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels>
    </v-card>
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
  </v-navigation-drawer>
</template>

<script>
import store from "@/store";
import router from "@/router";

export default {
  name: "NavigationDrawer",
  data() {
    return {
      
    };
  },
  methods: {
    async logout() {
      await store.dispatch('logout');
      router.push("/login");
    },
  },
  computed: {
    user() {
      return store.state.user;
    },
    userInitials() {
      return this.user.firstName[0] + this.user.lastName[0];
    },
    items() {
      return store.getters.getUserNavigationItems;
    }
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


