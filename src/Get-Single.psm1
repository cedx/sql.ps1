using module ./Invoke-Reader.psm1

<#
.SYNOPSIS
	Executes a parameterized SQL query and returns the single row.
.PARAMETER Connection
	The connection to the data source.
.PARAMETER Command
	The SQL query to be executed.
.PARAMETER Parameters
	The parameters of the SQL query.
.PARAMETER As
	The type of object to return.
.OUTPUTS
	The single record.
	If not found: throws an error if `-ErrorAction` is set to `Stop`, otherwise returns `$null`.
#>
function Get-Single {
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

	$adapter = Invoke-Reader $Connection -Command $Command -Parameters $Parameters
	$record = $null
	$rowCount = 0
	while ($adapter.Reader.Read()) {
		if (++$rowCount -gt 1) { break }
		$record = $adapter.Mapper.ConvertRecord($adapter.Reader, $As)
	}

	$adapter.Reader.Close()
	$invalidOperation = $record ? $null : [InvalidOperationException] "The result set is empty or contains more than one record."
	if ($invalidOperation -and ($ErrorActionPreference -eq "Stop")) { throw $invalidOperation }
	$record
}
