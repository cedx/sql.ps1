namespace Belin.Sql.Cmdlets;

using System.Data;
using System.Text.RegularExpressions;

/// <summary>
/// Returns the version of the server associated with the specified connection.
/// </summary>
[Cmdlet(VerbsCommon.Get, "Version"), OutputType(typeof(Version))]
public partial class GetVersionCommand: Cmdlet {

	/// <summary>
	/// Gets the regular expression used to parse the server version.
	/// </summary>
	/// <returns>The regular expression used to parse the server version.</returns>
	[GeneratedRegex(@"^\d+(\.\d+)+")]
	private static partial Regex VersionPattern();

	/// <summary>
	/// The SQL query to be executed.
	/// </summary>
	public string? Command => Connection.GetType().FullName switch {
		"Microsoft.Data.SqlClient.SqlConnection" => "SELECT SERVERPROPERTY('ProductVersion')",
		"Microsoft.Data.Sqlite.SqlConnection" => "SELECT sqlite_version()",
		"MySql.Data.MySqlClient.MySqlConnection" => "SELECT VERSION()",
		"MySqlConnector.MySqlConnection" => "SELECT VERSION()",
		"Npgsql.NpgsqlConnection" => "SHOW server_version",
		"System.Data.SqlClient.SqlConnection" => "SELECT SERVERPROPERTY('ProductVersion')",
		_ => null
	};

	/// <summary>
	/// The connection to the data source.
	/// </summary>
	[Parameter(Mandatory = true, Position = 0)]
	public required IDbConnection Connection { get; set; }

	/// <summary>
	/// Performs execution of this command.
	/// </summary>
	protected override void ProcessRecord() {
		var version = Command is string command ? Connection.ExecuteScalar<string>(command) : null;
		if (version is not null) {
			var match = VersionPattern().Match(version);
			if (match.Success) {
				WriteObject(Version.Parse(match.Value));
				return;
			}
		}

		var exception = new InvalidOperationException("The server version could not be determined.");
		WriteError(new ErrorRecord(exception, "UnknownServerVersion", ErrorCategory.InvalidOperation, null));
		WriteObject(null);
	}
}
