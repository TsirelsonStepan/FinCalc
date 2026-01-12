const searchInput = document.getElementById("assets-search-input");
const searchResults = document.getElementById("search-results");
const assetsSearch = searchResults.parentElement;

async function searchAssets() {
	const query = searchInput.value.trim();
	if (!query) {
		searchResults.innerHTML = `<p data-i18n="no_search_query_mes"></p>`;
		applyTranslations(assetsSearch);
		return;
	}
	if (query.length === 1) {
		searchResults.innerHTML = `<p data-i18n="only_one_symbol_query_mes"></p>`;
		applyTranslations(assetsSearch);
		return;
	}

	searchResults.innerHTML = `<p data-i18n="searching_mes"></p>`;
	applyTranslations(assetsSearch);
	
	const response = await fetch(`/getSecuritiesList?partialName=${query}`);
	if (!response.ok) {
		searchResults.innerHTML = `<p data-i18n="searching_error_mes"></p>`;
		applyTranslations(assetsSearch);
		throw new Error(response.status);
	}
	const short_names = await response.json();
	renderSearchResults(short_names);
}

function renderSearchResults(assets) {
	searchResults.innerHTML = "";

	if (assets.length === 0) {
		searchResults.innerHTML = `<p data-i18n="searching_no_assets_mes"></p>`;
		applyTranslations(assetsSearch);
		return;
	}

	assets.forEach(asset => {
		const wrapper = document.createElement("div");
		wrapper.className = "search-result-item";
		wrapper.innerHTML = Templates.search_result_item;
		wrapper.querySelector("#short_name").textContent = asset.shortname;
		//wrapper.querySelector("#secid").textContent = asset.secid;
		
		wrapper.querySelector(".add-btn").addEventListener("click", () => {
			addAsset(asset);
		});

		wrapper.querySelector(".tooltip").addEventListener("mouseover", e => {
			tooltip.innerHTML = `<span>${asset.description}</span>`;
			const rect = e.target.closest(".tooltip").getBoundingClientRect();
			tooltip.style.left = rect.left + "px";
			tooltip.style.top = rect.bottom + 6 + "px";
			
			tooltip.style.display = "block";
		});
		
		wrapper.querySelector(".tooltip").addEventListener("mouseout", () => {
			tooltip.style.display = "none";
		});

		searchResults.appendChild(wrapper);
	});
}

document.getElementById("search-btn").addEventListener("click", searchAssets);
searchInput.addEventListener("keydown", function(e) {
	if (e.key === "Enter") {
		e.preventDefault();
		searchAssets();
    }
});

const tooltip = document.createElement("div");
tooltip.className = "tooltip-text";
document.querySelector(".main").appendChild(tooltip);