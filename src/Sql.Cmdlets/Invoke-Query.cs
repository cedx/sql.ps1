namespace Belin.Sql.Cmdlets;

using System.Collections;
using System.Data;
using System.Dynamic;

/// <summary>
/// Executes a parameterized SQL query and returns an array of objects whose properties correspond to the columns.
/// </summary>
[Cmdlet(VerbsLifecycle.Invoke, "Query", DefaultParameterSetName = nameof(Parameters))]
[OutputType(typeof(object[]))]
public class InvokeQueryCommand: PSCmdlet {

	/// <summary>
	/// The type of objects to return.
	/// </summary>
	[Parameter]
	public Type As { get; set; } = typeof(ExpandoObject);

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
	[Parameter(ParameterSetName = nameof(Parameters), Position = 2)]
	public Hashtable Parameters { get; set; } = [];

	/// <summary>
	/// The positional parameters of the SQL query.
	/// </summary>
	[Parameter(ParameterSetName = nameof(PositionalParameters))]
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
		if (Connection.State == ConnectionState.Closed) Connection.Open();

		var adapter =
			new InvokeReaderCommand { Command = Command, Connection = Connection, Parameters = Parameters, PositionalParameters = PositionalParameters, Timeout = Timeout }
			.Invoke<DataAdapter>()
			.Single();

		WriteObject(adapter.Mapper.CreateInstances(As ?? typeof(PSObject), adapter.Reader).ToArray());
	}
}
