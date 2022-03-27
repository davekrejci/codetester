<template>
  <!-- <v-toolbar v-if="$vuetify.breakpoint.smAndDown" dense>
      <v-app-bar-nav-icon></v-app-bar-nav-icon>

      <v-toolbar-title>Title</v-toolbar-title>

      <v-spacer></v-spacer>

      <v-btn icon>
        <v-icon>mdi-magnify</v-icon>
      </v-btn>

      <v-btn icon>
        <v-icon>mdi-heart</v-icon>
      </v-btn>

      <v-btn icon>
        <v-icon>mdi-dots-vertical</v-icon>
      </v-btn>
    </v-toolbar> -->
  <v-toolbar app v-if="$vuetify.breakpoint.smAndDown">
    <v-dialog
      v-model="dialog"
      fullscreen
      hide-overlay
      transition="dialog-bottom-transition"
    >
      <template v-slot:activator="{ on, attrs }">
        <v-app-bar-nav-icon v-bind="attrs" v-on="on"></v-app-bar-nav-icon>
      </template>
      <v-card>
        <v-toolbar flat color="blue-grey darken-2">
          <v-toolbar-title>Mobile Menu</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-btn icon @click.native="dialog = false">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-toolbar>

        <v-list nav dense>
          <v-list-item-group color="primary">
            <v-list-item
              v-for="item in items"
              :key="item.title"
              :to="item.path"
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
      </v-card>
    </v-dialog>
    <v-spacer></v-spacer>

    <v-btn depressed text small>
          <v-badge
            bordered
            bottom
            color="success"
            dot
            offset-x="10"
            offset-y="18"
          >
            <v-list-item-avatar size="30" class="mx-0" color="primary">
              <!-- <v-icon dark>mdi-account-circle</v-icon> -->
              <v-img
                src="https://randomuser.me/api/portraits/men/32.jpg"
              ></v-img>
            </v-list-item-avatar>
          </v-badge>
		<span class="ml-2">David Krejci</span>

	</v-btn>
        
  </v-toolbar>
  <!-- <v-bottom-navigation color="primary" v-if="$vuetify.breakpoint.smAndDown" app>
    <v-btn  v-for="item in items" :key="item.title" :to="item.path" :value="item.title">
      <span>{{ item.title }}</span>

      <v-icon >{{ item.icon }}</v-icon>
    </v-btn>
    <v-btn>
				<v-badge bordered bottom color="success" dot offset-x="10" offset-y="18">
					<v-list-item-avatar class="mx-0" color="primary">
						<v-img src="https://randomuser.me/api/portraits/men/32.jpg"></v-img>
					</v-list-item-avatar>
				</v-badge>
				</v-btn>
  </v-bottom-navigation> -->
</template>

<script>
export default {
  name: "MobileNavigation",
  data() {
    return {
      user: this.$store.state.user,
      dialog: false,
      items: [
        { title: "Moje Testy", path: "/myexams", icon: "mdi-pencil" },
        { title: "Testy", path: "/exams", icon: "mdi-format-list-bulleted" },
        { title: "Předměty", path: "/courses", icon: "mdi-book-open" },
        { title: "Otázky", path: "/questions", icon: "mdi-help-circle" },
        { title: "Uživatelé", path: "/users", icon: "mdi-account-multiple" },
      ],
    };
  },
};
</script>

<style>
</style>