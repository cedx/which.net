if ($release) { . $PSScriptRoot/Default.ps1 }
else {
	Write-Output 'The "Release" configuration must be enabled!'
	exit 1
}

Write-Output "Building the Windows installer..."
iscc Setup.iss
