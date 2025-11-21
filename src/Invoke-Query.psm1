using module ./Invoke-Reader.psm1

<#
.SYNOPSIS
	Executes a parameterized SQL query and returns an array of objects whose properties correspond to the columns.
.PARAMETER Connection
	The connection to the data source.
.PARAMETER Command
	The SQL query to be executed.
.PARAMETER Parameters
	The named parameters of the SQL query.
.PARAMETER PositionalParameters
	The positional parameters of the SQL query.
.PARAMETER As
	The type of objects to return.
.PARAMETER Timeout
	The wait time, in seconds, before terminating the attempt to execute the command and generating an error.
.OUTPUTS
	The array of objects whose properties correspond to the queried columns.
#>
function Invoke-Query {
	[CmdletBinding()]
	[OutputType([object[]])]
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

		[ValidateNotNull()]
		[type] $As = ([psobject]),

		[ValidateRange("NonNegative")]
		[int] $Timeout = 30
	)

	$adapter = Invoke-Reader $Connection -Command $Command -Parameters $Parameters -PositionalParameters $PositionalParameters -Timeout $Timeout
	$adapter.Mapper.ConvertReader($adapter.Reader, $As)
}
