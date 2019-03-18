# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.2.3-alpha] - 2019-03-18
## Fixed
- [#37](https://github.com/robert-j-engdahl/ngettext-wpf/issues/37) GNOME glib syntax for `[EnumMsgId]` attribute.

## [1.2.2-alpha] - 2019-02-27
## Fixed
 - [#34](https://github.com/robert-j-engdahl/ngettext-wpf/issues/34) Crash when msgId was null.
 

## [1.2.1-alpha] - 2019-02-27
## Fixed
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
