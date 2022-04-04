<template>
  <v-container fluid>
    <v-card flat>
      <v-tabs v-model="tab" center-active class="rounded-b-0">
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
        <v-tab-item> 
          <exams-list :exams="openExams"></exams-list>
        </v-tab-item>
        <v-tab-item> 
          <exams-list :exams="closedExams" isClosed></exams-list>
        </v-tab-item>
      </v-tabs>
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
        return moment.utc(exam.exam.endDate).isAfter(this.$store.state.currentTimeUtc); 
      });
    },
    currentTimeUtc() {
      return this.$store.state.currentTimeUtc;
    },
    closedExams() {
      return this.myExams.filter(exam => {
        return moment.utc(exam.exam.endDate).isBefore(this.$store.state.currentTimeUtc); 
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
