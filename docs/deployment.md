# Deployment 

## From source 

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

## Release 

Another way is to just downlaod the pre-built docker container from releases and just run it.