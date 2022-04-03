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
            <router-link :to="{ name: 'CreateExam' }">
              <v-btn
                class="mx-2"
                depressed
                fab
                small
                color="primary"
                v-bind="attrs"
                v-on="on"
              >
                <v-icon dark> mdi-plus </v-icon>
              </v-btn>
            </router-link>
          </template>
          <span>Přidat Test</span>
        </v-tooltip>
      </v-card-title>
      <v-data-table
        :headers="headers"
        dense
        :items="exams"
        :items-per-page="15"
        :search="search"
        :loading="loading"
        loading-text="Načítání dat..."
        no-data-text="Žádné data"
        item-key="exam"
        v-model="selected"
        :custom-filter="customFilter"
      >
        <template v-slot:[`item.startDate`]="{ item }">
          <v-tooltip top content-class="custom-tooltip">
            <template v-slot:activator="{ on, attrs }">
              <div v-bind="attrs" v-on="on">
                {{ moment.utc(item.startDate).local().format("LLL") }}
              </div>
            </template>
            <span>{{ moment.utc(item.startDate).local().fromNow() }}</span>
          </v-tooltip>
        </template>
        <template v-slot:[`item.endDate`]="{ item }">
          <v-tooltip top content-class="custom-tooltip">
            <template v-slot:activator="{ on, attrs }">
              <div v-bind="attrs" v-on="on">
                {{ moment.utc(item.endDate).local().format("LLL") }}
              </div>
            </template>
            <span>{{ moment.utc(item.endDate).local().fromNow() }}</span>
          </v-tooltip>
        </template>
        <template v-slot:[`item.status`]="{ item }">
          <v-chip label small :color="statusIndicatorColor(item.status)" outlined>
            <v-icon class="mr-0" left :color="statusIndicatorColor(item.status)">
              mdi-circle-medium
            </v-icon>
            {{ item.status }}
          </v-chip>
        </template>
        <template v-slot:[`item.tags`]="{ item }">
          <div
            v-if="item.tags.length > 0"
            class="text-truncate"
            style="max-width: 400px"
          >
            <v-slide-group show-arrows="always">
              <v-chip
                small
                v-for="tag in item.tags"
                :key="tag.tagText"
                class="mx-1"
              >
                {{ tag.tagText }}
              </v-chip>
            </v-slide-group>
          </div>
        </template>
        <template v-slot:[`item.semester`]="{ item }">
          <span>{{
            item.semester.year + " " + item.semester.semesterType.displayText
          }}</span>
        </template>
        <template v-slot:[`item.actions`]="{ item }">
          <v-row
            align="center"
            justify="space-around"
            class="d-flex flex-nowrap"
          >
            <router-link
              :to="{ name: 'Exam', params: { id: item.id } }"
              style="text-decoration: none; color: inherit"
            >
              <v-btn small plain icon color="primary" class="mx-1">
                <v-icon> mdi-magnify </v-icon>
              </v-btn>
            </router-link>
            <v-btn
              @click="showDeleteDialogForExam(item)"
              small
              icon
              plain
              color="error"
              class="mx-1"
            >
              <v-icon> mdi-delete </v-icon>
            </v-btn>
          </v-row>
        </template>
      </v-data-table>
    </v-card>

    <!-- Delete Dialog -->
    <v-dialog v-model="showDeleteDialog" max-width="400px">
      <v-card class="text-center pa-4">
        <v-icon color="error" x-large>mdi-alert-circle-outline</v-icon>
        <v-card-title class="text-h5">
          <!-- <span class="mx-auto my-4"> Jste si jistý?</span> -->
        </v-card-title>
        <v-card-text
          >Opravdu si přejete smazat test "<strong>{{
            examToDelete.name
          }}</strong
          >"? Tato akce je nevratná.</v-card-text
        >
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="grey"
            class="mx-2"
            outlined
            @click="showDeleteDialog = false"
          >
            Ne
          </v-btn>
          <v-btn
            color="error"
            class="mx-2"
            outlined
            @click="deleteExam(examToDelete.id)"
          >
            Ano
          </v-btn>
          <v-spacer></v-spacer>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <default-snackbar
      :type="snackbar.type"
      :text="snackbar.text"
      v-on:close-snackbar="error = null"
    ></default-snackbar>
  </div>
</template>

<script>
import api from "api-client";
import DefaultSnackbar from "@/components/DefaultSnackbar.vue";
import moment from "moment";

moment.locale("cs");

export default {
  name: "Exams",
  components: { DefaultSnackbar },
  data() {
    return {
      moment: moment,
      search: "",
      loading: false,
      error: null,
      hasBeenDeleted: false,
      examToDelete: {},
      showDeleteDialog: false,
      headers: [
        { text: "Jméno", value: "name" },
        { text: "Status", value: "status", width:"100px" },
        { text: "Předmět", value: "semester.course.courseCode" },
        { text: "Semestr", value: "semester" },
        { text: "Začátek", value: "startDate" },
        { text: "Konec", value: "endDate" },
        { text: "Tagy", value: "tags" },
        { text: "Akce", value: "actions", sortable: false, width: "15" },
      ],
      selected: [],
      breadcrumbs: [
        {
          text: "Testy",
          disabled: true,
        },
      ],
    };
  },
  methods: {
    customFilter(value, search, item) {
      let tagTexts = item.tags.map((tag) => tag.tagText);
      return (
        (value != null &&
          search != null &&
          typeof value !== "boolean" &&
          value
            .toString()
            .toLocaleLowerCase()
            .indexOf(search.toLocaleLowerCase()) !== -1) ||
        tagTexts.some((tagText) => tagText.includes(search))
      );
    },
    showDeleteDialogForExam(exam) {
      this.examToDelete = exam;
      this.showDeleteDialog = true;
    },
    async deleteExam(id) {
      this.hasBeenDeleted = false;
      this.examToDelete = {};
      this.showDeleteDialog = false;
      this.error = null;
      this.loading = true;
      try {
        await api.deleteExam(id);
        this.hasBeenDeleted = true;
      } catch (error) {
        this.error = error;
      }
      this.fetchExams();
    },
    async fetchExams() {
      this.loading = true;
      try {
        await this.$store.dispatch("fetchExams");
      } catch (error) {
        console.error(error, error.stack);
        this.error = error;
      }
      this.loading = false;
    },
    statusIndicatorColor(status) {
      switch(status) {
        case "planned":
          return "primary";
        case "open":
          return "warning"
        case "closed":
          return "success"
        default: return "grey"
      }
    }
  },
  computed: {
    exams() {
      return this.$store.state.exams;
    },
    snackbar() {
      if (this.error != null) {
        return {
          type: "error",
          text: this.error.toString(),
          show: true,
        };
      }
      if (this.hasBeenDeleted) {
        return {
          type: "success",
          text: "Test byl smazán",
          show: true,
        };
      }
      return {
        show: false,
      };
    },
  },
  created() {
    this.fetchExams();
  },
};
</script>

<style>
.custom-tooltip {
  opacity: 1 !important;
}
</style>