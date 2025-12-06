namespace Belin.Sql.Cmdlets;

using System.Data;

/// <summary>
/// Closes the specified database connection.
/// </summary>
[Cmdlet(VerbsCommon.Close, "Connection")]
[OutputType(typeof(void))]
public class CloseConnectionCommand: Cmdlet {

	/// <summary>
	/// The connection to the data source.
	/// </summary>
	[Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
	public required IDbConnection Connection { get; set; }

	/// <summary>
	/// Performs execution of this command.
	/// </summary>
	protected override void ProcessRecord() => Connection.Close();
}
