namespace Belin.Sql.Cmdlets;

using System.Data;

/// <summary>
/// Creates a new parameter associated with the specified command.
/// </summary>
[Cmdlet(VerbsCommon.New, "Parameter")]
[OutputType(typeof(IDbDataParameter))]
public class NewParameterCommand: Cmdlet {

	/// <summary>
	/// A command connected to a data source.
	/// </summary>
	[Parameter(Mandatory = true, Position = 0)]
	public required IDbCommand Command { get; set; }
	
	/// <summary>
	/// The parameter direction.
	/// </summary>
	[Parameter]
	public ParameterDirection? Direction { get; set; }

	/// <summary>
	/// The parameter name.
	/// </summary>
	[Parameter(Mandatory = true, Position = 1)]
	public required string Name { get; set; }

	/// <summary>
	/// The parameter type.
	/// </summary>
	[Parameter]
	public DbType? Type { get; set; }

	/// <summary>
	/// The parameter value.
	/// </summary>
	[Parameter(Mandatory = true, Position = 2), AllowNull]
	public required object? Value { get; set; }

	/// <summary>
	/// Performs execution of this command.
	/// </summary>
	protected override void ProcessRecord() =>
		WriteObject(Command.CreateParameter(Name, Value, Type, Direction));
}
