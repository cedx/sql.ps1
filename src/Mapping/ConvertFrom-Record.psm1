using namespace System.Data

<#
.SYNOPSIS
	Converts the specified data record to a custom object or a hash table.
.PARAMETER InputObject
	The data record to convert.
.PARAMETER AsHashtable
	Value indicating whether to convert the data record to a hash table.
.INPUTS
	The data record to convert.
.OUTPUTS
	[ordered] The hash table corresponding to the specified data record.
.OUTPUTS
	[psobject] The custom object corresponding to the specified data record.
#>
function ConvertFrom-Record {
	[OutputType([ordered], [psobject])]
	param (
		[Parameter(Mandatory, Position = 0, ValueFromPipeline)]
		[IDataRecord] $InputObject,

		[Parameter()]
		[switch] $AsHashtable
	)

	process {
		$hashtable = [ordered]@{}
		for ($index = 0; $index -lt $InputObject.FieldCount; $index++) {
			$key = $InputObject.GetName($index)
			$hashtable.$key = $InputObject.IsDBNull($index) ? $null : $InputObject.GetValue($index)
		}

		$AsHashtable ? $hashtable : [PSCustomObject] $hashtable
	}
}
