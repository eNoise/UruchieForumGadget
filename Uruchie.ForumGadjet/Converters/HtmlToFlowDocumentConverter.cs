using System;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Navigation;
using HTMLConverter;
using Uruchie.Core.Helpers;
using Uruchie.ForumGadjet.Helpers;

namespace Uruchie.ForumGadjet.Converters
{
    /// <summary>
    /// TODO: needs refactoring!
    /// </summary>
    public class HtmlToFlowDocumentConverter
    {
        public static Section Convert(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Section section = null;
                try
                {
                    string xaml = HtmlToXamlConverter.ConvertHtmlToXaml(BbCodesToHtmlConverter.BbCodesToHtml(value),
                                                                        false);
                    section = XamlReader.Parse(xaml) as Section;
                    CommonHelper.Try(() => SetHyperlinksHandler(section));
                }
                catch
                {
                    section = new Section(new Paragraph(new Run(value)));
                }

                return section;
            }
            return new Section(new Paragraph(new Run(value)));
        }


        private static void SetHyperlinksHandler(Section doc)
        {
            foreach (Paragraph p in doc.Blocks.OfType<Paragraph>())
            {
                foreach (Inline inline in p.Inlines)
                    LookupHyperlinks(inline);
            }
        }

        private static void LookupHyperlinks(Inline inline)
        {
            if (inline is Hyperlink)
            {
                var hyperlink = ((Hyperlink) inline);
                hyperlink.RequestNavigate += HyperlinkRequestNavigate;
            }
            if (inline is Span)
                foreach (Inline subInline in ((Span) inline).Inlines)
                    LookupHyperlinks(subInline);
        }

        private static void HyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                e.Handled = true;
                if (e.Uri != null && !string.IsNullOrEmpty(e.Uri.ToString()))
                    CommonHelper.OpenBrowser(e.Uri.ToString());
            }
            catch
            {
            }
        }
    }
}