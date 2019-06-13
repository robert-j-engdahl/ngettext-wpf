using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using JetBrains.Annotations;

namespace NGettext.Wpf.Serialization
{
    public class TranslationSerializer
    {
        private readonly Func<CultureInfo, ICatalog> _createCatalog;

        public TranslationSerializer(Func<CultureInfo, ICatalog> createCatalog)
        {
            _createCatalog = createCatalog;
        }

        [StringFormatMethod("msgId")]
        [Obsolete("This method is experimental, and may go away.")]
        public string SerializedGettext(IEnumerable<CultureInfo> cultureInfos, string msgId, params object[] args)
        {
            var msgIdWithContext = LocalizerExtensions.ConvertToMsgIdWithContext(msgId);
            var result = new StringBuilder();
            result.Append("{");
            bool addComma = false;
            foreach (var cultureInfo in cultureInfos)
            {
                if (addComma)
                {
                    result.Append(", ");
                }
                else
                {
                    addComma = true;
                }

                var catalog = _createCatalog(cultureInfo);
                string message;

                if (string.IsNullOrEmpty(msgIdWithContext.Context))
                {
                    if (args.Any())
                    {
                        message = catalog.GetString(msgIdWithContext.MsgId, args);
                    }
                    else
                    {
                        message = catalog.GetString(msgIdWithContext.MsgId);
                    }
                }
                else
                {
                    if (args.Any())
                    {
                        message = catalog.GetParticularString(msgIdWithContext.Context, msgIdWithContext.MsgId, args);
                    }
                    else
                    {
                        message = catalog.GetParticularString(msgIdWithContext.Context, msgIdWithContext.MsgId);
                    }
                }

                result.AppendFormat("\"{0}\": \"{1}\"", cultureInfo.Name,
                    HttpUtility.JavaScriptStringEncode(message));
            }
            result.Append("}");
            return result.ToString();
        }
    }
}