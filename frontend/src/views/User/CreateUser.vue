<template>
  <v-container class="">
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
    <default-snackbar :type="snackbar.type" :text="snackbar.text" v-on:close-snackbar="error = null"></default-snackbar>
  </v-container>
</template>

<script>
import api from "api-client";
import DefaultSnackbar from '@/components/DefaultSnackbar.vue';

export default {
  components: { DefaultSnackbar },
  name: "CreateUser",
  data() {
    return {
      error: null,
      hasSaved: false,
      loading: false,
      possibleRoles: [
          {
              value: "Admin",
              text: "Admin"
          },
          {
              value: "Teacher",
              text: "Učitel"
          },
          {
              value: "Student",
              text: "Student"
          }
        ],
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
      this.hasSaved = false;
      this.loading = true;
      try {
        await api.createUser(this.user);
        this.hasSaved = true;
        this.reset();
      } catch (error) {
        console.log(error);
        console.log("it's here!");
        this.error = error;
      }
      this.loading = false;
      window.scrollTo(0, 0);
    },
  },
  computed: {
    snackbar() {
      if (this.error != null) {
        return {
          type: "error",
          text: this.error.toString(),
          show: true
        };
      }
      if (this.hasSaved) {
        return {
          type: 'success',
          text: "Uživatel byl vytvořen",
          show: true
        };
      }
      return {
        show: false
      }
    },
  }
};
</script>

<style>
</style>