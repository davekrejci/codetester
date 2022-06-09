// Just simple request to free online translation service. Might need changing in future / limited
async function translate(text, sourceLanguage, targetLanguage) {

    //try API #1
    let res = await fetch(`https://api.mymemory.translated.net/get?q=${text}&langpair=${sourceLanguage + "|" + targetLanguage}`, {
        method: "GET"
    });

    const result = await res.json();
    console.log(result);

    if(result.responseData) {
        return result.responseData.translatedText;
    }
    if(!result.responseData) {
        // try API #2
        res = await fetch("https://libretranslate.de/translate", {
            method: "POST",
            body: JSON.stringify({
                q: text,
                source: sourceLanguage,
                target: targetLanguage,
                format: "text"
            }),
            headers: { "Content-Type": "application/json" }
        });
        let parsed = await res.json()
        console.log(parsed);
        if (parsed.error) {
            throw new Error(parsed.error);
        }
        return parsed.translatedText;
    }
}
export { translate }