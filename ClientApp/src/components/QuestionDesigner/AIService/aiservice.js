import codeFromDescriptionContext from './codeFromDescriptionContext.txt';
import descriptionFromCodeContext from './descriptionFromCodeContext.txt';

import { translate } from "./translator.js";
import Vue from "vue";

export const AiService = new Vue({
    methods: {
        async getCodeFromDescription(description, settings) {
            // translate description to english
            console.log("description: " + description);
            let translated = description;
            if (settings.translate) {
                try{
                    translated = await translate(description, "cs", "en");
                } catch(e) {
                    console.log("translation failed, proceeding with original description");
                    this.$emit("translation-failed");
                }
                console.log("translated: " + translated);
            }
            // get the completion context and add description
            let input = codeFromDescriptionContext + translated + "\n";
        
            console.log("the input " + input);
        
            // get completion
            let completion = await _getCompletion(input, settings);
            completion = completion.trim();
        
            console.log("the completion " + completion);
        
            return completion;
        },

        async getDescriptionFromCode(code, settings) {
            // get the completion context and add description
            let input = descriptionFromCodeContext + code;
        
            console.log("the input " + input);
        
            // get completion
            let completion = await _getCompletion(input, settings);
            completion = completion.trim();
        
            console.log("the completion " + completion);

            let translated = completion;
            if(settings.translate) {
                try{
                    translated = await translate(completion, "en", "cs");
                } catch(e) {
                    console.log(e);
                    console.log("translation failed, proceeding with original completion");
                    this.$emit("translation-failed");
                    return completion;
                }
            }
            return translated;
        }
    }
  });

async function _getCompletion(prompt, settings) {
    let response = await fetch("https://api.ai21.com/studio/v1/j1-large/complete", {
    headers: {
        "Authorization": `Bearer ${process.env.VUE_APP_AI21_API_KEY}`,
        "Content-Type": "application/json"
    },
    body: JSON.stringify({
        "prompt": prompt,
        "numResults": 1,
        "maxTokens": settings.maxLength,
        "stopSequences":["Q:"],
        "topKReturn": settings.topP,
        "temperature": settings.temperature,
    }),
    method: "POST"
    });
    let result = await response.json();
    console.log(result);
    let completion = result.completions[0].data.text;
    return completion;
}