using module ./Get-Scalar.psm1

<#
.SYNOPSIS
	Returns the version of the server associated with the specified connection.
.OUTPUTS
	The version of the server associated with the specified connection.
#>
function Get-ServerVersion {
	[CmdletBinding()]
	[OutputType([version])]
	param (
		[Parameter(Mandatory, Position = 0)]
		[System.Data.IDbConnection] $Connection
	)

	$command = switch ($Connection.GetType().FullName) {
		"Microsoft.Data.SqlClient.SqlConnection" { "SELECT SERVERPROPERTY('ProductVersion')"; break }
		"Microsoft.Data.Sqlite.SqlConnection" { "SELECT sqlite_version()"; break }
		"MySql.Data.MySqlClient.MySqlConnection" { "SELECT VERSION()"; break }
		"MySqlConnector.MySqlConnection" { "SELECT VERSION()"; break }
		"Npgsql.NpgsqlConnection" { "SHOW server_version"; break }
		"System.Data.SqlClient.SqlConnection" { "SELECT SERVERPROPERTY('ProductVersion')"; break }
		default { throw [InvalidOperationException] "TODO" }
	}

	$version = Get-Scalar $Connection -Command $command
	if (-not $version) { throw [InvalidOperationException] "TODO" }
	if ($version -notmatch "^(\d+(\.\d+)*)[^\.\d]*") { throw [InvalidOperationException] "TODO" }
	[version] $Matches.1

	# TODO !!!!
	# MariaDB : SELECT VERSION() => 10.11.7-MariaDB
	# PostgreSQL : SHOW server_version => 16.10 (Ubuntu 16.10-0ubuntu0.24.04.1)
	# SQL Server : SELECT SERVERPROPERTY('ProductVersion') => 16.0.1150.1
}
