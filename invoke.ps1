#!/usr/bin/env pwsh
param ([Parameter(Position = 0)] [string] $Command = "Default", [switch] $Release)
Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
$PSNativeCommandUseErrorActionPreference = $true
. $PSScriptRoot/tool/$Command.ps1
