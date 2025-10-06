if ($release) { . tool/Default.ps1 }
else {
	Write-Host 'The "Release" configuration must be enabled!'
	exit 1
}

Write-Host "Building the Windows installer..."
iscc Setup.iss
