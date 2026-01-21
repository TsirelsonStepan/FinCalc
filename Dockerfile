FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/FinCalc/FinCalc.csproj FinCalc/
RUN dotnet restore FinCalc/FinCalc.csproj

COPY src/FinCalc/ FinCalc/
WORKDIR /src/FinCalc
RUN dotnet publish -c Release

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build src/FinCalc/bin/Release/net8.0/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "FinCalc.dll"]