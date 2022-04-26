<template>
  <div>
    <p class="mb-4">{{ question.codeDescription }}</p>
    <fill-blank-result-code-editor
      :content="question.code"
      :blocks="question.fillInCodeBlocks"
      :id="question.id"
    ></fill-blank-result-code-editor>
  </div>
</template>

<script>
import FillBlankResultCodeEditor from "@/components/FillBlankResultCodeEditor.vue";

export default {
  name: "FillInCodeQuestionResult",
  components: {
    FillBlankResultCodeEditor
  },
  props: {
    question: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {};
  },
  methods: {},
};
</script>

<style>
</style>