using module ./New-Command.psm1

<#
.SYNOPSIS
	Executes a parameterized SQL query and returns a data reader.
.PARAMETER Connection
	The connection to the data source.
.PARAMETER Command
	The SQL query to be executed.
.PARAMETER Parameters
	The parameters of the SQL query.
.OUTPUTS
	A custom object with a `Reader` property containing the data reader that can be used to iterate over the results of the SQL query.
#>
function Invoke-Reader {
	[CmdletBinding()]
	[OutputType([psobject])]
	param (
		[Parameter(Mandatory, Position = 0)]
		[System.Data.IDbConnection] $Connection,

		[Parameter(Mandatory, Position = 1)]
		[string] $Command,

		[Parameter(Position = 2)]
		[ValidateNotNull()]
		[hashtable] $Parameters = @{}
	)

	if ($Connection.State -eq [System.Data.ConnectionState]::Closed) { $Connection.Open() }
	$dbCommand = New-Command $Connection -Command $Command -Parameters $Parameters
	$reader = $dbCommand.ExecuteReader()
	$dbCommand.Dispose()
	[pscustomobject]@{ Reader = $reader }
}
