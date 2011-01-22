using System.Windows;
using System.Windows.Documents;
using Uruchie.ForumGadjet.Converters;

namespace Uruchie.ForumGadjet.Controls
{
    public class BindableFlowDocument : FlowDocument
    {
        // Using a DependencyProperty as the backing store for TextSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextSourceProperty =
            DependencyProperty.Register("TextSource", typeof (string), typeof (BindableFlowDocument),
                                        new UIPropertyMetadata(null, TextSourceChangedStatic));

        public string TextSource
        {
            get { return (string) GetValue(TextSourceProperty); }
            set { SetValue(TextSourceProperty, value); }
        }

        private static void TextSourceChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BindableFlowDocument) d).TextSourceChanged();
        }

        protected void TextSourceChanged()
        {
            Blocks.Clear();
            if (!string.IsNullOrEmpty(TextSource))
                Blocks.Add(HtmlToFlowDocumentConverter.Convert(TextSource));
        }
    }
}