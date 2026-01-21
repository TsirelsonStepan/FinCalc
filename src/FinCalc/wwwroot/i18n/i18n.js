const translations = {};

async function loadTranslations() {
    translations["en"] = await fetch("/i18n/en.json").then(res => res.json()),
    translations["ru"] = await fetch("/i18n/ru.json").then(res => res.json())
    localStorage.setItem("lang", "en");
    applyTranslations();
}
loadTranslations();

function applyTranslations(root = document) {
    const lang = localStorage.getItem("lang");
    const dict = translations[lang];
    document.getElementById("lang-select").value = lang;

    root.querySelectorAll("[data-i18n]").forEach(el => {
        const key = el.dataset.i18n;
        if (dict[key]) {
            el.textContent = dict[key];
        }
    });
}

document.getElementById("lang-select").addEventListener("change", e => {
    localStorage.setItem("lang", e.target.value);
    applyTranslations();
});