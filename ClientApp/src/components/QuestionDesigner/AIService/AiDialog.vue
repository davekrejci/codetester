<template>
  <!-- a vuetify dialog with tabs -->
  <v-dialog v-model="aiDialog" max-width="800px" @click:outside="closeDialog">
    <v-toolbar flat dense>
      <!-- <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon> -->
      <v-toolbar-title><v-icon>mdi-memory</v-icon>AI Pomocník</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn class="mr-1" small icon @click="showSettings = true">
        <v-icon>mdi-cog</v-icon>
      </v-btn>
      <v-btn class="mr-1" small icon @click="closeDialog()">
        <v-icon>mdi-close</v-icon>
      </v-btn>
      <template v-slot:extension>
        <v-tabs v-model="tab" show-arrows color="primary">
          <v-tab :disabled="loading" class="text-capitalize">
            <v-icon class="mr-1">mdi-text</v-icon> Popis → Kód
          </v-tab>
          <v-tab :disabled="loading" class="text-capitalize">
            <v-icon class="mr-1">mdi-code-braces</v-icon> Kód → Popis
          </v-tab>
          <v-spacer></v-spacer>
        </v-tabs>
      </template>
    </v-toolbar>
    <v-card min-height="400" height="100%" class="flat rounded-t-0 pa-4 pb-8">
      <v-tabs-items v-model="tab" class="px-4">
        <v-tab-item class="pa-0">
          <v-card flat class="ma-0">
            <v-card-title>Vytvořit kód z textového popisu</v-card-title>
            <v-card-text>
              <v-textarea
                v-model="textToCode.text"
                outlined
                :disabled="loading"
                auto-grow
                hide-details
                clearable
                no-resize
                placeholder=" "
                name="input-7-4"
                label="Textový popis"
              ></v-textarea>
              <p class="text-caption mt-1">
                <v-icon small class="mr-1">mdi-information-outline</v-icon>Pro
                lepší výsledek zkuste zadat textový popis v angličtině.
              </p>
              <v-alert
                v-model="translationAlert"
                dismissible
                dense
                text
                border="left"
                type="warning"
                color="orange"
                class="my-1"
              >
                Nastala chyba při překladu. Pro lepší výsledek zkuste zadat
                anglický popis.
              </v-alert>
              <div class="d-flex justify-center">
                <v-btn
                  :disabled="loading"
                  :loading="loading"
                  x-large
                  outlined
                  class="my-2"
                  @click="transformTextToCode()"
                >
                  <v-icon>mdi-arrow-down</v-icon>
                </v-btn>
              </div>
              <simple-code-editor ref="textToCodeEditor" id="text-to-code">
                <copy-button :textToCopy="textToCode.code"></copy-button>
              </simple-code-editor>
            </v-card-text>
          </v-card>
        </v-tab-item>
        <v-tab-item class="pa-0">
          <v-card flat class="ma-0">
            <v-card-title>Vytvořit textový popis z kódu</v-card-title>
            <v-card-text>
              <simple-code-editor
                ref="codeToTextEditor"
                id="code-to-text"
                @contentChanged="
                  (content) => {
                    codeToText.code = content;
                  }
                "
              ></simple-code-editor>
              <v-alert
                v-model="translationAlert"
                dismissible
                dense
                text
                border="left"
                type="warning"
                color="orange"
                class="my-1"
              >
                Nastala chyba při překladu. Vrácen popis v originálu.
              </v-alert>
              <div class="d-flex justify-center">
                <v-btn
                  :disabled="loading"
                  :loading="loading"
                  x-large
                  outlined
                  class="my-2"
                  @click="transformCodeToText()"
                >
                  <v-icon>mdi-arrow-down</v-icon>
                </v-btn>
              </div>
              <v-textarea
                v-model="codeToText.text"
                outlined
                :disabled="loading"
                :loading="loading"
                auto-grow
                hide-details
                no-resize
                name="input-7-4"
                label="Textový popis"
                placeholder=" "
              >
                <template v-slot:append>
                  <copy-button :textToCopy="codeToText.text"></copy-button>
                </template>
              </v-textarea>
            </v-card-text>
          </v-card>
        </v-tab-item>
      </v-tabs-items>
      <v-dialog v-model="showSettings" max-width="400px">
        <v-card min-height="400" height="100%" class="flat pa-4">
          <v-card-title class="mb-4">Nastavení</v-card-title>
          <v-btn
            absolute
            top
            right
            class="mr-1"
            small
            icon
            @click="showSettings = false"
          >
            <v-icon>mdi-close</v-icon>
          </v-btn>
          <v-card-text>
            <v-label
              >Max délka
              <v-btn
                class="mb-1"
                x-small
                text
                @click="hints.maxLength = !hints.maxLength"
              >
                <v-icon small class="">mdi-information-outline</v-icon>
              </v-btn>
            </v-label>
            <v-expand-transition>
              <p v-show="hints.maxLength" class="text-caption">
                Maximální počet tokenů k vygenerování. Model má celkovou
                kapacitu 2048 tokenů, včetně vstupu a vygenerovaného výstupu. To
                odpovídá v průměru 2300 - 2500 slovům.
              </p>
            </v-expand-transition>
            <v-row no-gutters class="mt-2">
              <v-col cols="3">
                <v-text-field
                  v-model="settings.maxLength"
                  hide-details
                  single-line
                  outlined
                  dense
                  type="number"
                />
              </v-col>
              <v-col cols="9">
                <v-slider
                  v-model="settings.maxLength"
                  class="align-center mt-2"
                  :max="2048"
                  :min="1"
                  thumb-label
                ></v-slider>
              </v-col>
            </v-row>

            <v-divider class="mb-4"></v-divider>
            <v-label
              >Teplota
              <v-btn
                class="mb-1"
                x-small
                text
                @click="hints.temperature = !hints.temperature"
              >
                <v-icon small>mdi-information-outline</v-icon>
              </v-btn>
            </v-label>
            <v-expand-transition>
              <p v-show="hints.temperature" class="text-caption">
                Řídí náhodnost vzorkování. Zvýšení teploty má tendenci vést k
                rozmanitějším a kreativnějším výsledkům. Jeho snížení přináší
                stabilnější a opakující se výsledky. Nulová teplota účinně činí
                výsledek deterministickým.
              </p>
            </v-expand-transition>
            <v-row no-gutters class="mt-2">
              <v-col cols="3">
                <v-text-field
                  v-model="settings.temperature"
                  hide-details
                  single-line
                  outlined
                  dense
                  type="number"
                />
              </v-col>
              <v-col cols="9">
                <v-slider
                  v-model="settings.temperature"
                  class="align-center mt-2"
                  :max="1"
                  :min="0"
                  step="0.01"
                  thumb-label
                ></v-slider>
              </v-col>
            </v-row>
            <v-divider class="mb-4"></v-divider>
            <v-label
              >Top P
              <v-btn
                class="mb-1"
                x-small
                text
                @click="hints.topP = !hints.topP"
              >
                <v-icon small>mdi-information-outline</v-icon>
              </v-btn>
            </v-label>
            <v-expand-transition>
              <p v-show="hints.topP" class="text-caption">
                Percentil pravděpodobnosti, ze kterého jsou tokeny vzorkovány.
                Hodnota nižší než 1 znamená, že se bere v úvahu pouze
                odpovídající horní percentil možností a že méně pravděpodobné
                možnosti nebudou generovány, což vede ke stabilnějším a
                opakujícím se výsledkům.
              </p>
            </v-expand-transition>
            <v-row no-gutters class="mt-2">
              <v-col cols="3">
                <v-text-field
                  v-model="settings.topP"
                  hide-details
                  single-line
                  outlined
                  dense
                  type="number"
                />
              </v-col>
              <v-col cols="9">
                <v-slider
                  v-model="settings.topP"
                  class="align-center mt-2"
                  :max="1"
                  :min="0"
                  step="0.01"
                  thumb-label
                ></v-slider>
              </v-col>
            </v-row>
            <v-divider class="mb-4"></v-divider>
            <v-label
              >Překládat text
              <v-btn
                    class="mb-1"
                    x-small
                    text
                    @click="hints.translate = !hints.translate"
                >
                    <v-icon small>mdi-information-outline</v-icon>
                </v-btn>
            </v-label>
            <v-expand-transition>
            <p v-show="hints.translate" class="text-caption">
                Překládá výsledný text z/do českého jazyka. Jelikož model funguje v anglickém jazyce, je doporučeno používat překládání pokud chcete využít zadávání čí generování v češtině. Výsledky překladu mohou být horší než originální anglický text.
            </p>
            </v-expand-transition>
            <v-checkbox
            v-model="settings.translate"
            :label="settings.translate? 'Text je překládán': 'Text není překládán'"
            ></v-checkbox>
          </v-card-text>
        </v-card>
      </v-dialog>
    </v-card>
  </v-dialog>
</template>

<script>
import { AiService } from "./aiservice.js";
import SimpleCodeEditor from "./SimpleCodeEditor.vue";
import CopyButton from "@/components/CopyButton.vue";

// method that returns a random integer between min and max

export default {
  name: "AiDialog",
  components: {
    SimpleCodeEditor,
    CopyButton,
  },
  props: {
    aiDialog: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      showSettings: false,
      translationAlert: false,
      loading: false,
      tab: 0,
      textToCode: {
        text: "",
        code: "",
      },
      codeToText: {
        code: "",
        text: "",
      },
      settings: {
        maxLength: 150,
        temperature: 0.5,
        topP: 0.1,
        translate: true
      },
      hints: {
        maxLength: false,
        temperature: false,
        topP: false,
        translate: false
      }
    };
  },
  methods: {
    closeDialog() {
      this.$emit("update:aiDialog", false);
    },
    async transformTextToCode() {
      AiService.$on("translation-failed", () => {
        this.translationAlert = true;
      });
      this.loading = true;
      try {
        this.textToCode.code = await AiService.getCodeFromDescription(
          this.textToCode.text,
          this.settings
        );
        this.$refs.textToCodeEditor.setContent(this.textToCode.code);
      } catch (e) {
        console.log(e);
      }
      this.loading = false;
    },
    async transformCodeToText() {
      AiService.$on("translation-failed", () => {
        this.translationAlert = true;
      });
      this.loading = true;
      try {
        this.codeToText.text = await AiService.getDescriptionFromCode(
          this.codeToText.code,
          this.settings
        );
      } catch (e) {
        console.log(e);
      }
      this.loading = false;
    },
  },
};
</script>

<style scoped>
.border-right {
  border-right: 1px solid rgba(0, 0, 0, 0.12);
}
</style>