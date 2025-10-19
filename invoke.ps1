#!/usr/bin/env pwsh
param ([switch] $Release)
Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
$PSNativeCommandUseErrorActionPreference = $true
. $PSScriptRoot/tool/$($args.Count -eq 0 ? "Default" : $args[0]).ps1
