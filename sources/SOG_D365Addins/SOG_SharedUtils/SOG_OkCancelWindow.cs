using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SOG_SharedUtils
{
    public class SOG_OkCancelWindow : Window
    {
        public Action OkAction { get; set; }
        public Action CancelAction { get; set; }
        public Func<bool?> DialogResultFunc { get; set; }

        public StackPanel Stack { get; private set; }

        public new object Content { get; private set; }

        private Button okButton;

        public SOG_OkCancelWindow(string title)
        {
            Title = title;
            SizeToContent = SizeToContent.WidthAndHeight;
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            // Set VS main window as owner so dialog stays on top
            Owner = Application.Current.MainWindow;

            var contentStack = new StackPanel
            {
                Margin = new Thickness(20),
            };

            Stack = new StackPanel();

            contentStack.Children.Add(Stack);

            var btnRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            okButton = WindowUtils.GetOkButton(this, OkActionMethod);

            btnRow.Children.Add(okButton);
            btnRow.Children.Add(WindowUtils.GetCancelButton(this, CancelActionMethod));

            contentStack.Children.Add(btnRow);

            Content = contentStack;
            base.Content = Content;
        }

        private void OkActionMethod()
        {
            OkAction?.Invoke();
            var dialogResult = DialogResultMethod();
            if (dialogResult == true || dialogResult == null)
            {
                DialogResult = dialogResult;
                Close();
            }
            else
            {
                DialogResult = null;
            }
        }

        private void CancelActionMethod()
        {
            CancelAction?.Invoke();
            DialogResult = false;
            Close();
        }

        private bool? DialogResultMethod()
        {
            return DialogResultFunc?.Invoke();
        }

        public void AddWithLabel(string label, UIElement element)
        {
            WindowUtils.AddWithLabel(Stack.Children, label, element);

            element.KeyUp += (s, e) => {
                okButton.IsEnabled = DialogResultMethod() ?? true;
            };
            element.LostFocus += (s, e) => {
                okButton.IsEnabled = DialogResultMethod() ?? true;
            };

            okButton.IsEnabled = DialogResultMethod() ?? true;
        }

        public new bool? ShowDialog()
        {
            okButton.IsEnabled = DialogResultMethod() ?? true;

            return base.ShowDialog();
        }
    }
}
