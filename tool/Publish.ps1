if ($Release) { & "$PSScriptRoot/Default.ps1" }
else {
	"The ""-Release"" switch must be set!"
	exit 1
}

"Publishing the module..."
$module = Get-Item "Sql.psd1"
$version = (Import-PowerShellDataFile $module).ModuleVersion
git tag "v$version"
git push origin "v$version"

$output = "var/$($module.BaseName)"
New-Item $output/bin -ItemType Directory | Out-Null
Copy-Item $module $output
Copy-Item *.md $output
Copy-Item bin/Belin.*.dll $output/bin

Compress-PSResource $output var
Publish-PSResource -ApiKey $Env:PSGALLERY_API_KEY -NupkgPath "var/$($module.BaseName).$version.nupkg"
