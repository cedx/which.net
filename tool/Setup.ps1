if ($Release) { . $PSScriptRoot/Default.ps1 }
else {
	'The "-Release" switch must be set!'
	exit 1
}

"Building the Windows installer..."
iscc Setup.iss
