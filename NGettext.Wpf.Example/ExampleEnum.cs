using NGettext.Wpf.EnumTranslation;

namespace NGettext.Wpf.Example
{
    public enum ExampleEnum
    {
        [EnumMsgId("Some value")]
        SomeValue,

        [EnumMsgId("Some other value")]
        SomeOtherValue,

        [EnumMsgId("Some third value")]
        SomeThirdValue,

        [EnumMsgId("EnumMsgId example|Some fourth value")]
        SomeFourthValue,

        SomeValueWithoutEnumMsgId,
    }
}