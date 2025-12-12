namespace Belin.Sql.Cmdlets;

using System.Data;

/// <summary>
/// Creates a new command parameter.
/// </summary>
[Cmdlet(VerbsCommon.New, "Parameter"), OutputType(typeof(Parameter))]
public class NewParameterCommand: Cmdlet {

	/// <summary>
	/// The database type of this parameter.
	/// </summary>
	[Parameter]
	public DbType? DbType { get; set; }

	/// <summary>
	/// Value indicating whether this parameter is input-only, output-only, bidirectional, or a stored procedure return value parameter.
	/// </summary>
	[Parameter]
	public ParameterDirection? Direction { get; set; }

	/// <summary>
	/// The parameter name.
	/// </summary>
	[Parameter(Mandatory = true, Position = 1)]
	public required string Name { get; set; }

	/// <summary>
	/// Indicates the precision of numeric parameters.
	/// </summary>
	[Parameter]
	public byte? Precision { get; set; }

	/// <summary>
	/// Indicates the scale of numeric parameters.
	/// </summary>
	[Parameter]
	public byte? Scale { get; set; }

	/// <summary>
	/// The maximum size of this parameter, in bytes.
	/// </summary>
	[Parameter]
	public int? Size { get; set; }

	/// <summary>
	/// The parameter value.
	/// </summary>
	[Parameter(Position = 2)]
	public object? Value { get; set; }

	/// <summary>
	/// Performs execution of this command.
	/// </summary>
	protected override void ProcessRecord() => WriteObject(new Parameter(Name, Value) {
		DbType = DbType,
		Direction = Direction,
		Precision = Precision,
		Scale = Scale,
		Size = Size
	});
}
