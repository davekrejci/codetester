<template>
  <v-container fluid>
    <!-- Change Password Dialog -->
    <v-dialog v-model="showChangePasswordDialog" max-width="400px">
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
          Změnit heslo <v-icon right dark> mdi-lock </v-icon>
        </v-btn>
      </template>
      <v-card class="text-center pa-4">
        <v-icon color="primary" x-large>mdi-lock-reset</v-icon>
        <v-card-title class="text-h5"> </v-card-title>
        <v-form ref="passwordChangeForm">
          <v-text-field
            label="Současné heslo"
            v-model="currentPassword"
            single-line
            :rules="[rules.required]"
            outlined
            flat
            class=""
            prepend-inner-icon="mdi-lock-outline"
            :append-icon="
              currentPasswordHidden ? 'mdi-eye-outline' : 'mdi-eye-off-outline'
            "
            @click:append="
              () => (currentPasswordHidden = !currentPasswordHidden)
            "
            :type="currentPasswordHidden ? 'password' : 'text'"
            :disabled="loading"
          >
          </v-text-field>
          <v-text-field
            label="Nové heslo"
            v-model="newPassword"
            single-line
            :rules="[rules.required]"
            outlined
            flat
            class="mb-4"
            prepend-inner-icon="mdi-lock-outline"
            :append-icon="
              newPasswordHidden ? 'mdi-eye-outline' : 'mdi-eye-off-outline'
            "
            @click:append="() => (newPasswordHidden = !newPasswordHidden)"
            :type="newPasswordHidden ? 'password' : 'text'"
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
            @click="showChangePasswordDialog = false"
          >
            Zrušit
          </v-btn>
          <v-btn
            color="primary"
            class="mx-2"
            outlined
            @click="changeUserPassword"
          >
            Změnit
          </v-btn>
          <v-spacer></v-spacer>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-switch
      v-model="$vuetify.theme.dark"
      class="ma-2"
      label="Dark Mode"
    ></v-switch>
  </v-container>
</template>

<script>
import api from "api-client";

export default {
  name: "Settings",
  data() {
    return {
      loading: false,
      showChangePasswordDialog: false,
      currentPassword: "",
      newPassword: "",
      currentPasswordHidden: true,
      newPasswordHidden: true,
      rules: {
        required: (value) => !!value || "Povinné.",
      },
    };
  },
  methods: {
    validatePasswordChange() {
      return this.$refs.passwordChangeForm.validate();
    },
    async changeUserPassword() {
      this.showChangePasswordDialog = false;
      let isFormValid = this.validatePasswordChange();
      if (!isFormValid) return;
      this.loading = true;
      let userPasswordChangeDto = {
        currentPassword: this.currentPassword,
        newPassword: this.newPassword,
      };
      try {
        await api.changeUserPassword(userPasswordChangeDto);
        this.$notify({
          title: "Úspěch",
          text: "Změny byly uloženy",
          type: "success",
        });
      } catch (error) {
        this.$notify({
          title: "Error",
          text: "Změny se nepodařilo uložit",
          type: "error",
        });
      }
      this.showChangePasswordDialog = false;
      this.loading = false;
      window.scrollTo(0, 0);
    },
  }
};
</script>
