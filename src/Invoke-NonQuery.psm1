using namespace System.Collections.Specialized
using module ./New-Command.psm1

<#
.SYNOPSIS
	Executes a parameterized SQL statement.
.PARAMETER Connection
	The connection to the data source.
.PARAMETER Command
	The SQL statement to be executed.
.PARAMETER Parameters
	The parameters of the SQL statement.
.PARAMETER Timeout
	The wait time, in seconds, before terminating the attempt to execute the command and generating an error.
.OUTPUTS
	The number of rows affected.
#>
function Invoke-NonQuery {
	[CmdletBinding()]
	[OutputType([int])]
	param (
		[Parameter(Mandatory, Position = 0)]
		[System.Data.IDbConnection] $Connection,

		[Parameter(Mandatory, Position = 1)]
		[string] $Command,

		[Parameter(Position = 2)]
		[ValidateNotNull()]
		[OrderedDictionary] $Parameters = @{},

		[ValidateRange("NonNegative")]
		[int] $Timeout = 30
	)

	if ($Connection.State -eq [System.Data.ConnectionState]::Closed) { $Connection.Open() }
	$dbCommand = New-Command $Connection -Command $Command -Parameters $Parameters -Timeout $Timeout
	$rowsAffected = $dbCommand.ExecuteNonQuery()
	$dbCommand.Dispose()
	$rowsAffected
}
