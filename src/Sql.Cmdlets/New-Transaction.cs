namespace Belin.Sql.Cmdlets;

using System.Data;

/// <summary>
/// Creates a new transaction associated with the specified connection.
/// </summary>
[Cmdlet(VerbsCommon.New, "Transaction"), OutputType(typeof(IDbTransaction))]
public class NewTransactionCommand: Cmdlet {

	/// <summary>
	/// The connection to the data source.
	/// </summary>
	[Parameter(Mandatory = true, Position = 0)]
	public required IDbConnection Connection { get; set; }

	/// <summary>
	/// The isolation level for the transaction to use.
	/// </summary>
	[Parameter(Position = 1)]
	public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.Unspecified;

	/// <summary>
	/// Performs execution of this command.
	/// </summary>
	protected override void ProcessRecord() => WriteObject(Connection.BeginTransaction(IsolationLevel));
}
