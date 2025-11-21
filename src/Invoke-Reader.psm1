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
	The named parameters of the SQL query.
.PARAMETER PositionalParameters
	The positional parameters of the SQL query.
.PARAMETER Timeout
	The wait time, in seconds, before terminating the attempt to execute the command and generating an error.
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
		[hashtable] $Parameters = @{},

		[ValidateNotNull()]
		[object[]] $PositionalParameters = @(),

		[ValidateRange("NonNegative")]
		[int] $Timeout = 30
	)

	if ($Connection.State -eq [System.Data.ConnectionState]::Closed) { $Connection.Open() }
	$dbCommand = New-Command $Connection -Command $Command -Parameters $Parameters -PositionalParameters $PositionalParameters -Timeout $Timeout
	$reader = $dbCommand.ExecuteReader()
	$dbCommand.Dispose()
	[DataAdapter]@{ Mapper = [DataMapper]::new(); Reader = $reader }
}
