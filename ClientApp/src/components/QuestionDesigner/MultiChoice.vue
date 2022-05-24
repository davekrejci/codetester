<template>
  <div>
    <!-- <v-textarea
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
    ></v-textarea> -->
    <rich-text-editor
      class="mb-4" 
      :value="questionText" 
      placeholder="Zadání otázky..."
      @input="updateQuestionText"  
    ></rich-text-editor>

    <div v-for="(answer, index) in answers" :key="index" class="mb-2">
      <v-text-field
        outlined
        :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
        :success="answer.isCorrect"
        class="answerField"
        :label="'Odpověď ' + (index + 1)"
        dense
        prepend-icon="mdi-drag"
        hide-details
        :value="answer.text"
        @input="setMultiChoiceAnswerText({ index: index, value: $event })"
        :rules="[rules.required]"
      >
        <template v-slot:append>
          <v-tooltip bottom>
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                outlined
                small
                :disabled="answers.length < 2"
                @click="removeMultiChoiceAnswer(index)"
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
                @click="toggleMultiChoiceAnswerCorrect(index)"
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
        @click="addMultiChoiceAnswer()"
        :disabled="answers.length >= 5"
      >
        <span>Přidat odpověď </span>
        <v-icon small>mdi-plus</v-icon>
      </v-btn>
    </v-row>
  </div>
</template>

<script>
import { mapMutations } from "vuex";
import RichTextEditor from "@/components/RichTextEditor/RichTextEditor.vue";

export default {
  name: "MultiChoice",
  components: { RichTextEditor },
  data() {
    return {
      rules: {
        required: (value) => !!value || "Povinné.",
      },
    };
  },
  computed: {
    questionText: {
      get() {
        return this.$store.state.questionDesigner.multiChoice.questionText;
      },
      set(value) {
        this.$store.commit(
          "questionDesigner/setMultiChoiceQuestionText",
          value
        );
      },
    },
    answers: {
      get() {
        return this.$store.state.questionDesigner.multiChoice.answers;
      },
    },
  },
  methods: {
    ...mapMutations("questionDesigner", [
      "addMultiChoiceAnswer",
      "removeMultiChoiceAnswer",
      "toggleMultiChoiceAnswerCorrect",
      "setMultiChoiceAnswerText",
    ]),
    updateQuestionText(text) {
      this.questionText = text;
    }
  },
};
</script>

<style>
.answerButton {
  margin-top: -8px;
  border-color: #9e9e9e;
}
.leftButton {
  border-right: none;
}
.answerField > .v-input__control > .v-input__slot {
  padding-right: 0px !important;
}
</style>