Write-Host "Starting CDN server..." -ForegroundColor Cyan

docker-compose up -d stock-cdn

Write-Host "Stopping the front and back..." -ForegroundColor Cyan

docker compose stop stock-front
docker compose stop stock-back

Write-Host "CDN server ready" -ForegroundColor Green

Start-Sleep -Seconds 15