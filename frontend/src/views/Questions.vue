<template>
  <div>
    <v-card flat class="pa-4">
      <v-card-title>
        <v-text-field
          v-model="search"
          append-icon="mdi-magnify"
          placeholder="Vyhledat"
          single-line
          hide-details
          filled
          rounded
          dense
          class="shrink mx-4"
        ></v-text-field>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <router-link :to="{ name: 'QuestionDesigner' }">
              <v-btn
                class="mx-2"
                depressed
                fab
                small
                dark
                color="primary"
                v-bind="attrs"
                v-on="on"
              >
                <v-icon dark> mdi-plus </v-icon>
              </v-btn>
            </router-link>
          </template>
          <span>Přidat Otázku</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              class="mx-2"
              depressed
              fab
              small
              dark
              color="primary"
              v-bind="attrs"
              v-on="on"
            >
              <v-icon dark> mdi-cloud-upload </v-icon>
            </v-btn>
          </template>
          <span>Nahrát Otázky</span>
        </v-tooltip>
      </v-card-title>
      <v-data-table
        :headers="headers"
        :items="questions"
        :items-per-page="15"
        :search="search"
        :loading="loading"
        loading-text="Načítání dat..."
        item-key="question"
        v-model="selected"
      >
        <template v-slot:item.actions="{ item }">
          <router-link
            :to="{ name: 'Question', params: { id: item.id } }"
          >
            <v-btn fab depressed x-small color="primary" class="mx-1">
              <v-icon small> mdi-magnify </v-icon>
            </v-btn>
          </router-link>
          <v-btn fab depressed x-small color="error" class="mx-1">
            <v-icon small @click="deleteItem(item)"> mdi-delete </v-icon>
          </v-btn>
        </template>
        <template v-slot:item.tags="{ item }">
          <div class="text-truncate" style="max-width: 200px">
            <v-chip v-for="(tag) in item.tags" :key="tag.id" class="mx-1">
              {{ tag.tagText }}
            </v-chip>
          </div>
        </template>
      </v-data-table>
    </v-card>
  </div>
</template>

<script>
export default {
  name: "Questions",
  data() {
    return {
      search: "",
      loading: false,
      headers: [
        { text: "Id", value: "id" },
        { text: "Otázka", value: "questionText" },
        { text: "Typ", value: "questionType" },
        //{ text: "Obtížnost", value: "difficulty" },
        { text: "Tagy", value: "tags" },
        { text: "Akce", value: "actions", sortable: false },
      ],
      selected: [],
      breadcrumbs: [
        {
          text: "Otázky",
          disabled: true,
          to: "Questions",
        },
      ],
    };
  },
  computed: {
    questions() {
      let questions = this.$store.state.questions;
      questions.forEach(question => {
        if(question.questionType == "fill-in-code"){
          question.questionText = question.codeDescription
        }
      });
      return questions;
    },
  },
  created() {
    this.loading = true;
    this.$store.dispatch("fetchQuestions").then(() => {
      this.loading = false;
    });
  },
};
</script>

<style>
</style>