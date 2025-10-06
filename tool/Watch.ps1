Write-Host "Watching for file changes..."
Push-Location src
try { dotnet watch build }
finally { Pop-Location }
