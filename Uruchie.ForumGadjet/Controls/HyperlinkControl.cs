using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Uruchie.ForumGadjet.Controls
{
    public class HyperlinkControl : Hyperlink
    {
        public HyperlinkControl()
        {
            this.RequestNavigate += HyperlinkControlRequestNavigate;
        }

        private static void HyperlinkControlRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            e.Handled = true;
            if (e.Uri != null)
                Process.Start(e.Uri.ToString());
        }
    }
}
