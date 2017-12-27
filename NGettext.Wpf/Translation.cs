namespace NGettext.Wpf
{
    public static class Translation
    {
        public static string _(string msgId)
        {
            return Localizer?.Catalog.GetString(msgId) ?? msgId;
        }

        public static ILocalizer Localizer { get; set; }
    }
}