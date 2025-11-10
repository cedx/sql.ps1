using namespace System.Diagnostics.CodeAnalysis

<#
.SYNOPSIS
	Creates a new parameter associated with the specified command.
.PARAMETER Command
	A command connected to a data source.
.PARAMETER Name
	The parameter name.
.PARAMETER Value
	The parameter value.
.OUTPUTS
	The newly created parameter.
#>
function New-Parameter {
	[CmdletBinding()]
	[OutputType([System.Data.IDbDataParameter])]
	[SuppressMessage("PSUseShouldProcessForStateChangingFunctions", "")]
	param (
		[Parameter(Mandatory, Position = 0)]
		[System.Data.IDbCommand] $Command,

		[Parameter(Mandatory, Position = 1)]
		[string] $Name,

		[Parameter(Mandatory, Position = 2)]
		[AllowNull()]
		[object] $Value
	)

	$parameter = $Command.CreateParameter()
	$parameter.IsNullable = $null -eq $Value
	$parameter.ParameterName = $Name
	$parameter.Value = $null -eq $Value ? [DBNull]::Value : $Value
	$parameter
}
