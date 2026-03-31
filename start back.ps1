Write-Host "Stating Stock API..."

docker compose up stock-back --build -d

docker compose stop stock-front

Start-Sleep -Seconds 15