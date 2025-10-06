Write-Host "Deleting all generated files..."
if (Test-Path "bin") { Remove-Item "bin" -Force -Recurse }
foreach ($item in Get-ChildItem "*/obj") { Remove-Item $item -Force -Recurse }
Remove-Item "var/*" -Exclude ".gitkeep" -Force -Recurse
