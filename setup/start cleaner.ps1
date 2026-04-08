Write-Host "Starting Cleaner server..." -ForegroundColor Cyan

docker-compose up -d --force-recreate stock-cleaner

Write-Host "Cleaner server ready" -ForegroundColor Green

Start-Sleep -Seconds 15