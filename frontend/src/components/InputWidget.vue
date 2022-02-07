<template>
  <input placeholder=" " type="text" :class="['editablesection', colorClass ]" :style="{width:width + 'px'}" :maxlength="length" :data-wid="id" spellcheck="false">
</template>

<script>
export default {
    props: {
        id: String,
        length: Number
    },
    data(){
        return{
            width: "0",
        }
    },
    mounted() {
        this.width = this.getTextWidth(this.length);
    },
    computed: {
        colorClass: function() { return this.$vuetify.theme.dark ? "dark" : "light" }
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
        }
    }
}
</script>

<style>
.editablesection{
    height: 26px;
    border-radius: 5px;
    padding-left: 5px;
    padding-right: 5px;
}
.light{
    background: rgb(235,235,235);
}
.dark{
    background: rgb(95, 93, 93);
    color: white;
}
.editablesection:placeholder-shown {
    border: 2px dashed #333;
}
</style>