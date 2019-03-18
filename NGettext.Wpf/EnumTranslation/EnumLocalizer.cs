using System;
using System.Linq;
using System.Reflection;

namespace NGettext.Wpf.EnumTranslation
{
    public interface IEnumLocalizer
    {
        string LocalizeEnum(Enum value);
    }

    public class EnumLocalizer : IEnumLocalizer
    {
        private readonly ILocalizer _localizer;

        public EnumLocalizer(ILocalizer localizer)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public string LocalizeEnum(Enum value)
        {
            var type = value.GetType();
            var enumMemberName = value.ToString();
            var msgIdAttribute = (EnumMsgIdAttribute)type.GetMember(enumMemberName).Single().GetCustomAttribute(typeof(EnumMsgIdAttribute), true);

            if (msgIdAttribute is null)
            {
                Console.Error.WriteLine($"{type}.{enumMemberName} lacks the [MsgId(\"...\")] attribute.");
                return enumMemberName;
            }

            return _localizer.Gettext(msgIdAttribute.MsgId);
        }
    }
}