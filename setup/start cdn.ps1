Write-Host "Starting CDN server..."

docker-compose up -d stock-cdn

Write-Host "Stopping the front and back..."

docker compose stop stock-front
docker compose stop stock-back

Write-Host "CDN server ready"

Start-Sleep -Seconds 15