using module ./SqlMapper.psm1

<#
.SYNOPSIS
	Converts the specified data record to a custom object or a hash table.
.PARAMETER InputObject
	The data record to convert.
.PARAMETER As
	Value indicating whether to convert the data record to a hash table.
.INPUTS
	The data record to convert.
.PARAMETER As
	The type to which the returned record should be converted.
#>
function ConvertFrom-Record {
	[OutputType([object])]
	param (
		[Parameter(Mandatory, Position = 0, ValueFromPipeline)]
		[System.Data.IDataRecord] $InputObject,

		[ValidateNotNull()]
		[type] $As = ([psobject])
	)

	process {
		$hashtable = [ordered]@{}
		for ($index = 0; $index -lt $InputObject.FieldCount; $index++) {
			$key = $InputObject.GetName($index)
			$hashtable.$key = $InputObject.IsDBNull($index) ? $null : $InputObject.GetValue($index)
		}

		switch ($As) {
			([hashtable]) { $hashtable; break }
			([ordered]) { $hashtable; break }
			([psobject]) { [pscustomobject] $hashtable; break }
			default { [SqlMapper]::CreateInstance($As, $hashtable) }
		}
	}
}
