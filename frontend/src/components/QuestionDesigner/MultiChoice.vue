<template>
  <div>
    <v-textarea
      outlined
      auto-grow
      :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
      rows="1"
      clearable
      clear-icon="mdi-close-circle"
      label="Zadání otázky"
      v-model="questionText"
      class="mb-2"
      :rules="[rules.required]"
    ></v-textarea>

    <div v-for="(answer, index) in answers" :key="index" class="mb-2">
      <v-text-field
        outlined
        :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
        :success="answer.isCorrect"
        class="answerField"
        :label="'Odpověď ' + (index+1)"
        dense
        prepend-icon="mdi-drag"
        hide-details
        v-model="answer.text"
        :rules="[rules.required]"
      >
        <template v-slot:append>
          <v-tooltip bottom>
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                outlined
                small
                :disabled="answers.length < 2"
                @click="removeAnswer(index)"
                v-bind="attrs"
                v-on="on"
                height="40px"
                width="40px"
                tile
                class="answerButton leftButton"
              >
                <v-icon>mdi-close</v-icon>
              </v-btn>
            </template>
            <span>Odstranit</span>
          </v-tooltip>
          <v-tooltip bottom>
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                outlined
                small
                @click="toggleCorrect(index)"
                :color="answer.isCorrect ? 'green' : ''"
                v-bind="attrs"
                v-on="on"
                height="40px"
                width="40px"
                class="answerButton rounded-l-0"
              >
                <v-icon>mdi-check</v-icon>
              </v-btn>
            </template>
            <span>Označit správně</span>
          </v-tooltip>
        </template>
      </v-text-field>
    </div>
    <v-row class="justify-end mt-6">
      <v-btn
        outlined
        color="primary"
        class="mr-8"
        @click="addAnswer()"
        :disabled="answers.length >= 5"
      >
        <span>Přidat odpověď </span>
        <v-icon small>mdi-plus</v-icon>
      </v-btn>
    </v-row>
  </div>
</template>

<script>
export default {
  name: "MultiChoice",
  data() {
    return {
      questionText: "",
      answers: [
        {
          text: "",
          isCorrect: true,
        },
        {
          text: "",
          isCorrect: false,
        },
        {
          text: "",
          isCorrect: false,
        },
        {
          text: "",
          isCorrect: false,
        },
      ],
      rules: {
        required: (value) => !!value || "Povinné.",
      },
    };
  },
  methods: {
    addAnswer() {
      this.answers.push({
        text: "",
        isCorrect: false,
      });
    },
    removeAnswer(index) {
      this.answer = this.answers.splice(index, 1);

      //if only one answer remaining, it must be the correct answer
      if (this.answers.length < 2) {
        this.answers[0].isCorrect = true;
      }
    },
    toggleCorrect(index) {
      this.answers[index].isCorrect = !this.answers[index].isCorrect;
    },
  },
};
</script>

<style>
.answerButton {
    margin-top: -8px;
    border-color: #9E9E9E;
}
.leftButton {
    border-right: none;
}
.answerField > .v-input__control > .v-input__slot {
    padding-right: 0px !important;
}

</style>