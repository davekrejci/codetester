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
              <a
                id="forgotPasswordText"
                @click="forgotPassword = !forgotPassword"
                class="text--secondary text-caption"
                >Zapomněli jste heslo?</a
              >
              <transition name="slide-y-transition">
                <div
                  :style="{ opacity: forgotPassword ? 1 : 0 }"
                  class="text--secondary text-caption ma-0"
                >
                  Kontaktujte prosím svého administrátora a požádejte o obnovení
                  hesla
                </div>
              </transition>
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
            </v-card-actions></div>
        </v-row>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import store from "@/store";
import router from "@/router";

export default {
  name: "Login",
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
          if (errorMessage == "User not found") {
            this.error = "Uživatel nenalezen";
            this.$notify({
              title: "Error",
              text: "Uživatel nenalezen",
              type: "error",
            });
          }
          if (errorMessage == "Wrong password") {
            this.$notify({
              title: "Error",
              text: "Nesprávné heslo",
              type: "error",
            });
          }
        } else {
          this.$notify({
              title: "Error",
              text: "Nastala chyba při přihlašování",
              type: "error",
            });
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
    forgotPassword: false,
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
