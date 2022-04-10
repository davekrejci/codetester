<template>
  <v-dialog v-model="show" max-width="400px" @click:outside="$emit('close-dialog')">
    <v-card class="text-center">
      <v-btn
        absolute
        top
        right
        class="mr-1"
        icon
        small
        @click="$emit('close-dialog')"
      >
        <v-icon>mdi-close</v-icon>
      </v-btn>
      <div class="pa-4">
        <v-icon v-if="icon" :color="color" x-large>
          {{ icon }}
        </v-icon>
        <div class="mt-4 mb-2 text-center text-body-1 font-weight-bold">
            <slot name="title"></slot>
        </div>
        <v-card-text><slot name="text"></slot></v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="grey"
            class="mx-2 px-4"
            text
            @click="$emit('close-dialog')"
          >
            Zrušit
          </v-btn>
          <v-btn
            :color="color"
            class="mx-2 px-4"
            outlined
            @click="confirmAction"
          >
            {{ confirmationButtonText }}
          </v-btn>
          <v-spacer></v-spacer>
        </v-card-actions>
      </div>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: "DefaultConfirmationDialog",
  props: {
    show: {
      type: Boolean,
      default: false,
    },
    color: {
      type: String,
      default: "primary",
    },
    icon: {
        type: String
    },
    confirmAction: {
        type: Function,
        required: true
    },
    confirmationButtonText: {
        type: String,
        default: "Potvrdit"
    }
  },
};
</script>

<style>
</style>