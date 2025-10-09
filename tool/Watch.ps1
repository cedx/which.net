Write-Output "Watching for file changes..."
$configuration = $release ? "Release" : "Debug"
Start-Process dotnet "watch build --configuration=$configuration" -NoNewWindow -Wait -WorkingDirectory src
