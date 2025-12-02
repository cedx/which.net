if ($Release) {
	& "$PSScriptRoot/Clean.ps1"
	& "$PSScriptRoot/Version.ps1"
}
else {
	"The ""-Release"" switch must be set!"
	exit 1
}

"Publishing the package..."
$version = (Import-PowerShellDataFile "Which.psd1").ModuleVersion
git tag "v$version"
git push origin "v$version"

dotnet pack --output var
foreach ($item in Get-Item "var/*.nupkg") {
	dotnet nuget push $item --api-key $Env:NUGET_API_KEY --source https://api.nuget.org/v3/index.json
}
