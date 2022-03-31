<template>
  <div>
    <v-card flat class="pa-4">
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
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <router-link :to="{ name: 'CreateUser' }">
              <v-btn
                class="mx-2"
                depressed
                fab
                small
                color="primary"
                v-bind="attrs"
                v-on="on"
              >
                <v-icon dark> mdi-plus </v-icon>
              </v-btn>
            </router-link>
          </template>
          <span>Přidat Uživatele</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <div v-bind="attrs" v-on="on">
              <v-btn class="mx-2" depressed fab disabled small color="primary">
                <v-icon dark> mdi-cloud-upload </v-icon>
              </v-btn>
            </div>
          </template>
          <span>Nahrát Uživatele</span>
        </v-tooltip>
      </v-card-title>
      <v-data-table
        :headers="headers"
        dense
        :items="users"
        :items-per-page="15"
        :search="search"
        :loading="loading"
        loading-text="Načítání dat..."
        no-data-text="Žádné data"
        item-key="user"
        v-model="selected"
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
              @click="showDeleteDialogForItem(item)"
              small
              icon
              plain
              color="error"
              class="mx-1"
            >
              <v-icon> mdi-delete </v-icon>
            </v-btn>
          </v-row>
        </template>
      </v-data-table>
    </v-card>

    <!-- Delete Dialog -->
    <v-dialog v-if="userToDelete" v-model="showDeleteDialog" max-width="400px">
      <v-card class="text-center pa-4">
        <v-icon color="error" x-large>mdi-alert-circle-outline</v-icon>
        <v-card-title class="text-h5">
          <!-- <span class="mx-auto my-4"> Jste si jistý?</span> -->
        </v-card-title>
        <v-card-text
          >Opravdu si přejete smazat uživatele {{ userToDelete.username }}? Tato akce je
          nevratná.</v-card-text
        >
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="grey"
            class="mx-2"
            outlined
            @click="showDeleteDialog = false"
          >
            Ne
          </v-btn>
          <v-btn
            color="error"
            class="mx-2"
            outlined
            @click="deleteUser(userToDelete.id)"
          >
            Ano
          </v-btn>
          <v-spacer></v-spacer>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <default-snackbar
      :type="snackbar.type"
      :text="snackbar.text"
      v-on:close-snackbar="error = null"
    ></default-snackbar>
  </div>
</template>

<script>
import api from "api-client";
import DefaultSnackbar from '@/components/DefaultSnackbar.vue';

export default {
  name: "Users",
  components: { DefaultSnackbar },
  data() {
    return {
      userToDelete: null,
      search: "",
      loading: false,
      error: null,
      hasBeenDeleted: false,
      showDeleteDialog: false,
      selected: [],
      headers: [
        { text: "Uživatelské jméno", value: "username" },
        { text: "Email", value: "email" },
        { text: "Jméno", value: "firstName" },
        { text: "Příjmení", value: "lastName" },
        { text: "Role", value: "role" },
        { text: "Akce", value: "actions", sortable: false },
      ],
    };
  },
  methods: {
    showDeleteDialogForItem(user) {
      this.userToDelete = user;
      this.showDeleteDialog = true;
    },
    async deleteUser(id) {
      this.hasBeenDeleted = false;
      this.userToDelete = null;
      this.showDeleteDialog = false;
      this.error = null;
      this.loading = true;
      try {
        await api.deleteUser(id);
        this.hasBeenDeleted = true;
      } catch (error) {
        this.error = error;
      }
      this.fetchUsers();
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
  },
  computed: {
    users() {
      return this.$store.state.users;
    },
    snackbar() {
      if (this.error != null) {
        return {
          type: "error",
          text: this.error.toString(),
          show: true
        };
      }
      if (this.hasBeenDeleted) {
        return {
          type: 'success',
          text: "Uživatel byl smazán",
          show: true
        };
      }
      return {
        show: false
      }
    },
  },
  created() {
    this.fetchUsers();
  },
};
</script>

<style>
</style>