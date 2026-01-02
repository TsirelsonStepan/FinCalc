const searchInput = document.getElementById("search-input");
const searchResults = document.getElementById("search-results");

async function searchAssets() {
	const query = searchInput.value.trim();
	if (!query) {
		searchResults.innerHTML = "<p>Please enter search query</p>";
		return;
	}

	searchResults.innerHTML = "<p>Searching...</p>";
	
	const response = await fetch(`/getSecuritiesList?partialName=${query}`);
	if (!response.ok) {
		searchResults.innerHTML = "<p>Error loading results</p>";
		throw new Error(response.status);
	}
	const short_names = await response.json();
	renderSearchResults(short_names);
}

function renderSearchResults(assets) {
	searchResults.innerHTML = "";

	if (assets.length === 0) {
		searchResults.innerHTML = "<p>No assets found</p>";
		return;
	}

	assets.forEach(asset => {
		const item = document.createElement("div");
		item.classList.add("search-result-item");

		item.innerHTML = `
			<span>${asset.shortname}</span>
			<button class="add-btn">Add</button>
		`;

		item.querySelector(".add-btn").addEventListener("click", () => {
			addAsset(asset);
		});

		searchResults.appendChild(item);
	});
}

document.getElementById("search-btn").addEventListener("click", searchAssets);
searchInput.addEventListener("keydown", function(e) {
	if (e.key === "Enter") {
		e.preventDefault(); // prevent any default behavior
		searchAssets();
    }
});