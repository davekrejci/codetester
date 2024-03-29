<template>
<v-container fluid class="">
    <div v-if="semester != null">
      <v-breadcrumbs :items="breadcrumbs" class="pa-0 pb-4 pl-1"></v-breadcrumbs>
      <h1 class="ml-1 mb-6 mt-0">{{semester.course.courseName}} - {{formattedSemesterName}} semestr {{semester.year}}</h1>
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
       
        <v-card outlined class="mb-1">
            <v-tabs 
                v-model="tab" 
                center-active
                class=""
            >
                <v-tab>Studenti</v-tab>
                <v-tab>Testy</v-tab>
            </v-tabs>
        </v-card>
        <v-card outlined>
          <v-tabs-items v-model="tab">
                <v-tab-item>
                    <semester-students 
                      :currentUsers="this.semester.enrolledStudents"
                      v-on:selectedUsersChanged="updateSelectedUsers"
                    ></semester-students>
                </v-tab-item>
                <v-tab-item>
                    <semester-exams
                      :exams="this.semester.exams"
                      v-on:exam-delete-success="handleExamDeleteSuccess" 
                      v-on:exam-delete-error="handleExamDeleteError" 
                    ></semester-exams>
                </v-tab-item>
          </v-tabs-items>
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
</v-container>
</template>

<script>
import api from "api-client";
import SemesterStudents from '@/components/SemesterStudents.vue';
import SemesterExams from '@/components/SemesterExams.vue';


export default {
  name: "Semester",
  components: {
    SemesterStudents,
    SemesterExams
  },
  data() {
    return {
      semester: null,
      loading: false,
      showDeleteDialog: false,
      tab: null,
    };
  },
  methods: {
    validate() {
      return this.$refs.semesterForm.validate();
    },
    async fetchSemester() {
      this.semester = null;
      this.loading = true;
      try {
        this.semester = await api.fetchSemester(this.$route.params.id);
      } catch (error) {
        this.$notify({
          title: "Error",
          text: "Semestr se nepodařilo načíst",
          type: "error",
        });
      }
      this.loading = false;
    },
    async deleteSemester() {
      this.loading = true;
      try {
        await api.deleteSemester(this.$route.params.id);
        this.$router.push({
            name: "Course",
            params: {
              coursecode: this.semester.course.courseCode,
            },
          });
        this.$notify({
          title: "Úspěch",
          text: "Semestr byl smazán",
          type: "success",
        });
      } catch (error) {
        this.$notify({
          title: "Error",
          text: "Semestr se nepodařilo smazat",
          type: "error",
        });
      }
      this.loading = false;
    },
    updateSelectedUsers(users) {
      this.semester.enrolledStudents = users;
    },
    async updateSemester() {
      let isFormValid = this.validate();
      if (!isFormValid) return;
      this.loading = true;
      try {
        let userIds = this.semester.enrolledStudents.map((user) => user.id);
        let semesterUpdateDto = {
          userIds: userIds
        };
        await api.updateSemester(this.$route.params.id, semesterUpdateDto);
        this.$notify({
          title: "Úspěch",
          text: "Změny byly uloženy",
          type: "success",
        });
      } catch (error) {
        this.$notify({
          title: "Error",
          text: "Změny se nepodařilo uložit",
          type: "error",
        });
      }
      this.loading = false;
      window.scrollTo(0, 0);
    },
    handleExamDeleteError() {
      this.$notify({
          title: "Error",
          text: "Test se nepodařilo smazat",
          type: "error",
        });
    },
    handleExamDeleteSuccess() {
      this.$notify({
          title: "Úspěch",
          text: "Test byl smazán",
          type: "success",
        });
      this.fetchSemester();
    }
  },
  computed: {
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
          text: "Management",
          disabled: true,
        },
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