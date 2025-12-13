namespace Belin.Sql;

using Belin.Sql.Reflection;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Runtime.CompilerServices;

/// <summary>
/// Maps data records to entity objects.
/// </summary>
public sealed class Mapper {
	
	/// <summary>
	/// The mapping between the entity types and their associated database tables.
	/// </summary>
	private static readonly Dictionary<Type, TableInfo> mapping = [];

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
		var table = GetTableInfo<T>();
		foreach (var key in properties.Keys.Where(table.Columns.ContainsKey)) {
			var column = table.Columns[key];
			if (column.CanWrite) column.SetValue(instance, ChangeType(properties[key], column.Type, column.IsNullable));
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
	/// Converts the specified object into an equivalent value of the specified type. 
	/// </summary>
	/// <param name="value">The object to convert.</param>
	/// <param name="conversionType">The type of object to return.</param>
	/// <param name="isNullable">Value indicating whether the specified conversion type is nullable.</param>
	/// <returns>The value of the given type corresponding to the specified object.</returns>
	internal object? ChangeType(object? value, Type conversionType, bool isNullable = true) {
		var nullableType = Nullable.GetUnderlyingType(conversionType);
		var targetType = nullableType ?? conversionType;

		if (value is not null) return true switch {
			true when targetType.IsEnum && value.GetType() == typeof(string) => Enum.Parse(targetType, (string) value, ignoreCase: true),
			true when targetType.IsEnum => Enum.ToObject(targetType, Convert.ChangeType(value, Enum.GetUnderlyingType(targetType), CultureInfo.InvariantCulture)),
			_ => targetType.IsInstanceOfType(value) ? value : Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture),
		};

		return true switch {
			true when nullableType is not null => default,
			true when targetType.IsValueType => RuntimeHelpers.GetUninitializedObject(targetType),
			true when targetType == typeof(string) => isNullable ? default : string.Empty,
			_ => isNullable ? default : Activator.CreateInstance(targetType)
		};
	}

	/// <summary>
	/// Gets the table information associated with the specified type.
	/// </summary>
	/// <typeparam name="T">The type to inspect.</typeparam>
	/// <returns>The table information associated with the specified type.</returns>
	private static TableInfo GetTableInfo<T>() where T: class, new() {
		var type = typeof(T);
		return mapping.TryGetValue(type, out var value) ? value : mapping[type] = new TableInfo(type);
	}
}
