using module ./PSResource.psm1

"Checking for outdated dependencies..."
dotnet package list --outdated
(Import-PowerShellDataFile PSModules.psd1).Keys | Get-InstalledPSResource | Test-PSResourceUpdate
