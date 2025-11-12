using module ./New-Command.psm1
using module ./Mapping/DataAdapter.psm1
using module ./Mapping/DataMapper.psm1

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
	An object with a `Reader` property containing the data reader that can be used to iterate over the results of the SQL query.
#>
function Invoke-Reader {
	[CmdletBinding()]
	[OutputType([DataAdapter])]
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
	[DataAdapter]@{ Mapper = [DataMapper]::new(); Reader = $reader }
}
