FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

COPY src/ /src/
RUN dotnet publish /src/FinCalc/FinCalc.csproj -c Release

FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY --from=build /src/FinCalc/bin/Release/net8.0/publish /app

WORKDIR /app
ENTRYPOINT ["dotnet", "FinCalc.dll"]