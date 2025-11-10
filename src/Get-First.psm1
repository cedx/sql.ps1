using namespace System.Data
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
.PARAMETER AsHashtable
	Value indicating whether to convert the row to a hash table.
.OUTPUTS
	[hashtable] The first row as a hash table. If not found: throws an error if `-ErrorAction` is set to `Stop`, otherwise returns `$null`.
.OUTPUTS
	[psobject] The first row as a custom object. If not found: throws an error if `-ErrorAction` is set to `Stop`, otherwise returns `$null`.
#>
function Get-First {
	[CmdletBinding()]
	[OutputType([hashtable], [psobject])]
	param (
		[Parameter(Mandatory, Position = 0)]
		[IDbConnection] $Connection,

		[Parameter(Mandatory, Position = 1)]
		[string] $Command,

		[Parameter(Position = 2)]
		[ValidateNotNull()]
		[hashtable] $Parameters = @{},

		[Parameter()]
		[switch] $AsHashtable
	)

	$reader = Invoke-Reader $Connection -Command $Command -Parameters $Parameters -AsHashtable:$AsHashtable
	$record = $reader.Read() ? (ConvertFrom-Record $InputObject -AsHashtable:$AsHashtable) : $null
	$reader.Close()

	if ((-not $record) -and ($ErrorActionPreference -eq "Stop")) { throw [InvalidOperationException] "TODO" }
	$record
}
