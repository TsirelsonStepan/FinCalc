const translations = {
    en: {
        selected_assets: "Selected assets",
        calculate_for_this_assets: "Calculate for these assets",
        search_assets: "Search Assets",
        search: "Search",
        historic_data: "Historic Data",
        return_ratios: "Return Ratios",
        weighted_average_portfolio_return: "Weighted Average Portfolio Return",
        expected_portfolio_return: "Expected Portfolio Return",
        info_label: "Portfolio Calculator by Stephan Tsirelson (Pet-project)",
        this_project: "This Project",
        my_github: "My Github",
        my_linkedin: "My Linkedin"
    },

    ru: {
        selected_assets: "Выбранные Активы",
        calculate_for_this_assets: "Расчитать для этих активов",
        search_assets: "Найти активы",
        search: "Поиск",
        historic_data: "Исторические Значения",
        return_ratios: "Показатели Доходности",
        weighted_average_portfolio_return: "Средневзвешенная Доходность Портфеля",
        expected_portfolio_return: "Ожидаемая Доходность Портфеля",        
        info_label: "Финансовый Калькулятор Инвестиционного Портфеля",
        this_project: "Этот Проект",
        my_github: "Мой Github",
        my_linkedin: "Мой Linkedin"
    },
};

function getBrowserLanguage() {
    return navigator.language.split("-")[0];
}

function applyTranslations(lang, root = document) {
    const dict = translations[lang] || translations.en;

    root.querySelectorAll("[data-i18n]").forEach(el => {
        const key = el.dataset.i18n;
        if (dict[key]) {
            el.textContent = dict[key];
        }
    });
}

document.getElementById("lang-select").addEventListener("change", e => {
    applyTranslations(e.target.value);
    localStorage.setItem("lang", e.target.value);
});

let currentLang = localStorage.getItem("lang") || getBrowserLanguage();
applyTranslations(currentLang);