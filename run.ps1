#!/usr/bin/env pwsh
param ([switch] $release)
Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
$PSNativeCommandUseErrorActionPreference = $true
. tool/$($args.Count -eq 0 ? "Default" : $args[0]).ps1
