<template>
  <v-hover v-slot:default="{ hover }">
    <v-card
      max-width="600"
      outlined
      class="my-2 pa-1"
      flat
      :class="{ 'on-hover': hover }"
    >
      <div class="d-flex">
        <v-card-subtitle class="overline pb-2">
          <!-- <v-chip label> -->
          {{ exam.exam.semester.course.courseName }}
          <!-- </v-chip> -->
        </v-card-subtitle>
        <v-spacer></v-spacer>
        <div class="justify-end pa-4 caption">
          <v-icon class="mr-2">{{
            canBeStarted ? "mdi-clock-outline" : "mdi-lock-clock"
          }}</v-icon>
          <!-- <v-icon  class="mr-2">mdi-lock-clock</v-icon> -->
          <span> {{ formattedStartDate }} - {{ formattedEndDate }} </span>
        </div>
      </div>
      <v-card-title class="pt-0">
        {{ exam.exam.name }}
      </v-card-title>

      <v-card-text>
        <div v-if="exam.exam.description && descriptionLongerThanMax">
          <span v-if="summarizeCardText">
            {{ exam.exam.description | limitLengthTo(maxDescriptionLength) }}
          </span>
          <span v-else>
            {{ exam.exam.description }}
          </span>
          <v-btn
            v-if="summarizeCardText"
            small
            icon
            @click="summarizeCardText = false"
            ><v-icon>mdi-chevron-down</v-icon></v-btn
          >
          <v-btn
            v-if="!summarizeCardText"
            small
            icon
            @click="summarizeCardText = true"
            ><v-icon>mdi-chevron-up</v-icon></v-btn
          >
        </div>
        <div v-else-if="exam.exam.description && !descriptionLongerThanMax">
          {{ exam.exam.description }}
        </div>
        <div v-else>Bez popisku.</div>
      </v-card-text>

      <!-- <v-list-item three-line>
				<v-list-item-content>
				</v-list-item-content>
			</v-list-item> -->
      <v-card-actions v-if="!isClosed">
        <v-tooltip right :disabled="canBeStarted">
          <template v-slot:activator="{ on }">
            <div v-on="on" class="d-inline-block">
              <v-btn
                :disabled="!canBeStarted"
                outlined
                color="primary"
                :to="'/examinstance/' + exam.id"
                >Spustit test</v-btn
              >
            </div>
          </template>
          <span>Test bude možné spustit {{ formattedStartDate }}</span>
        </v-tooltip>
      </v-card-actions>
      <v-card-actions v-if="isClosed">
        <v-btn outlined color="primary" :to="'todo' + exam.id"
          >Zobrazit výsledky</v-btn
        >
      </v-card-actions>
    </v-card>
  </v-hover>
</template>

<script>
import moment from "moment";
moment.locale("cs");

export default {
  name: "ExamCard",
  props: {
    exam: {
      type: Object,
      required: true,
    },
    isClosed: {
      type: Boolean,
      required: true,
    },
  },
  data() {
    return {
      moment: moment,
      maxDescriptionLength: 300,
      summarizeCardText: true,
    };
  },
  filters: {
    limitLengthTo: function (value, size) {
      if (!value) return "";
      value = value.toString();

      if (value.length <= size) {
        return value;
      }
      return value.substr(0, size) + "...";
    },
  },
  computed: {
    descriptionLongerThanMax() {
      return this.exam.exam.description.length > this.maxDescriptionLength;
    },
    // summarizeCardText() {
    //   return this.exam.exam.description.length > this.maxDescriptionLength;
    // },
    startTimeUtc() {
      return moment.utc(this.exam.exam.startDate);
    },
    endTimeUtc() {
      return moment.utc(this.exam.exam.endDate);
    },
    currentTimeUtc() {
      return this.$store.state.currentTimeUtc;
    },
    formattedStartDate() {
      return moment
        .utc(this.exam.exam.startDate)
        .local()
        .format("DD.MM.yyyy HH:mm");
    },
    formattedEndDate() {
      let startDaySameAsEndDay = this.startTimeUtc.isSame(
        this.endTimeUtc,
        "date"
      );
      if (startDaySameAsEndDay) {
        return this.endTimeUtc.local().format("HH:mm");
      } else {
        return this.endTimeUtc.local().format("DD.MM.yyyy HH:mm");
      }
    },
    canBeStarted() {
      return this.currentTimeUtc >= this.startTimeUtc;
    },
  },
};
</script>

<style scoped>
.v-card {
  transition: opacity 0.8s ease-in-out;
}

/* .v-card:hover {
	border-color: var(--v-secondary-base) !important;
} */
/* .v-card:hover .v-list-item__title,
.v-card:hover .v-list-item__subtitle,
.v-card:hover div {
	color: var(--v-primary-base) !important;
} */
</style>
