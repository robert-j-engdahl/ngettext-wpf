using NGettext.Wpf.EnumTranslation;
using NSubstitute;
using Xunit;

namespace NGettext.Wpf.Tests.EnumTranslation
{
    public class EnumLocalizerTest
    {
        private readonly ILocalizer _localizer = Substitute.For<ILocalizer>();
        private readonly EnumLocalizer _target;

        public EnumLocalizerTest()
        {
            _target = new EnumLocalizer(_localizer);
        }

        public enum TestEnum
        {
            [EnumMsgId("enum value")]
            EnumValue,

            [EnumMsgId("another enum value")]
            AnotherEnumValue,

            [EnumMsgId("some context|a third enum value")]
            ThirdEnumValue,

            EnumValueWithoutMsgId,
        }

        public enum EmptyEnum
        {
        }

        [Fact]
        public void Handles_Invalid_Enum_Member()
        {
            var e = Record.Exception(() => _target.LocalizeEnum(default(EmptyEnum)));

            Assert.Null(e);
        }

        [Theory]
        [InlineData(TestEnum.EnumValue, "enum value")]
        [InlineData(TestEnum.AnotherEnumValue, "another enum value")]
        public void Translates_MsgId_Of_Enum_Value(TestEnum enumValue, string msgId)
        {
            _localizer.Catalog.GetString(Arg.Is(msgId)).Returns("expected translation");

            var actual = _target.LocalizeEnum(enumValue);

            Assert.Equal("expected translation", actual);
        }

        [Theory]
        [InlineData(TestEnum.ThirdEnumValue, "some context", "a third enum value")]
        public void Translates_MsgId_Of_Enum_Value_With_Context(TestEnum enumValue, string context, string msgId)
        {
            _localizer.Catalog.GetParticularString(Arg.Is(context), Arg.Is(msgId)).Returns("expected translation");

            var actual = _target.LocalizeEnum(enumValue);

            Assert.Equal("expected translation", actual);
        }

        [Fact]
        public void Translates_Enum_Value_Without_MsgId_To_Value_Name()
        {
            Assert.Equal("EnumValueWithoutMsgId", _target.LocalizeEnum(TestEnum.EnumValueWithoutMsgId));
        }
    }
}