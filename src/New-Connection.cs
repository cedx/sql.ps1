namespace Belin.Sql.Cmdlets;

using System.Data;

/// <summary>
/// Creates a new database connection.
/// </summary>
[Cmdlet(VerbsCommon.New, "Connection")]
[OutputType(typeof(IDbConnection))]
public class NewConnection: Cmdlet {

	/// <summary>
	/// The connection string used to open the database.
	/// </summary>
	[Parameter(Mandatory = true, Position = 1, ValueFromPipeline = true)]
	public required string ConnectionString { get; set; }

	/// <summary>
	/// Value indicating whether to open the connection.
	/// </summary>
	[Parameter]
	public SwitchParameter Open { get; set; }

	/// <summary>
	/// The type of connection class to instantiate.
	/// </summary>
	[Parameter(Mandatory = true, Position = 0)]
	public required Type Type { get; set; }

	/// <summary>
	/// Performs execution of this command.
	/// </summary>
	protected override void ProcessRecord() {
		var connection = (IDbConnection) Activator.CreateInstance(Type, ConnectionString)!;
		if (Open) connection.Open();
		WriteObject(connection);
	}
}
