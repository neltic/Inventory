# Script to configure Git quick commands in the PowerShell Profile

$ProfilePath = $PROFILE
if (!(Test-Path $ProfilePath)) {
    New-Item -Type File -Path $ProfilePath -Force
}

$Commands = @"

# Save changes in develop: XSave "type(scope): message"
function XSave(`$msg) {
    git add .
    git commit -m "`$msg"
    if (`$?) { git push origin develop }
}

# Deploy develop to main: XDeploy
function XDeploy {
    git checkout main
    git pull origin main
    git merge develop
    if (`$?) { 
        git push origin main
        git checkout develop
    } else {
        Write-Host "ERROR: Conflicts detected. Resolve manually." -ForegroundColor Red
    }
}
# -------------------------------------------------------
"@

Add-Content -Path $ProfilePath -Value $Commands

Write-Host "'XSave' and 'XDeploy' commands successfully installed!" -ForegroundColor Green
Write-Host "Please restart your terminal or run: . `$PROFILE" -ForegroundColor Cyan

Start-Sleep -Seconds 15