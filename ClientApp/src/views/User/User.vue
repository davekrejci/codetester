<template>
  <v-container fluid class="">
    <div v-if="this.user != null">
      <v-breadcrumbs
        :items="breadcrumbs"
        class="pa-0 pb-4 pl-1"
      ></v-breadcrumbs>
      <h1 class="ml-1 mt-0">
        {{ this.user.firstName }} {{ this.user.lastName }}
      </h1>
      <div class="ml-1 mt-n2 mb-8 text-subtitle-1">{{ this.user.email }}</div>

      <v-form ref="userForm">
        <v-text-field
          outlined
          :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
          label="Uživatelské jméno"
          v-model="user.username"
          :rules="[rules.required]"
          disabled
        >
        </v-text-field>
        <v-text-field
          outlined
          :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
          label="Email"
          v-model="user.email"
          :rules="[rules.required, rules.email]"
        >
        </v-text-field>
        <v-select
          v-model="user.role"
          :items="possibleRoles"
          label="Role"
          item-text="text"
          item-value="value"
          outlined
          :rules="[rules.required]"
        ></v-select>
        <v-text-field
          outlined
          :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
          label="Jméno"
          v-model="user.firstName"
          :rules="[rules.required]"
        >
        </v-text-field>
        <v-text-field
          outlined
          :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
          label="Příjmení"
          v-model="user.lastName"
          :rules="[rules.required]"
        >
        </v-text-field>

        <!-- Action buttons -->
        <!-- Delete Dialog -->
        <v-dialog
          v-if="loggedUser.role == 'Admin'"
          v-model="showDeleteDialog"
          max-width="400px"
        >
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              v-bind="attrs"
              v-on="on"
              :loading="loading"
              :disabled="loading"
              color="error"
              outlined
              class="mr-4 mb-2"
            >
              Smazat <v-icon right dark> mdi-trash-can-outline </v-icon>
            </v-btn>
          </template>
          <v-card class="text-center pa-4">
            <v-icon color="error" x-large>mdi-alert-circle-outline</v-icon>
            <v-card-title class="text-h5">
              <!-- <span class="mx-auto my-4"> Jste si jistý?</span> -->
            </v-card-title>
            <v-card-text
              >Opravdu si přejete uživatele smazat? Tato akce je
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
              <v-btn color="error" class="mx-2" outlined @click="deleteUser">
                Ano
              </v-btn>
              <v-spacer></v-spacer>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <!-- Reset Password Dialog -->
        <v-dialog v-model="showResetPasswordDialog" max-width="400px">
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              v-bind="attrs"
              v-on="on"
              :loading="loading"
              :disabled="loading"
              color="primary"
              outlined
              class="mr-4 mb-2"
            >
              Resetovat heslo <v-icon right dark> mdi-lock </v-icon>
            </v-btn>
          </template>
          <v-card class="text-center pa-4">
            <v-icon color="error" x-large>mdi-alert-circle-outline</v-icon>
            <v-card-title class="text-h5"> </v-card-title>
            <v-card-text>Zadejte nové heslo pro uživatele. </v-card-text>
            <v-form ref="userPasswordResetForm">
              <v-text-field
                label="Heslo"
                v-model="newPassword"
                single-line
                :rules="[rules.required]"
                outlined
                flat
                class="mb-4"
                prepend-inner-icon="mdi-lock-outline"
                :append-icon="
                  passwordHidden ? 'mdi-eye-outline' : 'mdi-eye-off-outline'
                "
                @click:append="() => (passwordHidden = !passwordHidden)"
                :type="passwordHidden ? 'password' : 'text'"
                :disabled="loading"
              >
              </v-text-field>
            </v-form>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn
                color="grey"
                class="mx-2"
                outlined
                @click="showResetPasswordDialog = false"
              >
                Zrušit
              </v-btn>
              <v-btn
                color="error"
                class="mx-2"
                outlined
                @click="resetUserPassword"
              >
                Resetovat
              </v-btn>
              <v-spacer></v-spacer>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-btn
          color="primary"
          :loading="loading"
          :disabled="loading"
          depressed
          class="mr-4 mb-2"
          @click="updateUser"
        >
          Uložit <v-icon right dark> mdi-plus-circle-outline </v-icon>
        </v-btn>
      </v-form>
    </div>
  </v-container>
</template>

<script>
import api from "api-client";
import store from "@/store";

export default {
  name: "User",
  data() {
    return {
      showDeleteDialog: false,
      showResetPasswordDialog: false,
      newPassword: "",
      passwordHidden: true,
      loading: false,
      rules: {
        required: (value) => !!value || "Povinné.",
        email: (value) => /.+@.+\..+/.test(value) || 'Použijte platnou emailovou adresu',
      },
      user: null,
    };
  },
  methods: {
    validate() {
      return this.$refs.userForm.validate();
    },
    validatePasswordReset() {
      return this.$refs.userPasswordResetForm.validate();
    },
    reset() {
      this.$refs.userForm.reset();
    },
    async fetchUser() {
      this.user = null;
      this.loading = true;
      try {
        // load from store if data is already there
        this.user = this.$store.getters.getUserById(this.$route.params.id);
        if (this.user) {
          this.loading = false;
        } else {
          // fetch user from api otherwise and then retrieve from store
          await this.$store.dispatch("fetchUsers");
          this.user = this.$store.getters.getUserById(this.$route.params.id);
        }
      } catch (error) {
        this.$notify({
          title: "Error",
          text: 'Nepodařilo se načíst data.',
          type: "error",
        });
      }
      this.loading = false;
    },
    async deleteUser() {
      this.error = null;
      this.loading = true;
      try {
        await api.deleteUser(this.$route.params.id);
        this.$router.push({
          name: "Users",
        });
      } catch (error) {
        this.$notify({
          title: "Error",
          text: "Smazání uživatele se nepodařilo",
          type: "error",
        });
      }
      this.loading = false;
    },
    async updateUser() {
      let isFormValid = this.validate();
      if (!isFormValid) return;
      this.loading = true;
      try {
        let userUpdateDto = {
          email: this.user.email,
          role: this.user.role,
          firstName: this.user.firstName,
          lastName: this.user.lastName,
        };
        await api.updateUser(this.$route.params.id, userUpdateDto);
        this.$notify({
          title: "Úspěch",
          text: "Změny byly uloženy",
          type: "success",
        });
      } catch (error) {
        this.$notify({
          title: "Error",
          text: 'Změny se nepodařilo uložit',
          type: "error",
        });
      }
      this.loading = false;
      window.scrollTo(0, 0);
    },
    async resetUserPassword() {
      let isFormValid = this.validatePasswordReset();
      if (!isFormValid) return;
      this.loading = true;
      let userPasswordResetDto = {
        password: this.newPassword,
      };
      try {
        await api.resetUserPassword(
          this.$route.params.id,
          userPasswordResetDto
        );
        this.$notify({
          title: "Úspěch",
          text: "Heslo bylo změněno",
          type: "success",
        });
      } catch (error) {
        this.$notify({
          title: "Error",
          text: 'Heslo se nepodařilo změnit',
          type: "error",
        });
      }
    this.showResetPasswordDialog = false;
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
    breadcrumbs() {
      return [
        {
          text: "Management",
          disabled: true,
        },
        {
          text: "Uživatelé",
          disabled: false,
          link: true,
          exact: true,
          to: { name: "Users" },
        },
        {
          text: this.user.username,
          disabled: true,
        },
      ];
    },
  },
  created() {
    this.fetchUser();
  },
};
</script>

<style>
</style>