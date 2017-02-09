using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace EndlessLegenedSaveEditor
{
    /// <summary>
    /// Interaction logic for EmpirePage.xaml
    /// </summary>
    public partial class EmpirePage : Page
    {

        public EmpirePage()
        {
            InitializeComponent();
        }

        private void textBox_TextChanged_ValidateFloatCappedTo999(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;

            Regex regex = new Regex(@"^(([0]|[1-9][0-9]{0,2}))(\.[0-9]{0,8})?$");
            Match match = regex.Match(box.Text);
            if (!match.Success)
            {
                validationRemoveChanges(box, e);
            }
        }

        private void textBox_TextChanged_ValidateFloatCappedTo99999(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;

            Regex regex = new Regex(@"^(([0]|[1-9][0-9]{0,4}))(\.[0-9]{0,8})?$");
            Match match = regex.Match(box.Text);
            if (!match.Success)
            {
                validationRemoveChanges(box, e);
            }
        }

        private void textBox_TextChanged_ValidateFloatCappedTo9999999(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;

            Regex regex = new Regex(@"^(([0]|[1-9][0-9]{0,6}))(\.[0-9]{0,8})?$");
            Match match = regex.Match(box.Text);
            if (!match.Success)
            {
                validationRemoveChanges(box, e);
            }
        }

        private void validationRemoveChanges(TextBox box, TextChangedEventArgs e)
        {
            string originalText = box.Text;
            var originalSelectionStart = box.SelectionStart;
            string text = box.Text;
            var cursorPos = box.SelectionStart;

            foreach (var change in e.Changes.OrderByDescending(x => x.Offset))
            {
                if (change.AddedLength <= 0)
                    continue;
                Console.Write(change.AddedLength + " - ");
                string str = text.Substring(change.Offset, change.AddedLength);
                Console.Write(str);
                Console.Write(" is invalid.");
                text = text.Remove(change.Offset, change.AddedLength);
                cursorPos--;
                Console.WriteLine();
            }
            try
            {
                box.Text = originalText;
                box.SelectionStart = originalSelectionStart;
            }
            catch (ArgumentOutOfRangeException exp)
            {
                Console.WriteLine("Validation failed {0}", exp.Message);
                box.Text = originalText;
                box.SelectionStart = originalSelectionStart;
            }
        }
    }
}
