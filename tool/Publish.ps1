. tool/Clean.ps1
. tool/Version.ps1

Write-Host "Publishing the package..."
$version = [xml] (Get-Content "etc/Package.xml") | Select-Xml "//Version"
git tag "v$version"
git push origin "v$version"

dotnet pack Which.slnx --output=var
foreach ($item in Get-ChildItem "var/*.nupkg") {
	dotnet nuget push $item --api-key=$Env:NUGET_API_KEY --source=https://api.nuget.org/v3/index.json
	dotnet nuget push $item --api-key=$Env:GITHUB_TOKEN --source=https://nuget.pkg.github.com/cedx/index.json
}
