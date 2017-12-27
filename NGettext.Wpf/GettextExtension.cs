using System;
using System.Windows;
using System.Windows.Markup;

namespace NGettext.Wpf
{
    public class GettextExtension : MarkupExtension
    {
        private FrameworkElement _frameworkElement;
        private DependencyProperty _dependencyProperty;

        [ConstructorArgument("params")] public object[] Params { get; set; }

        [ConstructorArgument("msgId")] public string MsgId { get; set; }

        public GettextExtension(string msgId)
        {
            MsgId = msgId;
            Params = new object[] { };
        }

        public GettextExtension(string msgId, params object[] @params)
        {
            MsgId = msgId;
            Params = @params;
        }

        public static ILocalizer Localizer { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValueTarget = (IProvideValueTarget) serviceProvider.GetService(typeof(IProvideValueTarget));
            _frameworkElement = (FrameworkElement) provideValueTarget.TargetObject;
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(_frameworkElement))
            {
                return string.Format(MsgId, Params);
            }

            AttachToCultureChangedEvent();
            _frameworkElement.Unloaded += DetatchFromCultureChangedEvent;

            _dependencyProperty = (DependencyProperty) provideValueTarget.TargetProperty;


            return Localizer.Catalog.GetString(MsgId, Params);
        }

        private void AttachToCultureChangedEvent()
        {
            if (Localizer is null)
            {
                throw new Exception("GettextExtension.Localizer must be initialized");
            }

            Localizer.CultureTracker.CultureChanged += UpdateTranslation;
        }

        private void DetatchFromCultureChangedEvent(object sender, RoutedEventArgs e)
        {
            Localizer.CultureTracker.CultureChanged -= UpdateTranslation;
        }

        private void UpdateTranslation(object sender, CultureEventArgs e)
        {
            _frameworkElement.SetValue(_dependencyProperty, Localizer.Catalog.GetString(MsgId, Params));
        }
    }
}