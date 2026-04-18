$services = @(
    "AuthService.API",
    "PolicyService.API",
    "ClaimsService.API",
    "AdminService.API",
    "ApiGateway"
)

foreach ($service in $services) {
    Write-Host "Starting $service..."
    Start-Process -FilePath "powershell.exe" -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot'; dotnet run --project .\$service\$service.csproj"
}

Write-Host "Starting frontend..."
Start-Process -FilePath "powershell.exe" -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\smartsure-frontend'; npm start"

Write-Host "All services started. Each runs in its own window."
Write-Host "Press any key to exit this launcher..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
