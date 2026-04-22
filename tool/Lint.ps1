using module PSScriptAnalyzer

"Performing the static analysis of source code..."
Invoke-ScriptAnalyzer $PSScriptRoot -ExcludeRule PSUseShouldProcessForStateChangingFunctions -Recurse
Test-ModuleManifest Which.psd1 | Out-Null
