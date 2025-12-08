namespace Belin.Sql.Cmdlets;

using System.Collections;
using System.Data;
using System.Dynamic;
using System.Reflection;

/// <summary>
/// Executes a parameterized SQL query and returns an array of objects whose properties correspond to the columns.
/// </summary>
[Cmdlet(VerbsLifecycle.Invoke, "Query", DefaultParameterSetName = nameof(Parameters)), OutputType(typeof(object[]))]
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
		IDictionary<string, object?> parameters = ParameterSetName == nameof(PositionalParameters)
			? PositionalParameters.ToOrderedDictionary()
			: Parameters.Cast<DictionaryEntry>().ToDictionary(entry => entry.Key.ToString()!, entry => entry.Value);

		var method = typeof(ConnectionExtensions).GetMethod(nameof(ConnectionExtensions.Query))!.MakeGenericMethod(As);
		var records = (IEnumerable<object>) method.Invoke(null, [Connection, Command, parameters, new QueryOptions(Timeout: Timeout, Type: CommandType)])!;
		WriteObject(records.ToArray());
	}
}
