#!/usr/bin/env pwsh
$ErrorActionPreference = "Stop"
$PSNativeCommandUseErrorActionPreference = $true
Set-StrictMode -Version Latest

Import-Module "$PSScriptRoot/Sql.psd1"
# Insert the command to be debugged here.

$version = "10.11.7"
if ($version -notmatch "^(\d+(\.\d+)*)[^\.\d]*") { throw $invalidOperation }
[version] $Matches.1
