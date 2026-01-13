const selectedAssets = document.getElementById("selected-assets");
const assetsList = selectedAssets.parentElement;
var selectedArr = [];

function addAsset(asset) {
	const newAsset = {market: currentActiveSegment.dataset.type, secid: asset.secid, amount: 0};
	selectedArr.forEach(element => {
		if (element.secid === newAsset.secid) return;
	});

	if (selectedArr.length === 0) {
		const topRaw = document.createElement("div");
		topRaw.className = "selected-asset-item";
		topRaw.innerHTML = Templates.selected_items_header;
		applyTranslations(topRaw);
		assetsList.insertBefore(topRaw, selectedAssets);
	}

	const wrapper = document.createElement("div");
	wrapper.className = "selected-asset-item";
	wrapper.innerHTML = Templates.selected_asset_item;

	wrapper.querySelector("#shortname").textContent = asset.shortname;

	const amountInput = wrapper.querySelector(".asset-amount-input");
	amountInput.addEventListener("input", () => {
		const amount = amountInput.value.trim();
		if (amount === "") newAsset.amount = 0;
		else newAsset.amount = parseInt(amount);
	});

	wrapper.querySelector(".delete-btn").addEventListener("click", () => {
		selectedArr = selectedArr.filter(x => x.secid != asset.secid);
		if (selectedArr.length === 0) {
			assetsList.querySelector(".selected-asset-item").remove();
		}
		wrapper.remove();
	});

	selectedArr.push(newAsset);
	selectedAssets.appendChild(wrapper);
}