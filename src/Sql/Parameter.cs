namespace Belin.Sql;

using System.Data;

/// <summary>
/// Represents a parameter of a parameterized SQL statement.
/// </summary>
/// <param name="parameterName">The parameter name.</param>
/// <param name="parameterValue">The parameter value.</param>
public sealed class Parameter(string parameterName, object? parameterValue = default) {

	/// <summary>
	/// The database type of this parameter.
	/// </summary>
	public DbType? DbType { get; set; }

	/// <summary>
	/// Value indicating whether this parameter is input-only, output-only, bidirectional, or a stored procedure return value parameter.
	/// </summary>
	public ParameterDirection? Direction { get; set; }

	/// <summary>
	/// The parameter name.
	/// </summary>
	public string ParameterName { get; set; } = parameterName;

	/// <summary>
	/// Indicates the precision of numeric parameters.
	/// </summary>
	public byte? Precision { get; set; }

	/// <summary>
	/// Indicates the scale of numeric parameters.
	/// </summary>
	public byte? Scale { get; set; }

	/// <summary>
	/// The maximum size of this parameter, in bytes.
	/// </summary>
	public int? Size { get; set; }

	/// <summary>
	/// The parameter value.
	/// </summary>
	public object? Value { get; set; } = parameterValue;

	/// <summary>
	/// Creates a new parameter from the specified name/value pair.
	/// </summary>
	/// <param name="parameter">The name/value pair providing the parameter name and value.</param>
	/// <returns>The parameter corresponding to the specified name/value pair.</returns>
	public static implicit operator Parameter((string Name, object? Value) parameter) =>
		new(parameter.Name, parameter.Value);

	/// <summary>
	/// Creates a new parameter from the specified name/value pair.
	/// </summary>
	/// <param name="parameter">The name/value pair providing the parameter name and value.</param>
	/// <returns>The parameter corresponding to the specified name/value pair.</returns>
	public static implicit operator Parameter((string Name, object? Value, DbType DbType) parameter) =>
		new(parameter.Name, parameter.Value) { DbType = parameter.DbType };
}
