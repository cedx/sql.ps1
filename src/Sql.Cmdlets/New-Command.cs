namespace Belin.Sql.Cmdlets;

using System.Data;

/// <summary>
/// Creates a new command associated with the specified connection.
/// </summary>
[Cmdlet(VerbsCommon.New, "Command"), OutputType(typeof(IDbCommand))]
public class NewCommandCommand: PSCmdlet {

	/// <summary>
	/// The SQL query to be executed.
	/// </summary>
	[Parameter(Mandatory = true, Position = 1)]
	public required string Command { get; set; }

	/// <summary>
	/// Value indicating how the command is interpreted.
	/// </summary>
	[Parameter]
	public CommandType CommandType { get; set; } = CommandType.Text;

	/// <summary>
	/// The connection to the data source.
	/// </summary>
	[Parameter(Mandatory = true, Position = 0)]
	public required IDbConnection Connection { get; set; }

	/// <summary>
	/// The parameters of the SQL query.
	/// </summary>
	[Parameter(Position = 2)]
	public ParameterCollection Parameters { get; set; } = [];

	/// <summary>
	/// The wait time, in seconds, before terminating the attempt to execute the command and generating an error.
	/// </summary>
	[Parameter, ValidateRange(ValidateRangeKind.Positive)]
	public int Timeout { get; set; } = 30;

	/// <summary>
	/// The transaction to use, if any.
	/// </summary>
	[Parameter]
	public IDbTransaction? Transaction { get; set; }

	/// <summary>
	/// Performs execution of this command.
	/// </summary>
	protected override void ProcessRecord() =>
		WriteObject(Connection.CreateCommand(Command, Parameters, new(Timeout, Transaction, CommandType)));
}
