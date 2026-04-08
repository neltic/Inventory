Write-Host "Starting App..." -ForegroundColor Cyan

docker compose up --build -d

Write-Host "Ready to work" -ForegroundColor Green

Start-Sleep -Seconds 15