using namespace System.Diagnostics.CodeAnalysis
using module ./New-Parameter.psm1

<#
.SYNOPSIS
	Creates a new command associated with the specified connection.
.PARAMETER Connection
	The connection to the data source.
.PARAMETER Command
	The SQL query to be executed.
.PARAMETER Parameters
	The parameters of the SQL query.
.OUTPUTS
	The newly created command.
#>
function New-Command {
	[CmdletBinding()]
	[OutputType([System.Data.IDbCommand])]
	[SuppressMessage("PSUseShouldProcessForStateChangingFunctions", "")]
	param (
		[Parameter(Mandatory, Position = 0)]
		[System.Data.IDbConnection] $Connection,

		[Parameter(Mandatory, Position = 1)]
		[string] $Command,

		[Parameter(Position = 2)]
		[ValidateNotNull()]
		[hashtable] $Parameters = @{}
	)

	$dbCommand = $Connection.CreateCommand()
	$dbCommand.CommandText = $Command

	foreach ($key in $Parameters.Keys) {
		$dbParameter = New-Parameter $dbCommand -Name "@$key" -Value $Parameters.$key
		$dbCommand.Parameters.Add($dbParameter) | Out-Null
	}

	$dbCommand
}
