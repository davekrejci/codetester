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
          class="shrink"
          dense
            ></v-text-field>
        </v-card-title>
        <v-data-table
        :headers="headers"
        :items="users"
        :items-per-page="5"
        :search="search"
        :loading="loading"
        loading-text="Načítání dat..."
        >
        <template v-slot:[`item.actions`]="{ item }">
            <router-link :to="{ name: 'UserDetail', params: { id: item.id } }">
        <v-btn fab depressed x-small color="primary" class="mx-1">
                <v-icon
                small
                @click="editItem(item)"
                >
                    mdi-magnify
                </v-icon>
        </v-btn>
            </router-link>
        <v-btn fab depressed x-small color="error" class="mx-1">
            <v-icon
                small
                @click="deleteItem(item)"
            >
                mdi-delete
            </v-icon>
        </v-btn>
        </template>
        </v-data-table>
    </v-card>



  </div>
</template>

<script>
export default {
    name: 'Users',
    data () {
      return {
          search: '',
          loading: false,
          headers: [
            { text: "Univerzitní číso", value: "universityNumber" },
            { text: "Email", value: "email" },
            { text: "Jméno", value: "firstName" },
            { text: "Příjmení", value: "lastName" },
            { text: "Typ", value: "type" },
            { text: "Akce", value: "actions", sortable: false }
          ],
      }
    },
    computed: {
        users () {
            return this.$store.state.users
        }
    },
    created() {
        this.loading = true
        this.$store.dispatch('fetchUsers')
        .then(() => {
            this.loading = false
        })
    },
    
  
}
</script>

<style>

</style>