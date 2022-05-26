<template>
  <v-container fluid>
    <v-breadcrumbs :items="breadcrumbs" class="pa-0 pb-4 pl-1"></v-breadcrumbs>
    <h1 class="ml-1 mb-6 mt-0">Vytvořené Testy</h1>    
    <!-- <v-card outlined class="rounded rounded-b-0 noBottomBorder">
    <v-tabs>
      <v-tab class="text-capitalize text-subtitle-2">Vše</v-tab>
      <v-tab class="text-capitalize text-subtitle-2">Naplánované</v-tab>
      <v-tab class="text-capitalize text-subtitle-2">Probíhající</v-tab>
      <v-tab class="text-capitalize text-subtitle-2">Hotové</v-tab>
    </v-tabs>
    </v-card> -->
    <v-card flat outlined class="pb-4 rounded rounded-t-0">
      <v-card-title class="mb-4">
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
        class="mx-4"
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
    <default-confirmation-dialog
      color="error"
      icon="mdi-alert-circle-outline"
      confirmationButtonText="Smazat"
      :show="showDeleteDialog"
      :confirmAction="deleteExam"
      @close-dialog="showDeleteDialog = false"
    >
      <template v-slot:title>
        Smazat test?
      </template>
      <template v-slot:text>
        Opravdu si přejete smazat test "<strong>{{ examToDelete.name }}</strong>"? Tato akce je nevratná.
      </template>
    </default-confirmation-dialog>
  </v-container>
</template>

<script>
import api from "api-client";
import moment from "moment";
import DefaultConfirmationDialog from '../../components/DefaultConfirmationDialog.vue';

moment.locale("cs");

export default {
  name: "Exams",
  components: { DefaultConfirmationDialog },
  data() {
    return {
      moment: moment,
      search: "",
      loading: false,
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
          text: "Management",
          disabled: true,
        },
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
    async deleteExam() {
      this.showDeleteDialog = false;
      this.loading = true;
      try {
        await api.deleteExam(this.examToDelete.id);
        this.$notify({
          title: "Úspěch",
          text: "Test byl smazán",
          type: "success",
        });
      } catch (error) {
        this.$notify({
          title: "Error",
          text: "Smazání testu se nepodařilo",
          type: "error",
        });
      }
      this.examToDelete = {};
      this.fetchExams();
    },
    async fetchExams() {
      this.loading = true;
      try {
        await this.$store.dispatch("fetchExams");
      } catch (error) {
        this.$notify({
          title: "Error",
          text: "Načtení testů se nepodařilo",
          type: "error",
        });
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
.noBottomBorder {
  border-bottom: none !important;
}
</style>