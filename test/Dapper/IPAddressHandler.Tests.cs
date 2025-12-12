namespace Belin.Sql.Dapper;

using Microsoft.Data.SqlClient;
using System.Net;

/// <summary>
/// Tests the features of the <see cref="IPAddressHandler"/> class.
/// </summary>
[TestClass]
public sealed class IPAddressTypeHandlerTests {

	[TestMethod]
	public void Parse() {
		var typeHandler = new IPAddressHandler();

		// It should return `null` if the value is invalid.
		IsNull(typeHandler.Parse(123));
		IsNull(typeHandler.Parse(""));

		// It should return an IP address if the value is valid.
		var value = typeHandler.Parse("127.0.0.1");
		IsNotNull(value);
		AreEqual("127.0.0.1", value.ToString());
	}

	[TestMethod]
	public void SetValue() {
		var parameter = new SqlParameter();

		// It should set the parameter to `null` if the value is `null`.
		new IPAddressHandler().SetValue(parameter, null);
		IsNull(parameter.Value);

		// It should set the parameter to the string representation if the value is not `null`.
		new IPAddressHandler(mapToIPv6: false).SetValue(parameter, IPAddress.Parse("127.0.0.1"));
		AreEqual("127.0.0.1", parameter.Value);

		new IPAddressHandler(mapToIPv6: true).SetValue(parameter, IPAddress.Parse("127.0.0.1"));
		AreEqual("::ffff:127.0.0.1", parameter.Value);
	}
}
