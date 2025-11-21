# Changelog

## Version [0.5.0](https://github.com/cedx/sql.ps1/compare/v0.4.0...v0.5.0)
- Added the `-PositionalParameters` argument to most cmdlets.
- Restored the type of the `-Parameters` argument to `[hashtable]`.

## Version [0.4.0](https://github.com/cedx/sql.ps1/compare/v0.3.0...v0.4.0)
- Changed the type of the `-Parameters` argument to `[System.Collections.Specialized.OrderedDictionary]`.

## Version [0.3.0](https://github.com/cedx/sql.ps1/compare/v0.2.0...v0.3.0)
- Added the `-Timeout` parameter to most cmdlets.
- Renamed the `Get-ServerVersion` cmdlet to `Get-Version`.

## Version [0.2.0](https://github.com/cedx/sql.ps1/compare/v0.1.0...v0.2.0)
- Added a simple object mapping feature.
- Added new cmdlets: `Get-ServerVersion`, `New-Command` and `New-Parameter`.
- Replaced the `-AsHastable` parameter of `Get-First`, `Get-Single` and `Invoke-Query` cmdlets by the `-As` parameter.

## Version 0.1.0
- Initial release.
