using namespace System.Collections.Generic
using namespace System.ComponentModel.DataAnnotations.Schema
using namespace System.Diagnostics.CodeAnalysis
using namespace System.Reflection

<#
.SYNOPSIS
	Maps data records to entity objects.
#>
class DataMapper {

	<#
	.SYNOPSIS
		The property maps, keyed by type.
	#>
	hidden static [hashtable] $PropertyMaps = @{}

	<#
	.SYNOPSIS
		Converts the specified data reader into an array of objects of the specified type.
	.PARAMETER Reader
		The data reader to be converted.
	.PARAMETER Type
		The type of objects to return.
	.OUTPUTS
		The array of objects corresponding to the specified data reader.
	#>
	[object[]] ConvertReader([System.Data.IDataReader] $Reader, [type] $Type) {
		$list = [List[object]]::new()
		while ($Reader.Read()) { $list.Add($this.ConvertRecord($Reader, $Type)) }
		$Reader.Close()
		return $list.ToArray()
	}

	<#
	.SYNOPSIS
		Converts the specified data record to the specified type.
	.PARAMETER Record
		The data record to be converted.
	.PARAMETER Type
		The type of object to return.
	.OUTPUTS
		The object corresponding to the specified data record.
	#>
	[SuppressMessage("PSUseDeclaredVarsMoreThanAssignments", "")]
	[object] ConvertRecord([System.Data.IDataRecord] $Record, [type] $Type) {
		$properties = [ordered]@{}
		for ($index = 0; $index -lt $Record.FieldCount; $index++) {
			$key = $Record.GetName($index)
			$properties.$key = $Record.IsDBNull($index) ? $null : $Record.GetValue($index)
		}

		return $discard = switch ($Type) {
			([hashtable]) { [hashtable] $properties; break }
			([ordered]) { $properties; break }
			([psobject]) { [pscustomobject] $properties; break }
			default { $this.CreateInstance($Type, $properties) }
		}
	}

	<#
	.SYNOPSIS
		Creates a new entity of the specified type using that type's parameterless constructor.
	.PARAMETER Type
		The entity type.
	.PARAMETER Properties
		The properties to be set on the newly created object.
	.OUTPUTS
		The newly created object.
	#>
	[object] CreateInstance([type] $Type, [hashtable] $Properties) {
		$culture = [cultureinfo]::InvariantCulture
		$object = $Type::new()
		$propertyMap = $this.GetPropertyMap($Type)

		foreach ($key in $Properties.Keys.Where{ $_ -in $propertyMap.Keys }) {
			$propertyInfo = $propertyMap.$key
			if ($propertyInfo.CanWrite -and (-not [Attribute]::IsDefined($propertyInfo, ([NotMappedAttribute])))) {
				$object.$($propertyInfo.Name) = [Convert]::ChangeType($Properties.$key, $propertyInfo.PropertyType, $culture)
			}
		}

		return $object
	}

	<#
	.SYNOPSIS
		Gets the property map associated with the specified entity type.
	.PARAMETER Type
		The entity type.
	.OUTPUTS
		The property map associated with the specified entity type.
	#>
	[hashtable] GetPropertyMap([type] $Type) {
		if ($Type -in [DataMapper]::PropertyMaps.Keys) { return [DataMapper]::PropertyMaps.$Type }

		$propertyMap = @{}
		$propertyInfos = $Type.GetProperties([BindingFlags]::Instance -bor [BindingFlags]::Public)
		foreach ($propertyInfo in $propertyInfos) {
			$column = [Attribute]::GetCustomAttribute($propertyInfo, ([ColumnAttribute]))
			$key = $column ? $column.Name : $propertyInfo.Name
			$propertyMap.$key = $propertyInfo
		}

		return [DataMapper]::PropertyMaps.$Type = $propertyMap
	}
}
