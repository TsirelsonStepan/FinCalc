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
    const wAPR = (calculations.weightedAveragePortfolioReturn * 100).toFixed(2) + "%";
    const ePR = (calculations.expectedPortfolioReturn * 100).toFixed(2) + "%";
    const B = calculations.portfolioBeta;

    detailsPanel = document.getElementById("details-panel-1").querySelector(".stats-list");
    detailsPanel.innerHTML = `
        <p>Weighted Average Portfolio Return: ${wAPR}</p>
        <p>Expected Portfolio Return: ${ePR}</p>
        <p>Beta: ${B}</p>
    `;
}

calculateButton.addEventListener("click", calculatePortfolio);