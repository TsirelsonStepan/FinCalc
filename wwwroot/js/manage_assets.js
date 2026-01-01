const assetsList = document.getElementsByClassName("assets-list")[0];
var selectedAssets = [];

function addAsset(asset) {
	const item = document.createElement("div");
	item.classList.add("asset-item");
	item.id = asset.secid;
	asset.amount = 0;

	item.innerHTML = `
		<span>${asset.shortname}</span>
		<input type="number" 
			class="asset-amount-input">
		<span>rub.</span>
		<button class="delete-btn">X</button>
	`;

	const amountInput = item.querySelector(".asset-amount-input");

	amountInput.addEventListener("input", () => {
		const amount = amountInput.value.trim();
		if (amount != "") {
			asset.amount = parseInt(amount);
		}
		else asset.amount = 0;
	});

	item.querySelector(".delete-btn").addEventListener("click", () => {
		selectedAssets = selectedAssets.filter(x => x != asset);
		item.remove();
	});

	selectedAssets.push(asset);
	assetsList.appendChild(item);
}