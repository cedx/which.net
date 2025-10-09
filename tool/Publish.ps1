. $PSScriptRoot/Clean.ps1
. $PSScriptRoot/Version.ps1

Write-Output "Publishing the package..."
$version = (Select-Xml "//Version" Package.xml).Node.InnerText
git tag "v$version"
git push origin "v$version"

dotnet pack Which.slnx --output=var
Get-ChildItem "var/*.nupkg" | ForEach-Object {
	dotnet nuget push $_ --api-key=$Env:NUGET_API_KEY --source=https://api.nuget.org/v3/index.json
	dotnet nuget push $_ --api-key=$Env:GITHUB_TOKEN --source=https://nuget.pkg.github.com/cedx/index.json
}
