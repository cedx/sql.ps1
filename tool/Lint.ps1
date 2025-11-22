"Performing the static analysis of source code..."
Import-Module PSScriptAnalyzer
Invoke-ScriptAnalyzer $PSScriptRoot -Recurse
Invoke-ScriptAnalyzer src -Recurse
Test-ModuleManifest "$PSScriptRoot/../Sql.psd1" | Out-Null
