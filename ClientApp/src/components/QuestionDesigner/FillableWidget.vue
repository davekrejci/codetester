<template>
  <!-- <input :value="content" placeholder=" " type="text" class="editablesection" :style="{width:width + 'px'}" :maxlength="length" :data-wid="id" spellcheck="false"> -->
    <v-chip
      color="primary"
      text-color="white"
      :close="deletable"
      label
      small
      class="unselectable"
      @click:close="removeWidget"
      >{{content}}</v-chip>
</template>

<script>
export default {
    props: {
        id: Number,
        length: Number,
        content: String,
        deletable: {
            type: Boolean,
            default: true
        }
    },
    data(){
        return{
            width: "0",
        }
    },
    mounted() {
        this.width = this.getTextWidth(this.length);
    },
    methods: {
        getTextWidth(textlength) {
            // re-use canvas object for better performance
            const canvas = this.getTextWidth.canvas || (this.getTextWidth.canvas = document.createElement("canvas"));
            const context = canvas.getContext("2d");
            context.font = this.getCanvasFontSize();
            const dummyText = '#'.repeat(textlength);
            const metrics = context.measureText(dummyText);
            return metrics.width;
        },
        
        getCssStyle(element, prop) {
            return window.getComputedStyle(element, null).getPropertyValue(prop);
        },
        
        getCanvasFontSize() {
            const fontWeight = 'normal';
            const fontSize = '16px';
            const fontFamily = 'monospace';
            return `${fontWeight} ${fontSize} ${fontFamily}`;
        },
        removeWidget(){
            this.$emit('removeWidget', this.id);
        }
    }
}
</script>

<style>
.editablesection{
    height: 26px;
    background: rgb(235,235,235);
    border-radius: 5px;
    padding-left: 5px;
    padding-right: 5px;
    
}
.editablesection:hover {
    
}
.unselectable{
  -webkit-user-select:none;
  -khtml-user-select:none;
  -moz-user-select:none;
  -ms-user-select:none;
  -o-user-select:none;
  user-select:none;
}
</style>