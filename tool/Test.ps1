"Running the test suite..."
& "$PSScriptRoot/Build.ps1"

pwsh -Command {
	Import-Module Pester
	Invoke-Pester test
	exit $LASTEXITCODE
}
