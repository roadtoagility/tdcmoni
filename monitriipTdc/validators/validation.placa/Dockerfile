FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

#add
COPY . ./

COPY validators/validation.placa/validation.placa.csproj ./validators/validation.placa

WORKDIR /app/validators/validation.placa

RUN dotnet restore

WORKDIR /app

COPY validators/validation.placa/. ./validators/validation.placa/
WORKDIR /app/validators/validation.placa
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/validators/validation.placa/out ./
ENTRYPOINT ["dotnet", "validation.placa.dll"]