const translations = {
    en: {
        selected_assets: "Selected assets",
        calculate_for_this_assets: "Calculate for these assets",
        search_assets: "Search Assets",
        search: "Search",
        historic_data: "Historic Data",
        return_ratios: "Return Ratios",
        no_search_query_mes: "Please enter search query",
        only_one_symbol_query_mes: "Query should contain at least 2 simbols",
        searching_mes: "Searching...",
        searching_error_mes: "Error loading results",
        searching_no_assets_mes: "No assets found",
        weighted_average_portfolio_return: "Weighted Average Portfolio Return",
        expected_portfolio_return: "Expected Portfolio Return",
        info_label: "Portfolio Calculator by Stephan Tsirelson (Pet-project)",
        this_project: "This Project",
        my_github: "My Github",
        my_linkedin: "My Linkedin",
        my_cv: "My CV",

    },

    ru: {
        selected_assets: "Выбранные Активы",
        calculate_for_this_assets: "Расчитать для этих активов",
        search_assets: "Найти активы",
        search: "Поиск",
        historic_data: "Исторические Значения",
        return_ratios: "Показатели Доходности",
        no_search_query_mes: "Введите поисковой запрос",
        only_one_symbol_query_mes: "Поисковой запрос должен содержать минимум 2 символа",
        searching_mes: "Поиск...",
        searching_error_mes: "Ошибка при выгрузке результатов",
        searching_no_assets_mes: "По данному запросу ничего не найдено",
        weighted_average_portfolio_return: "Средневзвешенная Доходность Портфеля",
        expected_portfolio_return: "Ожидаемая Доходность Портфеля",        
        info_label: "Финансовый Калькулятор Инвестиционного Портфеля",
        this_project: "Этот Проект",
        my_github: "Мой Github",
        my_linkedin: "Мой Linkedin",
        my_cv: "Моё резюме"
    },
};

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

applyTranslations();