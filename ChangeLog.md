# Changelog

## Version [0.8.0](https://github.com/cedx/sql.net/compare/v0.7.0...v0.8.0)
- Moved the data mapping to the `Belin.Sql` assembly.
- Moved the cmdlets to the `Belin.Sql.Cmdlets` assembly.
- Added the `Belin.Sql.Dapper` assembly providing type maps and type handlers for [Dapper](https://www.learndapper.com).
- Added the `-CommandType` parameter to most cmdlets.
- Added the `-DbType` and `-Direction` parameters to the `New-Parameter` cmdlet.

## Version [0.7.0](https://github.com/cedx/sql.net/compare/v0.6.0...v0.7.0)
- Ported the cmdlets to [C#](https://learn.microsoft.com/en-us/dotnet/csharp).

## Version [0.6.0](https://github.com/cedx/sql.net/compare/v0.5.1...v0.6.0)
- Added the `New-DataMapper` cmdlet.
- Fixed the deserialization of `Enum` and `Nullable` types.

## Version [0.5.1](https://github.com/cedx/sql.net/compare/v0.5.0...v0.5.1)
- Fixed the `New-Command` cmdlet.

## Version [0.5.0](https://github.com/cedx/sql.net/compare/v0.4.0...v0.5.0)
- Added the `-PositionalParameters` argument to most cmdlets.
- Restored the type of the `-Parameters` argument to `[hashtable]`.

## Version [0.4.0](https://github.com/cedx/sql.net/compare/v0.3.0...v0.4.0)
- Changed the type of the `-Parameters` argument to `[System.Collections.Specialized.OrderedDictionary]`.

## Version [0.3.0](https://github.com/cedx/sql.net/compare/v0.2.0...v0.3.0)
- Added the `-Timeout` parameter to most cmdlets.
- Renamed the `Get-ServerVersion` cmdlet to `Get-Version`.

## Version [0.2.0](https://github.com/cedx/sql.net/compare/v0.1.0...v0.2.0)
- Added a simple object mapping feature.
- Added new cmdlets: `Get-ServerVersion`, `New-Command` and `New-Parameter`.
- Replaced the `-AsHastable` parameter of `Get-First`, `Get-Single` and `Invoke-Query` cmdlets by the `-As` parameter.

## Version 0.1.0
- Initial release.
