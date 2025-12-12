namespace Belin.Sql.Dapper;

using System.Data;

/// <summary>
/// Maps a uniform resource identifier (URI) to or from a string.
/// </summary>
public class UriHandler: SqlMapper.TypeHandler<Uri> {

	/// <summary>
	/// Parses a database value back to a typed value.
	/// </summary>
	/// <param name="value">The value from the database.</param>
	/// <returns>The typed value.</returns>
	public override Uri? Parse(object value) =>
		value is string uri && uri.Length > 0 ? new Uri(uri, UriKind.Absolute) : null;

	/// <summary>
	/// Assigns the value of a parameter before a command executes.
	/// </summary>
	/// <param name="parameter">The parameter to configure.</param>
	/// <param name="value">The parameter value.</param>
	public override void SetValue(IDbDataParameter parameter, Uri? value) =>
		parameter.Value = value?.ToString();
}
