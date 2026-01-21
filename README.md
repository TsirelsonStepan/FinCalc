# Financial Portfolio Calculator
REST API service that provides tools to create custom portfolios of financial assets and analyze them using historical market data as well as some embedded financial models.

You can check out ![demo of this project](https://fincalc-jrqm.onrender.com/) *(hosted on render.com free tier so can take ~50 seconds to load)*

---

## Features

- Creation of fully customizabe portfolio using all publicly traded shares and indexes on MOEX
- Retrieval of historical market data from MOEX
- Calculation of aggregated portfolio values

---

## How to deploy locally
This repository contains both Dockerfile and docker-compose.yml so you use either of them.

For docker-compose:
```bash
git clone https://github.com/TsirelsonStepan/FinCalc.git
cd FinCalc

# Build Docker image
docker build -t fincalc-app:latest .

# Run container
docker-compose up
```
If you prefer using simple Dockerfile:
```bash
git clone https://github.com/TsirelsonStepan/FinCalc.git
cd FinCalc

# Build Docker image
docker build -t fincalc-app:latest .

# Run container
docker run -p 80:8080 fincalc-app:latest
```

---

## API Endpoints

All endpoints as well as some other information are documented in OpenAPI format in ![json file](/swagger.json). For OpenAPI UI deploy locally and go to /swagger/index.html or visit ![demo](https://fincalc-jrqm.onrender.com/).

---

## Architecture Overview

The API relies on several key objects:

- **Controllers**: Handle all the incoming requests *(Works only with "application/json")*.
- **MOEX API**: The main source of market data. *(The main direction of development for this project is introduction of new data sources)*.
- **Data Structures**: The collection of datatypes that represent market related data:
	- Historic data - for all sequences of values in time, contains additional fields for intervals and real time Period.
	- Asset and AssetInPortfolio - for all financial assets, contains id, market asset is traded on and for AssetInPortfolio - amount.
	- Portfolio - main datatype that contains list of assets and all calculated indicators and datasets - historical averages, weighted average annual retruns, etc.
- **Calculator** - contains sevral models for calculating financial indicators:
	- CAPM (Capital Assets Pricing Model): for estimating future returns.
	- As well as basic math operations: Annualize returns and weighted average of annual returns.

For more detailed documentation of the architechture check the diagrams.

---

## Sequence Diagrams

### General Structure

![Structure diagram](/diagrams/Structure.drawio.svg)

### Portfolio creation process

![Portfolio creation sequence](diagrams/PortfolioCreationSequence.drawio.svg)

### Asset search process

![Asset search sequence](diagrams/SearchSequence.drawio.svg)

---

## Future development

In future updates I plan mainly on increasing the number of financial data providers to add new markets, new types of assets, etc. Also in near future I plan to add import/export functionality to allow saving custom portfolios.

---

## Author

Stephan Tsirelson - ![Linkedin](https://www.linkedin.com/in/stephan-tsirelson-046598293)

If you are interested in this project, have ideas for improvement or have a job offer for me - here is my email - tsirelsons@gmail.com)