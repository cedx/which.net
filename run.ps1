#!/usr/bin/env pwsh
Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
$PSNativeCommandUseErrorActionPreference = $true
& dotnet "$PSScriptRoot/bin/Belin.Which.dll" @args
