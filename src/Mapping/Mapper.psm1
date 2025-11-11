using namespace System.ComponentModel.DataAnnotations.Schema
using namespace System.Reflection

<#
.SYNOPSIS
	TODO
#>
class SqlMapper {

	<#
	.SYNOPSIS
		TODO
	#>
	hidden static [hashtable] $TypeMaps = @{}

	<#
	.SYNOPSIS
		TODO
	#>
	static [object] CreateInstance([type] $Type, [hashtable] $Properties) {
		if ($Type -in [SqlMapper]::TypeMaps.Keys) { $typeMap = [SqlMapper]::TypeMaps.$Type }
		else {
			$typeMap =  @{}
			$propertyInfos = $Type.GetProperties([BindingFlags]::Instance -bor [BindingFlags]::NonPublic -bor [BindingFlags]::Public)
			foreach ($propertyInfo in $propertyInfos) {
				$column = [CustomAttributeExtensions]::GetCustomAttribute[ColumnAttribute]($propertyInfo)
				$key = $column ? $column.Name : $propertyInfo.Name
				$typeMap.$key = $propertyInfo
			}

			[SqlMapper]::TypeMaps.$Type = $typeMap
		}

		Write-Host $typeMap

		$object = [Activator]::CreateInstance($Type)
		foreach ($key in $Properties.Keys.Where{ $_ -in $typeMap.Keys }) {
			Write-Host $key

			$propertyInfo = $typeMap.$key
						Write-Host $propertyInfo

			$object.$($propertyInfo.Name) = [Convert]::ChangeType($Properties.$key, $propertyInfo.PropertyType)
		}

		return $object
	}

	<#
	.SYNOPSIS
		Gets the SQL type map associated with the specified entity type.
	.PARAMETER Type
		The entity type.
	.OUTPUTS
		The SQL type map associated with the specified entity type.
	#>
	hidden static [hashtable] GetTypeMap([type] $Type) {
		if ($Type -in [SqlMapper]::TypeMaps.Keys) { return [SqlMapper]::TypeMaps.$Type }
		return $null
	}
}
