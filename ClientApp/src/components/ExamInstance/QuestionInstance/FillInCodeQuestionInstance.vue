<template>
  <div>
    <p class="mb-4">{{ question.codeDescription }}</p>
    <fill-blank-code-editor
      :content="question.code"
      :id="question.id"
        
    ></fill-blank-code-editor>
  </div>
</template>

<script>
import FillBlankCodeEditor from "@/components/FillBlankCodeEditor.vue";

export default {
  name: "FillInCodeQuestionInstance",
  components: {
    FillBlankCodeEditor
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