<template>
  <v-tooltip content-class="pa-0 result-tooltip" top :disabled="isCorrect">
    <template v-slot:activator="{ on }">
      <input
        v-on="on"
        v-model="answer"
        type="text"
        :class="['px-2','editablesection', colorClass, resultColor]"
        :style="{ width: width + 'px' }"
        :maxlength="length"
        :data-wid="id"
        spellcheck="false"
        readonly
      />
    </template>
    <v-card color="success" flat outlined class="ma-0 py-0 px-3">
        <span class="white--text font-weight-medium">{{ correctAnswer }}</span>
    </v-card>
  </v-tooltip>
</template>

<script>
export default {
  name: "ResultInputWidget",
  props: {
    id: String,
    length: Number,
    isCorrect: Boolean,
    answer: String,
    correctAnswer: String,
  },
  data() {
    return {
      width: "0",
    };
  },
  mounted() {
    this.width = this.getTextWidth(this.length);
  },
  computed: {
    colorClass() {
      return this.$vuetify.theme.dark ? "dark" : "light";
    },
    resultColor() {
      if (this.isCorrect) {
        return "resultCorrect";
      }
      return "resultIncorrect";
    },
    isFilled() {
      return this.text.length > 0;
    },
  },
  methods: {
    getTextWidth(textlength) {
      // re-use canvas object for better performance
      const canvas =
        this.getTextWidth.canvas ||
        (this.getTextWidth.canvas = document.createElement("canvas"));
      const context = canvas.getContext("2d");
      context.font = this.getCanvasFontSize();
      const dummyText = "#".repeat(textlength);
      const metrics = context.measureText(dummyText);
      return metrics.width;
    },

    getCssStyle(element, prop) {
      return window.getComputedStyle(element, null).getPropertyValue(prop);
    },

    getCanvasFontSize() {
      const fontWeight = "normal";
      const fontSize = "16px";
      const fontFamily = "monospace";
      return `${fontWeight} ${fontSize} ${fontFamily}`;
    },
  },
};
</script>

<style>
.editablesection {
  height: 26px;
  border-radius: 5px;
  padding-left: 5px;
  padding-right: 5px;
  cursor: pointer;
  user-select: none; /* supported by Chrome and Opera */
  -webkit-user-select: none; /* Safari */
  -khtml-user-select: none; /* Konqueror HTML */
  -moz-user-select: none; /* Firefox */
  -ms-user-select: none; /* Internet Explorer/Edge */
}
.editablesection:focus {
  outline: none;
}
.light {
  background: rgb(235, 235, 235);
}
.dark {
  background: rgb(95, 93, 93);
  color: white;
}
.resultCorrect {
  color: white;
  background-color: var(--v-success-base);
}
.resultIncorrect {
  color: white;
  background-color: var(--v-error-base);
}
.result-tooltip {
    opacity: 1!important;
    color: white;
    background-color: var(--v-success-base);
    

}
.editablesection:placeholder-shown {
  border: 2px dashed #333;
}
</style>