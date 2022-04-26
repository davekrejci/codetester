<template>
  <div class="py-4">
    <ExamCard
      v-for="exam in exams"
      :key="exam.id"
      :exam="exam"
      :isClosed="isClosed"
    />
    <v-card class="transparent pa-8" flat v-if="exams.length == 0 && !isClosed">
      <v-row>
        <v-col>
          <h3 class="unselectable text-center font-weight-light italic mb-12">
            Gratulace. Máte vše hotové!
          </h3>
          <v-img
            class="mx-auto"
            src="@/assets/svg/illustrations/happy_news.svg"
            :max-width="illustrationWidth"
            alt=""
          />
        </v-col>
      </v-row>
    </v-card>
    <v-card class="transparent pa-8" flat v-if="exams.length == 0 && isClosed">
      <v-row>
        <v-col>
          <h3 class="unselectable text-center font-weight-light mb-12">
            Ještě nemáte hotové žádné testy!
          </h3>
          <v-img
            class="mx-auto"
            src="@/assets/svg/illustrations/empty.svg"
            :max-width="illustrationWidth + 200"
            alt=""
          />
        </v-col>
      </v-row>
    </v-card>
  </div>
</template>

<script>
import ExamCard from "@/components/ExamCard.vue";

export default {
  name: "ExamsList",
  components: {
    ExamCard,
  },
  props: {
    exams: Array,
    isClosed: Boolean,
  },
  computed: {
    illustrationWidth() {
      switch (this.$vuetify.breakpoint.name) {
        case "xs":
          return 400;
        case "sm":
          return 400;
        case "md":
          return 400;
        case "lg":
          return 500;
        case "xl":
          return 600;
        default:
          return 200;
      }
    },
  },
};
</script>

<style>
.unselectable {
  -webkit-user-select: none; /* Safari */
  -moz-user-select: none; /* Firefox */
  -ms-user-select: none; /* IE10+/Edge */
  user-select: none; /* Standard */
}
</style>