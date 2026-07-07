using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SOG_SharedUtils
{
    public class WindowUtils
    {
        public static Button GetOkButton(Window window, Action action = null)
        {
            var button = new Button
            {
                Content = "OK",
                Width = 80,
                Margin = new Thickness(0, 0, 8, 0),
                IsDefault = true
            };
            button.Click += (s, e) =>
            {
                action?.Invoke();
            };

            return button;
        }

        public static Button GetCancelButton(Window window, Action action = null)
        {
            var button = new Button
            {
                Content = "Cancel",
                Width = 80,
                IsCancel = true
            };
            button.Click += (s, e) =>
            {
                action?.Invoke();
            };

            return button;
        }

        public static Hyperlink GetHyperlink(string text, string uriString)
        {
            var hyperlink = new Hyperlink(new Run(text))
            {
                NavigateUri = new Uri(uriString)
            };

            // Open in default browser
            hyperlink.RequestNavigate += (s, e) =>
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)
                {
                    UseShellExecute = true
                });
                e.Handled = true;
            };

            return hyperlink;
        }

        public static void AddWithLabel(UIElementCollection collection, string label, UIElement element)
        {
            collection.Add(new TextBlock
            {
                Text = label,
                Margin = new Thickness(0, 0, 0, 3)
            });
            collection.Add(element);
        }

        public static bool? ShowDebugDialog(string value, double maxHeight = 300)
        {
            var window = new SOG_OkCancelWindow("ShowDebugDialog")
            {
                MaxHeight = maxHeight,
            };

            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Padding = new Thickness(12),
                Content = new TextBox
                {
                    Text = value,
                    Margin = new Thickness(0, 0, 0, 8)
                }
            };

            window.Stack.Children.Add(scrollViewer);

            return window.ShowDialog();
        }
    }
}
