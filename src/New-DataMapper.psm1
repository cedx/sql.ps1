using namespace System.Diagnostics.CodeAnalysis
using module ./Mapping/DataMapper.psm1

<#
.SYNOPSIS
	Creates a new data mapper.
.OUTPUTS
	The newly created data mapper.
#>
function New-DataMapper {
	[CmdletBinding()]
	[OutputType([DataMapper])]
	[SuppressMessage("PSUseShouldProcessForStateChangingFunctions", "")]
	param ()

	[DataMapper]::new()
}
