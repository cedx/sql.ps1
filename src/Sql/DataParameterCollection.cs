namespace Belin.Sql;

using System.Collections;
using System.Data;

/// <summary>
/// Collects all parameters relevant to a parameterized SQL statement.
/// </summary>
public class DataParameterCollection: List<DataParameter> {
	
	/// <summary>
	/// Creates a new parameter list.
	/// </summary>
	public DataParameterCollection(): base() {}

	/// <summary>
	/// Creates a new parameter list that contains the elements copied from the specified collection.
	/// </summary>
	/// <param name="collection">The collection whose elements are copied to the parameter list.</param>
	public DataParameterCollection(IEnumerable<DataParameter> collection): base(collection) {}
	
	/// <summary>
	/// Creates a new parameter list from the specified array of positional parameters.
	/// </summary>
	/// <param name="array">The array whose elements are copied to the parameter list.</param>
	/// <returns>The parameter list corresponding to the specified array.</returns>
	public static implicit operator DataParameterCollection(object?[] array) => [.. array.Index().Select(entry => 
		entry.Item is DataParameter dbParameter ? dbParameter : new DataParameter($"PositionalParameter{entry.Index}", entry.Item)
	)];

	/// <summary>
	/// Creates a new parameter list from the specified list of positional parameters.
	/// </summary>
	/// <param name="list">The list whose elements are copied to the parameter list.</param>
	/// <returns>The parameter list corresponding to the specified list.</returns>
	public static implicit operator DataParameterCollection(ArrayList list) => list.ToArray();

	/// <summary>
	/// Creates a new parameter list from the specified dictionary of named parameters.
	/// </summary>
	/// <param name="dictionary">The dictionary whose elements are copied to the parameter list.</param>
	/// <returns>The parameter list corresponding to the specified dictionary.</returns>
	public static implicit operator DataParameterCollection(Dictionary<string, object?> dictionary) => [.. dictionary.Select(entry =>
		entry.Value is DataParameter dbParameter ? dbParameter : new DataParameter($"@{entry.Key}", entry.Value)
	)];

	/// <summary>
	/// Creates a new parameter list from the specified hash table of named parameters.
	/// </summary>
	/// <param name="hashtable">The dictionary whose elements are copied to the parameter list.</param>
	/// <returns>The parameter list corresponding to the specified hash table.</returns>
	public static implicit operator DataParameterCollection(Hashtable hashtable) =>
		hashtable.Cast<DictionaryEntry>().ToDictionary(entry => entry.Key.ToString() ?? "", entry => entry.Value);

	/// <summary>
	/// Creates a new parameter list from the specified list of positional parameters.
	/// </summary>
	/// <param name="list">The list whose elements are copied to the parameter list.</param>
	/// <returns>The parameter list corresponding to the specified list.</returns>
	public static implicit operator DataParameterCollection(List<object?> list) => list.ToArray();
}
