using namespace System.Collections.Specialized
using module ./New-Command.psm1

<#
.SYNOPSIS
	Executes a parameterized SQL query that selects a single value.
.PARAMETER Connection
	The connection to the data source.
.PARAMETER Command
	The SQL query to be executed.
.PARAMETER Parameters
	The parameters of the SQL query.
.PARAMETER Timeout
	The wait time, in seconds, before terminating the attempt to execute the command and generating an error.
.OUTPUTS
	The value of the first column of the first row returned, or `$null` if not found.
#>
function Get-Scalar {
	[CmdletBinding()]
	[OutputType([object])]
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
	$value = $dbCommand.ExecuteScalar()
	$dbCommand.Dispose()
	$value -is [DBNull] ? $null : $value
}
