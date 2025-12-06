namespace Belin.Sql;

/// <summary>
/// Defines the options of a SQL command.
/// </summary>
/// <param name="Timeout">The wait time, in seconds, before terminating the attempt to execute the command and generating an error.</param>
public sealed record QueryOptions(
	int Timeout = 30
);
