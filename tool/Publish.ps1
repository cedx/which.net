. $PSScriptRoot/Clean.ps1
. $PSScriptRoot/Version.ps1

Write-Output "Publishing the package..."
$version = (Import-PowerShellDataFile "Package.psd1").Version
git tag "v$version"
git push origin "v$version"

dotnet pack Which.slnx --output=var
foreach ($item in Get-ChildItem "var/*.nupkg") {
	dotnet nuget push $item --api-key=$Env:NUGET_API_KEY --source=https://api.nuget.org/v3/index.json
	dotnet nuget push $item --api-key=$Env:GITHUB_TOKEN --source=https://nuget.pkg.github.com/cedx/index.json
}
