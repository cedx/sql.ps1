<#
.SYNOPSIS
	Closes the specified database connection.
.PARAMETER Connection
	The connection to the data source.
.INPUTS
	The connection to the data source.
#>
function Close-Connection {
	[CmdletBinding()]
	[OutputType([void])]
	param (
		[Parameter(Mandatory, Position = 0, ValueFromPipeline)]
		[System.Data.IDbConnection] $Connection
	)

	process {
		$connection.Close()
	}
}
