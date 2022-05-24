<template>
  <v-dialog
    :value="uploadImageDialog"
    max-width="600"
    width="600"
    @click:outside="closeDialog"
  >
    <v-card class="pa-2" min-height="300">
      <v-card-actions class="ma-0 pa-0">
        <v-spacer></v-spacer>
        <v-btn @click="closeDialog" icon>
          <v-icon id="close-button">mdi-close</v-icon>
        </v-btn>
      </v-card-actions>
      <h2 class="text-center mb-2 mt-n8">Přidat obrázek</h2>
      <div class="mx-16">
        <v-tabs fixed-tabs v-model="tab">
          <v-tab>
            <v-icon class="mr-2">mdi-link</v-icon>
            URL
          </v-tab>
          <v-tab>
            <v-icon class="mr-2">mdi-upload</v-icon>
            Nahrát
          </v-tab>
        </v-tabs>
        <v-tabs-items v-model="tab">
          <!-- URL Upload -->
          <v-tab-item>
            <v-card flat class="mx-4 my-8">
              <v-text-field
                v-model="imgSrc"
                outlined
                hide-details
                clearable
                label="URL Obrázku"
                class="mb-2"
                @change="imgLoadError = false"
              ></v-text-field>
              <v-alert v-if="imgLoadError" dense border="left" type="error">
                Obrázek nelze načíst z URL. Ujistěte se že máte správný odkaz.
              </v-alert>
              <v-img
                class="mx-auto"
                max-width="400"
                v-if="imgSrc && !imgLoadError"
                :src="imgSrc"
                @error="imageLoadError"
              ></v-img>
              <div class="text-center">
                <v-btn
                  outlined
                  text
                  color="primary"
                  class="mt-4 mx-auto text-centered"
                  v-if="imgSrc && !imgLoadError"
                  @click="insertImage(imgSrc)"
                  >Vložit</v-btn
                >
              </div>
            </v-card>
          </v-tab-item>

          <!-- File Upload -->
          <v-tab-item>
            <div
              v-cloak
              @drop.prevent="onDrop"
              @dragover.prevent="dragover = true"
              @dragenter.prevent="dragover = true"
              @dragleave.prevent="dragover = false"
              :class="{ dragover: dragover }"
              class="mt-8 pa-2 rounded"
            >
              <v-file-input
                counter
                show-size
                truncate-length="24"
                outlined
                accept="image/png, image/jpeg"
                :rules="rules"
                label="Přetáhněte soubor zde, nebo klikněte pro výběr."
                @change="onChange"
                clearable
                v-model="uploadedFile"
              ></v-file-input>
            </div>
            <v-card v-if="uploadedFile" class="mx-4 my-8" flat>
              <v-img
                v-if="imgBase64"
                :src="imgBase64"
                max-width="300"
                max-height="300"
                class="mx-auto"
              ></v-img>
              <div class="text-center">
                <v-btn
                  outlined
                  text
                  color="primary"
                  class="mt-4 mx-auto text-centered"
                  @click="insertImage(imgBase64)"
                >
                  Vložit
                </v-btn>
              </div>
            </v-card>
          </v-tab-item>
        </v-tabs-items>
      </div>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: "UploadImageDialog",
  props: {
    uploadImageDialog: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      tab: null,
      imgSrc: null,
      imgBase64: null,
      imgLoadError: false,
      dragover: false,
      uploadedFile: null,
      rules: [
        value => !value || value.size < 2000000 || 'Velikost musí být menší než 2 MB!',
      ],
    };
  },
  methods: {
    insertImage(src) {
      console.log("inserting image: 1");
      const data = {
          src: src
          // alt: "YOU CAN ADD ALT",
          // title: "YOU CAN ADD TITLE"
      };
      this.$emit("insertImage", data);
      this.closeDialog();
    },
    closeDialog() {
      this.$emit("update:uploadImageDialog", false);
      this.uploadedFile = null;
      this.imgBase64 = null;
      this.imgSrc = null;
      this.imgLoadError = false;
      this.tab = null;
    },
    imageLoadError() {
      this.imgLoadError = true;
    },
    onChange(file) {
      if(!file) {
        this.uploadedFile = null;
        this.imgBase64 = null;
      }
      else {
        this.uploadedFile = file;
        this.encodeImageFileAsURL(file);
      }
    },
    onDrop(e) {
      this.dragover = false;
      this.uploadedFile = e.dataTransfer.files[0];
      this.encodeImageFileAsURL(e.dataTransfer.files[0]);
    },
    encodeImageFileAsURL(file) {
      var reader = new FileReader();
      var self = this;
      reader.onloadend = function () {
        self.imgBase64 = reader.result;
      };
      reader.readAsDataURL(file);
    },
  },
};
</script>

<style scoped>
.dragArea {
  outline: 2px solid grey;
  outline-style: dashed;
}
.dragover {
  background-color: rgba(0, 0, 0, 0.1);
}
</style>
