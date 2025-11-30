namespace Belin.Sql.Cmdlets;

using Belin.Sql.Mapping;
using System.Collections;
using System.Data;

/// <summary>
/// Executes a parameterized SQL query and returns the first row.
/// </summary>
[Cmdlet(VerbsCommon.Get, "First")]
[OutputType(typeof(object))]
public class GetFirst: Cmdlet {

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

		var record = adapter.Reader.Read() ? adapter.Mapper.CreateInstance(As ?? typeof(PSObject), adapter.Reader) : null;
		adapter.Reader.Close();

		if (record is null) {
			var exception = new InvalidOperationException("The result set is empty.");
			WriteError(new ErrorRecord(exception, "EmptyResultSet", ErrorCategory.InvalidOperation, null));
		}

		WriteObject(record);
	}
}
