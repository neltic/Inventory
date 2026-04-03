Write-Host "Starting API..."

docker compose up stock-back --build -d

Write-Host "Starting CDN server..."

docker-compose up -d stock-cdn

Write-Host "Stopping the front application..."

docker compose stop stock-front

Write-Host "Ready to work with the front end"

Start-Sleep -Seconds 15