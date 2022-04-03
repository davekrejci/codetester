<template>
  <div>
    <v-row class="px-4 pt-4">
      <h1>Moje testy</h1>
      <v-badge
        class="font-weight-bold"
        :content="exams.length"
        offset-x="0"
        offset-y="10"
      ></v-badge>
    </v-row>
    <v-row>
      <v-col cols="12" md="6">
        <ExamCard v-for="exam in exams" :key="exam.id" :exam="exam" />
      </v-col>
      <v-col cols="6"></v-col>
    </v-row>
  </div>
</template>

<script>
// @ is an alias to /src
import ExamCard from "@/components/ExamCard.vue";

export default {
  name: "MyExams",
  components: {
    ExamCard,
  },
  data: () => ({
    exams: [
      {
        id: 1,
        className: "NAVZO - Návrhové vzory",
        examName: "Semestrální test #1",
      },
      {
        id: 2,
        className: "EPARP - Multivláknové a paralelní programování",
        examName: "Průběžný test #4",
      },
      {
        id: 3,
        className: "Wapid - Webové API a strojová výměna dat",
        examName: "REST API",
      },
    ],
  }),
};
</script>
