<template>
    <v-btn
      x-small
      plain
      depressed
      color="grey"
      dark
      class="mt-n4 mr-n4"
      absolute
      top
      right
      @click="copyToClipboard()"
      style="z-index: 20001"
      >Kopírovat</v-btn
    >
</template>

<script>
export default {
  name: "CopyButton",
  props: {
    textToCopy: {
      type: String,
      default: "",
    },
  },
  methods: {
    async copyToClipboard() {
        try {
            await navigator.clipboard.writeText(this.textToCopy);
            this.$notify({
              title: "Úspěch",
              text: "Obsah byl zkopírován.",
              type: "success",
            });
        }
        catch (err) {
            console.log(err);
            this.$notify({
              title: "Error",
              text: "Obsah se nepodařilo zkopírovat.",
              type: "error",
            });
        }
    },
  },
};
</script>

<style>
</style>