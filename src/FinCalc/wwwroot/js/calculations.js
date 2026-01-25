const calculateButton = document.getElementById("calculate-btn");
const content = document.querySelector('.content');

async function displayCalculations() {
	content.innerHTML = "";
	await createGraphGroup();
	await createReturnRatiosGroup(7, 5 * 365);
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
			interaction: {//for pop-up without precise hover on line
				mode: "index",
				intersect: false
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

	const response = await fetch(`/api/historicData/portfolio/values?frequency=${freq}&period=${period}`, {
		method: "POST",
		headers: { "Content-Type": "application/json" },
		body: JSON.stringify(selectedArr)
	});
	if (!response.ok) throw new Error(response.status);
	const portfolioData = await response.json();

	var realLength;
    for (let i = portfolioData.data.values.length - 1; i >= 0; i--) {
        if (portfolioData.data.values[i] !== null) {
			realLength = i + 1;
			break;
		}
    }
	portfolioData.data.values.length = realLength;
	portfolioData.data.dates.length = realLength;

	data.push({label: "Portfolio", data: [...portfolioData.data.values].reverse()});

	for (let i = 0; i < otherSelectedArr.length; i++) {
		const assetData = await getOtherAssetData(otherSelectedArr[i].api, otherSelectedArr[i].market, otherSelectedArr[i].secid, freq, period);
		assetData.values.length = realLength;
		assetData.dates.length = realLength;
		assetData.values = assetData.values.map(x => {
			if (x !== null) return x * (portfolioData.data.values[realLength - 1] / assetData.values[realLength - 1])
		});

		data.push({label: otherSelectedArr[i].secid, data: [...assetData.values].reverse()});
	}
	
	labels = [...portfolioData.data.dates].reverse();
	return [labels, data];
}

async function getOtherAssetData(api, market, secid, freq, period) {
	const response = await fetch(`/api/historicData/prices`, {
		method: "POST",
		headers: { "Content-Type": "application/json" },
		body: JSON.stringify({
			source: {api: api, market: market},
			secid: secid,
			timeSeries: {frequency: parseInt(freq), period: parseInt(period)}
		})
	});
	if (!response.ok) throw new Error(response.status);
	const assetData = await response.json()
	return assetData;
}

async function createReturnRatiosGroup(freq, period) {
	const wrapper = document.createElement("div");
	wrapper.innerHTML = Templates.return_ratios_group;
	content.appendChild(wrapper);
	const group = wrapper.querySelector(".group")
	
	const responseWAPR = await fetch(`/api/indicators/war?frequency=${freq}&period=${period}`, {
		method: "POST",
		headers: { "Content-Type": "application/json" },
		body: JSON.stringify(selectedArr)
	});
	if (!responseWAPR.ok) throw new Error(responseWAPR.status);
	const WAPR = await responseWAPR.json();
	
	const responseCAPM = await fetch("/api/indicators/capm", {
		method: "POST",
		headers: { "Content-Type": "application/json" },
		body: JSON.stringify({
			assets: selectedArr,
			benchmark: {
				source: {api: otherSelectedArr[0].api, market: otherSelectedArr[0].market},
				secid: otherSelectedArr[0].secid,
				timeSeries: {frequency: freq, period: period}
			}
		})
	});
	if (!responseCAPM.ok) throw new Error(responseCAPM.status);
	const CAPM = await responseCAPM.json();
	
	wrapper.querySelector("#WAR").textContent = (WAPR * 100).toFixed(2) + "%";
	wrapper.querySelector("#WARPeriod").textContent = (period / 365);
	wrapper.querySelector("#CAPM").textContent = (CAPM * 100).toFixed(2) + "%";
	wrapper.querySelector("#CAPMPeriod").textContent = (period / 365);

	group.addEventListener('click', () => manageGroups(group));
	applyTranslations(wrapper);
}

calculateButton.addEventListener("click", displayCalculations);