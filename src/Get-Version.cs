namespace Belin.Sql.Cmdlets;

using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

/// <summary>
/// Returns the version of the server associated with the specified connection.
/// </summary>
[Cmdlet(VerbsCommon.Get, "Version")]
[OutputType(typeof(Version))]
public partial class GetVersion: Cmdlet {

	/// <summary>
	/// Gets the regular expression used to parse the server version.
	/// </summary>
	/// <returns>The regular expression used to parse the server version.</returns>
	[GeneratedRegex(@"^\d+(\.\d+)+")]
	private static partial Regex VersionPattern();

	/// <summary>
	/// The connection to the data source.
	/// </summary>
	[Parameter(Mandatory = true, Position = 0)]
	public required IDbConnection Connection { get; set; }

	/// <summary>
	/// Performs execution of this command.
	/// </summary>
	protected override void ProcessRecord() {
		if (Connection is DbConnection dbConnection) {
			var match = VersionPattern().Match(dbConnection.ServerVersion);
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
