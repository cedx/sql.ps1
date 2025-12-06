namespace Belin.Sql.Cmdlets;

using System.Collections;
using System.Data;

/// <summary>
/// Creates a new command associated with the specified connection.
/// </summary>
[Cmdlet(VerbsCommon.New, "Command")]
[OutputType(typeof(IDbCommand))]
public class NewCommandCommand: Cmdlet {

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
		var command = Connection.CreateCommand();
		command.CommandText = Command;
		command.CommandTimeout = Timeout;

		if (PositionalParameters is not null) for (var index = 0; index < PositionalParameters.Length; index++) {
			var parameters = new NewParameterCommand { Command = command, Name = $"QuestionMark{index}", Value = PositionalParameters[index] };
			command.Parameters.Add(parameters.Invoke<IDbDataParameter>().Single());
		}

		if (Parameters is not null) foreach (var key in Parameters.Keys) {
			var parameters = new NewParameterCommand { Command = command, Name = $"@{key}", Value = Parameters[key] };
			command.Parameters.Add(parameters.Invoke<IDbDataParameter>().Single());
		}

		WriteObject(command);
	}
}
