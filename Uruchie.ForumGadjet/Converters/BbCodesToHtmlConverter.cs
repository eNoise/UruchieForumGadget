using System;
using Uruchie.ForumGadjet.Helpers;

namespace Uruchie.ForumGadjet.Converters
{
    public static class BbCodesToHtmlConverter
    {
        public static string BbCodesToHtml(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            //to many strings :)

            var simpleTags = new[] {"B", "I", "S", "U", "b", "i", "s", "u"};
            foreach (string tag in simpleTags)
                text = text.Replace("[" + tag + "]", "<" + tag + ">").Replace("[/" + tag + "]", "</" + tag + ">");

            CommonHelper.Try(() => text = ReplaceParamaterizedTag(text, "color", "font", a => string.Format("<font color={0}>", a)));
            CommonHelper.Try(() => text = ReplaceParamaterizedTag(text, "font", "font", a => string.Format("<font face={0}>", a)));
            CommonHelper.Try(() => text = ReplaceParamaterizedTag(text, "size", "font", a => string.Format("<font size={0}>", a)));
            CommonHelper.Try(() => text = ReplaceParamaterizedTag(text, "url", "a", a => string.Format("<a href={0}>", a)));
            CommonHelper.Try(() => text = ReplaceParamaterizedTag(text, "quote", "font", a => string.Format("<font color=lightgray>")));

            text = text.Replace("[CENTER]", "").Replace("[/CENTER]", "");

            if (text.Contains("[SPOILER") && text.Contains("[/SPOILER]"))
                CommonHelper.Try(() =>
                                     {
                                         int pos = 0;
                                         text = text.Remove(pos = text.IndexOf("[SPOILER"),
                                                            text.LastIndexOf("[/SPOILER]") - pos + 10);
                                     });

            return text;
        }

        /// <summary>
        /// «asking regexes to parse arbitrary HTML is like asking Paris Hilton to write an operating system» ©
        /// </summary>
        private static string ReplaceParamaterizedTag(string text, string bbTag, string htmlTag,
                                                      Func<string, string> replaceWith)
        {
            int colortagIndex = 0;
            while ((colortagIndex = text.IndexOf("[" + bbTag.ToLower(), 
                colortagIndex, StringComparison.InvariantCultureIgnoreCase)) > -1)
            {
                int endTokenPosition = text.IndexOf(']', colortagIndex);
                if (endTokenPosition == -1)
                    break;

                string tag = text.Substring(colortagIndex, endTokenPosition - colortagIndex + 1);

                string argument = string.Empty;
                if (tag.Contains("="))
                    argument = tag.Remove(0, tag.IndexOf("=") + 1).Replace("]", "");
                else
                    argument = text.Substring(endTokenPosition + 1, text.IndexOf("[/" + bbTag + "]", endTokenPosition, StringComparison.CurrentCultureIgnoreCase) - endTokenPosition - 1);



                text = text.Replace(tag, replaceWith(argument));
            }

            text = text.Replace("[/" + bbTag.ToUpper() + "]", "</" + htmlTag + ">").Replace(
                "[/" + bbTag.ToLower() + "]", "</" + htmlTag + ">");

            return text;
        }
    }
}