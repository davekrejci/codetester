<template>
  <div class="text-center">
    <v-snackbar
      :timeout="snackbar.timeout"
      :value="snackbar.show"
      absolute
      top
      :color="snackbar.color"
      middle
      tile
      multi-line
    >
      <v-layout align-center pr-4>
        <v-icon class="pr-3" dark large>{{ snackbar.icon }}</v-icon>
        <v-layout column>
          <div>
            <strong>{{ snackbar.title }}</strong>
          </div>
          <div>{{ snackbar.text }}</div>
        </v-layout>
        <v-btn v-if="snackbar.timeout === -1" icon @click="$emit('close-snackbar')">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-layout>
    </v-snackbar>
  </div>
</template>

<script>
export default {
  name: "DefaultSnackbar",
  props: {
      type: String,
      text: String
  },
  computed: {
    snackbar() {
      if (this.type === 'error') {
        return {
          show: true,
          icon: "mdi-alert-circle",
          color: "error",
          title: "Error",
          text: this.text,
          timeout: -1,
        };
      }
      if (this.type === 'success') {
        return {
          show: true,
          icon: "mdi-check-circle",
          color: "success",
          title: "Úspěch",
          text: this.text,
          timeout: 3000,
        };
      }
      return {
        show: false,
      };
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