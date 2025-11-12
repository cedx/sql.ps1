using module ./Get-Scalar.psm1

<#
.SYNOPSIS
	Returns the version of the server associated with the specified connection.
.OUTPUTS
	The version of the server associated with the specified connection.
#>
function Get-Version {
	[CmdletBinding()]
	[OutputType([version])]
	param (
		[Parameter(Mandatory, Position = 0)]
		[System.Data.IDbConnection] $Connection
	)

	$invalidOperation = [InvalidOperationException] "The server version cannot be determined."
	$command = switch ($Connection.GetType().FullName) {
		"Microsoft.Data.SqlClient.SqlConnection" { "SELECT SERVERPROPERTY('ProductVersion')"; break }
		"Microsoft.Data.Sqlite.SqlConnection" { "SELECT sqlite_version()"; break }
		"MySql.Data.MySqlClient.MySqlConnection" { "SELECT VERSION()"; break }
		"MySqlConnector.MySqlConnection" { "SELECT VERSION()"; break }
		"Npgsql.NpgsqlConnection" { "SHOW server_version"; break }
		"System.Data.SqlClient.SqlConnection" { "SELECT SERVERPROPERTY('ProductVersion')"; break }
		default { throw $invalidOperation }
	}

	$version = Get-Scalar $Connection -Command $command
	if ((-not $version) -or ($version -notmatch "^\d+(\.\d+)*")) { throw $invalidOperation }
	[version] $Matches.0
}
