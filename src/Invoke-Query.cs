namespace Belin.Sql.Cmdlets;

using Belin.Sql.Mapping;
using System.Collections;
using System.Data;

/// <summary>
/// Executes a parameterized SQL query and returns an array of objects whose properties correspond to the columns.
/// </summary>
[Cmdlet(VerbsLifecycle.Invoke, "Query")]
[OutputType(typeof(object[]))]
public class InvokeQuery: Cmdlet {

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
		if (Connection.State == ConnectionState.Closed) Connection.Open();

		var adapter =
			new InvokeReader { Command = Command, Connection = Connection, Parameters = Parameters, PositionalParameters = PositionalParameters, Timeout = Timeout }
			.Invoke<DataAdapter>()
			.Single();

		WriteObject(adapter.Mapper.CreateInstances(As ?? typeof(PSObject), adapter.Reader).ToArray());
	}
}
