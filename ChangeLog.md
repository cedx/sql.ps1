# Changelog

## Version [0.13.0](https://github.com/cedx/sql.net/compare/v0.12.0...v0.13.0)
- Added non-generic overloads to the methods of `ConnectionExtensions` class.
- Fixed the `Invoke-Query` cmdlet.

## Version [0.12.0](https://github.com/cedx/sql.net/compare/v0.11.0...v0.12.0)
- Added implicit conversions to the `Parameter` class.

## Version [0.11.0](https://github.com/cedx/sql.net/compare/v0.10.0...v0.11.0)
- Removed the `ConnectionExtensions.ExecuteScalar()` non-generic method.
- The `Adapter` class now implements the `IDisposable` interface.
- The `Get-Scalar` and `Invoke-Query` cmdlets now use a non-terminating error.

## Version [0.10.0](https://github.com/cedx/sql.net/compare/v0.9.0...v0.10.0)
- Added the `TableInfo` and `ColumnInfo` classes.
- The `Get-First` and `Get-Single` cmdlets now use a non-terminating error.

## Version [0.9.0](https://github.com/cedx/sql.net/compare/v0.8.0...v0.9.0)
- Removed the `CommandExtensions` and `ListExtensions` classes.
- Renamed the `DataMapper` class to `Mapper`.
- Renamed the `QueryOptions` record to `CommandOptions`.
- Renamed the `New-DataMapper` cmdlet to `New-Mapper`.
- Merged the `-Parameters` and `-PositionalParameters` cmdlet parameters.
- Added the `New-Transaction`, `Approve-Transaction` and `Deny-Transaction` cmdlets.
- Added the `-Precision`, `-Scale` and `-Size` parameters to the `New-Parameter` cmdlet.
- Added the `-Transaction` parameter to most cmdlets.

## Version [0.8.0](https://github.com/cedx/sql.net/compare/v0.7.0...v0.8.0)
- Moved the data querying and mapping to the `Belin.Sql` assembly.
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
