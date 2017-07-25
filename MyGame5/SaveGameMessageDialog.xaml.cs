using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Isometric
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SaveGameMessageDialog : Page
    {
        public SaveGameMessageDialog()
        {
            this.InitializeComponent();
        }

        public string UserName
        {
            get { return _textBoxUserName.Text; }
        }
        //public string Desription
        //{
        //    get { return _textBoxDesription.Text; }
        //}
        public string GameName
        {
            get { return _textBoxGameName.Text; }
        }
        public string Header
        {
            get { return header.Text; }
            set { header.Text = value; }
        }

        public event RoutedEventHandler OnSave;
        private void _buttonSave_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<TextBox, bool> nonValid = new Dictionary<TextBox, bool>();
            string errorText = "";
            nonValid[_textBoxUserName] = _textBoxUserName.Text.Trim() != "";
            nonValid[_textBoxGameName] = _textBoxGameName.Text.Trim() != "";//.BorderBrush = new SolidColorBrush(Colors.Red);
            foreach (var item in nonValid)
            {
                if (item.Value)
                {
                    (item.Key.Parent as Border).BorderBrush = null;
                }
                else
                {
                    (item.Key.Parent as Border).BorderBrush = new SolidColorBrush(Colors.Red);
                    errorText += item.Key.Tag.ToString().Replace("\\n", "\n");
                }
            }

            if (!nonValid.Values.Contains(false))
            {
                error.Visibility = Visibility.Collapsed;
                if (OnSave != null)
                    OnSave(sender, e);
            }
            else
            {
                error.Visibility = Visibility.Visible;
                error.Text = errorText;
            }
        }
    }
}
