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
    const wAPR = ((calculations.weightedAveragePortfolioReturn - 1) * 100).toFixed(2) + "%";
    const ePR = ((calculations.expectedPortfolioReturn - 1) * 100).toFixed(2) + "%";
    const B = calculations.portfolioBeta;

    //document.getElementById("details-panel-1").querySelector(".stats-list");
    document.querySelector('.main').innerHTML = `
        <div class="group" id="group-return-ratios" data-target="details-panel-return-ratios">
            <div class="collapse-icon">▶</div>
            <div class="group-title">Return ratios</div>
        </div>
        <div class="details-panel" id="details-panel-return-ratios">
            <div class="stats-list">
                <p>Weighted Average Portfolio Return: ${wAPR}</p>
                <p>Expected Portfolio Return: ${ePR}</p>
            </div>
        </div>
        
        <div class="group" id="group-historical-data" data-target="details-panel-historical-data">
            <div class="collapse-icon">▶</div>
            <div class="group-title">Historical data</div>
        </div>
        <div class="details-panel" id="details-panel-historical-data">
            <div class="graph-holder">
                <canvas id="myChart"></canvas>
            </div>
        </div>`;

    new Chart("myChart", {
        type: "line",
        data: {
            labels: calculations.portfolioHistoricData[0].dates,
            datasets: [{
                label: "Portfolio value",
                data: calculations.portfolioHistoricData[0].values
            }]
        }
    });

    Array.from(document.getElementsByClassName('group')).forEach(group => {
        group.addEventListener('click', () => manageGroups(group));
    });
}

calculateButton.addEventListener("click", calculatePortfolio);