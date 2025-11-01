. "$PSScriptRoot/Clean.ps1"
. "$PSScriptRoot/Version.ps1"

"Publishing the package..."
$version = (Import-PowerShellDataFile "Which.psd1").ModuleVersion
git tag "v$version"
git push origin "v$version"

dotnet pack --output var
foreach ($item in Get-Item "var/*.nupkg") {
	dotnet nuget push $item --api-key $Env:NUGET_API_KEY --source https://api.nuget.org/v3/index.json
	dotnet nuget push $item --api-key $Env:GITHUB_TOKEN --source https://nuget.pkg.github.com/cedx/index.json
}
