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
    }
}