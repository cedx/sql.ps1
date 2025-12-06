namespace Belin.Sql;

using System.Data;

/// <summary>
/// Provides extension members for database connections.
/// </summary>
public static partial class ConnectionExtensions {

	/// <summary>
	/// TODO The data mapper used to TODO
	/// </summary>
	public static DataMapper Mapper { get; set; } = new();

	/// <summary>
	/// Creates a new command associated with the specified connection.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="sql">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The newly created command.</returns>
	public static IDbCommand CreateCommand(this IDbConnection connection, string sql, IDictionary<string, object?>? parameters = null, QueryOptions? options = null) {
		var command = connection.CreateCommand();
		command.CommandText = sql;
		command.CommandTimeout = options?.Timeout ?? 30;

		if (parameters is not null)
			foreach (var (key, value) in parameters)
				command.Parameters.Add(command.CreateParameter($"@{key}", value ?? DBNull.Value));

		return command;
	}

	/// <summary>
	/// Creates a new parameter associated with the specified command.
	/// </summary>
	/// <param name="command">A command connected to a data source.</param>
	/// <param name="name">The parameter name.</param>
	/// <param name="value">The parameter value.</param>
	/// <returns>The newly created parameter.</returns>
	public static IDbDataParameter CreateParameter(this IDbCommand command, string name, object? value) {
		var parameter = command.CreateParameter();
		parameter.ParameterName = name;
		parameter.Value = value ?? DBNull.Value;
		return parameter;
	}

	/// <summary>
	/// Converts the specified list into an ordered dictionary.
	/// </summary>
	/// <param name="list">The list to convert.</param>
	/// <returns>The ordered dictionary corresponding to the specified list.</returns>
	private static OrderedDictionary<string, object?> ToOrderedDictionary(IList<object?> list) =>
		new(list.Index().Select((index, value) => new KeyValuePair<string, object?>($"QuestionMark{index}", value)));
}
