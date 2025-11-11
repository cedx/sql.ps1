using namespace System.ComponentModel.DataAnnotations.Schema
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
		Creates a new entity of the specified type using that type's parameterless constructor.
	.PARAMETER Type
		The entity type.
	.PARAMETER Properties
		The properties to be set on the newly created object.
	.OUTPUTS
		The newly created object.
	#>
	static [object] CreateInstance([type] $Type, [hashtable] $Properties) {
		$culture = [cultureinfo]::InvariantCulture
		$object = $Type::new()
		$propertyMap = [DataMapper]::GetPropertyMap($Type)

		foreach ($key in $Properties.Keys.Where{ $_ -in $propertyMap.Keys }) {
			$propertyInfo = $propertyMap.$key
			if ($propertyInfo.CanWrite -and (-not [CustomAttributeExtensions]::IsDefined[NotMappedAttribute]($propertyInfo))) {
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
	hidden static [hashtable] GetPropertyMap([type] $Type) {
		if ($Type -in [DataMapper]::PropertyMaps.Keys) { return [DataMapper]::PropertyMaps.$Type }

		$propertyMap = @{}
		$propertyInfos = $Type.GetProperties([BindingFlags]::Instance -bor [BindingFlags]::Public)
		foreach ($propertyInfo in $propertyInfos) {
			$column = [CustomAttributeExtensions]::GetCustomAttribute[ColumnAttribute]($propertyInfo)
			$key = $column ? $column.Name : $propertyInfo.Name
			$propertyMap.$key = $propertyInfo
		}

		return [DataMapper]::PropertyMaps.$Type = $propertyMap
	}
}
