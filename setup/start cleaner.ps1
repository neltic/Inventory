Write-Host "Starting Cleaner server..."

docker-compose up -d --force-recreate stock-cleaner

Write-Host "Cleaner server ready"

Start-Sleep -Seconds 15