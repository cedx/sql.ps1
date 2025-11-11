using module ./Invoke-Reader.psm1
using module ./Mapping/ConvertFrom-Reader.psm1

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
	The type to which the returned records should be converted.
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

	$reader = (Invoke-Reader $Connection -Command $Command -Parameters $Parameters).Reader
	ConvertFrom-Reader $reader -As:$As
}
