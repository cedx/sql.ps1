namespace Belin.Sql;

using System.Data;

/// <summary>
/// Provides extension members for database commands.
/// </summary>
public static class CommandExtensions {
	// TODO (.NET 10) extension(IDbCommand command)

	/// <summary>
	/// Creates a new parameter associated with the specified command.
	/// </summary>
	/// <param name="command">A command connected to a data source.</param>
	/// <param name="name">The parameter name.</param>
	/// <param name="value">The parameter value.</param>
	/// <param name="dbType">The parameter type.</param>
	/// <param name="direction">The parameter direction.</param>
	/// <returns>The newly created parameter.</returns>
	public static IDbDataParameter CreateParameter(this IDbCommand command, string name, object? value, DbType? dbType = null, ParameterDirection direction = ParameterDirection.Input) {
		var parameter = command.CreateParameter();
		parameter.Direction = direction;
		parameter.ParameterName = name;
		parameter.Value = value ?? DBNull.Value;
		if (dbType is not null) parameter.DbType = dbType.Value;
		return parameter;
	}
}
