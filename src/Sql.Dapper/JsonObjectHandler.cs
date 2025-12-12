namespace Belin.Sql.Dapper;

using System.Data;
using System.Text.Json;

/// <summary>
/// Maps a JSON object to a dictionary or from a string.
/// </summary>
/// <typeparam name="T">The type of the object properties.</param>
/// <param name="options">The options for controlling deserialization and serialization behavior.</param>
public class JsonObjectHandler<T>(JsonSerializerOptions? options = null): SqlMapper.TypeHandler<Dictionary<string, T>> {

	/// <summary>
	/// Parses a database value back to a typed value.
	/// </summary>
	/// <param name="value">The value from the database.</param>
	/// <returns>The typed value.</returns>
	public override Dictionary<string, T>? Parse(object value) =>
		value is string json && json.Length > 0 ? JsonSerializer.Deserialize<Dictionary<string, T>>(json, options) : null;

	/// <summary>
	/// Assigns the value of a parameter before a command executes.
	/// </summary>
	/// <param name="parameter">The parameter to configure.</param>
	/// <param name="value">The parameter value.</param>
	public override void SetValue(IDbDataParameter parameter, Dictionary<string, T>? value) =>
		parameter.Value = JsonSerializer.Serialize(value, options);
}
