namespace Belin.Sql.Dapper;

using System.Data;
using System.Net.NetworkInformation;

/// <summary>
/// Maps a Media Access Control (MAC) address to or from a string.
/// </summary>
public class PhysicalAddressHandler: SqlMapper.TypeHandler<PhysicalAddress> {

	/// <summary>
	/// Parses a database value back to a typed value.
	/// </summary>
	/// <param name="value">The value from the database.</param>
	/// <returns>The typed value.</returns>
	public override PhysicalAddress? Parse(object value) =>
		value is string address && address.Length > 0 ? PhysicalAddress.Parse(address) : null;

	/// <summary>
	/// Assigns the value of a parameter before a command executes.
	/// </summary>
	/// <param name="parameter">The parameter to configure.</param>
	/// <param name="value">The parameter value.</param>
	public override void SetValue(IDbDataParameter parameter, PhysicalAddress? value) =>
		parameter.Value = value?.ToString();
}
