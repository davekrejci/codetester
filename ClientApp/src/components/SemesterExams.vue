<template>
  <div>
    <v-card  flat class="pa-4 rounded-t-0">
      <v-card-title>
          <span>Vytvořené Testy</span>
          <v-spacer></v-spacer>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <router-link :to="{ name: 'CreateExam' }">
              <v-btn
                class="mx-2 text-right"
                depressed
                fab
                x-small
                color="primary"
                v-bind="attrs"
                v-on="on"
              >
                <v-icon dark> mdi-plus </v-icon>
              </v-btn>
            </router-link>
          </template>
          <span>Vytvořit test</span>
        </v-tooltip>
      </v-card-title>
      <v-data-table
        dense
        :headers="headers"
        :items="exams"
        :items-per-page="15"
        :search="search"
        :loading="loading"
        loading-text="Načítání dat..."
        no-data-text="Žádné data"
        item-key="exam"
        v-model="selected"
      >
      <template v-slot:[`item.startDate`]="{ item }">
        <v-tooltip top>
          <template v-slot:activator="{ on, attrs }">
            <div
            v-bind="attrs"
                v-on="on"
            >{{ moment.utc(item.startDate).local().format('LLL') }}</div>
          </template>
          <span>{{ moment.utc(item.startDate).local().fromNow() }}</span>
        </v-tooltip>
      </template>
      <template v-slot:[`item.endDate`]="{ item }">
        <v-tooltip top>
          <template v-slot:activator="{ on, attrs }">
            <div
            v-bind="attrs"
                v-on="on"
            >{{ moment.utc(item.endDate).local().format('LLL') }}</div>
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
              <v-chip small v-for="tag in item.tags" :key="tag.tagText" class="mx-1">
                {{ tag.tagText }}
              </v-chip>
            </v-slide-group>
          </div>
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
          >Opravdu si přejete smazat test? Tato akce je
          nevratná. </v-card-text
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
  </div>
</template>

<script>
//import api from "api-client";
import moment from 'moment';

moment.locale('cs');

export default {
  name: "SemesterExams",
  props: {
    exams: { type: Array, required: true },
  },
  data() {
    return {
      moment: moment,
      search: "",
      loading: false,
      examToDelete: {},
      showDeleteDialog: false,
      headers: [
        { text: "Jméno", value: "name" },
        { text: "Status", value: "status" },
        { text: "Začátek", value: "startDate" },
        { text: "Konec", value: "endDate" },
        { text: "Tagy", value: "tags" },
        { text: "Akce", value: "actions", sortable: false, width: "15" },
      ],
      selected: [],
    };
  },
  methods: {
    showDeleteDialogForExam(exam) {
      this.examToDelete = exam;
      this.showDeleteDialog = true;
    },
    async deleteExam() {
    //   this.userToRemove = {};
    //   this.showRemoveDialog = false;
    //   this.loading = true;
    //   try {
    //     await api.removeUserFromSemester(semesterId, userId);
    //     this.$emit('user-remove-success');
    //   } catch (error) {
    //     this.$emit('user-remove-error', error);
    //   }
    //   this.loading = false;
    //   // TODO: reload data
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
};
</script>

<style>
.v-snack__wrapper {
  max-width: none;
  min-width: 100%;
  margin: 0;
}
</style>