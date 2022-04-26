<template>
  <v-card flat class="px-4">
    <v-card-title>
      <v-spacer></v-spacer>
      <v-text-field
        v-model="searchSelected"
        append-icon="mdi-magnify"
        placeholder="Vyhledat"
        single-line
        hide-details
        filled
        rounded
        dense
        class="shrink mx-4"
      ></v-text-field>
    </v-card-title>
    <v-data-table
      :headers="headers"
      dense
      :items="examInstances"
      :items-per-page="15"
      :search="searchSelected"
      :loading="loading"
      loading-text="Načítání dat..."
      no-data-text="Žádné výsledky."
      item-key="id"
    >
    <template
            v-slot:[`item.percentage`]="{ item }">
    {{ (item.score / item.maxScore) * 100 }}%
  </template>
      <template v-slot:[`item.actions`]="{ item }">
        <v-row align="center" justify="" class="d-flex flex-nowrap">
          <router-link
            :to="{ name: 'ExamResult', params: { id: item.id } }"
            style="text-decoration: none; color: inherit"
          >
            <v-btn small plain icon color="primary" class="mx-1">
              <v-icon> mdi-magnify </v-icon>
            </v-btn>
          </router-link>
        </v-row>
      </template>
    </v-data-table>
  </v-card>
</template>

<script>
export default {
  name: "ExamResults",
  props: {
    examInstances: {
      type: Array,
      default: () => new Array(),
    },
  },
  data() {
    return {
      search: "",
      searchSelected: "",
      loading: false,
      headers: [
        { text: "Uživatel", value: "user.username" },
        { text: "Jméno", value: "user.firstName" },
        { text: "Příjmení", value: "user.lastName" },
        { text: "Email", value: "user.email" },
        { text: "Body", value: "score" },
        { text: "Max", value: "maxScore" },
        { text: "Hodnocení", value: "percentage" },
        { text: "Akce", value: "actions", sortable: false },
      ],
    };
  },
};
</script>

<style>
</style>