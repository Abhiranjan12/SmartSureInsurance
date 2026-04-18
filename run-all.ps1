$services = @(
    "AuthService.API",
    "PolicyService.API",
    "ClaimsService.API",
    "AdminService.API",
    "ApiGateway"
)

foreach ($service in $services) {
    Write-Host "Starting $service..."
    Start-Process -FilePath "dotnet" -ArgumentList "run --project .\$service\$service.csproj"
}

Write-Host "Starting frontend..."
Start-Process -FilePath "cmd.exe" -ArgumentList "/c npm start" -WorkingDirectory ".\smartsure-frontend"

Write-Host "All services started in new windows."
