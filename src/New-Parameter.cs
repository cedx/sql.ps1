namespace Belin.Sql.Cmdlets;

using System.Data;
using System.Reflection.Metadata;

/// <summary>
/// Creates a new parameter associated with the specified command.
/// </summary>
[Cmdlet(VerbsCommon.New, "Parameter")]
[OutputType(typeof(void))]
public class NewParameter: Cmdlet {

	/// <summary>
	/// A command connected to a data source.
	/// </summary>
	[Parameter(Mandatory = true, Position = 0)]
	public required IDbCommand Command { get; set; }

	/// <summary>
	/// A command connected to a data source.
	/// </summary>
	[Parameter(Mandatory = true, Position = 1)]
	public required string Name { get; set; }

	/// <summary>
	/// A command connected to a data source.
	/// </summary>
	[Parameter(Mandatory = true, Position = 2), AllowNull]
	public required object? Value { get; set; }

	/// <summary>
	/// Performs execution of this command.
	/// </summary>
	protected override void ProcessRecord() {
		var parameter = Command.CreateParameter();
		parameter.ParameterName = Name;
		parameter.Value = Value is null ? DBNull.Value : Value;
		WriteObject(parameter);
	}
}
