namespace Belin.Sql.Dapper;

using Microsoft.Data.SqlClient;
using System.Net.Mail;

/// <summary>
/// Tests the features of the <see cref="MailAddressHandler"/> class.
/// </summary>
[TestClass]
public sealed class MailAddressTypeHandlerTests {

	[TestMethod]
	public void Parse() {
		var typeHandler = new MailAddressHandler();

		// It should return `null` if the value is invalid.
		IsNull(typeHandler.Parse(123));
		IsNull(typeHandler.Parse(""));

		// It should return a mail address if the value is valid.
		var value = typeHandler.Parse("contact@cedric-belin.fr");
		IsNotNull(value);
		AreEqual("contact@cedric-belin.fr", value.Address);
	}

	[TestMethod]
	public void SetValue() {
		var parameter = new SqlParameter();
		var typeHandler = new MailAddressHandler();

		// It should set the parameter to `null` if the value is `null`.
		typeHandler.SetValue(parameter, null);
		IsNull(parameter.Value);

		// It should set the parameter to the string representation if the value is not `null`.
		typeHandler.SetValue(parameter, new MailAddress("contact@cedric-belin.fr"));
		AreEqual("contact@cedric-belin.fr", parameter.Value);
	}
}
