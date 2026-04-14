if ($Release) { & "$PSScriptRoot/Default.ps1" }
else {
	"The ""-Release"" switch must be set!"
	exit 1
}

"Publishing the package..."
$version = (Import-PowerShellDataFile Which.psd1).ModuleVersion
git tag "v$version"
git push origin "v$version"

$output = "var/NuGet"
dotnet pack --no-build --output $output
Get-Item "$output/*.nupkg" | ForEach-Object { dotnet nuget push $_ --api-key $Env:NUGET_API_KEY --source NuGet }
