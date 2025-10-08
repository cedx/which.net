Write-Host "Deleting all generated files..."
if (Test-Path "bin") { Remove-Item "bin" -Force -Recurse }
Remove-Item "*/obj" -Force -Recurse
Remove-Item "var/*" -Exclude ".gitkeep" -Force -Recurse
