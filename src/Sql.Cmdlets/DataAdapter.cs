namespace Belin.Sql.Cmdlets;

using System.Data;

/// <summary>
/// Encapsulates a data reader and a data mapper into a single type.
/// </summary>
/// <param name="Mapper">The data mapper that can be used to convert the records returned by the reader.</param>
/// <param name="Reader">The data reader that can be used to iterate over the results of the SQL query.</param>
public sealed record DataAdapter(
	DataMapper Mapper,
	IDataReader Reader
);
