namespace Belin.Sql;

using System.Data;

/// <summary>
/// Defines the options of a SQL command.
/// </summary>
/// <param name="Timeout">The wait time, in seconds, before terminating the attempt to execute the command and generating an error.</param>
/// <param name="Transaction">The transaction within which the command executes.</param>
/// <param name="Type">Value indicating how the command is interpreted.</param>
public sealed record CommandOptions(
	int Timeout = 30,
	IDbTransaction? Transaction = null,
	CommandType Type = CommandType.Text
);
