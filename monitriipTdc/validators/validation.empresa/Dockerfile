FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app


COPY . ./


COPY validators/validation.empresa/validation.empresa.csproj ./validators/validation.empresa


WORKDIR /app/validators/validation.empresa

RUN dotnet restore

WORKDIR /app

COPY validators/validation.empresa/. ./validators/validation.empresa/
WORKDIR /app/validators/validation.empresa
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/validators/validation.empresa/out ./
ENTRYPOINT ["dotnet", "validation.empresa.dll"]