@{
	ModuleVersion = "1.0.0-rc7"
	PowerShellVersion = "7.5"
	RootModule = "bin/Belin.Which.dll"

	Author = "Cédric Belin <cedx@outlook.com>"
	CompanyName = "Cedric-Belin.fr"
	Copyright = "© Cédric Belin"
	Description = "Find the instances of an executable in the system path. Like the `which` Linux command."
	GUID = "d7a20797-defc-4db5-9fba-11e2906ee7a2"

	AliasesToExport = @()
	CmdletsToExport = @()
	FunctionsToExport = @()
	VariablesToExport = @()

	PrivateData = @{
		PSData = @{
			LicenseUri = "https://github.com/cedx/which.net/blob/main/License.md"
			ProjectUri = "https://github.com/cedx/which.net"
			ReleaseNotes = "https://github.com/cedx/which.net/releases"
			Tags = "find", "path", "system", "utility", "which"
		}
	}
}
