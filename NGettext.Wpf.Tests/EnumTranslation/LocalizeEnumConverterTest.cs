using System.Windows.Data;
using NGettext.Wpf.EnumTranslation;
using NSubstitute;
using Xunit;

namespace NGettext.Wpf.Tests.EnumTranslation
{
    public class LocalizeEnumConverterTest
    {
        private readonly IEnumLocalizer _enumLocalizer = Substitute.For<IEnumLocalizer>();
        private readonly LocalizeEnumConverter _target = new LocalizeEnumConverter();

        public LocalizeEnumConverterTest()
        {
            LocalizeEnumConverter.EnumLocalizer = _enumLocalizer;
        }

        public enum TestEnum
        {
            EnumValue,
        }

        [Fact]
        public void Converts_Enum_Value_To_Localized_Value()
        {
            _enumLocalizer.LocalizeEnum(Arg.Is(TestEnum.EnumValue)).Returns("localized value");

            var actual = Assert.IsAssignableFrom<IValueConverter>(_target).Convert(TestEnum.EnumValue, null, null, null);

            Assert.Equal("localized value", actual);
        }

        [Fact]
        public void Converts_Null_To_Null()
        {
            var actual = Assert.IsAssignableFrom<IValueConverter>(_target).Convert(null, null, null, null);
            Assert.Null(actual);
        }
    }
}