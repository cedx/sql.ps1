namespace Belin.Sql;

using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Reflection;

/// <summary>
/// Maps data records to entity objects.
/// </summary>
public sealed class DataMapper {

	/// <summary>
	/// The property nullability map.
	/// </summary>
	private static readonly Dictionary<PropertyInfo, NullabilityInfo> nullabilityMap = [];

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
	public T CreateInstance<T>(IDataRecord record) where T: class, new() {
		var properties = new OrderedDictionary<string, object?>();
		for (var index = 0; index < record.FieldCount; index++) {
			var value = record.GetValue(index);
			properties.TryAdd(record.GetName(index), value is DBNull ? null : value);
		}

		return CreateInstance<T>(properties);
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
	public T CreateInstance<T>(IDictionary<string, object?> properties) where T: class, new() {
		if (typeof(T) == typeof(ExpandoObject)) {
			var expandoObject = (IDictionary<string, object?>) new ExpandoObject();
			foreach (var (key, value) in properties) expandoObject.Add(key, value);
			return (T) expandoObject;
		}

		var instance = Activator.CreateInstance<T>()!;
		var propertyMap = GetPropertyMap<T>();

		foreach (var key in properties.Keys.Where(propertyMap.ContainsKey)) {
			var propertyInfo = propertyMap[key];
			if (!propertyInfo.CanWrite) continue;

			var nullableType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
			var targetType = nullableType ?? propertyInfo.PropertyType;
			var value = properties[key];

			propertyInfo.SetValue(instance, true switch {
				true when value is null && nullableType is not null => default,
				true when value is null && targetType == typeof(string) => IsNullable(propertyInfo) ? default : "",
				true when value is null && !targetType.IsValueType => IsNullable(propertyInfo) ? default : Activator.CreateInstance(targetType),
				true when targetType.IsEnum => Enum.ToObject(targetType, Convert.ChangeType(value ?? default!, Enum.GetUnderlyingType(targetType), CultureInfo.InvariantCulture)),
				_ => Convert.ChangeType(value ?? default, targetType, CultureInfo.InvariantCulture)
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
	public IEnumerable<T> CreateInstances<T>(IDataReader reader) where T: class, new() {
		while (reader.Read()) yield return CreateInstance<T>(reader);
		reader.Close();
	}

	/// <summary>
	/// Retrives a dictionary of mapped properties of the specified type.
	/// </summary>
	/// <typeparam name="T">The type to inspect.</typeparam>
	/// <returns>The dictionary of mapped properties of the specified type.</returns>
	private static Dictionary<string, PropertyInfo> GetPropertyMap<T>() where T: class, new() {
		var type = typeof(T);
		if (propertyMaps.TryGetValue(type, out var value)) return value;

		var propertyInfos = type
			.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
			.Where(propertyInfo => !propertyInfo.IsDefined(typeof(NotMappedAttribute)) && (propertyInfo.IsDefined(typeof(ColumnAttribute)) || (propertyInfo.CanRead && propertyInfo.CanWrite)));

		var propertyMap = new Dictionary<string, PropertyInfo>();
		foreach (var propertyInfo in propertyInfos) {
			var column = propertyInfo.GetCustomAttribute<ColumnAttribute>();
			propertyMap[column?.Name ?? propertyInfo.Name] = propertyInfo;
		}

		return propertyMaps[type] = propertyMap;
	}

	/// <summary>
	/// Gets the nullability information for the specified property.
	/// </summary>
	/// <param name="propertyInfo">The property to inspect.</param>
	/// <returns>The nullability information for the specified property.</returns>
	private static NullabilityInfo GetNullability(PropertyInfo propertyInfo) {
		if (nullabilityMap.TryGetValue(propertyInfo, out var nullability)) return nullability;
		return nullabilityMap[propertyInfo] = new NullabilityInfoContext().Create(propertyInfo);
	}

	/// <summary>
	/// Returns a value indicating whether the specified property is nullable.
	/// </summary>
	/// <param name="propertyInfo">The property to inspect.</param>
	/// <returns><see langword="true"/> if the specified property is nullable, otherwise <see langword="false"/>.</returns>
	private static bool IsNullable(PropertyInfo propertyInfo) => GetNullability(propertyInfo).WriteState != NullabilityState.NotNull;
}
