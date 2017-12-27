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
    }
}