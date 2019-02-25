using System;
using System.Windows.Markup;
using NGettext.Wpf.Common;
using NSubstitute;
using Xunit;

namespace NGettext.Wpf.Tests
{
    public class GettextFormatConverterExtensionTest
    {
        private readonly MarkupExtension _target;

        public GettextFormatConverterExtensionTest()
        {
            _target = new GettextFormatConverterExtension("MSGID {0}");
        }

        [Fact]
        public void Provides_GettextStringFormatConverter()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();

            var value = Assert.IsAssignableFrom<GettextStringFormatConverter>(
                _target.ProvideValue(serviceProvider));

            Assert.Equal("MSGID {0}", value.MsgId);
        }
    }
}