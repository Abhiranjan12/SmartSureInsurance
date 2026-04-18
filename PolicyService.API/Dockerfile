FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["PolicyService.API/PolicyService.API.csproj", "PolicyService.API/"]
COPY ["PolicyService.Application/PolicyService.Application.csproj", "PolicyService.Application/"]
COPY ["PolicyService.Domain/PolicyService.Domain.csproj", "PolicyService.Domain/"]
COPY ["PolicyService.Infrastructure/PolicyService.Infrastructure.csproj", "PolicyService.Infrastructure/"]
COPY ["SmartSure.Contracts/SmartSure.Contracts.csproj", "SmartSure.Contracts/"]

RUN dotnet restore "PolicyService.API/PolicyService.API.csproj"

COPY . .

RUN dotnet publish "PolicyService.API/PolicyService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PolicyService.API.dll"]
