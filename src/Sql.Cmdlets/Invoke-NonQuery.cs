namespace Belin.Sql.Cmdlets;

using System.Collections;
using System.Data;

/// <summary>
/// Executes a parameterized SQL statement.
/// </summary>
[Cmdlet(VerbsLifecycle.Invoke, "NonQuery", DefaultParameterSetName = nameof(Parameters)), OutputType(typeof(int))]
public class InvokeNonQueryCommand: PSCmdlet {

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

		WriteObject(Connection.Execute(Command, parameters, new(Timeout: Timeout, Type: CommandType)));
	}
}
