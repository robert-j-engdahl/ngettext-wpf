# ngettext-wpf
Proper internationalization support for WPF (via NGettext)

## Getting Started
NGettext.Wpf is intended to work with dependency injection.  A few static properties need to be initialized at the entry point of your application:

```c#
var cultureTracker = new CultureTracker();
ChangeCultureCommand.CultureTracker = cultureTracker;
GettextExtension.Localizer = new Localizer(cultureTracker, "ExampleDomainName");
```

The `"ExampleDomainName"` string is the domain name.  This means that when the current culture is set to `"da-DK"` translations will be loaded from `"Locale\da-DK\LC_MESSAGES\ExampleDomainName.mo"` relative to where your WPF app is running.

Now you can do something like this in XAML:

```XAML
<Button CommandParameter="en-US" 
        Command="{StaticResource ChangeCultureCommand}" 
        Content="{wpf:Gettext English}" />
```
Which demonstrates two features of this library.  The most important is the Gettext markup extension which will make sure the `Content` is set to the translation of "English" with respect to the current culture, and update it when the current culture is changed.  The other feature it demonstrates is the `ChangeCultureCommand` which changes the current culture to the given culture, in this case `"en-US"`.

Have a look at <a href="NGettext.Wpf.Example/UpdateTranslations.ps1">NGettext.Wpf.Example\UpdateTranslations.ps1</a> for how to extract msgids from both xaml and cs files.

## Conventions
Keep your compiled translations in `"Locale\<LOCALE>\LC_MESSAGES\<DOMAIN>.mo"`.  This library will force you to follow this convention.  Or rather, NGettext forces you to follow a convention like `"<PATH_TO_LOCALES>\<LOCALE>\LC_MESSAGES\<DOMAIN>.mo"`, and I refined it.

Keep your raw translations in `"Locale\<LOCALE>\LC_MESSAGES\<DOMAIN>.po"`.  This is not enforced, but when working with POEdit it will compile the `".mo"` file into the correct location when following this convention, and it doesn't remember your previous choice, so stick with the defaults.

There are lots of GNU conventions related to internationalization (i18n) and localization (l10n).  One of them is that the notion that the original program is written in US English, so you don't need to translate anything to facilitate internationalization.  The original text in US English is called the `msgId`.

