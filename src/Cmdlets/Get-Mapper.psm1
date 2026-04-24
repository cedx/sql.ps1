using namespace Belin.Sql

<#
.SYNOPSIS
	Gets the singleton instance of the data mapper.
.OUTPUTS
	The singleton instance of the data mapper.
#>
function Get-Mapper {
	[CmdletBinding()]
	[OutputType([Belin.Sql.Mapper])]
	param ()

	[Mapper]::Instance
}
