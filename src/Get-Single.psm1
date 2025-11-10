using module ./Invoke-Reader.psm1
using module ./Mapping/ConvertFrom-Record.psm1

<#
.SYNOPSIS
	Executes a parameterized SQL query and returns the single row.
.PARAMETER Connection
	The connection to the data source.
.PARAMETER Command
	The SQL query to be executed.
.PARAMETER Parameters
	The parameters of the SQL query.
.PARAMETER AsHashtable
	Value indicating whether to convert the row to a hash table.
.OUTPUTS
	[hashtable] The single row as a hash table. If not found: throws an error if `-ErrorAction` is set to `Stop`, otherwise returns `$null`.
.OUTPUTS
	[psobject] The single row as a custom object. If not found: throws an error if `-ErrorAction` is set to `Stop`, otherwise returns `$null`.
#>
function Get-Single {
	[CmdletBinding()]
	[OutputType([hashtable], [psobject])]
	param (
		[Parameter(Mandatory, Position = 0)]
		[System.Data.IDbConnection] $Connection,

		[Parameter(Mandatory, Position = 1)]
		[string] $Command,

		[Parameter(Position = 2)]
		[ValidateNotNull()]
		[hashtable] $Parameters = @{},

		[Parameter()]
		[switch] $AsHashtable
	)

	$reader = Invoke-Reader $Connection -Command $Command -Parameters $Parameters
	$record = $null
	$rowCount = 0
	while ($reader.Read()) {
		if (++$rowCount -gt 1) { break }
		$record = ConvertFrom-Record $reader -AsHashtable:$AsHashtable
	}

	$reader.Close()
	$invalidOperation = $record -and ($rowCount -eq 1) ? $null : [InvalidOperationException] "Unable to fetch the single row."
	if ($invalidOperation -and ($ErrorActionPreference -eq "Stop")) { throw $invalidOperation }
	$record
}
