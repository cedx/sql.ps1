namespace Belin.Sql;

using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Reflection;

/// <summary>
/// Maps data records to entity objects.
/// </summary>
public class DataMapper {

	/// <summary>
	/// The property maps, keyed by type.
	/// </summary>
	private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> propertyMaps = [];
	
	/// <summary>
	/// Creates a new dyamic object from the specified data record.
	/// </summary>
	/// <param name="record">A data record providing the properties to be set on the created object.</param>
	/// <returns>The newly created object.</returns>
	public dynamic CreateInstance(IDataRecord record) => CreateInstance<ExpandoObject>(record);

	/// <summary>
	/// Creates a new object of a given type from the specified data record.
	/// </summary>
	/// <typeparam name="T">The object type.</typeparam>
	/// <param name="record">A data record providing the properties to be set on the created object.</param>
	/// <returns>The newly created object.</returns>
	public T CreateInstance<T>(IDataRecord record) => (T) CreateInstance(typeof(T), record);

	/// <summary>
	/// Creates a new object of a given type from the specified data record.
	/// </summary>
	/// <param name="type">The object type.</param>
	/// <param name="record">A data record providing the properties to be set on the created object.</param>
	/// <returns>The newly created object.</returns>
	public object CreateInstance(Type type, IDataRecord record) {
		var properties = new OrderedDictionary<string, object?>();
		for (var index = 0; index < record.FieldCount; index++) properties.TryAdd(record.GetName(index), record.GetValue(index));
		return CreateInstance(type, properties);
	}

	/// <summary>
	/// Creates a new dynamic object from the specified dictionary.
	/// </summary>
	/// <param name="properties">A dictionary providing the properties to be set on the created object.</param>
	/// <returns>The newly created object.</returns>
	public dynamic CreateInstance(IDictionary<string, object?> properties) => CreateInstance<ExpandoObject>(properties);

	/// <summary>
	/// Creates a new object of a given type from the specified dictionary.
	/// </summary>
	/// <typeparam name="T">The object type.</typeparam>
	/// <param name="properties">A dictionary providing the properties to be set on the created object.</param>
	/// <returns>The newly created object.</returns>
	public T CreateInstance<T>(IDictionary<string, object?> properties) => (T) CreateInstance(typeof(T), properties);

	/// <summary>
	/// Creates a new object of a given type from the specified dictionary.
	/// </summary>
	/// <param name="type">The object type.</param>
	/// <param name="properties">A dictionary providing the properties to be set on the created object.</param>
	/// <returns>The newly created object.</returns>
	public object CreateInstance(Type type, IDictionary<string, object?> properties) {
		if (type == typeof(ExpandoObject)) {
			var expandoObject = (IDictionary<string, object?>) new ExpandoObject();
			foreach (var (key, value) in properties) expandoObject.Add(key, value);
			return expandoObject;
		}

		var instance = Activator.CreateInstance(type)!;
		var propertyMap = GetPropertyMap(type);

		foreach (var key in properties.Keys.Where(propertyMap.ContainsKey)) {
			var propertyInfo = propertyMap[key];
			if (!propertyInfo.CanWrite) continue;

			var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
			var value = properties[key];

			propertyInfo.SetValue(instance, true switch {
				true when value is null || value is DBNull => null,
				true when propertyType.IsEnum => Enum.ToObject(propertyType, value),
				_ => Convert.ChangeType(value, propertyType, CultureInfo.InvariantCulture)
			});
		}

		return instance;
	}
	
	/// <summary>
	/// Creates new dynamic objects from the specified data reader.
	/// </summary>
	/// <param name="reader">A data reader providing the properties to be set on the created objects.</param>
	/// <returns>An enumerable of newly created objects.</returns>
	public IEnumerable<dynamic> CreateInstances(IDataReader reader) => CreateInstances<ExpandoObject>(reader);

	/// <summary>
	/// Creates new objects of a given type from the specified data reader.
	/// </summary>
	/// <typeparam name="T">The object type.</typeparam>
	/// <param name="reader">A data reader providing the properties to be set on the created objects.</param>
	/// <returns>An enumerable of newly created objects.</returns>
	public IEnumerable<T> CreateInstances<T>(IDataReader reader) => (IEnumerable<T>) CreateInstances(typeof(T), reader);

	/// <summary>
	/// Creates new objects of a given type from the specified data reader.
	/// </summary>
	/// <param name="type">The object type.</param>
	/// <param name="reader">A data reader providing the properties to be set on the created objects.</param>
	/// <returns>An enumerable of newly created objects.</returns>
	public IEnumerable<object> CreateInstances(Type type, IDataReader reader) {
		while (reader.Read()) yield return CreateInstance(type, reader);
		reader.Close();
	}

	/// <summary>
	/// Retrives a dictionary of mapped properties of the specified type.
	/// </summary>
	/// <typeparam name="T">The type to inspect.</typeparam>
	/// <returns>The dictionary of mapped properties of the specified type.</returns>
	public IDictionary<string, PropertyInfo> GetPropertyMap<T>() => GetPropertyMap(typeof(T));

	/// <summary>
	/// Retrives a dictionary of mapped properties of the specified type.
	/// </summary>
	/// <param name="type">The type to inspect.</param>
	/// <returns>The dictionary of mapped properties of the specified type.</returns>
	public IDictionary<string, PropertyInfo> GetPropertyMap(Type type) {
		if (propertyMaps.TryGetValue(type, out var value)) return value;

		var propertyInfos = type
			.GetProperties(BindingFlags.Instance | BindingFlags.Public)
			.Where(propertyInfo => !propertyInfo.IsDefined(typeof(NotMappedAttribute)) && (propertyInfo.IsDefined(typeof(ColumnAttribute)) || (propertyInfo.CanRead && propertyInfo.CanWrite)));

		var propertyMap = new Dictionary<string, PropertyInfo>();
		foreach (var propertyInfo in propertyInfos) {
			var column = propertyInfo.GetCustomAttribute<ColumnAttribute>();
			propertyMap[column?.Name ?? propertyInfo.Name] = propertyInfo;
		}

		return propertyMaps[type] = propertyMap;
	}
}
