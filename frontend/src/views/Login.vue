<template>
  <v-container class="pa-0" fill-height fluid>
    <v-row dense class="fill-height">
      <v-col class="col-12 col-md-6">
        <v-row class="fill-height" align="center" justify="center">
          <v-img
            class="mx-auto"
            src="@/assets/svg/illustrations/login.svg"
            :max-width="illustrationWidth"
            alt=""
          />
        </v-row>
      </v-col>
      <v-col class="">
        <v-row
          class="fill-height"
          :align="$vuetify.breakpoint.smAndDown ? 'start' : 'center'"
          justify="center"
        >
          <div
            id="loginContainer"
            :align="$vuetify.breakpoint.smAndDown ? 'center' : 'start'"
          >
            <v-img
              class="mb-4"
              src="../assets/svg/logo/text_logo.svg"
              max-height="100"
              max-width="150"
              contain
            ></v-img>
            <h1>Vítejte zpět!</h1>
            <p>Pro pokračování se prosím přihlaště.</p>
            <v-form
              v-model="valid"
              ref="form"
              lazy-validation
              @keyup.native.enter="submit"
            >
              <v-text-field
                label="Uživatelské jméno"
                v-model="username"
                required
                outlined
                flat
                hide-details="auto"
                single-line
                prepend-inner-icon="mdi-account-outline"
                class="mb-1"
                :disabled="loading"
              ></v-text-field>
              <v-text-field
                label="Heslo"
                v-model="password"
                single-line
                required
                hide-details="auto"
                outlined
                flat
                class="mb-1"
                prepend-inner-icon="mdi-lock-outline"
                :append-icon="value ? 'mdi-eye-outline' : 'mdi-eye-off-outline'"
                @click:append="() => (value = !value)"
                :type="value ? 'password' : 'text'"
                :disabled="loading"
              >
              </v-text-field>
              <a id="forgotPasswordText" class="text--secondary text-caption"
                >Zapomněli jste heslo?</a
              >
            </v-form>
            <v-card-actions>
              <v-btn
                large
                block
                rounded
                text
                dark
                class="mt-2 px-5 primary"
                @click="submit"
                :disabled="!valid || loading"
                :loading="loading"
                >Přihlásit</v-btn
              >
            </v-card-actions>
            <!-- <v-divider class="my-4"></v-divider> -->
            <!-- <p class="text-center text--secondary">Nemáte ještě účet? <a id="signUpText" class="text-decoration-none" href="#">Zaregistrujte se</a></p> -->
          </div>
        </v-row>
      </v-col>
    </v-row>
    <default-snackbar
      :type="snackbar.type"
      :text="snackbar.text"
      v-on:close-snackbar="error = null"
    ></default-snackbar>
  </v-container>
</template>

<script>
//import api from "api-client";
import store from "@/store";
import router from "@/router";
import DefaultSnackbar from "@/components/DefaultSnackbar.vue";

export default {
  name: "Login",
  components: {
    DefaultSnackbar,
  },
  methods: {
    async submit() {
      this.loading = true;
      console.log(`submitting 
			username: ${this.username}
			password: ${this.password}`);

      let userLoginDto = {
        username: this.username,
        password: this.password,
      };
      try {
        await store.dispatch("login", userLoginDto);
        router.push("/");
      } catch (error) {
        // Bad request => incorrect username or password
        if (error.response && error.response.status == 400) {
          let errorMessage = error.response.data;
          if (errorMessage == "User not found")
            this.error = "Uživatel nenalezen";
          if (errorMessage == "Wrong password") this.error = "Nesprávné heslo";
        } else {
          console.log(error);
          this.error = "Error logging in";
        }
      }
      this.loading = false;
    },
  },
  data: () => ({
    loading: false,
    error: null,
    valid: true,
    username: "",
    password: "",
    responseStatus: null,
    value: String,
  }),
  computed: {
    illustrationWidth() {
      switch (this.$vuetify.breakpoint.name) {
        case "xs":
          return 200;
        case "sm":
          return 250;
        case "md":
          return 500;
        case "lg":
          return 600;
        case "xl":
          return 800;
        default:
          return 200;
      }
    },
    snackbar() {
      if (this.error != null) {
        return {
          type: "error",
          text: this.error,
          show: true,
        };
      }
      return {
        show: false,
      };
    },
  },
};
</script>

<style scoped>
#loginContainer {
  width: 80%;
  max-width: 500px;
}
#forgotPasswordText:hover {
  color: var(--v-primary-lighten1) !important;
}
#signUpText:hover {
  color: var(--v-primary-lighten1);
}
</style>
