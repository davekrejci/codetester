<template>
  <div v-if="editor" class="pa-0">
    <v-toolbar dense outlined elevation="0" class="rich-text-toolbar rounded rounded-b-0">
      <div>
      <!-- Undo -->
      <v-tooltip top>
        <template v-slot:activator="{ on }">
          <v-btn
            text
            x-small
            v-on="on"
            class="py-5 px-2 toolbar-button mb-1"
            @click="editor.commands.undo()"
            :disabled="!editable"
          >
            <v-icon color="grey">mdi-undo</v-icon>
          </v-btn>
        </template>
        <span>Zpět</span>
      </v-tooltip>

      <!-- Redo -->
      <v-tooltip top>
        <template v-slot:activator="{ on }">
          <v-btn
            text
            x-small
            v-on="on"
            class="py-5 px-2 toolbar-button mb-1"
            @click="editor.commands.redo()"
            :disabled="!editable"
          >
            <v-icon color="grey">mdi-redo</v-icon>
          </v-btn>
        </template>
        <span>Vpřed</span>
      </v-tooltip>

      <!-- Bold -->
      <v-tooltip top>
        <template v-slot:activator="{ on }">
          <v-btn
            text
            x-small
            v-on="on"
            class="py-5 px-2 toolbar-button mb-1"
            @click="editor.chain().focus().toggleBold().run()"
            :input-value="editor.isActive('bold')"
            :disabled="!editable"
          >
            <v-icon color="grey">mdi-format-bold</v-icon>
          </v-btn>
        </template>
        <span>Tučně</span>
      </v-tooltip>

      <!-- Italic -->
      <v-tooltip top>
        <template v-slot:activator="{ on }">
          <v-btn
            text
            x-small
            v-on="on"
            class="py-5 px-2 toolbar-button mb-1"
            @click="editor.chain().focus().toggleItalic().run()"
            :input-value="editor.isActive('italic')"
            :disabled="!editable"
          >
            <v-icon color="grey">mdi-format-italic</v-icon>
          </v-btn>
        </template>
        <span>Kurzíva</span>
      </v-tooltip>

      <!-- Strike -->
      <v-tooltip top>
        <template v-slot:activator="{ on }">
          <v-btn
            text
            x-small
            v-on="on"
            class="py-5 px-2 toolbar-button mb-1"
            @click="editor.chain().focus().toggleStrike().run()"
            :input-value="editor.isActive('strike')"
            :disabled="!editable"
          >
            <v-icon color="grey">mdi-format-strikethrough</v-icon>
          </v-btn>
        </template>
        <span>Přeškrtnuté</span>
      </v-tooltip>

      <!-- Bullet List -->
      <v-tooltip top>
        <template v-slot:activator="{ on }">
          <v-btn
            text
            x-small
            v-on="on"
            class="py-5 px-2 toolbar-button mb-1"
            @click="editor.chain().focus().toggleBulletList().run()"
            :input-value="editor.isActive('bulletList')"
            :disabled="!editable"
          >
            <v-icon color="grey">mdi-format-list-bulleted</v-icon>
          </v-btn>
        </template>
        <span>Bodový seznam</span>
      </v-tooltip>

      <!-- Numbered List -->
      <v-tooltip top>
        <template v-slot:activator="{ on }">
          <v-btn
            text
            x-small
            v-on="on"
            class="py-5 px-2 toolbar-button mb-1"
            @click="editor.chain().focus().toggleOrderedList().run()"
            :input-value="editor.isActive('orderedList')"
            :disabled="!editable"
          >
            <v-icon color="grey">mdi-format-list-numbered</v-icon>
          </v-btn>
        </template>
        <span>Číslovaný seznam</span>
      </v-tooltip>

      <!-- Headings -->
      <v-menu
        origin="top right"
        content-class="elevation-3 "
        offset-y 
      >
        <template v-slot:activator="{ on: onMenu }">
          <v-tooltip top>
            <template v-slot:activator="{ on: onTooltip }">
              <v-text-field
                hide-details
                v-model="selectedHeading"
                solo flat
                readonly
                class="px-1 d-inline-flex toolbar-select"
                v-on="{ ...onMenu, ...onTooltip }"
                :disabled="!editable"
              >
                <template v-slot:append>
                  <v-icon v-on="{ ...onMenu, ...onTooltip }" :disabled="!editable">mdi-menu-down</v-icon>
                </template>
              </v-text-field>
            </template>
            <span>Úroveň nadpisu</span>
          </v-tooltip>
          
        </template>

        <v-list>
          <v-list-item @click="editor.chain().focus().setParagraph().run()">
            <span>Odstavec</span>
          </v-list-item>
          <v-list-item @click="editor.chain().focus().setHeading({ level: 1 }).run()">
            <h1>Nadpis 1</h1>
          </v-list-item>
          <v-list-item @click="editor.chain().focus().setHeading({ level: 2 }).run()">
            <h2>Nadpis 2</h2>
          </v-list-item>
          <v-list-item @click="editor.chain().focus().setHeading({ level: 3 }).run()">
            <h3>Nadpis 3</h3>
          </v-list-item>
        </v-list>
      </v-menu>

      <!-- Codeblock -->
      <v-tooltip top>
        <template v-slot:activator="{ on }">
          <v-btn
            text
            x-small
            v-on="on"
            class="py-5 px-2 toolbar-button mb-1"
            @click="editor.chain().focus().toggleCodeBlock().run()"
            :input-value="editor.isActive('codeBlock')"
            :disabled="!editable"
          >
            <v-icon color="grey">mdi-code-tags</v-icon>
          </v-btn>
        </template>
        <span>Blok kódu</span>
      </v-tooltip>

      <!-- Link -->
      <v-tooltip top>
        <template v-slot:activator="{ on }">
          <v-btn
            text
            x-small
            v-on="on"
            class="py-5 px-2 toolbar-button mb-1"
            @click="showLinkDialog()"
            :disabled="!editable"
          >
            <v-icon color="grey">mdi-link</v-icon>
          </v-btn>
        </template>
        <span>Odkaz</span>
      </v-tooltip>

      <!-- Image upload -->
      <v-tooltip top>
        <template v-slot:activator="{ on }">
          <v-btn
            text
            x-small
            v-on="on"
            class="py-5 px-2 toolbar-button mb-1"
            @click="showUploadImageDialog()"
            :disabled="!editable"
          >
            <v-icon color="grey">mdi-image-outline</v-icon>
          </v-btn>
        </template>
        <span>Obrázek</span>
      </v-tooltip>
      </div>
        
    </v-toolbar>
    <v-card outlined elevation="0" class="rounded rounded-t-0 no-top-border">
      <editor-content :editor="editor" />
    </v-card>
    <upload-image-dialog
      :uploadImageDialog.sync="uploadImageDialog"
      @insertImage="addImage"
    ></upload-image-dialog>
    <link-dialog :linkDialog.sync="linkDialog" @addLink="addLink" :textSelection="selectedText"></link-dialog>
  </div>
</template>

<script>
import { Editor, EditorContent } from "@tiptap/vue-2";
import StarterKit from "@tiptap/starter-kit";
import Link from "@tiptap/extension-link";
import Placeholder from '@tiptap/extension-placeholder'
import CustomImageExtension from "@/components/RichTextEditor/CustomImageExtension.js";
import UploadImageDialog from "@/components/RichTextEditor/UploadImageDialog.vue";
import LinkDialog from "@/components/RichTextEditor/LinkDialog.vue";

export default {
  name: "RichTextEditor",
  components: {
    EditorContent,
    UploadImageDialog,
    LinkDialog
  },

  props: {
    value: {
      type: String,
      default: "",
    },
    placeholder: {
      type: String,
      default: "",
    },
    editable: {
      type: Boolean,
      default: true
    }
  },

  data() {
    return {
      editor: null,
      uploadImageDialog: false,
      linkDialog: false,
    };
  },
  computed: {
    selectedHeading() {
      if(this.editor.isActive('heading', { level: 1 })) return "Nadpis 1";
      if(this.editor.isActive('heading', { level: 2 })) return "Nadpis 2";
      if(this.editor.isActive('heading', { level: 3 })) return "Nadpis 3";
      return "Odstavec"
    },
    selectedText() {
        const state = this.editor.view.state
        const selection = this.editor.view.state.selection
        const { from, to } = selection
        const text = state.doc.textBetween(from, to)
        return text;
    }
  },
  methods: {
    showUploadImageDialog() {
      this.editor.commands.blur();
      this.uploadImageDialog = true;
    },
    showLinkDialog() {
      // this.editor.commands.blur();
      this.linkDialog = true;
    },
    setHeading(level) {
      if(level == 0) {
        this.editor.chain().focus().setParagraph().run()
      } else {
        this.editor.chain().focus().toggleHeading({ level: level }).run()
      }
    },
    addImage(data) {
      console.log("inserting image: 2");
      console.log(data);
      this.editor.chain().focus().setImage(data).run();
    },
    addLink(data) {
      this.editor
        .chain()
        .focus()
        .extendMarkRange('link')
        .setLink({ href: data.href })
        .command(({ tr }) => {
          tr.insertText(data.text)
          return true
        })
        .run();
    }
  },

  watch: {
    value(value) {
      // HTML
      const isSame = this.editor.getHTML() === value

      // JSON
      // const isSame = JSON.stringify(this.editor.getJSON()) === JSON.stringify(value)

      if (isSame) {
        return
      }

      this.editor.commands.setContent(value, false)
    },
    editable(value){
        this.editor.setOptions({editable : value})
    }
  },

  mounted() {
    this.editor = new Editor({
      editable: this.editable,
      content: this.value,
      extensions: [
        StarterKit,
        CustomImageExtension.configure({
          allowBase64: true,
        }),
        Link,
        Placeholder.configure({
          // Use a placeholder:
          placeholder: this.placeholder,
          // Use different placeholders depending on the node type:
          // placeholder: ({ node }) => {
          //   if (node.type.name === 'heading') {
          //     return 'What’s the title?'
          //   }

          //   return 'Can you add some further context?'
          // },
        })
      ],
      onUpdate: () => {
        // HTML
        this.$emit('input', this.editor.getHTML())
        // JSON
        // this.$emit('input', this.editor.getJSON())
      },
    });
  },

  beforeDestroy() {
    this.editor.destroy();
  },
};
</script>

<style>
.no-top-border {
  border-top: none !important;
}
.toolbar-button {
  margin: 0px 2px;
}
.rich-text-toolbar{
    display: flex;
    height: auto!important;
    padding: 5px;
}
.rich-text-toolbar .v-toolbar__content{
    height: auto!important;
    flex-wrap: wrap;
    padding: 0;
}

/* Scoped to the editor */
.ProseMirror p {
  margin: 0px;
}
.ProseMirror pre code {
  padding: 0.75rem 1rem;
}

/* Placeholder (at the top) */
.ProseMirror p.is-editor-empty:first-child::before {
  content: attr(data-placeholder);
  float: left;
  color: #adb5bd;
  pointer-events: none;
  height: 0;
}
.ProseMirror {
  padding: 20px;
  outline: none !important;
}
.ProseMirror-focused {
  border: none !important;
}
.toolbar-select{
  max-width: 130px;
}
.toolbar-select input{
  cursor: pointer !important;
}
.toolbar-select .v-input__control{
  min-height: 30px !important;
}
.toolbar-select .v-input__control .v-input__slot{
  cursor: pointer !important;
  min-height: 30px !important;
  background-color: rgba(0, 0, 0, 0.05) !important;
  font-size: 14px;
}
</style>