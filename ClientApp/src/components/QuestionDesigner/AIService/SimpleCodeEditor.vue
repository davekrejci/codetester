<template>
  <v-card flat outlined>
    <textarea v-model="content" :id="'editor_' + id"></textarea>
    <slot></slot>
  </v-card>
</template>

<script>
import * as CodeMirror from "codemirror";
import "codemirror/lib/codemirror.css";
import "codemirror/theme/duotone-light.css";
import "codemirror/theme/nord.css";
import "codemirror/mode/clike/clike.js";
import "codemirror/addon/search/searchcursor";
import "codemirror/addon/display/autorefresh";

export default {
  name: "SimpleCodeEditor",
  data() {
    return {
      cm: null,
      content: "",
    };
  },
  props: {
    id: {
      type: String,
      default: "default",
    },
  },
  methods: {
    setContent(content) {
      this.content = content;
      this.cm.setValue(content);
      this.cm.refresh();
    },
    async copyToClipboard() {
      await navigator.clipboard.writeText(this.cm.getValue());
      this.$notify({
        title: "Úspěch",
        text: "Obsah byl zkopírován.",
        type: "info",
      });
    },
  },
  watch: {
    content(newContent) {
      this.$emit("contentChanged", newContent);
    },
  },
  mounted() {
    // need to wait for vue to change DOM so codemirror can find element to bind to
    this.$nextTick(() => {
      this.cm = CodeMirror.fromTextArea(
        document.getElementById(`editor_${this.id}`),
        {
          autoRefresh: true,
          lineNumbers: true,
          autoCloseTags: true,
          lineWrapping: true,
          theme: this.$vuetify.theme.dark ? "nord" : "duotone-light",
          mode: "text/x-java",
          defaultTextHeight: 32,
          placeholder: "",
        }
      );
      this.cm.refresh();
      this.cm.on("change", (cm) => {
        this.content = cm.getValue();
      });
    });
  },
  unmounted() {
    this.cm.toTextArea();
  },
};
</script>

<style>
.CodeMirror {
  border-radius: 5px;
  line-height: 32px;
  padding: 10px;
  max-width: 800px;
  height: auto;
}
</style>