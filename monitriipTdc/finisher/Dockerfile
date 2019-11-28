FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY . ./

COPY finisher/finisher.csproj ./finisher

WORKDIR /app/finisher

RUN dotnet restore

WORKDIR /app


COPY finisher/. ./finisher/
WORKDIR /app/finisher
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/finisher/out ./
ENTRYPOINT ["dotnet", "finisher.dll"]