@{
	DefaultCommandPrefix = "Sql"
	ModuleVersion = "0.6.0"
	PowerShellVersion = "7.4"
	RootModule = "src/Main.psm1"

	Author = "Cédric Belin <cedx@outlook.com>"
	CompanyName = "Cedric-Belin.fr"
	Copyright = "© Cédric Belin"
	Description = "A simple micro-ORM supporting MySQL, PostgreSQL, SQL Server and SQLite."
	GUID = "d2b1c123-e1bc-4cca-84c5-af102244e3c5"

	AliasesToExport = @()
	CmdletsToExport = @()
	VariablesToExport = @()

	FunctionsToExport = @(
		"Close-Connection"
		"Get-First"
		"Get-Scalar"
		"Get-Single"
		"Get-Version"
		"Invoke-NonQuery"
		"Invoke-Query"
		"Invoke-Reader"
		"New-Command"
		"New-Connection"
		"New-DataMapper"
		"New-Parameter"
	)

	NestedModules = @(
		"src/Close-Connection.psm1"
		"src/Get-First.psm1"
		"src/Get-Scalar.psm1"
		"src/Get-Single.psm1"
		"src/Invoke-NonQuery.psm1"
		"src/Invoke-Query.psm1"
		"src/Invoke-Reader.psm1"
		"src/New-Command.psm1"
		"src/New-Connection.psm1"
		"src/New-DataMapper.psm1"
		"src/New-Parameter.psm1"
	)

	PrivateData = @{
		PSData = @{
			LicenseUri = "https://github.com/cedx/sql.ps1/blob/main/License.md"
			ProjectUri = "https://github.com/cedx/sql.ps1"
			ReleaseNotes = "https://github.com/cedx/sql.ps1/releases"
			Tags = "ado.net", "client", "database", "orm", "query", "sql"
		}
	}
}
