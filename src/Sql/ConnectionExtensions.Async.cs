namespace Belin.Sql;

using System.Data;
using System.Data.Common;
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
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The number of rows affected.</returns>
	public static Task<int> ExecuteAsync(this DbConnection connection, string command, QueryOptions? options = null, CancellationToken cancellationToken = default) =>
		ExecuteAsync(connection, command, new Dictionary<string, object?>(), options, cancellationToken);

	/// <summary>
	/// Executes a parameterized SQL statement.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The number of rows affected.</returns>
	public static async Task<int> ExecuteAsync(this DbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) {
		if (connection.State == ConnectionState.Closed) await connection.OpenAsync(cancellationToken);
		using var dbCommand = (DbCommand) CreateCommand(connection, command, parameters, options);
		return await dbCommand.ExecuteNonQueryAsync(cancellationToken);
	}

	/// <summary>
	/// Executes a parameterized SQL statement.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The number of rows affected.</returns>
	public static Task<int> ExecuteAsync(this DbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) =>
		ExecuteAsync(connection, command, parameters.ToOrderedDictionary(), options, cancellationToken);

	/// <summary>
	/// Executes a parameterized SQL query and returns a data reader.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The data reader that can be used to access the results.</returns>
	public static Task<IDataReader> ExecuteReaderAsync(this DbConnection connection, string command, QueryOptions? options = null, CancellationToken cancellationToken = default) =>
		ExecuteReaderAsync(connection, command, new Dictionary<string, object?>(), options, cancellationToken);

	/// <summary>
	/// Executes a parameterized SQL query and returns a data reader.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The data reader that can be used to access the results.</returns>
	public static async Task<IDataReader> ExecuteReaderAsync(this DbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) {
		if (connection.State == ConnectionState.Closed) await connection.OpenAsync(cancellationToken);
		using var dbCommand = (DbCommand) CreateCommand(connection, command, parameters, options);
		return await dbCommand.ExecuteReaderAsync(cancellationToken);
	}

	/// <summary>
	/// Executes a parameterized SQL query and returns a data reader.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The data reader that can be used to access the results.</returns>
	public static Task<IDataReader> ExecuteReaderAsync(this DbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) =>
		ExecuteReaderAsync(connection, command, parameters.ToOrderedDictionary(), options, cancellationToken);

	/// <summary>
	/// Executes a parameterized SQL query that selects a single value.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The first column of the first row.</returns>
	public static Task<object?> ExecuteScalarAsync(this DbConnection connection, string command, QueryOptions? options = null, CancellationToken cancellationToken = default) =>
		ExecuteScalarAsync(connection, command, new Dictionary<string, object?>(), options, cancellationToken);

	/// <summary>
	/// Executes a parameterized SQL query that selects a single value.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The first column of the first row.</returns>
	public static async Task<object?> ExecuteScalarAsync(this DbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) {
		if (connection.State == ConnectionState.Closed) await connection.OpenAsync(cancellationToken);
		using var dbCommand = (DbCommand) CreateCommand(connection, command, parameters, options);
		var value = await dbCommand.ExecuteScalarAsync(cancellationToken);
		return value is DBNull ? null : value;
	}

	/// <summary>
	/// Executes a parameterized SQL query that selects a single value.
	/// </summary>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The first column of the first row.</returns>
	public static Task<object?> ExecuteScalarAsync(this DbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) =>
		ExecuteScalarAsync(connection, command, parameters.ToOrderedDictionary(), options, cancellationToken);

	/// <summary>
	/// Executes a parameterized SQL query that selects a single value.
	/// </summary>
	/// <typeparam name="T">The type of object to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The first column of the first row.</returns>
	public static Task<T?> ExecuteScalarAsync<T>(this DbConnection connection, string command, QueryOptions? options = null) where T: IConvertible =>
		ExecuteScalarAsync<T>(connection, command, new Dictionary<string, object?>(), options);

	/// <summary>
	/// Executes a parameterized SQL query that selects a single value.
	/// </summary>
	/// <typeparam name="T">The type of object to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The first column of the first row.</returns>
	public static async Task<T?> ExecuteScalarAsync<T>(this DbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null) where T: IConvertible =>
		(T?) Convert.ChangeType(await ExecuteScalarAsync(connection, command, parameters, options), typeof(T), CultureInfo.InvariantCulture);

	/// <summary>
	/// Executes a parameterized SQL query that selects a single value.
	/// </summary>
	/// <typeparam name="T">The type of object to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <returns>The first column of the first row.</returns>
	public static Task<T?> ExecuteScalarAsync<T>(this DbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null) where T: IConvertible =>
		ExecuteScalarAsync<T>(connection, command, parameters.ToOrderedDictionary(), options);

	/// <summary>
	/// Executes a parameterized SQL query and returns a sequence of objects whose properties correspond to the columns.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The sequence of objects whose properties correspond to the columns.</returns>
	public static Task<IEnumerable<T>> QueryAsync<T>(this DbConnection connection, string command, QueryOptions? options = null, CancellationToken cancellationToken = default) where T: class, new() =>
		QueryAsync<T>(connection, command, new Dictionary<string, object?>(), options, cancellationToken);

	/// <summary>
	/// Executes a parameterized SQL query and returns a sequence of objects whose properties correspond to the columns.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The sequence of objects whose properties correspond to the columns.</returns>
	public static async Task<IEnumerable<T>> QueryAsync<T>(this DbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) where T: class, new() =>
		dataMapper.CreateInstances<T>(await ExecuteReaderAsync(connection, command, parameters, options, cancellationToken));

	/// <summary>
	/// Executes a parameterized SQL query and returns a sequence of objects whose properties correspond to the columns.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The sequence of objects whose properties correspond to the columns.</returns>
	public static Task<IEnumerable<T>> QueryAsync<T>(this DbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) where T: class, new() =>
		QueryAsync<T>(connection, command, parameters.ToOrderedDictionary(), options, cancellationToken);

	/// <summary>
	/// Executes a parameterized SQL query and returns the first row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The first row.</returns>
	public static Task<T> QueryFirstAsync<T>(this DbConnection connection, string command, QueryOptions? options = null, CancellationToken cancellationToken = default) where T: class, new() =>
		QueryFirstAsync<T>(connection, command, new Dictionary<string, object?>(), options, cancellationToken);

	/// <summary>
	/// Executes a parameterized SQL query and returns the first row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The first row.</returns>
	/// <exception cref="InvalidOperationException">The result set is empty.</exception>
	public static async Task<T> QueryFirstAsync<T>(this DbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) where T: class, new() {
		using var reader = await ExecuteReaderAsync(connection, command, parameters, options, cancellationToken);
		return reader.Read() ? dataMapper.CreateInstance<T>(reader) : throw new InvalidOperationException("The result set is empty.");
	}

	/// <summary>
	/// Executes a parameterized SQL query and returns the first row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The first row.</returns>
	public static Task<T> QueryFirstAsync<T>(this DbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) where T: class, new() =>
		QueryFirstAsync<T>(connection, command, parameters.ToOrderedDictionary(), options, cancellationToken);

	/// <summary>
	/// Executes a parameterized SQL query and returns the first row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The first row, or <see langword="null"/> if not found.</returns>
	public static Task<T?> QueryFirstOrDefaultAsync<T>(this DbConnection connection, string command, QueryOptions? options = null, CancellationToken cancellationToken = default) where T: class, new() =>
		QueryFirstOrDefaultAsync<T>(connection, command, new Dictionary<string, object?>(), options, cancellationToken);

	/// <summary>
	/// Executes a parameterized SQL query and returns the first row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The first row, or <see langword="null"/> if not found.</returns>
	public static async Task<T?> QueryFirstOrDefaultAsync<T>(this DbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) where T: class, new() {
		using var reader = await ExecuteReaderAsync(connection, command, parameters, options, cancellationToken);
		return reader.Read() ? dataMapper.CreateInstance<T>(reader) : default;
	}

	/// <summary>
	/// Executes a parameterized SQL query and returns the first row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The positional parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The first row, or <see langword="null"/> if not found.</returns>
	public static Task<T?> QueryFirstOrDefaultAsync<T>(this DbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) where T: class, new() =>
		QueryFirstOrDefaultAsync<T>(connection, command, parameters.ToOrderedDictionary(), options, cancellationToken);

	/// <summary>
	/// Executes a parameterized SQL query and returns the single row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The single row.</returns>
	public static Task<T> QuerySingleAsync<T>(this DbConnection connection, string command, QueryOptions? options = null, CancellationToken cancellationToken = default) where T: class, new() =>
		QuerySingleAsync<T>(connection, command, new Dictionary<string, object?>(), options, cancellationToken);

	/// <summary>
	/// Executes a parameterized SQL query and returns the single row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The single row.</returns>
	/// <exception cref="InvalidOperationException">The result set is empty or contains more than one record.</exception>
	public static async Task<T> QuerySingleAsync<T>(this DbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) where T: class, new() {
		T? record = default;
		var rowCount = 0;

		using var reader = await ExecuteReaderAsync(connection, command, parameters, options, cancellationToken);
		while (reader.Read()) {
			if (++rowCount > 1) break;
			record = dataMapper.CreateInstance<T>(reader);
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
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The single row.</returns>
	public static Task<T> QuerySingleAsync<T>(this DbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) where T: class, new() =>
		QuerySingleAsync<T>(connection, command, parameters.ToOrderedDictionary(), options, cancellationToken);
	
	/// <summary>
	/// Executes a parameterized SQL query and returns the single row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The single row, or <see langword="null"/> if not found.</returns>
	public static Task<T?> QuerySingleOrDefaultAsync<T>(this DbConnection connection, string command, QueryOptions? options = null, CancellationToken cancellationToken = default) where T: class, new() =>
		QuerySingleOrDefaultAsync<T>(connection, command, new Dictionary<string, object?>(), options, cancellationToken);

	/// <summary>
	/// Executes a parameterized SQL query and returns the single row.
	/// </summary>
	/// <typeparam name="T">The type of objects to return.</typeparam>
	/// <param name="connection">The connection to the data source.</param>
	/// <param name="command">The SQL query to be executed.</param>
	/// <param name="parameters">The named parameters of the SQL query.</param>
	/// <param name="options">The query options.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The single row, or <see langword="null"/> if not found.</returns>
	public static async Task<T?> QuerySingleOrDefaultAsync<T>(this DbConnection connection, string command, IDictionary<string, object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) where T: class, new() {
		T? record = default;
		var rowCount = 0;

		using var reader = await ExecuteReaderAsync(connection, command, parameters, options, cancellationToken);
		while (reader.Read()) {
			if (++rowCount > 1) break;
			record = dataMapper.CreateInstance<T>(reader);
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
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The single row, or <see langword="null"/> if not found.</returns>
	public static Task<T?> QuerySingleOrDefaultAsync<T>(this DbConnection connection, string command, IList<object?> parameters, QueryOptions? options = null, CancellationToken cancellationToken = default) where T: class, new() =>
		QuerySingleOrDefaultAsync<T>(connection, command, parameters.ToOrderedDictionary(), options, cancellationToken);
}
