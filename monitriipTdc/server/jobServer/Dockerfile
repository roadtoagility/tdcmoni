FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY . ./

COPY server/jobServer/jobServer.csproj ./server/jobServer

WORKDIR /app/server/jobServer

RUN dotnet restore

WORKDIR /app


COPY server/jobServer/. ./server/jobServer/
WORKDIR /app/server/jobServer
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/server/jobServer/out ./
ENTRYPOINT ["dotnet", "jobServer.dll"]