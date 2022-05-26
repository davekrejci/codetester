<template>
  <v-container fluid class="">
    <v-breadcrumbs :items="breadcrumbs" class="pa-0 pb-4 pl-1"></v-breadcrumbs>
    <h1 class="ml-1 mb-6 mt-0">Vytvořit nového uživatele</h1>
    <v-form ref="createUserForm">
      <v-text-field
        outlined
        :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
        label="Uživatelské jméno"
        hint="Použijte studijní číslo"
        v-model="user.username"
        :rules="[rules.required]"
      >
      </v-text-field>
      <v-text-field
        outlined
        :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
        label="Email"
        v-model="user.email"
        hint="Použijte školní email"
        :rules="[rules.required, rules.email]"
        class=""
      >
      </v-text-field>
      <v-text-field
        outlined
        :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
        label="Heslo"
        v-model="user.password"
        :rules="[rules.required]"
        class=""
      >
      </v-text-field>
      <v-select
          v-model="user.role"
          :items="possibleRoles"
          label="Role"
          item-text="text"
          item-value="value"
          outlined
    ></v-select>
      <v-text-field
        outlined
        :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
        label="Jméno"
        v-model="user.firstName"
        :rules="[rules.required]"
        class=""
      >
      </v-text-field>
      <v-text-field
        outlined
        :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
        label="Příjmení"
        v-model="user.lastName"
        :rules="[rules.required]"
        class="mb-4"
      >
      </v-text-field>

      <!-- Action buttons -->
      <v-btn
        color="error"
        :disabled="loading"
        outlined
        class="mr-4 mb-2"
        @click="reset"
      >
        Smazat <v-icon right dark> mdi-trash-can-outline </v-icon>
      </v-btn>
      <v-btn
        color="primary"
        :loading="loading"
        depressed
        class="mr-4 mb-2"
        @click="createUser"
      >
        Vytvořit <v-icon right dark> mdi-plus-circle-outline </v-icon>
      </v-btn>
    </v-form>
  </v-container>
</template>

<script>
import api from "api-client";
import store from "@/store";

export default {
  name: "CreateUser",
  data() {
    return {
      loading: false,
      user: {
        username: "",
        email: "",
        password: "",
        role: "",
        firstName: "",
        lastName: "",
      },
      rules: {
        required: (value) => !!value || "Povinné.",
        email: (value) => /.+@.+\..+/.test(value) || 'Použijte platnou emailovou adresu',
      },
      breadcrumbs: [
        {
          text: "Management",
          disabled: true,
        },
        {
          text: "Uživatelé",
          disabled: false,
          link: true,
          exact: true,
          to: { name: "Users" }
        },
        {
          text: "Nový Uživatel",
          disabled: true,
          link: true,
          exact: true,
          to: { name: "CreateUser" }
        },
      ],
    };
  },
  methods: {
    validate() {
      return this.$refs.createUserForm.validate();
    },
    reset() {
      this.$refs.createUserForm.reset();
    },
    async createUser() {
      let isFormValid = this.validate();
      if (!isFormValid) return;
      this.loading = true;
      try {
        let response = await api.createUser(this.user);
        let id = response.data.id;
        this.reset();
        this.$router.push({
          name: "User",
          params: { id: id }
        });
        this.$notify({
          title: "Úspěch",
          text: "Uživatel byl vytvořen",
          type: "success",
        });
      } catch (error) {
        this.$notify({
          title: "Error",
          text: "Vytvoření uživatele se nepodařilo",
          type: "error",
        });
      }
      this.loading = false;
      window.scrollTo(0, 0);
    },
  },
  computed: {
    loggedUser() {
      return store.state.user;
    },
    possibleRoles() {
      if(this.loggedUser.role == 'Admin') {
        return [
          {
            value: "Admin",
            text: "Admin",
          },
          {
            value: "Teacher",
            text: "Učitel",
          },
          {
            value: "Student",
            text: "Student",
          }
        ]
      } 
      else {
        return [
          {
            value: "Teacher",
            text: "Učitel",
          },
          {
            value: "Student",
            text: "Student",
          }
        ]
      }
    },
  }
};
</script>

<style>
</style>