<template>
  <div>
    <v-card  flat class="pa-4 rounded-t-0">
      <v-card-title>
          <span>Zapsaní Studenti</span>
          <v-spacer></v-spacer>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <router-link :to="{ name: 'Users' }">
              <v-btn
                class="mx-2 text-right"
                depressed
                fab
                x-small
                color="primary"
                v-bind="attrs"
                v-on="on"
              >
                <v-icon dark> mdi-plus </v-icon>
              </v-btn>
            </router-link>
          </template>
          <span>Přidat studenty</span>
        </v-tooltip>
      </v-card-title>
      <v-data-table
        dense
        :headers="headers"
        :items="students"
        :items-per-page="15"
        :search="search"
        :loading="loading"
        loading-text="Načítání dat..."
        no-data-text="Žádné data"
        item-key="student"
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
              @click="showRemoveDialogForUser(item)"
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
    <v-dialog v-model="showRemoveDialog" max-width="400px">
      <v-card class="text-center pa-4">
        <v-icon color="error" x-large>mdi-alert-circle-outline</v-icon>
        <v-card-title class="text-h5">
          <!-- <span class="mx-auto my-4"> Jste si jistý?</span> -->
        </v-card-title>
        <v-card-text
          >Opravdu si přejete odebrat uživatele <strong>{{ userToRemove.username }}</strong> z tohoto semestru??</v-card-text
        >
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="grey"
            class="mx-2"
            outlined
            @click="showRemoveDialog = false"
          >
            Ne
          </v-btn>
          <v-btn
            color="error"
            class="mx-2"
            outlined
            @click="removeUser(userToRemove.id)"
          >
            Ano
          </v-btn>
          <v-spacer></v-spacer>
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
    students: { type: Array, required: true },
  },
  data() {
    return {
      search: "",
      loading: false,
      userToRemove: {},
      showRemoveDialog: false,
      headers: [
        { text: "Studentské číslo", value: "username" },
        { text: "Email", value: "email" },
        { text: "Jméno", value: "firstName" },
        { text: "Příjmení", value: "lastName" },
        { text: "Akce", value: "actions", sortable: false, width: "15" },
      ],
      selected: [],
    };
  },
  methods: {
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