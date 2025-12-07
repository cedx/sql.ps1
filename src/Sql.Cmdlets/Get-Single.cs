namespace Belin.Sql.Cmdlets;

using System.Collections;
using System.Data;

/// <summary>
/// Executes a parameterized SQL query and returns the single row.
/// </summary>
[Cmdlet(VerbsCommon.Get, "Single", DefaultParameterSetName = "Parameters")]
[OutputType(typeof(object))]
public class GetSingleCommand: PSCmdlet {

	/// <summary>
	/// The type of objects to return.
	/// </summary>
	[Parameter]
	public Type? As { get; set; }

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
	/// The named parameters of the SQL query.
	/// </summary>
	[Parameter(ParameterSetName = "Parameters", Position = 2)]
	public Hashtable Parameters { get; set; } = [];

	/// <summary>
	/// The positional parameters of the SQL query.
	/// </summary>
	[Parameter(ParameterSetName = "PositionalParameters")]
	public object[] PositionalParameters { get; set; } = [];

	/// <summary>
	/// The wait time, in seconds, before terminating the attempt to execute the command and generating an error.
	/// </summary>
	[Parameter, ValidateRange(ValidateRangeKind.NonNegative)]
	public int Timeout { get; set; } = 30;

	/// <summary>
	/// Performs execution of this command.
	/// </summary>
	protected override void ProcessRecord() {
		var adapter =
			new InvokeReaderCommand { Command = Command, Connection = Connection, Parameters = Parameters, PositionalParameters = PositionalParameters, Timeout = Timeout }
			.Invoke<DataAdapter>()
			.Single();

		object? record = null;
		var rowCount = 0;
		while (adapter.Reader.Read()) {
			if (++rowCount > 1) break;
			record = adapter.Mapper.CreateInstance(As ?? typeof(PSObject), adapter.Reader);
		}

		adapter.Reader.Close();
		if (rowCount != 1) {
			var exception = new InvalidOperationException("The record set is empty or contains more than one record.");
			WriteError(new ErrorRecord(exception, "InvalidRecordSet", ErrorCategory.InvalidOperation, null));
		}

		WriteObject(rowCount == 1 ? record : null);
	}
}
