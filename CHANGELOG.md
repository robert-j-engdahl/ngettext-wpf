# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.2.6-alpha] - 2019-08-16
### Added
 - XGettext-Xaml annotated locations match the given parameter instead of just the filename.  This is useful when making PoEdit friendly .po files where the locations must be relative.  Thanks to @PGPoulsen for the PR.

 ### Fixed
 - [46](https://github.com/robert-j-engdahl/ngettext-wpf/issues/46) Crash when localizing invalid enum value

## [1.2.5-alpha] - 2019-06-13
### Added
 - Experimental TranslationSerializer for making localized json objects that can be used from TSQL like so

```sql
SELECT 
    JSON_VALUE([Message], '$."en-US"') AS 'English', 
    JSON_VALUE([Message], '$."da-DK"') AS 'Danish'
FROM (VALUES 
    (N'{"en-US": "Some message", "da-DK": "En eller anden besked"}')) 
AS Example([Message])
```

## [1.2.4] - 2019-06-13

## [1.2.4-alpha] - 2019-05-29
### Fixed
 - [40](https://github.com/robert-j-engdahl/ngettext-wpf/issues/40) Show `Text="{wpf:Gettext Context|MsgId}"` as `MsgId` in XAML Designer

## [1.2.3-alpha] - 2019-03-18
### Fixed
- [#37](https://github.com/robert-j-engdahl/ngettext-wpf/issues/37) GNOME glib syntax for `[EnumMsgId]` attribute.

## [1.2.2-alpha] - 2019-02-27
### Fixed
 - [#34](https://github.com/robert-j-engdahl/ngettext-wpf/issues/34) Crash when msgId was null.

## [1.2.1-alpha] - 2019-02-27
### Fixed
 - Wrong binaries released with 1.2.0-alpha.

## [1.2.0-alpha] - 2019-02-27
### Added
- [#30](https://github.com/robert-j-engdahl/ngettext-wpf/issues/30) `GettextFormatConverter` XAML extension.

### Fixed
- Multiple keywords for `XGettext-Xaml.ps1` didn't work.
- [#31](https://github.com/robert-j-engdahl/ngettext-wpf/issues/31) GNOME glib syntax does not work for static translation methods in C# (`Translation._()`).

## [1.1.0-alpha] - 2019-02-19
### Deprecated
- `Translation.PluralGettext()` will be replaced by `Translation.GetPluralString()` in 2.x

### Changed
- `Gettext` XAML extension follows Gnome GLib style context syntax.
