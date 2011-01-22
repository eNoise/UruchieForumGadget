using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Uruchie.ForumGadjet.Controls
{
    public class HyperlinkControl : Hyperlink
    {
        public HyperlinkControl()
        {
            RequestNavigate += HyperlinkControlRequestNavigate;
        }

        private static void HyperlinkControlRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            e.Handled = true;
            if (e.Uri != null)
                Process.Start(e.Uri.ToString());
        }
    }
}