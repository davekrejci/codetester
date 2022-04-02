<template>
<v-container class="px-12">
    <div v-if="semester != null">
        <v-breadcrumbs :items="breadcrumbs" class="mb-4"></v-breadcrumbs>
      <v-form ref="semesterForm">
        <v-text-field
          outlined
          disabled
          :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
          label="Kód předmětu"
          :value="semester.course.courseCode"
          readonly
          color="grey"
        >
        </v-text-field>
        <v-row>
            <v-col>
 <v-text-field
          outlined
          :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
          label="Rok"
          :value="semester.year"
          readonly
          disabled
          color="grey"
          class="mb-4"
        >
        </v-text-field>
            </v-col>
            <v-col>
                <v-text-field
          outlined
          :background-color="$vuetify.theme.dark ? '#3D4351' : 'white'"
          label="Semestr"
          :value="formattedSemesterName"
          readonly
          disabled
          color="grey"
          class="mb-4"
        >
        </v-text-field>
            </v-col>
        </v-row>
      </v-form>
       
        <v-card outlined>
            <v-tabs 
                v-model="tab" 
                
                center-active
                
                class="rounded-b-0"
            >
                <v-tab>Studenti</v-tab>
                <v-tab>Testy</v-tab>
                <v-tab-item>
                    <semester-students 
                      :currentUsers="this.semester.enrolledStudents"
                      v-on:selectedUsersChanged="updateSelectedUsers"
                    ></semester-students>
                </v-tab-item>
                <v-tab-item>
                    <semester-exams :exams="this.semester.exams"></semester-exams>
                </v-tab-item>
            </v-tabs>
        </v-card>
        <!-- Action buttons -->
        <div class="mt-12">
        <v-dialog v-model="showDeleteDialog" max-width="400px">
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              v-bind="attrs"
              v-on="on"
              :loading="loading"
              :disabled="loading"
              color="error"
              outlined
              class="mr-4"
            >
              Smazat <v-icon right dark> mdi-trash-can-outline </v-icon>
            </v-btn>
          </template>
          <v-card class="text-center pa-4">
            <v-icon color="error" x-large>mdi-alert-circle-outline</v-icon>
            <v-card-title class="text-h5">
              <!-- <span class="mx-auto my-4"> Jste si jistý?</span> -->
            </v-card-title>
            <v-card-text
          >Opravdu si přejete smazat <strong>{{ formattedSemesterName }}</strong> semestr <strong>{{ semester.year }}</strong>? Tato akce
          je nevratná.</v-card-text
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
              <v-btn color="error" class="mx-2" outlined @click="deleteSemester()"> Ano </v-btn>
              <v-spacer></v-spacer>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-btn
            color="primary"
            :loading="loading"
            :disabled="loading"
            depressed
            class=""
            @click="updateSemester"
          >
            Uložit <v-icon right dark> mdi-plus-circle-outline </v-icon>
          </v-btn>
        </div>
    </div>
    <div v-else>not found</div>
    <default-snackbar
      :type="snackbar.type"
      :text="snackbar.text"
      v-on:close-snackbar="error = null"
    ></default-snackbar>
</v-container>
</template>

<script>
import api from "api-client";
import DefaultSnackbar from "@/components/DefaultSnackbar.vue";
import SemesterStudents from '@/components/SemesterStudents.vue';
import SemesterExams from '@/components/SemesterExams.vue';


export default {
  name: "Semester",
  components: { 
    DefaultSnackbar,
    SemesterStudents,
    SemesterExams
  },
  data() {
    return {
      semester: null,
      loading: false,
      hasSaved: false,
      error: null,
      showDeleteDialog: false,
      tab: null,
    };
  },
  methods: {
    validate() {
      return this.$refs.semesterForm.validate();
    },
    async fetchSemester() {
      this.error = this.semester = null;
      this.loading = true;
      try {
        
        this.semester = await api.fetchSemester(this.$route.params.id);
      } catch (error) {
        console.log(error);
        this.error = error;
      }
      this.loading = false;
    },
    async deleteSemester() {
      this.error = null;
      this.loading = true;
      try {
        await api.deleteSemester(this.$route.params.id);
        this.$router.push({
            name: "Course",
            params: {
              coursecode: this.semester.course.courseCode,
            },
          });
      } catch (error) {
        this.error = error;
      }
      this.loading = false;
    },
    updateSelectedUsers(users) {
      this.semester.enrolledStudents = users;
    },
    async updateSemester() {
      let isFormValid = this.validate();
      if (!isFormValid) return;
      this.hasSaved = false;
      this.loading = true;
      try {
        let userIds = this.semester.enrolledStudents.map((user) => user.id);
        let semesterUpdateDto = {
          userIds: userIds
        };
        await api.updateSemester(this.$route.params.id, semesterUpdateDto);
        this.hasSaved = true;
      } catch (error) {
        console.log(error);
        this.error = error;
      }
      this.loading = false;
      window.scrollTo(0, 0);
    },
  },
  computed: {
      snackbar() {
      if (this.error != null) {
        return {
          type: "error",
          text: this.error.toString(),
          show: true,
        };
      }
      if (this.hasSaved) {
        return {
          type: "success",
          text: "Změny byly uloženy",
          show: true,
        };
      }
      if (this.semesterHasBeenDeleted) {
        return {
          type: "success",
          text: "Semestr byl smazán",
          show: true,
        };
      }
      return {
        show: false,
      };
    },
    formattedSemesterName() {
        let formattedSemesterName = this.semester.semesterType;
        if(this.semester.semesterType == 'winter') {
            formattedSemesterName = 'Zimní';
        } else if (this.semester.semesterType == 'summer') {
            formattedSemesterName = 'Letní';
        }
        return formattedSemesterName
    },
    breadcrumbs() {
        
      return [
        {
          text: "Předměty",
          disabled: false,
          link: true,
          exact: true,
          to: { name: "Courses" },
        },
        {
          text: this.semester.course.courseCode,
          disabled: false,
          link: true,
          exact: true,
          to: {
            name: "Course",
            params: {
              coursecode: this.semester.course.courseCode,
            },
          },
        },
        {
          text: this.semester.year + " - " + this.formattedSemesterName,
          disabled: true,
        },
      ];
    },
  },
  created() {
    this.fetchSemester();
  },
};
</script>

<style>
</style>