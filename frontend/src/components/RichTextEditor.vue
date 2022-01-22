<template>
  <div>
    <tiptap-vuetify 
    :card-props="{ outlined, color: 'transparent' }" 
    v-model="content" 
    :extensions="extensions"
    :toolbar-attributes="{ outlined, color: this.toolbarColor, dark: $vuetify.theme.dark }"
    :placeholder="placeholder"
     />
  </div>
</template>

<script>
import {
  TiptapVuetify,
  Heading,
  Bold,
  Italic,
  Strike,
  Underline,
  Code,
  Paragraph,
  BulletList,
  OrderedList,
  ListItem,
  Link,
  Blockquote,
  HardBreak,
  HorizontalRule,
  History,
} from "tiptap-vuetify";

export default {
  name: "RichTextEditor",
  components: { TiptapVuetify },
  props: ['placeholder'],
  data: () => ({
    extensions: [
      History,
      Blockquote,
      Link,
      Underline,
      Strike,
      Italic,
      ListItem,
      BulletList,
      OrderedList,
      [
        Heading,
        {
          options: {
            levels: [1, 2, 3],
          },
        },
      ],
      Bold,
      Code,
      HorizontalRule,
      Paragraph,
      HardBreak,
    ],
    content: "",
  }),
  computed: {
    toolbarColor: function () {
        return this.$vuetify.theme.dark ? '#2F333E' : '#F6F7F8';
      }
  }
};
</script>