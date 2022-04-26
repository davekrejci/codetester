<template>
  <v-container fluid>
    <v-breadcrumbs :items="breadcrumbs" class="pa-0 pb-4 pl-1"></v-breadcrumbs>
    <h1 class="ml-1 mb-6 mt-0">Moje Testy</h1>
      <v-card outlined class="rounded">
        <v-tabs v-model="tab" class="">
          <v-tab>
            <!-- <v-badge
              class="mr-8 font-weight-bold"
              v-if="myExams.length"
              :content="myExams.length"
              offset-x="0"
              offset-y="10"
            ></v-badge> -->
            Otevřené
          </v-tab>
          <v-tab>Dokončené</v-tab>
      </v-tabs>
      </v-card>
      <v-card flat class="transparent">
      <v-tabs-items v-model="tab" class="transparent">
        <v-tab-item> 
          <exams-list :exams="openExams"></exams-list>
        </v-tab-item>
        <v-tab-item> 
          <exams-list :exams="closedExams" isClosed></exams-list>
        </v-tab-item>
      </v-tabs-items>
      </v-card>
    <default-snackbar
      :type="snackbar.type"
      :text="snackbar.text"
      v-on:close-snackbar="error = null"
    ></default-snackbar>
  </v-container>
</template>

<script>
import DefaultSnackbar from "@/components/DefaultSnackbar.vue";
import ExamsList from "./ExamsList.vue";
import moment from 'moment';

export default {
  name: "MyExams",
  components: {
    ExamsList,
    DefaultSnackbar,
  },
  data() {
    return {
      tab: null,
      loading: false,
      error: null,
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
        console.log(error);
        this.error = error;
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
    snackbar() {
      if (this.error != null) {
        return {
          type: "error",
          text: this.error.toString(),
          show: true,
        };
      }
      return {
        show: false,
      };
    },
  },
  created() {
    this.fetchMyExams();
  },
};
</script>
