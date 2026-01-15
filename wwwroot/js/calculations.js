const calculateButton = document.getElementById("calculate-btn");
const content = document.querySelector('.content');

async function displayCalculations() {
	const response = await fetch("/portfolio", {
		method: "POST",
		headers: { "Content-Type": "application/json" },
		body: JSON.stringify(selectedArr),
	});
	if (!response.ok) throw new Error(response.status);

	content.innerHTML = "";
	await createReturnRatiosGroup();
	await createGraphGroup();
}

async function createGraphGroup() {
	const wrapper = document.createElement("div");
	wrapper.innerHTML = Templates.graph_group;
	content.appendChild(wrapper);
	const group = wrapper.querySelector(".group")

	const frequencySwitches = wrapper.querySelectorAll(".chart-frequency-switch-option");
	currentFrequency = frequencySwitches[1];
	currentFrequency.classList.add("active");
	frequencySwitches.forEach(seg => {
		seg.addEventListener("click", async () => {
			currentFrequency.classList.remove("active");
			seg.classList.add("active");
			wrapper.querySelector(".chart").innerHTML = "";
			
			[labels, data] = await getChartData(true, seg.dataset.freq, currentPeriod.dataset.period);
			mainChart.data.labels = labels;
			mainChart.data.datasets = data;
			mainChart.update();

			currentFrequency = seg;
		});
	});

	const periodSwitches = wrapper.querySelectorAll(".chart-period-switch-option");
	currentPeriod = periodSwitches[0];
	currentPeriod.classList.add("active");
	periodSwitches.forEach(seg => {
		seg.addEventListener("click", async () => {
			currentPeriod.classList.remove("active");
			seg.classList.add("active");
			wrapper.querySelector(".chart").innerHTML = "";
			
			[labels, data] = await getChartData(true, currentFrequency.dataset.freq, seg.dataset.period);
			mainChart.data.labels = labels;
			mainChart.data.datasets = data;
			mainChart.update();

			currentPeriod = seg;
		});
	});
	
	[labels, data] = await getChartData(false, currentFrequency.dataset.freq, currentPeriod.dataset.period);
	mainChart = new Chart("chart-1", {
		type: "line",
		data: {
			labels: labels,
			datasets: data,
		}
	});

	group.addEventListener('click', () => manageGroups(group));
	applyTranslations(wrapper);	
}

async function getChartData(update, freq, period) {
	//const response = await fetch("/totalHistoricValues");
	const response = await fetch(`/assetsHistoricPrices?freq=${freq}&update=${update}&period=${Math.trunc(period/freq)}`);
	if (!response.ok) throw new Error(response.status);
	const assetsData = await response.json();
	const data = [];
	assetsData.forEach(value => {
			data.push({label: value.secid, data: [...value.values].reverse()});
		});
	labels = [...assetsData[0].dates].reverse();
	return [labels, data];
}

async function createReturnRatiosGroup() {
	const wrapper = document.createElement("div");
	wrapper.innerHTML = Templates.return_ratios_group;
	content.appendChild(wrapper);
	const group = wrapper.querySelector(".group")
	
	const responseWAPR = await fetch("/WAPR");
	if (!responseWAPR.ok) throw new Error(responseWAPR.status);
	const WAPR = await responseWAPR.json();
	
	const responseEPR = await fetch("/EPR");
	if (!responseEPR.ok) throw new Error(responseEPR.status);
	const EPR = await responseEPR.json();
	
	wrapper.querySelector("#wAPR").textContent = ((WAPR - 1) * 100).toFixed(2) + "%";
	wrapper.querySelector("#ePR").textContent = ((EPR - 1) * 100).toFixed(2) + "%";
	
	group.addEventListener('click', () => manageGroups(group));
	applyTranslations(wrapper);
}

calculateButton.addEventListener("click", displayCalculations);