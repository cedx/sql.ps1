namespace Belin.Sql.Cmdlets;

using System.Data;

/// <summary>
/// Encapsulates a data reader and a data mapper into a single type.
/// </summary>
/// <param name="Mapper">The data mapper that can be used to convert the records returned by the reader.</param>
/// <param name="Reader">The data reader that can be used to iterate over the results of the SQL query.</param>
public sealed record Adapter(Mapper Mapper, IDataReader Reader): IDisposable {

	/// <summary>
	/// Releases any resources associated with this object.
	/// </summary>
	public void Dispose() {
		Reader.Close();
		GC.SuppressFinalize(this);
	}
}
