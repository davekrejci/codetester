<template>
  <v-dialog
    :value="linkDialog"
    max-width="600"
    width="600"
    @click:outside="closeDialog"
  >
    <v-card class="pa-2">
      <v-card-actions class="ma-0 pa-0">
        <v-spacer></v-spacer>
        <v-btn @click="closeDialog" icon>
          <v-icon id="close-button">mdi-close</v-icon>
        </v-btn>
      </v-card-actions>
      <h2 class="text-center mb-2 mt-n8">Přidat odkaz</h2>
      <v-card flat class="pa-4">
      <v-text-field
          outlined
          label="URL"
          v-model="url"
          dense
          placeholder="https://www.google.com/"
      ></v-text-field>
      <v-text-field
          outlined
          label="Text"
          v-model="text"
          dense
          placeholder="Google"
      ></v-text-field>
      <v-card-actions class="pa-0">
        <v-btn outlined color="primary" @click="addLink()">Vložit</v-btn>        
      </v-card-actions>
      </v-card>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: "LinkDialog",
  props: {
    linkDialog: {
      type: Boolean,
      default: false,
    },
    textSelection: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      url: '',
      text: ''
    };
  },
  watch: {
    textSelection(value) {
      this.text = value
    }
  },
  methods: {
    addLink() {
      let data = {
        href: this.url,
        text: this.text
      }
      this.$emit('addLink', data);
      this.closeDialog();
    },
    closeDialog() {
      this.url = '';
      this.$emit("update:linkDialog", false);
    },
  },
};
</script>

<style scoped>
</style>
