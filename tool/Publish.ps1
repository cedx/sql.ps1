if ($Release) { & "$PSScriptRoot/Default.ps1" }
else {
	"The ""-Release"" switch must be set!"
	exit 1
}

"Publishing the module..."
$module = Import-PowerShellDataFile "Sql.psd1"
$version = "v$($module.ModuleVersion)"
git tag $version
git push origin $version

$name = Split-Path "Sql.psd1" -LeafBase
$output = "var/$name"
New-Item $output/bin -ItemType Directory | Out-Null
Copy-Item "$name.psd1" $output
Copy-Item *.md $output
Copy-Item $module.RootModule $output/bin
if ("RequiredAssemblies" -in $module.Keys) { Copy-Item $module.RequiredAssemblies $output/bin }

Compress-PSResource $output var
Publish-PSResource -ApiKey $Env:PSGALLERY_API_KEY -NupkgPath "var/$($module.BaseName).$version.nupkg"
