using module ./Invoke-Reader.psm1
using module ./Mapping/ConvertFrom-Record.psm1

<#
.SYNOPSIS
	Executes a parameterized SQL query and returns the first row.
.PARAMETER Connection
	The connection to the data source.
.PARAMETER Command
	The SQL query to be executed.
.PARAMETER Parameters
	The parameters of the SQL query.
.PARAMETER As
	The type to which the returned record should be converted.
.OUTPUTS
	The first record. If not found: throws an error if `-ErrorAction` is set to `Stop`, otherwise returns `$null`.
#>
function Get-First {
	[CmdletBinding()]
	[OutputType([object])]
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
	$record = $reader.Read() ? (ConvertFrom-Record $reader -As:$As) : $null
	$reader.Close()

	$invalidOperation = $record ? $null : [InvalidOperationException] "Unable to fetch the first row."
	if ($invalidOperation -and ($ErrorActionPreference -eq "Stop")) { throw $invalidOperation }
	$record
}
