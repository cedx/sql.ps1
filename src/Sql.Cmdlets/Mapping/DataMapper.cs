namespace Belin.Sql.Cmdlets.Mapping;

using System.Collections;
using System.Dynamic;

/// <summary>
/// Maps data records to entity objects.
/// </summary>
public class DataMapper: Sql.DataMapper {

	/// <summary>
	/// Creates a new dyamic object from the specified hash table.
	/// </summary>
	/// <param name="properties">A hash table providing the properties to be set on the created object.</param>
	/// <returns>The newly created object.</returns>
	public dynamic CreateInstance(Hashtable properties) => CreateInstance<ExpandoObject>(properties);

	/// <summary>
	/// Creates a new object of a given type from the specified hash table.
	/// </summary>
	/// <typeparam name="T">The object type.</typeparam>
	/// <param name="properties">A hash table providing the properties to be set on the created object.</param>
	/// <returns>The newly created object.</returns>
	public T CreateInstance<T>(Hashtable properties) => (T) CreateInstance(typeof(T), properties);

	/// <summary>
	/// Creates a new object of a given type from the specified hash table.
	/// </summary>
	/// <param name="T">The object type.</param>
	/// <param name="properties">A hash table providing the properties to be set on the created object.</param>
	/// <returns>The newly created object.</returns>
	public object CreateInstance(Type type, Hashtable properties) =>
		CreateInstance(type, properties.Cast<DictionaryEntry>().ToDictionary(entry => entry.Key.ToString()!, entry => entry.Value));
}
