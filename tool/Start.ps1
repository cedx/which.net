Write-Host "Starting the application..."
$configuration = $release ? "Release" : "Debug"
dotnet run --configuration=$configuration --project=src
