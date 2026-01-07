const calculateButton = document.getElementById("calculate-btn");

async function calculatePortfolio() {
	const response = await fetch("/calculatePortfolio", {
		method: "POST",
		headers: { "Content-Type": "application/json" },
		body: JSON.stringify(selectedAssets),
	});
	if (!response.ok) throw new Error(response.status);
	const calculations = await response.json();
	displayCalculations(calculations);
}

function displayCalculations(calculations) {
	const weightedAverageReturn = ((calculations.weightedAveragePortfolioReturn - 1) * 100).toFixed(2) + "%";
	const expectedReturn = ((calculations.expectedPortfolioReturn - 1) * 100).toFixed(2) + "%";
	const historicalDates = calculations.portfolioAverageHistoricData.dates;
	const historicalValues = calculations.portfolioAverageHistoricData.values;
	const beta = calculations.portfolioBeta;

	document.querySelector('.content').innerHTML = "";
	createReturnRatiosGroup(weightedAverageReturn, expectedReturn);
	createGraphGroup(historicalDates, historicalValues);

	Array.from(document.getElementsByClassName('group')).forEach(group => {
		group.addEventListener('click', () => manageGroups(group));
	});
}

function createGraphGroup(dates, values) {
	const graphFill = `
	<div class="group" id="group-historical-data" data-target="details-panel-historical-data">
		<div class="collapse-icon">▶</div>
		<div data-i18n="historic_data" class="group-title"></div>
	</div>
	<div class="details-panel" id="details-panel-historical-data">
		<div class="graph-holder">
			<canvas id="myChart"></canvas>
		</div>
	</div>`;

	document.querySelector('.content').innerHTML += graphFill;
	applyTranslations(currentLang, document.querySelector('.content'));
	//const valuesData = [];
	//values.forEach(value => {
	//	valuesData.push({label: value.name, data: [...value.values].reverse()});
	//});

	new Chart("myChart", {
		type: "line",
		data: {
			labels: [...dates].reverse(),
			datasets: [{ label: values.name, data: [...values].reverse() }]//Data
		}
	});
}

function createReturnRatiosGroup(wAPR, ePR) {
	const textFill = `        
	<div class="group" id="group-return-ratios" data-target="details-panel-return-ratios">
		<div class="collapse-icon">▶</div>
		<div data-i18n="return_ratios" class="group-title"></div>
	</div>
	<div class="details-panel" id="details-panel-return-ratios">
		<div class="stats-list">
			<span data-i18n="weighted_average_portfolio_return"></span>: ${wAPR}
			<p></p>
			<span data-i18n="expected_portfolio_return"></span>: ${ePR}
		</div>
	</div>`;

	document.querySelector('.content').innerHTML += textFill;
}

calculateButton.addEventListener("click", calculatePortfolio);