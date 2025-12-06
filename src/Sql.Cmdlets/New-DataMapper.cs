namespace Belin.Sql.Cmdlets;

using Belin.Sql.Cmdlets.Mapping;

/// <summary>
/// Creates a new data mapper.
/// </summary>
[Cmdlet(VerbsCommon.New, "DataMapper")]
[OutputType(typeof(DataMapper))]
public class NewDataMapperCommand: Cmdlet {

	/// <summary>
	/// Performs execution of this command.
	/// </summary>
	protected override void ProcessRecord() => WriteObject(new DataMapper());
}
