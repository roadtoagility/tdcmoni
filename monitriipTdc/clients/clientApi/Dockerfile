FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app


COPY . ./


COPY *.sln ./

COPY clients/clientApi/clientApi.csproj ./clients/clientApi
COPY tests/tests.csproj ./tests/tests
COPY validators/validation.placa/validation.placa.csproj ./validators/validation.placa
COPY server/jobServer/jobServer.csproj ./server/jobServer
COPY finisher/finisher.csproj ./finisher
COPY validators/validation.empresa/validation.empresa.csproj ./validators/validation.empresa
COPY MoniLogs.Core/MoniLogs.Core.csproj ./MoniLogs.Core
COPY Moni.VelocidadeTempoLocalizacao/Moni.VelocidadeTempoLocalizacao.csproj ./Moni.VelocidadeTempoLocalizacao
COPY validators/validation/validation.csproj ./validators/validation
COPY clients/consoleClient/consoleClient.csproj ./clients/consoleClient

RUN dotnet restore


COPY clients/clientApi/. ./clients/clientApi/
WORKDIR /app/clients/clientApi
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/clients/clientApi/out ./
ENTRYPOINT ["dotnet", "clientApi.dll"]