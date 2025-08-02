#!/usr/bin/env pwsh
Set-StrictMode -Version Latest
$commandPath = Get-Item $PSCommandPath
$scriptRoot = $commandPath.LinkType ? (Split-Path $commandPath.LinkTarget) : $PSScriptRoot
& dotnet "$scriptRoot/lib/Belin.Which.dll" @args
