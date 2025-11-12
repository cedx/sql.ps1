using module ./Invoke-Reader.psm1

<#
.SYNOPSIS
	Executes a parameterized SQL query and returns an array of objects whose properties correspond to the columns.
.PARAMETER Connection
	The connection to the data source.
.PARAMETER Command
	The SQL query to be executed.
.PARAMETER Parameters
	The parameters of the SQL query.
.PARAMETER As
	The type of objects to return.
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
		[type] $As = ([psobject])
	)

	$adapter = Invoke-Reader $Connection -Command $Command -Parameters $Parameters
	$adapter.Mapper.ConvertReader($adapter.Reader, $As)
}
