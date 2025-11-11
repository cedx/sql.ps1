using namespace System.Collections.Generic
using module ./ConvertFrom-Record.psm1

<#
.SYNOPSIS
	Converts the specified data reader to an array of objects.
.PARAMETER InputObject
	The data reader to convert.
.PARAMETER As
	The type to which the returned records should be converted.
.INPUTS
	The data reader to convert.
.OUTPUTS
	The array of objects corresponding to the specified data reader.
#>
function ConvertFrom-Reader {
	[OutputType([object[]])]
	param (
		[Parameter(Mandatory, Position = 0, ValueFromPipeline)]
		[System.Data.IDataReader] $InputObject,

		[ValidateNotNull()]
		[type] $As = ([psobject])
	)

	process {
		$list = [List[object]]::new()
		while ($InputObject.Read()) { $list.Add((ConvertFrom-Record $InputObject -As:$As)) }
		$InputObject.Close()
		$list.ToArray()
	}
}
