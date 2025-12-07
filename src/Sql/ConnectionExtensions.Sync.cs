namespace Belin.Sql;

using System.Data;
using System.Globalization;

/// <summary>
/// Provides extension members for database connections.
/// </summary>
public static partial class ConnectionExtensions {

	/// <summary>
	/// Executes a parameterized SQL statement.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The number of rows affected.</returns>
	public static int Execute(this IDbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null) {
		if (connection.State == ConnectionState.Closed) connection.Open();
		using var dbCommand = CreateCommand(connection, command, parameters, options);
		return dbCommand.ExecuteNonQuery();
	}

	/// <summary>
	/// Executes a parameterized SQL statement.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The number of rows affected.</returns>
	public static int Execute(this IDbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null) =>
		Execute(connection, command, (parameters ?? []).ToOrderedDictionary(), options);

	/// <summary>
	/// Executes a parameterized SQL query and returns a data reader.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The data reader that can be used to access the results.</returns>
	public static IDataReader ExecuteReader(this IDbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null) {
		if (connection.State == ConnectionState.Closed) connection.Open();
		using var dbCommand = CreateCommand(connection, command, parameters, options);
		return dbCommand.ExecuteReader();
	}

	/// <summary>
	/// Executes a parameterized SQL query and returns a data reader.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The data reader that can be used to access the results.</returns>
	public static IDataReader ExecuteReader(this IDbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null) =>
		ExecuteReader(connection, command, (parameters ?? []).ToOrderedDictionary(), options);

	/// <summary>
	/// Executes a parameterized SQL query that selects a single value.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The first column of the first row.</returns>
	public static object? ExecuteScalar(this IDbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null) {
		if (connection.State == ConnectionState.Closed) connection.Open();
		using var dbCommand = CreateCommand(connection, command, parameters, options);
		return dbCommand.ExecuteScalar();
	}

	/// <summary>
	/// Executes a parameterized SQL query that selects a single value.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The first column of the first row.</returns>
	public static object? ExecuteScalar(this IDbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null) =>
		ExecuteScalar(connection, command, (parameters ?? []).ToOrderedDictionary(), options);

	/// <summary>
	/// Executes a parameterized SQL query that selects a single value.
	/// </summary>
	/// <typeparam name="T">The type of object to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The first column of the first row.</returns>
	public static T? ExecuteScalar<T>(this IDbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null) where T: IConvertible =>
		(T?) Convert.ChangeType(ExecuteScalar(connection, command, parameters, options), typeof(T), CultureInfo.InvariantCulture);

	/// <summary>
	/// Executes a parameterized SQL query that selects a single value.
	/// </summary>
	/// <typeparam name="T">The type of object to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The first column of the first row.</returns>
	public static T? ExecuteScalar<T>(this IDbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null) where T: IConvertible =>
		(T?) Convert.ChangeType(ExecuteScalar(connection, command, parameters, options), typeof(T), CultureInfo.InvariantCulture);

	/// <summary>
	/// Executes a parameterized SQL query and returns a sequence of objects whose properties correspond to the columns.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The sequence of objects whose properties correspond to the columns.</returns>
	public static IEnumerable<T> Query<T>(this IDbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null) where T: class, new() {
		using var reader = ExecuteReader(connection, command, parameters, options);
		return Mapper.CreateInstances<T>(reader);
	}

	/// <summary>
	/// Executes a parameterized SQL query and returns a sequence of objects whose properties correspond to the columns.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The sequence of objects whose properties correspond to the columns.</returns>
	public static IEnumerable<T> Query<T>(this IDbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null) where T: class, new() =>
		Query<T>(connection, command, (parameters ?? []).ToOrderedDictionary(), options);

	/// <summary>
	/// Executes a parameterized SQL query and returns the first row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The first row.</returns>
	/// <exception cref="InvalidOperationException">The result set is empty.</exception>
	public static T QueryFirst<T>(this IDbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null) where T: class, new() {
		using var reader = ExecuteReader(connection, command, parameters, options);
		return reader.Read() ? Mapper.CreateInstance<T>(reader) : throw new InvalidOperationException("The result set is empty.");
	}

	/// <summary>
	/// Executes a parameterized SQL query and returns the first row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The first row.</returns>
	/// <exception cref="InvalidOperationException">The result set is empty.</exception>
	public static T QueryFirst<T>(this IDbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null) where T: class, new() =>
		QueryFirst<T>(connection, command, (parameters ?? []).ToOrderedDictionary(), options);

	/// <summary>
	/// Executes a parameterized SQL query and returns the first row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The first row, or <see langword="null"/> if not found.</returns>
	public static T? QueryFirstOrDefault<T>(this IDbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null) where T: class, new() {
		using var reader = ExecuteReader(connection, command, parameters, options);
		return reader.Read() ? Mapper.CreateInstance<T>(reader) : default;
	}

	/// <summary>
	/// Executes a parameterized SQL query and returns the first row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The first row, or <see langword="null"/> if not found.</returns>
	public static T? QueryFirstOrDefault<T>(this IDbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null) where T: class, new() =>
		QueryFirstOrDefault<T>(connection, command, (parameters ?? []).ToOrderedDictionary(), options);

	/// <summary>
	/// Executes a parameterized SQL query and returns the single row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The single row.</returns>
	/// <exception cref="InvalidOperationException">The result set is empty or contains more than one record.</exception>
	public static T QuerySingle<T>(this IDbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null) where T: class, new() {
		T? record = default;
		var rowCount = 0;

		using var reader = ExecuteReader(connection, command, parameters, options);
		while (reader.Read()) {
			if (++rowCount > 1) break;
			record = Mapper.CreateInstance<T>(reader);
		}

		return rowCount == 1 ? record! : throw new InvalidOperationException("The result set is empty or contains more than one record.");
	}

	/// <summary>
	/// Executes a parameterized SQL query and returns the single row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The single row.</returns>
	/// <exception cref="InvalidOperationException">The result set is empty or contains more than one record.</exception>
	public static T QuerySingle<T>(this IDbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null) where T: class, new() =>
		QuerySingle<T>(connection, command, (parameters ?? []).ToOrderedDictionary(), options);

	/// <summary>
	/// Executes a parameterized SQL query and returns the single row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The single row, or <see langword="null"/> if not found.</returns>
	public static T? QuerySingleOrDefault<T>(this IDbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null) where T: class, new() {
		T? record = default;
		var rowCount = 0;

		using var reader = ExecuteReader(connection, command, parameters, options);
		while (reader.Read()) {
			if (++rowCount > 1) break;
			record = Mapper.CreateInstance<T>(reader);
		}

		return rowCount == 1 ? record : default;
	}

	/// <summary>
	/// Executes a parameterized SQL query and returns the single row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The single row, or <see langword="null"/> if not found.</returns>
	public static T? QuerySingleOrDefault<T>(this IDbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null) where T: class, new() =>
		QuerySingleOrDefault<T>(connection, command, (parameters ?? []).ToOrderedDictionary(), options);
}
