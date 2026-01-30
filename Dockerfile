FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

COPY src/FinCalc/FinCalc.csproj src/FinCalc/
COPY src/FinCalc.MoexApi/FinCalc.MoexApi.csproj src/FinCalc.MoexApi/
RUN dotnet restore src/FinCalc/FinCalc.csproj

COPY src/ /src/
WORKDIR /src/FinCalc
RUN dotnet publish -c Release

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build src/FinCalc/bin/Release/net8.0/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "FinCalc.dll"]