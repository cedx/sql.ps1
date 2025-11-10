using namespace System.Data
using module ./Invoke-Reader.psm1
using module ./Mapping/ConvertFrom-Reader.psm1

<#
.SYNOPSIS
	Executes a parameterized SQL query and returns a sequence of objects whose properties correspond to the columns.
.PARAMETER Connection
	The connection to the data source.
.PARAMETER Command
	The SQL query to be executed.
.PARAMETER Parameters
	The parameters of the SQL query.
.PARAMETER AsHashtable
	Value indicating whether to convert the rows to a hash table.
.OUTPUTS
	[hashtable[]] The sequence of hash tables whose keys correspond to the returned columns.
.OUTPUTS
	[psobject[]] The sequence of custom objects whose properties correspond to the returned columns.
#>
function Invoke-Query {
	[CmdletBinding()]
	[OutputType([hashtable[]], [psobject[]])]
	param (
		[Parameter(Mandatory, Position = 0)]
		[IDbConnection] $Connection,

		[Parameter(Mandatory, Position = 1)]
		[string] $Command,

		[Parameter(Position = 2)]
		[ValidateNotNull()]
		[hashtable] $Parameters = @{},

		[Parameter()]
		[switch] $AsHashtable
	)

	$reader = Invoke-Reader $Connection -Command $Command -Parameters $Parameters -AsHashtable:$AsHashtable
	ConvertFrom-Reader $reader
}
