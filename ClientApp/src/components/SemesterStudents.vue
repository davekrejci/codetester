<template>
  <div>
    <v-card  flat class="pa-4 rounded-t-0">
      <v-card-title>
          <span>Zapsaní Studenti</span>
          <v-spacer></v-spacer>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
              <v-btn
                class="mx-2 text-right"
                depressed
                fab
                x-small
                color="primary"
                v-bind="attrs"
                v-on="on"
                @click.stop="showAddDialog = true"
              >
                <v-icon dark> mdi-plus </v-icon>
              </v-btn>
          </template>
          <span>Přidat studenty</span>
        </v-tooltip>
      </v-card-title>
      <v-data-table
        dense
        :headers="headers"
        :items="selectedUsers"
        :items-per-page="15"
        :search="searchSelected"
        :loading="loading"
        loading-text="Načítání dat..."
        no-data-text="Žádné data"
        item-key="student"
      >
        <template v-slot:[`item.actions`]="{ item }">
          <v-row
            align="center"
            justify="space-around"
            class="d-flex flex-nowrap"
          >
            <router-link
              :to="{ name: 'User', params: { id: item.id } }"
              style="text-decoration: none; color: inherit"
            >
              <v-btn small plain icon color="primary" class="mx-1">
                <v-icon> mdi-magnify </v-icon>
              </v-btn>
            </router-link>
            <v-btn
              @click="removeFromSelectedUsers(item.id)"
              small
              icon
              plain
              color="error"
              class="mx-1"
            >
              <v-icon> mdi-minus </v-icon>
            </v-btn>
          </v-row>
        </template>
      </v-data-table>
    </v-card>

    <!-- possible users -->
    <v-dialog v-model="showAddDialog" max-width="1000">
      <v-card class="pa-8">
        <v-card-title>
          <v-text-field
            v-model="search"
            append-icon="mdi-magnify"
            placeholder="Vyhledat"
            single-line
            hide-details
            filled
            rounded
            dense
            class="shrink mx-4"
          ></v-text-field>
        </v-card-title>
        <v-data-table
          :headers="headers"
          dense
          :items="nonSelectedUsers"
          :items-per-page="15"
          :search="search"
          :loading="loading"
          loading-text="Načítání dat..."
          no-data-text="Žádné data"
          item-key="id"
          v-model="selected"
          show-select
        >
          <template v-slot:[`item.actions`]="{ item }">
            <v-row
              align="center"
              justify="space-around"
              class="d-flex flex-nowrap"
            >
              <router-link
                :to="{ name: 'User', params: { id: item.id } }"
                style="text-decoration: none; color: inherit"
              >
                <v-btn small plain icon color="primary" class="mx-1">
                  <v-icon> mdi-magnify </v-icon>
                </v-btn>
              </router-link>
            </v-row>
          </template>
        </v-data-table>
        <v-card-actions>
          <v-spacer></v-spacer>

          <v-btn color="error" outlined @click="showAddDialog = false"> Zrušit </v-btn>
          <v-btn
            color="primary"
            depressed
            @click="addSelectedToSelectedUsers()"
          >
            Přidat
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
//import api from "api-client";

export default {
  name: "SemesterStudents",
  props: {
    currentUsers: {
      type: Array,
      default: () => new Array(), 
    },
  },
  data() {
    return {
      search: "",
      searchSelected: "",
      loading: false,
      userToRemove: {},
      showAddDialog: false,
      headers: [
        { text: "Uživatelské jméno", value: "username" },
        { text: "Email", value: "email" },
        { text: "Jméno", value: "firstName" },
        { text: "Příjmení", value: "lastName" },
        { text: "Role", value: "role" },
        { text: "Akce", value: "actions", sortable: false },
      ],
      selected: [],
      selectedUsers: this.initCurrentUsers(),
    };
  },
  methods: {
    initCurrentUsers() {
      let users = this.currentUsers;
      return users;
    },
    showRemoveDialogForUser(user) {
      this.userToRemove = user;
      this.showRemoveDialog = true;
    },
    async removeUser(userId) {
        console.log(userId);
        this.userToRemove = {};
        this.showRemoveDialog = false;
    //   this.loading = true;
    //   try {
    //     await api.removeUserFromSemester(semesterId, userId);
    //     this.$emit('user-remove-success');
    //   } catch (error) {
    //     this.$emit('user-remove-error', error);
    //   }
    //   this.loading = false;
    //   // TODO: reload data
    },
    async fetchUsers() {
      this.loading = true;
      try {
        await this.$store.dispatch("fetchUsers");
      } catch (error) {
        console.log(error);
        this.error = error;
      }
      this.loading = false;
    },
    addSelectedToSelectedUsers() {
      this.selected.forEach((user) =>
        this.selectedUsers.push(user)
      );
      this.selected = [];
      this.showAddDialog = false;
    },
    removeFromSelectedUsers(id) {
      this.selectedUsers = this.selectedUsers.filter(
        (user) => user.id != id
      );
    },
  },
  computed: {
    users() {
      return this.$store.state.users;
    },
    nonSelectedUsers() {
      let selectedUserIds = this.selectedUsers.map(
        (user) => user.id
      );
      return this.users.filter(
        (user) => selectedUserIds.indexOf(user.id) === -1
      );
    },
  },
  watch: {
    // update parent component on selectedUsers change
    selectedUsers(val) {
      this.$emit("selectedUsersChanged", val);
    },
  },
  created() {
    if (this.users.length == 0) {
      this.fetchUsers();
    }
  },
};
</script>

<style>
.v-snack__wrapper {
  max-width: none;
  min-width: 100%;
  margin: 0;
}
</style>