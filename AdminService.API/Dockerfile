FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["AdminService.API/AdminService.API.csproj", "AdminService.API/"]
COPY ["AdminService.Application/AdminService.Application.csproj", "AdminService.Application/"]
COPY ["AdminService.Domain/AdminService.Domain.csproj", "AdminService.Domain/"]
COPY ["AdminService.Infrastructure/AdminService.Infrastructure.csproj", "AdminService.Infrastructure/"]
COPY ["SmartSure.Contracts/SmartSure.Contracts.csproj", "SmartSure.Contracts/"]

RUN dotnet restore "AdminService.API/AdminService.API.csproj"

COPY . .

RUN dotnet publish "AdminService.API/AdminService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AdminService.API.dll"]
