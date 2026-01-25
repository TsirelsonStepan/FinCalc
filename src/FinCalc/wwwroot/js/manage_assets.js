const selectedAssets = document.getElementById("selected-assets");
const otherSelectedAssets = document.getElementById("other-selected-assets");

const assetsList = selectedAssets.parentElement;
const otherAssetsList = otherSelectedAssets.parentElement;

var selectedArr = [];
var otherSelectedArr = [];

function addAsset(asset, toPortfolio) {
    if (toPortfolio && selectedArr.find(e => e.secid === asset.secid)) return;
	else if (!toPortfolio && otherSelectedArr.find(e => e.secid === asset.secid)) return;

	const newAsset = { api: "MOEX", market: currentActiveSegment.dataset.type, secid: asset.secid, amount: 0 };

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

	wrapper.querySelector("#shortname").textContent = asset.shortname;

	if (toPortfolio) {
		const amountInput = wrapper.querySelector(".asset-amount-input");
		amountInput.addEventListener("input", () => {
			const amount = amountInput.value.trim();
			if (amount === "") newAsset.amount = 0;
			else newAsset.amount = parseInt(amount);
		});
	}

	wrapper.querySelector(".delete-btn").addEventListener("click", () => {
		if (toPortfolio) selectedArr = selectedArr.filter(x => x.secid != asset.secid);
		else otherSelectedArr = otherSelectedArr.filter(x => x.secid != asset.secid);

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