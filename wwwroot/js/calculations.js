const calculateButton = document.getElementById("calculate-btn");
const content = document.querySelector('.content');

async function calculatePortfolio() {
	const response = await fetch("/calculatePortfolio", {
		method: "POST",
		headers: { "Content-Type": "application/json" },
		body: JSON.stringify(selectedArr),
	});
	if (!response.ok) throw new Error(response.status);
	const calculations = await response.json();
	displayCalculations(calculations);
}

function displayCalculations(calculations) {
	const weightedAverageReturn = calculations.weightedAveragePortfolioReturn;
	const expectedReturn = calculations.expectedPortfolioReturn;
	const historicData = calculations.portfolioAverageHistoricData;
	
	content.innerHTML = "";
	createReturnRatiosGroup(weightedAverageReturn, expectedReturn);
	createGraphGroup(historicData);

	Array.from(document.getElementsByClassName('group')).forEach(group => {
		group.addEventListener('click', () => manageGroups(group));
	});
}

function createGraphGroup(data) {
	const wrapper = document.createElement("div");
	wrapper.innerHTML = Templates.graph_group;
	content.appendChild(wrapper);
	applyTranslations(wrapper);

	//const valuesData = [];
	//values.forEach(value => {
	//	valuesData.push({label: value.name, data: [...value.values].reverse()});
	//});

	new Chart("chart-1", {
		type: "line",
		data: {
			labels: [...data.dates].reverse(),
			datasets: [{ label: data.name, data: [...data.values].reverse() }]
		}
	});
}

function createReturnRatiosGroup(weightedAveragePortfolioReturn, expectedPortfolioReturn) {
	const wrapper = document.createElement("div");
	wrapper.innerHTML = Templates.return_ratios_group;

	wrapper.querySelector("#wAPR").textContent = ((weightedAveragePortfolioReturn - 1) * 100).toFixed(2) + "%";
	wrapper.querySelector("#ePR").textContent = ((expectedPortfolioReturn - 1) * 100).toFixed(2) + "%";

	content.appendChild(wrapper);
	applyTranslations(wrapper);
}

calculateButton.addEventListener("click", calculatePortfolio);