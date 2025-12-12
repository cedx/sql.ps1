namespace Belin.Sql;

using System.Data;

/// <summary>
/// Provides extension members for database connections.
/// </summary>
public static partial class ConnectionExtensions {
	// TODO (.NET 10) extension(IDbConnection connection)

	/// <summary>
	/// The data mapper used to map data records to entity objects.
	/// </summary>
	private static readonly DataMapper dataMapper = new();

	/// <summary>
	/// Creates a new command associated with the specified connection.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="sql">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The newly created command.</returns>
	public static IDbCommand CreateCommand(this IDbConnection connection, string sql, IDictionary<string, object?>? parameters = null, CommandOptions? options = null) {
		var command = connection.CreateCommand();
		command.CommandText = sql;
		command.CommandTimeout = options?.Timeout ?? 30;
		command.CommandType = options?.Type ?? CommandType.Text;
		command.Transaction = options?.Transaction;

		if (parameters is not null)
			foreach (var (key, value) in parameters) command.Parameters.Add(command.CreateParameter($"@{key}", value));

		return command;
	}
}
