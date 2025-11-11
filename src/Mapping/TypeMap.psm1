<#
.SYNOPSIS
	TODO
#>
class TypeMap {

	<#
	.SYNOPSIS
		TODO
	#>
	hidden [hashtable] $Columns = @{}

	<#
	.SYNOPSIS
		TODO
	#>
	hidden [type] $Type

	<#
	.SYNOPSIS
		Creates a new type map.
	.PARAMETER Type
		The entity type to override.
	#>
	TypeMap([type] $Type) {
		$this.Type = $Type
	}

	<#
	.SYNOPSIS
		TODO
	.OUTPUTS
		TODO
	#>
	hidden [hashtable] GetColumnMapping() {
		# TODO
		return @{}
	}
}
