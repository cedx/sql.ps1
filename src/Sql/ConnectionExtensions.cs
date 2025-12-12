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
	private static readonly Mapper dataMapper = new();

	/// <summary>
	/// Creates a new command associated with the specified connection.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="sql">The SQL query to be executed.</param>
	/// <param name="parameters">The parameters of the SQL query.</param>
	/// <param name="options">The command options.</param>
	/// <returns>The newly created command.</returns>
	public static IDbCommand CreateCommand(this IDbConnection connection, string sql, ParameterCollection? parameters = null, CommandOptions? options = null) {
		var dbCommand = connection.CreateCommand();
		dbCommand.CommandText = sql;
		dbCommand.CommandTimeout = options?.Timeout ?? 30;
		dbCommand.CommandType = options?.Type ?? CommandType.Text;
		dbCommand.Transaction = options?.Transaction;

		if (parameters is not null) foreach (var parameter in parameters) {
			var dbParameter = dbCommand.CreateParameter();
			dbParameter.ParameterName =	parameter.ParameterName;
			dbParameter.Value = parameter.Value;
			if (parameter.DbType is not null) dbParameter.DbType = parameter.DbType.Value;
			if (parameter.Direction is not null) dbParameter.Direction = parameter.Direction.Value;
			if (parameter.Precision is not null) dbParameter.Precision = parameter.Precision.Value;
			if (parameter.Scale is not null) dbParameter.Scale = parameter.Scale.Value;
			if (parameter.Size is not null) dbParameter.Size = parameter.Size.Value;
			dbCommand.Parameters.Add(dbParameter);
		}

		return dbCommand;
	}
}
