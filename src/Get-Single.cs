namespace Belin.Sql.Cmdlets;

using Belin.Sql.Mapping;
using System.Collections;
using System.Data;

/// <summary>
/// Executes a parameterized SQL query and returns the single row.
/// </summary>
[Cmdlet(VerbsCommon.Get, "Single")]
[OutputType(typeof(object))]
public class GetSingle: Cmdlet {

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
	/// The connection to the data source.
	/// </summary>
	[Parameter(Mandatory = true, Position = 0)]
	public required IDbConnection Connection { get; set; }

	/// <summary>
	/// The named parameters of the SQL query.
	/// </summary>
	[Parameter(Position = 2)]
	public Hashtable? Parameters { get; set; }

	/// <summary>
	/// The positional parameters of the SQL query.
	/// </summary>
	[Parameter]
	public object[]? PositionalParameters { get; set; }

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
			new InvokeReader { Command = Command, Connection = Connection, Parameters = Parameters, PositionalParameters = PositionalParameters, Timeout = Timeout }
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
			var exception = new InvalidOperationException("The result set is empty or contains more than one record.");
			WriteError(new ErrorRecord(exception, "InvalidResultSet", ErrorCategory.InvalidOperation, null));
		}

		WriteObject(rowCount == 1 ? record : null);
	}
}
