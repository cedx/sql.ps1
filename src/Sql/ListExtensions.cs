namespace Belin.Sql;

/// <summary>
/// Provides extension members for lists.
/// </summary>
public static class ListExtensions {
	// TODO (.NET 10) extension(IList list)

	/// <summary>
	/// Converts the specified list into an ordered dictionary.
	/// </summary>
	/// <param name="list">The list to convert.</param>
	/// <param name="prefix">The prefix to use for generating the dictionary key names.</param>
	/// <returns>The ordered dictionary corresponding to the specified list.</returns>
	public static OrderedDictionary<string, T?> ToOrderedDictionary<T>(this IList<T?> list, string prefix = "") =>
		new(list.Index().Select(entry => new KeyValuePair<string, T?>($"{prefix}{entry.Index}", entry.Item)));
}
