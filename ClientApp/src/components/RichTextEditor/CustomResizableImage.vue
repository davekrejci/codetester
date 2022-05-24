<template>
  <node-view-wrapper
    as="span"
    class="image-container"
  >
    <img
      v-bind="node.attrs"
      ref="resizableImg"
      :draggable="isDraggable"
      :data-drag-handle="isDraggable"
      :class="{ active: isSelected && isEditable }"
      @mouseover="isSelected = true"
      @mouseout="isSelected = false"
    />
    <v-icon
      v-if="isEditable"
      class="ml-n3 resize-icon hidden outlined"
      ref="icon"
      @mouseover="isSelected = true"
      @mousedown="onMouseDown"
    >
      mdi-resize-bottom-right
    </v-icon>
    
  </node-view-wrapper>
</template>

<script>
import { NodeViewWrapper, nodeViewProps } from "@tiptap/vue-2";

export default {
  components: {
    NodeViewWrapper,
  },

  props: nodeViewProps,

  data: () => ({
    isResizing: false,
    isSelected: false,
    lastMovement: {},
    count: 0,
  }),

  computed: {
    isEditable() {
      return this.editor?.view?.editable;
    },
    isDraggable() {
      return this.node?.attrs?.isDraggable && this.isEditable;
    },
  },

  watch: {},

  mounted() {},

  methods: {
    onMouseDown(e) {
      e.preventDefault();

      this.isResizing = true;

      window.addEventListener("mousemove", this.throttle(this.onMouseMove));

      window.addEventListener("mouseup", this.onMouseUp);
    },

    onMouseUp() {
      // e.preventDefault();

      this.isResizing = false;

      this.lastMovement = {};

      window.removeEventListener("mousemove", this.throttle(this.onMouseMove));

      window.removeEventListener("mouseup", this.onMouseUp);
    },

    throttle(fn, wait = 60, leading = true) {
      let prev, timeout, lastargs;

      return (...args) => {
        lastargs = args;

        if (timeout) return;

        timeout = setTimeout(
          () => {
            timeout = null;

            prev = Date.now();

            // let's do this ... we'll release the stored args as we pass them through

            fn.apply(this, lastargs.splice(0, lastargs.length));

            // some fancy timing logic to allow leading / sub-offset waiting periods
          },

          leading ? (prev && Math.max(0, wait - Date.now() + prev)) || 0 : wait
        );
      };
    },

    onMouseMove(e) {
      e.preventDefault();

      if (!this.isResizing) {
        return;
      }

      if (!Object.keys(this.lastMovement).length) {
        this.lastMovement = { x: e.x, y: e.y };

        return;
      }

      if (e.x === this.lastMovement.x && e.y === this.lastMovement.y) {
        return;
      }

      let nextX = e.x - this.lastMovement.x;

      let nextY = e.y - this.lastMovement.y;

      const width = this.$refs.resizableImg.width + nextX;

      const height = this.$refs.resizableImg.height + nextY;

      this.lastMovement = { x: e.x, y: e.y };

      this.updateAttributes({ width, height });
    },
  },
};
</script>

<style lang="scss" scoped>
.image-container:hover {
  .hidden {
    visibility: visible !important;
  }
}

.image-container {
  overflow: hidden;

  position: relative;
}

.active {
  outline: 3px solid var(--v-primary-base);
}

.resize-icon {
  position: absolute;
  bottom: 5px;
  right: 1px;
  color: #fff;
}
.active .resize-icon {
  // color: var(--v-primary-base);
  font-size: 40px;
}

::v-deep.resize-icon {
  cursor: se-resize !important;
}
</style>