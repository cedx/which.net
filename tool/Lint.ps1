"Performing the static analysis of source code..."
Import-Module PSScriptAnalyzer -Scope Local
Invoke-ScriptAnalyzer $PSScriptRoot -Recurse
