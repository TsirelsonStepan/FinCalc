const calculateButton = document.getElementById("calculate-btn");
const content = document.querySelector('.content');

async function displayCalculations() {
	content.innerHTML = "";
	//await createReturnRatiosGroup();
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
			
			[labels, data] = await getChartData(seg.dataset.freq, currentPeriod.dataset.period);
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
			
			[labels, data] = await getChartData(currentFrequency.dataset.freq, seg.dataset.period);
			mainChart.data.labels = labels;
			mainChart.data.datasets = data;
			mainChart.update();

			currentPeriod = seg;
		});
	});
	
	[labels, data] = await getChartData(currentFrequency.dataset.freq, currentPeriod.dataset.period);

	Chart.defaults.elements.point.radius = 2;
	Chart.defaults.elements.point.hoverRadius = 4;
	mainChart = new Chart("chart-1", {
		type: "line",
		data: {
			labels: labels,
			datasets: data,
		},
		options: {
			scales: {
				portfolio: {
					type: "linear",
					position: "left"
				},
				benchmark: {
					type: "linear",
					position: "right",
					grid: {
						drawOnChartArea: false
					}
				}
			},
			interaction: {
				mode: "index",      // align by X label
				intersect: false    // no need to hover the dot exactly
			},
			plugins: {
				tooltip: {
					backgroundColor: "#020617",
					borderColor: "#1e40af",
					borderWidth: 1
				}
			}
		}
	});

	group.addEventListener('click', () => manageGroups(group));
	applyTranslations(wrapper);
}

async function getChartData(freq, period) {
	const data = [];

	const response = await fetch(`/api/portfolio/historicData/values?frequency=${freq}&period=${period}`, {
		method: "POST",
		headers: { "Content-Type": "application/json" },
		body: JSON.stringify(selectedArr)
	});
	if (!response.ok) throw new Error(response.status);
	const portfolioData = await response.json();
	data.push({label: "Portfolio", data: [...portfolioData.values].reverse(), yAxisID: "portfolio"});

	for (let i = 0; i < otherSelectedArr.length; i++) {
		const assetData = await getOtherAssetData(otherSelectedArr[i].market, otherSelectedArr[i].secid, freq, period);
		data.push({label: otherSelectedArr[i].secid, data: [...assetData.values].reverse(), yAxisID: "benchmark"});
	}
	
	labels = [...portfolioData.dates].reverse();
	return [labels, data];
}

async function getOtherAssetData(market, secid, freq, period) {
	const response = await fetch(`/api/historicData/prices?market=${market}&secid=${secid}&frequency=${freq}&period=${period}`);
	if (!response.ok) throw new Error(response.status);
	const assetData = await response.json()
	return assetData;
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