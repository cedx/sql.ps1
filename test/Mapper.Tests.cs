namespace Belin.Sql;

/// <summary>
/// Tests the features of the <see cref="Mapper"/> class.
/// </summary>
[TestClass]
public sealed class MapperTests {

	[DataRow(null, typeof(bool), false, false)]
	[DataRow(null, typeof(bool?), false, null)]
	[DataRow(0, typeof(bool), false, false)]
	[DataRow(0, typeof(bool?), false, false)]
	[DataRow(1, typeof(bool), false, true)]
	[DataRow(1, typeof(bool?), false, true)]
	[DataRow("false", typeof(bool), false, false)]
	[DataRow("true", typeof(bool), false, true)]

	[DataRow(null, typeof(char), false, char.MinValue)]
	[DataRow(null, typeof(char?), false, null)]
	[DataRow(0, typeof(char), false, char.MinValue)]
	[DataRow(0, typeof(char?), false, char.MinValue)]
	[DataRow(97, typeof(char), false, 'a')]
	[DataRow("a", typeof(char), false, 'a')]

	[DataRow(null, typeof(double), false, 0.0)]
	[DataRow(null, typeof(double?), false, null)]
	[DataRow(0, typeof(double), false, 0.0)]
	[DataRow(0, typeof(double?), false, 0.0)]
	[DataRow(123, typeof(double), false, 123.0)]
	[DataRow(-123.456, typeof(double?), false, -123.456)]
	[DataRow("123", typeof(double), false, 123.0)]
	[DataRow("-123.456", typeof(double), false, -123.456)]

	[DataRow(null, typeof(int), false, 0)]
	[DataRow(null, typeof(int?), false, null)]
	[DataRow(0, typeof(int), false, 0)]
	[DataRow(0, typeof(int?), false, 0)]
	[DataRow(123, typeof(int), false, 123)]
	[DataRow(-123.456, typeof(int?), false, -123)]
	[DataRow("123", typeof(int), false, 123)]
	[DataRow("-123", typeof(int), false, -123)]

	[DataRow(null, typeof(DayOfWeek), false, DayOfWeek.Sunday)]
	[DataRow(null, typeof(DayOfWeek?), false, null)]
	[DataRow(0, typeof(DayOfWeek), false, DayOfWeek.Sunday)]
	[DataRow(0, typeof(DayOfWeek?), false, DayOfWeek.Sunday)]
	[DataRow(5, typeof(DayOfWeek), false, DayOfWeek.Friday)]
	[DataRow(5, typeof(DayOfWeek?), false, DayOfWeek.Friday)]
	[DataRow("sunday", typeof(DayOfWeek), false, DayOfWeek.Sunday)]
	[DataRow("friday", typeof(DayOfWeek), false, DayOfWeek.Friday)]

	[TestMethod]
	public void ChangeType(object? value, Type conversionType, bool isNullableReferenceType, object? expected) =>
		AreEqual(expected, new Mapper().ChangeType(value, conversionType, isNullableReferenceType));
}
