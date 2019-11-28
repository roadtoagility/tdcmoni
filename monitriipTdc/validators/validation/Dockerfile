FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY . ./

COPY validators/validation/validation.csproj ./validators/validation

WORKDIR /app/validators/validation

RUN dotnet restore

WORKDIR /app

COPY validators/validation/. ./validators/validation/
WORKDIR /app/validators/validation
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/validators/validation/out ./
ENTRYPOINT ["dotnet", "validation.dll"]