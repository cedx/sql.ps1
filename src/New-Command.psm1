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
	The named parameters of the SQL query.
.PARAMETER PositionalParameters
	The positional parameters of the SQL query.
.PARAMETER Timeout
	The wait time, in seconds, before terminating the attempt to execute the command and generating an error.
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
		[hashtable] $Parameters = @{},

		[ValidateNotNull()]
		[object[]] $PositionalParameters = @(),

		[ValidateRange("NonNegative")]
		[int] $Timeout = 30
	)

	$dbCommand = $Connection.CreateCommand()
	$dbCommand.CommandText = $Command
	$dbCommand.CommandTimeout = $Timeout

	for ($index = 0; $index -lt $PositionalParameters; $index++) {
		$dbParameter = New-Parameter $dbCommand -Name "QuestionMark$index" -Value $PositionalParameters[$index]
		$dbCommand.Parameters.Add($dbParameter) | Out-Null
	}

	foreach ($key in $Parameters.Keys) {
		$dbParameter = New-Parameter $dbCommand -Name "@$key" -Value $Parameters.$key
		$dbCommand.Parameters.Add($dbParameter) | Out-Null
	}

	$dbCommand
}
