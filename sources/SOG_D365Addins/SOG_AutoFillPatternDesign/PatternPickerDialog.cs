using SOG_SharedUtils;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SOG_AutoFillPatternDesign
{
    public class PatternPickerDialog : SOG_OkCancelWindow
    {
        public string SelectedPattern { get; private set; }
        public bool AddOptional { get; private set; }

        public PatternPickerDialog(IEnumerable<string> options, string defaultValue = null) : base("SOG_AutoFillPatternDesign Dialog")
        {
            var combo = new ComboBox
            {
                Margin = new Thickness(0, 0, 0, 8),
                ItemsSource = options,
                SelectedIndex = defaultValue == null ? -1 : options.ToList().IndexOf(defaultValue)
            };
            AddWithLabel("Select a form pattern:", combo);

            var checkBox = new CheckBox
            {
                Margin = new Thickness(0, 0, 0, 8),
                IsChecked = false,
            };
            AddWithLabel("Add optional controls ?", checkBox);

            Stack.Children.Add(new TextBlock
            {
                Text = "IMPORTANT : IT WILL CLEAR YOUR CURRENT DESIGN\nONCE DONE, IF ERRORS REMAIN, TRY TO REOPEN THE FORM AND/OR BUILD MODEL",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 4)
            });

            var hyperlinkBlock = new TextBlock
            {
                Margin = new Thickness(0, 0, 0, 8)
            };
            hyperlinkBlock.Inlines.Add(WindowUtils.GetHyperlink("Don't forget to follow best practices here",
                "https://learn.microsoft.com/en-us/dynamics365/fin-ops-core/dev-itpro/user-interface/select-form-pattern#list-of-classes-of-top-level-form-patterns"));

            Stack.Children.Add(hyperlinkBlock);

            OkAction = () =>
            {
                SelectedPattern = combo.SelectedItem == null ? null : (combo.SelectedItem as string);
                AddOptional = checkBox.IsChecked ?? false;
            };
            DialogResultFunc = () => combo.SelectedItem != null;
        }
    }
}
