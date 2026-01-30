const selectedAssets = document.getElementById("selected-assets");
const otherSelectedAssets = document.getElementById("other-selected-assets");

const assetsList = selectedAssets.parentElement;
const otherAssetsList = otherSelectedAssets.parentElement;

var selectedArr = [];
var otherSelectedArr = [];

function addAsset(asset, toPortfolio) {
	if (toPortfolio && selectedArr.find(e => e.source.api === asset.source.api && e.source.assetPath === asset.source.assetPath)) return;
	else if (!toPortfolio && otherSelectedArr.find(e => e.source.api === asset.source.api && e.source.assetPath === asset.source.assetPath)) return;

	const newAsset = { source: asset.source, amount: null };

	if (selectedArr.length === 0 && toPortfolio) {
		const topRaw = document.createElement("div");
		topRaw.className = "selected-asset-item";
		topRaw.innerHTML = Templates.selected_items_header;
		applyTranslations(topRaw);
		if (toPortfolio) assetsList.insertBefore(topRaw, selectedAssets);
	}

	const wrapper = document.createElement("div");
	if (toPortfolio) {
		wrapper.className = "selected-asset-item";
		wrapper.innerHTML = Templates.selected_asset_item;
	}
	else {
		wrapper.className = "other-selected-asset-item";
		wrapper.innerHTML = Templates.other_selected_asset_item;	
	}

	if (asset.shortname != null) wrapper.querySelector("#shortname").textContent = asset.shortname;
	else if (asset.name != null) wrapper.querySelector("#shortname").textContent = asset.name;

	if (toPortfolio) {
		const amountInput = wrapper.querySelector(".asset-amount-input");
		amountInput.addEventListener("input", () => {
			const amount = parseInt(amountInput.value.trim());
			if (Number.isInteger(amount)) newAsset.amount = amount;
			else amountInput.value = "";
		});
	}

	wrapper.querySelector(".delete-btn").addEventListener("click", () => {
		if (toPortfolio) selectedArr = selectedArr.filter(x => x.source != asset.source);
		else otherSelectedArr = otherSelectedArr.filter(x => x.source != asset.source);

		if (toPortfolio && selectedArr.length === 0) assetsList.querySelector(".selected-asset-item").remove();
		wrapper.remove();
	});

	if (toPortfolio) {
		selectedArr.push(newAsset);
		selectedAssets.appendChild(wrapper);
	}
	else {
		otherSelectedArr = []; //remove later needed to prevent several benchmarks
		otherSelectedArr.push(newAsset);
		otherSelectedAssets.innerHTML = "";
		otherSelectedAssets.appendChild(wrapper);
	}
}