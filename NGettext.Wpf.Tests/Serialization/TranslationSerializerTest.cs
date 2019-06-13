using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using NGettext.Wpf.Serialization;
using NSubstitute;
using Xunit;

namespace NGettext.Wpf.Tests.Serialization
{
    public class TranslationSerializerTest
    {
        private readonly TranslationSerializer _target;
        private readonly ICatalog _enCatalog = Substitute.For<ICatalog>();
        private readonly ICatalog _daCatalog = Substitute.For<ICatalog>();

        public TranslationSerializerTest()
        {
            _target = new TranslationSerializer(CreateCatalog);

            _enCatalog.GetParticularString("Some context", "English message").Returns("English message");
            _enCatalog.GetString("Other English message").Returns("Other English message");
            _enCatalog.GetString("Quotes \"").Returns("Quotes \"");
            _enCatalog.GetParticularString("Some context", "English message {0}", 42).Returns("English message 42");
            _enCatalog.GetString("Other English message {0}", 42).Returns("Other English message 42");

            _daCatalog.GetParticularString("Some context", "English message").Returns("Dansk besked");
            _daCatalog.GetString("Other English message").Returns("Anden dansk besked");
            _daCatalog.GetString("Quotes \"").Returns("Gåseøjne \"");
            _daCatalog.GetParticularString("Some context", "English message {0}", 42).Returns("Dansk besked 42");
            _daCatalog.GetString("Other English message {0}", 42).Returns("Anden dansk besked 42");
        }

        private ICatalog CreateCatalog(CultureInfo cultureInfo)
        {
            switch (cultureInfo.Name)
            {
                case "en-US":
                    return _enCatalog;

                case "da-DK":
                    return _daCatalog;
            }
            throw new NotImplementedException();
        }

        [Theory]
        [InlineData("en-US", "Some context|English message", "English message")]
        [InlineData("da-DK", "Some context|English message", "Dansk besked")]
        [InlineData("en-US", "Other English message", "Other English message")]
        [InlineData("da-DK", "Other English message", "Anden dansk besked")]
        [InlineData("da-DK", "Quotes \"", "Gåseøjne \"")]
        public void Serializes_Translated_Messages(string locale, string msgId, string message)
        {
            var serializedGettext = _target.SerializedGettext(new[] { new CultureInfo("en-US"), new CultureInfo("da-DK") }, msgId);
            Assert.Equal(message, JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedGettext)[locale]);
        }

        [Theory]
        [InlineData("en-US", "Some context|English message {0}", "English message 42")]
        [InlineData("da-DK", "Some context|English message {0}", "Dansk besked 42")]
        [InlineData("en-US", "Other English message {0}", "Other English message 42")]
        [InlineData("da-DK", "Other English message {0}", "Anden dansk besked 42")]
        public void Serializes_Translated_Messages_Argv(string locale, string msgId, string message)
        {
            var serializedGettext = _target.SerializedGettext(new[] { new CultureInfo("en-US"), new CultureInfo("da-DK") }, msgId, 42);

            Assert.Equal(message, JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedGettext)[locale]);
        }
    }
}