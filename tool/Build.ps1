Write-Host "Building the project..."
$configuration = $release ? "Release" : "Debug"
dotnet build Which.slnx --configuration=$configuration
