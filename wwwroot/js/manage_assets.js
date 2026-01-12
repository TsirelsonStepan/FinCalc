const assetsList = document.getElementById("selected-assets");
var selectedAssets = [];

function addAsset(asset) {
	if (selectedAssets.includes(asset)) return;
	asset.amount = 0;
	const wrapper = document.createElement("div");
	wrapper.className = "selected-asset-item";
	wrapper.innerHTML = Templates.selected_asset_item;

	wrapper.querySelector("#short_name").textContent = asset.shortname;

	const amountInput = wrapper.querySelector(".asset-amount-input");
	amountInput.addEventListener("input", () => {
		const amount = amountInput.value.trim();
		if (amount === "") asset.amount = 0;
		else asset.amount = parseInt(amount);
	});

	wrapper.querySelector(".delete-btn").addEventListener("click", () => {
		selectedAssets = selectedAssets.filter(x => x != asset);
		wrapper.remove();
	});

	selectedAssets.push(asset);
	assetsList.appendChild(wrapper);
}