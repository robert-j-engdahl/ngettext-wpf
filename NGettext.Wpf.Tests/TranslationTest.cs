using NSubstitute;
using Xunit;

namespace NGettext.Wpf.Tests
{
    public class TranslationTest
    {
        private readonly ILocalizer _localizer = Substitute.For<ILocalizer>();

        public TranslationTest()
        {
            Translation.Localizer = _localizer;
        }

        [Theory, InlineData("some msgid", "some translation")]
        public void Underscore_Returns_Expected_Translation(string msgId, string expectedTranslation)
        {
            _localizer.Catalog.GetString(Arg.Is(msgId)).Returns(expectedTranslation);
            Assert.Equal(expectedTranslation, Translation._(msgId));
        }

        [Fact]
        public void Underscore_Allows_String_Interpolation()
        {
            _localizer.Catalog.GetString(Arg.Is("foo {0} bar {1} baz"), Arg.Is(0xdead), Arg.Is(0xbeef))
                .Returns("expected translation");

            var actual = Translation._("foo {0} bar {1} baz", 0xdead, 0xbeef);

            Assert.Equal("expected translation", actual);
        }

        [Fact]
        public void Underscore_Function_Handles_Localizer_Being_Null()
        {
            Translation.Localizer = null;
            Assert.Equal("untranslated", Translation._("untranslated"));
        }

        [Fact]
        public void Noop_Returns_MsgId()
        {
            Assert.Equal("some msgid", Translation.Noop("some msgid"));
        }

        [Theory]
        [InlineData(42, "singular string", "plural string", "expected translation")]
        public void PluralGettext_Returns_Expected_Translation(int n, string singularMsgId, string pluralMsgId, string expectedTranslation)
        {
            _localizer.Catalog.GetPluralString(singularMsgId, pluralMsgId, n).Returns(expectedTranslation);

            Assert.Equal(expectedTranslation, Translation.PluralGettext(n, singularMsgId, pluralMsgId));
        }

        [Theory]
        [InlineData(42, "singular string", "plural string", "plural string")]
        [InlineData(1, "singular string", "plural string", "singular string")]
        [InlineData(0, "singular string", "plural string", "plural string")]
        [InlineData(-12, "singular string", "plural string", "plural string")]
        public void PluralGettext_Function_Handles_Localizer_Being_Null(int n, string singularMsgId, string pluralMsgId, string expectedResult)
        {
            Translation.Localizer = null;

            Assert.Equal(expectedResult, Translation.PluralGettext(n, singularMsgId, pluralMsgId));
        }

        [Theory]
        [InlineData(42, "singular string", "plural string", "expected translation", 1, 2.0, "foo")]
        public void PluralGettext_Supports_String_Interpolation(int n, string singularMsgId, string pluralMsgId, string expectedTranslation, params object[] @params)
        {
            _localizer.Catalog.GetPluralString(singularMsgId, pluralMsgId, n, @params).Returns(expectedTranslation);

            Assert.Equal(expectedTranslation, Translation.PluralGettext(n, singularMsgId, pluralMsgId, @params));
        }


        [Theory]
        [InlineData(42, "0x{0:x} looks like beef", "all 0x{0:x} look like beef", "all 0xbeef look like beef", 0xbeef)]
        [InlineData(1, "0x{0:x} looks like beef", "all 0x{0:x} look like beef", "0xbeef looks like beef", 0xbeef)]
        [InlineData(0, "0x{0:x} looks like beef", "all 0x{0:x} look like beef", "all 0xbeef look like beef", 0xbeef)]
        [InlineData(-7, "0x{0:x} looks like beef", "all 0x{0:x} look like beef", "all 0xbeef look like beef", 0xbeef)]
        public void PluralGettext_Handles_Localizer_Being_Null_With_Parameters(int n, string singularMsgId, string pluralMsgId, string expectedResult, params object[] @params)
        {
            Translation.Localizer = null;

            Assert.Equal(expectedResult, Translation.PluralGettext(n, singularMsgId, pluralMsgId, @params));
        }
    }
}