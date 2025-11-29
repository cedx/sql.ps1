namespace Belin.Sql.Mapping;

using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Globalization;
using System.Reflection;

/// <summary>
/// Maps data records to entity objects.
/// </summary>
public sealed class DataMapper {

	/// <summary>
	/// The property maps, keyed by type.
	/// </summary>
	private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> propertyMaps = [];

	/// <summary>
	/// Creates a new object of a given type from the specified data record.
	/// </summary>
	/// <typeparam name="T">The object type.</typeparam>
	/// <param name="record">A data record providing the properties to be set on the created object.</param>
	/// <returns>The newly created object.</returns>
	public T CreateInstance<T>(IDataRecord record) {
		var properties = new Dictionary<string, object?>();
		for (var index = 0; index < record.FieldCount; index++) {
			var key = record.GetName(index);
			properties[key] = record.IsDBNull(index) ? null : record.GetValue(index);
		}

		return CreateInstance<T>(properties);
	}

	/// <summary>
	/// Creates a new object of a given type from the specified hash table.
	/// </summary>
	/// <typeparam name="T">The object type.</typeparam>
	/// <param name="properties">A hash table providing the properties to be set on the created object.</param>
	/// <returns>The newly created object.</returns>
	public T CreateInstance<T>(Hashtable properties) =>
		CreateInstance<T>(properties.Cast<DictionaryEntry>().ToDictionary(entry => entry.Key.ToString()!, entry => entry.Value));

	/// <summary>
	/// Creates a new object of a given type from the specified dictionary.
	/// </summary>
	/// <typeparam name="T">The object type.</typeparam>
	/// <param name="properties">A dictionary providing the properties to be set on the created object.</param>
	/// <returns>The newly created object.</returns>
	public T CreateInstance<T>(IDictionary<string, object?> properties) {
		var culture = CultureInfo.InvariantCulture;
		var instance = Activator.CreateInstance<T>();
		var propertyMap = GetPropertyMap<T>();

		foreach (var key in properties.Keys.Where(propertyMap.ContainsKey)) {
			var propertyInfo = propertyMap[key];
			var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
			var value = properties[key];

			propertyInfo.SetValue(instance, true switch {
				true when value is null => null,
				true when propertyType.IsEnum => Enum.ToObject(propertyType, value),
				_ => Convert.ChangeType(value, propertyType, culture)
			});
		}

		return instance;
	}

	/// <summary>
	/// Creates new objects of a given type from the specified data reader.
	/// </summary>
	/// <typeparam name="T">The object type.</typeparam>
	/// <param name="reader">A data reader providing the properties to be set on the created objects.</param>
	/// <returns>An enumerable of newly created objects.</returns>
	public IEnumerable<T> CreateInstances<T>(IDataReader reader) {
		while (reader.Read()) yield return CreateInstance<T>(reader);
		reader.Close();
	}

	/// <summary>
	/// Retrives a dictionary of mapped properties of the specified type.
	/// </summary>
	/// <typeparam name="T">The type to inspect.</typeparam>
	/// <returns>The dictionary of mapped properties of the specified type.</returns>
	public IDictionary<string, PropertyInfo> GetPropertyMap<T>() {
		var type = typeof(T);
		if (propertyMaps.TryGetValue(type, out var value)) return value;

		var propertyInfos = type
			.GetProperties(BindingFlags.Instance | BindingFlags.Public)
			.Where(propertyInfo => propertyInfo.CanWrite && !propertyInfo.IsDefined(typeof(NotMappedAttribute)));

		var propertyMap = new Dictionary<string, PropertyInfo>();
		foreach (var propertyInfo in propertyInfos) {
			var column = propertyInfo.GetCustomAttribute<ColumnAttribute>();
			propertyMap[column?.Name ?? propertyInfo.Name] = propertyInfo;
		}

		return propertyMaps[type] = propertyMap;
	}
}
