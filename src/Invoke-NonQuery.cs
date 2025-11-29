namespace Belin.Sql.Cmdlets;

using System.Collections;
using System.Data;

/// <summary>
/// Executes a parameterized SQL statement.
/// </summary>
[Cmdlet(VerbsLifecycle.Invoke, "NonQuery")]
[OutputType(typeof(int))]
public class InvokeNonQuery: Cmdlet {
	
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

		using var command =
			new NewCommand { Command = Command, Connection = Connection, Parameters = Parameters, PositionalParameters = PositionalParameters, Timeout = Timeout }
			.Invoke<IDbCommand>()
			.First();

		WriteObject(command.ExecuteNonQuery());
	}
}
