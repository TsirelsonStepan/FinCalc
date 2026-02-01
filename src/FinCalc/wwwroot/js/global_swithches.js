async function switchFrequency(seg, wrapper) {
	seg.addEventListener("click", async () => {
		currentFrequency.classList.remove("active");
		seg.classList.add("active");
		wrapper.querySelector(".chart").innerHTML = "";
		
		[labels, data] = await getChartData(seg.dataset.freq, currentPeriod.dataset.period);
		mainChart.data.labels = labels;
		mainChart.data.datasets = data;
		mainChart.update();

		currentFrequency = seg;
	});
}

async function switchPeriod(seg, wrapper) {
	seg.addEventListener("click", async () => {
		currentPeriod.classList.remove("active");
		seg.classList.add("active");
		wrapper.querySelector(".chart").innerHTML = "";
		
		[labels, data] = await getChartData(currentFrequency.dataset.freq, seg.dataset.period);
		mainChart.data.labels = labels;
		mainChart.data.datasets = data;
		mainChart.update();

		currentPeriod = seg;
	});
}

const assetTypeSwitches = document.querySelectorAll(".assets-search-switch-option");
var currentActiveSegment = assetTypeSwitches[0];
currentActiveSegment.classList.add("active");
for (const seg of assetTypeSwitches) {
	seg.addEventListener("click", () => {
		currentActiveSegment.classList.remove("active");
		seg.classList.add("active");
		currentActiveSegment = seg;
		searchResults.innerHTML = "";
	});
}

const APISwitches = document.querySelectorAll(".api-switch-option");
var currentAPI = APISwitches[0];
currentAPI.classList.add("active");
document.querySelector(".assets-search-switch").style.display = 'none';
for (const seg of APISwitches) {
	seg.addEventListener("click", async () => {
		currentAPI.classList.remove("active");
		seg.classList.add("active");
		if (seg.dataset.type == "yfinance") document.querySelector(".assets-search-switch").style.display = 'none';
		else if (seg.dataset.type == "moex") document.querySelector(".assets-search-switch").style.display = 'flex';
		searchResults.innerHTML = "";
		currentAPI = seg;
	});
}