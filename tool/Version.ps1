Write-Host "Updating the version number in the sources..."
$version = (Select-Xml "//Version" Package.xml).Node.InnerText
(Get-Content "Setup.iss") -replace 'version "\d+(\.\d+){2}.*"', "version ""$version""" | Out-File "Setup.iss"
foreach ($item in Get-ChildItem "*/*.csproj") {
	(Get-Content $item) -replace "<Version>\d+(\.\d+){2}.*</Version>", "<Version>$version</Version>" | Out-File $item
}
