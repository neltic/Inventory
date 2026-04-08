Write-Host "Starting API..." -ForegroundColor Cyan

docker compose up stock-back stock-cache-insight stock-cleaner --build -d

Write-Host "Stopping the front application..." -ForegroundColor Cyan

docker compose stop stock-front

Write-Host "Ready to work with the front end" -ForegroundColor Green

Start-Sleep -Seconds 15