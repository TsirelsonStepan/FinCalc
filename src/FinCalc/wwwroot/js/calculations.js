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
	currentFrequency = frequencySwitches[1];//sets default frequency
	currentFrequency.classList.add("active");
	for (const seg of frequencySwitches) {
		await switchFrequency(seg, wrapper);
	}

	const periodSwitches = wrapper.querySelectorAll(".chart-period-switch-option");
	currentPeriod = periodSwitches[0];
	currentPeriod.classList.add("active");
	for (const seg of periodSwitches) {
		await switchPeriod(seg, wrapper);
	}
	
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

	const response = await fetch(`/portfolio/values?frequency=${freq}&period=${period}`, {
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

	data.push({label: "Portfolio", data: portfolioData.data.values});

	const assetData = await getOtherAssetData(otherSelectedArr[0].source, freq, period);
	assetData.data.values.length = realLength;
	assetData.data.dates.length = realLength;
	
	var firstNonNullEl = -1;
	for (let j = 0; j < portfolioData.data.values.length; j++) {
		if (portfolioData.data.values[j] != null && assetData.data.values[j] != null) {
			firstNonNullEl = j;
			break;
		}
	}
	const m = portfolioData.data.values[firstNonNullEl] / assetData.data.values[firstNonNullEl];
	assetData.data.values = assetData.data.values.map(x => {
		if (x == null) return null;
		else return x * m;
	});

	data.push({label: "Benchmark", data: assetData.data.values});
	
	labels = portfolioData.data.dates;
	return [labels, data];
}

async function getOtherAssetData(source, freq, period) {
	const response = await fetch(`/historic/prices`, {
		method: "POST",
		headers: { "Content-Type": "application/json" },
		body: JSON.stringify({
			source: source,
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
	group.addEventListener('click', () => manageGroups(group));
	
	const responseWAPR = await fetch(`/indicators/war?frequency=${freq}&period=${period}`, {
		method: "POST",
		headers: { "Content-Type": "application/json" },
		body: JSON.stringify(selectedArr)
	});
	if (!responseWAPR.ok) throw new Error(responseWAPR.status);
	const WAPR = await responseWAPR.json();
	
	const responseCAPM = await fetch(`/indicators/capm`, {
		method: "POST",
		headers: { "Content-Type": "application/json" },
		body: JSON.stringify({
			assets: selectedArr,
			benchmark: {
				source: otherSelectedArr[0].source,
				timeSeries: {frequency: freq, period: period}
			}
		})
	});
	if (!responseCAPM.ok) {
		wrapper.querySelector(".details-panel").innerHTML = "Error encountered"
		throw new Error(responseCAPM.status);
	}
	const CAPM = await responseCAPM.json();
	
	wrapper.querySelector("#WAR").textContent = (WAPR.data * 100).toFixed(2) + "%";
	wrapper.querySelector("#WARPeriod").textContent = (period / 365);
	wrapper.querySelector("#CAPM").textContent = (CAPM.data * 100).toFixed(2) + "%";
	wrapper.querySelector("#CAPMPeriod").textContent = (period / 365);
	
	applyTranslations(wrapper);
}

calculateButton.addEventListener("click", displayCalculations);