using System;

namespace NGettext.Wpf.EnumTranslation
{
    public class EnumMsgIdAttribute : Attribute
    {
        public EnumMsgIdAttribute(string msgId)
        {
            if (msgId.Contains("|"))
            {
                msgId = msgId.Substring(msgId.IndexOf("|") + 1);
            }
            MsgId = msgId;
        }

        public string MsgId { get; }
    }
}