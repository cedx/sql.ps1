using namespace System.Diagnostics.CodeAnalysis

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

	foreach ($key in $Parameters) {
		$dbParameter = $dbCommand.CreateParameter()
		$dbParameter.ParameterName = "@$key"

		$value = $Parameters.$key
		$dbParameter.IsNullable = $null -eq $value
		$dbParameter.Value = $null -eq $value ? [DBNull]::Value : $value

		$dbCommand.Parameters.Add($dbParameter)
	}

	$dbCommand
}
