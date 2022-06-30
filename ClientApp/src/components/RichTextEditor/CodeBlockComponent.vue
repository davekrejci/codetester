<template>
  <node-view-wrapper class="code-block">
    <pre><node-view-content as="code" /></pre>
    <div id="language-picker">
      <v-select
        :items="languages"
        dense
        solo
        hide-details
        contenteditable="false"
        v-model="selectedLanguage"
      >
      </v-select>
    </div>
  </node-view-wrapper>
</template>

<script>
import { NodeViewWrapper, NodeViewContent, nodeViewProps } from "@tiptap/vue-2";

export default {
  components: {
    NodeViewWrapper,
    NodeViewContent,
  },

  props: nodeViewProps,

  data() {
    return {
      languages: ['auto', ...this.extension.options.lowlight.listLanguages()],
    };
  },

  computed: {
    selectedLanguage: {
      get() {
        return this.node.attrs.language;
      },
      set(language) {
        this.updateAttributes({ language });
      },
    },
  },
  mounted() {
    this.node.attrs.language = 'java'
  }
};
</script>

<style lang="scss">
.code-block {
  position: relative;

  #language-picker {
    color: #333;
    background-color: rgba(50, 50, 50, 0.05);
    position: absolute;
    top: 5px;
    right: 5px;
    width: 100px;
    height: 30px;
    padding: 0;
    font-size: 8px;
    .v-input {
      .v-input__control {
        .v-input__slot {
          min-height: 0px !important;
          height: 30px;
        }
      }
    }
  }
}
.theme--dark {
  #language-picker {
    background-color: hsla(0, 0%, 100%, 0.1);
    color: #eee;
  }
}
</style>