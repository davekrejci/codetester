<template>
  <v-container fluid>
    <v-breadcrumbs :items="breadcrumbs" class="pa-0 pb-4 pl-1"></v-breadcrumbs>
    <h1 class="ml-1 mb-6 mt-0">Moje Testy</h1>
      <v-card outlined class="rounded">
        <v-tabs v-model="tab" class="">
          <v-tab>
            <v-badge
              class="mr-8 font-weight-bold"
              v-if="openExams.length"
              :content="openExams.length"
              offset-x="0"
              offset-y="10"
            ></v-badge>
            Otevřené
          </v-tab>
          <v-tab>Dokončené</v-tab>
          <v-progress-linear
            :active="loading"
            :indeterminate="loading"
            absolute
            bottom
            color="primary"
          ></v-progress-linear>
      </v-tabs>
      </v-card>
      <v-card v-if="!loading" flat class="transparent">
      <v-tabs-items v-model="tab" class="transparent">
        <v-tab-item> 
          <exams-list :exams="openExams"></exams-list>
        </v-tab-item>
        <v-tab-item> 
          <exams-list :exams="closedExams" isClosed></exams-list>
        </v-tab-item>
      </v-tabs-items>
      </v-card>
  </v-container>
</template>

<script>
import ExamsList from "./ExamsList.vue";
import moment from 'moment';

export default {
  name: "MyExams",
  components: {
    ExamsList,
  },
  data() {
    return {
      tab: null,
      loading: false,
      breadcrumbs: [
        {
          text: "Dashboard",
          disabled: true,
          to: "Dashboard",
        },
        {
          text: "Moje testy",
          disabled: true,
          to: "MyExams",
        },
      ],
    };
  },
  methods: {
    async fetchMyExams() {
      this.loading = true;
      try {
        await this.$store.dispatch("fetchMyExams");
      } catch (error) {
        this.$notify({
          title: "Error",
          text: "Načtení testů se nepodařilo",
          type: "error",
        });
      }
      this.loading = false;
    },
  },
  computed: {
    myExams() {
      return this.$store.state.myExams;
    },
    openExams() {
      return this.myExams.filter(exam => {
        return (moment.utc(exam.exam.endDate).isAfter(this.$store.state.currentTimeUtc) && exam.isCompleted == false); 
      });
    },
    currentTimeUtc() {
      return this.$store.state.currentTimeUtc;
    },
    closedExams() {
      return this.myExams.filter(exam => {
        return (moment.utc(exam.exam.endDate).isBefore(this.$store.state.currentTimeUtc) || exam.isCompleted == true); 
      });
    },
  },
  created() {
    this.fetchMyExams();
  },
};
</script>
