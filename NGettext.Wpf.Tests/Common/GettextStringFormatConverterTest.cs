using System.Windows.Data;
using NGettext.Wpf.Common;
using NSubstitute;
using Xunit;

namespace NGettext.Wpf.Tests.Common
{
    public class GettextStringFormatConverterTest
    {
        private IValueConverter _target;

        public GettextStringFormatConverterTest()
        {
            _target = new GettextStringFormatConverter("MSGID {0}");
            GettextStringFormatConverter.Localizer = Substitute.For<ILocalizer>();
        }

        [Fact]
        public void Convert_Formats_Value_With_Translated_MsgId()
        {
            var value = "SOME VALUE";
            GettextStringFormatConverter.Localizer.Catalog.GetString(Arg.Is("MSGID {0}"), Arg.Is(value))
                .Returns("FORMATTED TRANSLATION");

            Assert.Equal("FORMATTED TRANSLATION", _target.Convert(value, null, null, null));
        }

        [Fact]
        public void Convert_Formats_Value_With_Translated_MsgId_Using_Translation_Context()
        {
            var target = new GettextStringFormatConverter("CTX|MSGID {0}");

            var value = "SOME VALUE";
            GettextStringFormatConverter.Localizer.Catalog.GetParticularString(Arg.Is("CTX"), Arg.Is("MSGID {0}"), Arg.Is(value))
                .Returns("FORMATTED TRANSLATION");

            Assert.Equal("FORMATTED TRANSLATION", target.Convert(value, null, null, null));
        }

        [Fact]
        public void Convert_Falls_Back_To_Formats_Value_With_Untranslated_MsgId()
        {
            var value = "SOME VALUE";
            GettextStringFormatConverter.Localizer = null;

            Assert.Equal("MSGID SOME VALUE", _target.Convert(value, null, null, null));
        }

        [Fact]
        public void Convert_Falls_Back_To_Formats_Value_With_Untranslated_MsgId_Stripping_Context()
        {
            var target = new GettextStringFormatConverter("CTX|MSGID {0}");
            var value = "SOME VALUE";
            GettextStringFormatConverter.Localizer = null;

            Assert.Equal("MSGID SOME VALUE", target.Convert(value, null, null, null));
        }
    }
}