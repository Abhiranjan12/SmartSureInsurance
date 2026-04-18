FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ClaimsService.API/ClaimsService.API.csproj", "ClaimsService.API/"]
COPY ["ClaimsService.Application/ClaimsService.Application.csproj", "ClaimsService.Application/"]
COPY ["ClaimsService.Domain/ClaimsService.Domain.csproj", "ClaimsService.Domain/"]
COPY ["ClaimsService.Infrastructure/ClaimsService.Infrastructure.csproj", "ClaimsService.Infrastructure/"]
COPY ["SmartSure.Contracts/SmartSure.Contracts.csproj", "SmartSure.Contracts/"]

RUN dotnet restore "ClaimsService.API/ClaimsService.API.csproj"

COPY . .

RUN dotnet publish "ClaimsService.API/ClaimsService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClaimsService.API.dll"]
