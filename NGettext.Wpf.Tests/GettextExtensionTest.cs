using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using NSubstitute;
using Xunit;

namespace NGettext.Wpf.Tests
{
    public class GettextExtensionTest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TestClass _valueTarget = new TestClass();

        public class TestClass : FrameworkElement
        {
            public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
                "Text", typeof(string), typeof(TestClass), new PropertyMetadata(default(string)));

            public string Text
            {
                get { return (string)GetValue(TextProperty); }
                set { SetValue(TextProperty, value); }
            }
        }

        public GettextExtensionTest()
        {
            _serviceProvider = Substitute.For<IServiceProvider>();
            var provideValueTarget = Substitute.For<IProvideValueTarget>();
            provideValueTarget.TargetObject.Returns(_valueTarget);
            provideValueTarget.TargetProperty.Returns(TestClass.TextProperty);
            _serviceProvider.GetService(Arg.Is(typeof(IProvideValueTarget))).Returns(provideValueTarget);
        }

        [StaFact]
        public void Is_A_MarkupExtension()
        {
            var target = new GettextExtension("some msgid");
            Assert.IsAssignableFrom<MarkupExtension>(target);
        }

        [StaFact]
        public void ProvideValue_Returns_Text_For_MsgId()
        {
            var msgId = "msgid";
            var text = "translation";
            var target = new GettextExtension(msgId);
            GettextExtension.Localizer = Substitute.For<ILocalizer>();
            GettextExtension.Localizer.Catalog.GetString(Arg.Is(msgId)).Returns(text);

            Assert.Equal(text, target.ProvideValue(_serviceProvider));
        }

        [StaFact]
        public void ProvideValue_Returns_Text_For_MsgId_With_Glib_Style_Context()
        {
            var msgId = "test|msgid";
            var text = "translation";
            var target = new GettextExtension("some|test|msgid");
            GettextExtension.Localizer = Substitute.For<ILocalizer>();
            var context = "some";
            GettextExtension.Localizer.Catalog.GetParticularString(Arg.Is(context),Arg.Is(msgId)).Returns(text);

            Assert.Equal(text, target.ProvideValue(_serviceProvider));
        }


        [StaFact]
        public void ProvideValue_Returns_Text_For_MsgId_With_Params()
        {
            var msgId = "msgid";
            var text = "translation";
            var @params = new object[] { "foo", 42 };
            var target = new GettextExtension(msgId, @params);
            GettextExtension.Localizer = Substitute.For<ILocalizer>();
            GettextExtension.Localizer.Catalog.GetString(Arg.Is(msgId), Arg.Is(@params)).Returns(text);

            Assert.Equal(text, target.ProvideValue(_serviceProvider));
        }

        [StaFact]
        public void ValueTarget_Is_Updated_On_Localizer_CultureTracker_CultureChanged()
        {
            var msgId = "msgid";
            var text = "translation";
            var @params = new object[] { "foo", 42 };
            var target = new GettextExtension(msgId, @params);
            GettextExtension.Localizer = Substitute.For<ILocalizer>();
            GettextExtension.Localizer.CultureTracker.Returns(new CultureTracker());

            target.ProvideValue(_serviceProvider);

            GettextExtension.Localizer.Catalog.GetString(Arg.Is(msgId), Arg.Is(@params)).Returns(text);
            GettextExtension.Localizer.CultureTracker.CurrentCulture = new CultureInfo("da-DK");

            Assert.Equal(text, _valueTarget.Text);
        }

        [StaFact]
        public void
            ValueTarget_Is_Not_Updated_On_Localizer_CultureTracker_CultureChanged_When_ValueTarget_Has_Been_Unloaded()
        {
            var msgId = "msgID";
            var text = "text";
            var oldText = "old text";
            _valueTarget.Text = oldText;
            var @params = new object[] { "foo", 42 };
            var target = new GettextExtension(msgId, @params);
            GettextExtension.Localizer = Substitute.For<ILocalizer>();

            target.ProvideValue(_serviceProvider);
            _valueTarget.RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent));

            GettextExtension.Localizer.Catalog.GetString(Arg.Is(msgId), Arg.Is(@params)).Returns(text);
            GettextExtension.Localizer.CultureTracker.CultureChanged +=
                Raise.Event<EventHandler<CultureEventArgs>>(new CultureEventArgs(new CultureInfo("en-US")));

            Assert.Equal(oldText, _valueTarget.Text);
        }
    }
}