FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/bin/Release/net8.0/publish .

ENTRYPOINT ["dotnet", "FinCalc.dll"]
