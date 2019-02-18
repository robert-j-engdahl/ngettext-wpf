using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace NGettext.Wpf
{
    public class GettextExtension : MarkupExtension, IWeakCultureObserver
    {
        private DependencyObject _dependencyObject;
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
            var provideValueTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            _dependencyObject = (DependencyObject)provideValueTarget.TargetObject;
            if (DesignerProperties.GetIsInDesignMode(_dependencyObject))
            {
                return string.Format(MsgId, Params);
            }

            AttachToCultureChangedEvent();

            _dependencyProperty = (DependencyProperty)provideValueTarget.TargetProperty;

            KeepGettextExtensionAliveForAsLongAsDependencyObject();

            return Gettext();
        }

        private string Gettext()
        {
            if (Localizer is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return MsgId;
            }

            if (MsgId.Contains("|"))
            {
                var array = MsgId.Split('|');

                return Localizer.Catalog.GetParticularString(array.First(), string.Join("|", array.Skip(1)));
            }

            return Params.Any() ? Localizer.Catalog.GetString(MsgId, Params) : Localizer.Catalog.GetString(MsgId);
        }

        private void KeepGettextExtensionAliveForAsLongAsDependencyObject()
        {
            SetGettextExtension(_dependencyObject, this);
        }

        private void AttachToCultureChangedEvent()
        {
            if (Localizer is null)
            {
                Console.Error.WriteLine("NGettext.WPF.GettextExtension.Localizer not set.  Localization is disabled.");
                return;
            }

            Localizer.CultureTracker.AddWeakCultureObserver(this);
        }

        public void HandleCultureChanged(ICultureTracker sender, CultureEventArgs eventArgs)
        {
            _dependencyObject.SetValue(_dependencyProperty, Gettext());
        }

        public static readonly DependencyProperty GettextExtensionProperty = DependencyProperty.RegisterAttached(
            "GettextExtension", typeof(GettextExtension), typeof(GettextExtension), new PropertyMetadata(default(GettextExtension)));

        public static void SetGettextExtension(DependencyObject element, GettextExtension value)
        {
            element.SetValue(GettextExtensionProperty, value);
        }

        public static GettextExtension GetGettextExtension(DependencyObject element)
        {
            return (GettextExtension)element.GetValue(GettextExtensionProperty);
        }
    }
}