using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Uruchie.ForumGadjet.Converters;

namespace Uruchie.ForumGadjet.Controls
{
    public class HtmlView : ContentControl
    {
        private WebBrowser webBrowser = new WebBrowser();

        public HtmlView()
        {
            Content = webBrowser;
            webBrowser.IsHitTestVisible = false;
        }

        public string TextSource
        {
            get { return (string)GetValue(TextSourceProperty); }
            set { SetValue(TextSourceProperty, value); }
        }

        public static readonly DependencyProperty TextSourceProperty =
            DependencyProperty.Register("TextSource", typeof(string), typeof(HtmlView), new UIPropertyMetadata("", TextSourceChangedStatic));

        private static void TextSourceChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((HtmlView)d).TextSourceChanged();
        }

        protected void TextSourceChanged()
        {
            //webBrowser.NavigateToString("<HTML></HTML>");
            if (!string.IsNullOrEmpty(TextSource))
                webBrowser.NavigateToString(BbCodesToHtmlConverter.BbCodesToHtml(TextSource));
        }
        
    }
}
