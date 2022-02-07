<template>
  <textarea v-model="content" :id="'editor_' + id"></textarea>
</template>

<script>
import * as CodeMirror from 'codemirror';
import Vue from 'vue';
import InputWidget from './InputWidget.vue';
import 'codemirror/lib/codemirror.css';
import 'codemirror/theme/dracula.css';
import 'codemirror/theme/duotone-light.css';
import 'codemirror/theme/material-palenight.css';
import 'codemirror/theme/eclipse.css';
import 'codemirror/mode/clike/clike.js';
import 'codemirror/addon/search/searchcursor';

const WidgetComponentClass = Vue.extend(InputWidget);
export default {
    name: 'FillBlankCodeEditor',
    data(){
        return {
            //content: `public {"widget_id":"1234", "length":5} HelloWorld(){ \n\t{"widget_id":"1235", "length":6} String name = "david";\n\tSystem.out.{"widget_id":"1236", "length":7}("Hello, " + name); \n}`,
        };
    },
    props:{
        content: {
            default: "No content",
            type: String
        },
        id: {
            default: 0,
            type: Number
        }
    },
    mounted(){
        console.log("mounting editor");
        this.cm = CodeMirror.fromTextArea(document.getElementById('editor_'+this.id), {
            //lineNumbers: true,
            theme: 'duotone-light',
            //theme: 'material-palenight',
            mode: 'text/x-java',
            readOnly: "nocursor",
            defaultTextHeight:32,
        }),
        this.widgetREGEX = /\{"widget_id":"(\w+)", "length":(\w+)\}/;
        this.replaceShortcode(this.widgetREGEX, this.cm,{line: 0, ch: 0});
        
    },
    unmounted() {
        console.log("unmounting editor");
        this.cm.toTextArea();
    },
    methods: {
        replaceShortcode(widgetREGEX, editor, loc) {
            const cursor = editor.getSearchCursor(widgetREGEX, loc, {multiline: 'disable'})

            while (cursor.findNext()) {
                const markers = editor.findMarks(cursor.from(), cursor.to())
                if (markers.length === 0) {
                    const from = cursor.from()
                    const to = cursor.to()
                    const match = editor.getRange(from, to)

                    const parsedObj = JSON.parse(match);
                    
                    const widgetInstance = new WidgetComponentClass({
                        propsData: { 
                            id: parsedObj.id,
                            length: parsedObj.length,
                        }
                    });
                    widgetInstance.$mount();
                    editor.markText(from, to, {replacedWith: widgetInstance.$el});
                    editor.refresh();
                }
            }
        },
    },
}
</script>

<style>

.CodeMirror{
    border-radius: 10px;
    line-height: 32px;
    padding:10px;
    max-width: 800px;
    height: auto;
}
</style>