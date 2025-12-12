namespace Belin.Sql.Dapper;

using Microsoft.Data.SqlClient;
using System.Net.NetworkInformation;

/// <summary>
/// Tests the features of the <see cref="PhysicalAddressHandler"/> class.
/// </summary>
[TestClass]
public sealed class PhysicalAddressTypeHandlerTests {

	[TestMethod]
	public void Parse() {
		var typeHandler = new PhysicalAddressHandler();

		// It should return `null` if the value is invalid.
		IsNull(typeHandler.Parse(123));
		IsNull(typeHandler.Parse(""));

		// It should return a physical address if the value is valid.
		var value = typeHandler.Parse("8C:F8:C5:DE:C2:E0");
		IsNotNull(value);
		AreEqual("8CF8C5DEC2E0", value.ToString());
	}

	[TestMethod]
	public void SetValue() {
		var parameter = new SqlParameter();
		var typeHandler = new PhysicalAddressHandler();

		// It should set the parameter to `null` if the value is `null`.
		typeHandler.SetValue(parameter, null);
		IsNull(parameter.Value);

		// It should set the parameter to the string representation if the value is not `null`.
		typeHandler.SetValue(parameter, PhysicalAddress.Parse("8C:F8:C5:DE:C2:E0"));
		AreEqual("8CF8C5DEC2E0", parameter.Value);
	}
}
