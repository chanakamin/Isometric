using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace Isometric
{
    public sealed partial class Settings : SettingsFlyout
    {
        public Settings()
        {
            this.InitializeComponent();
            //הצבת מקשי החיצים שמוגדרים
            for (int i = 0; i < _stackPanelArrowsKeys.Children.Count; i++)
            {
                var tb = _stackPanelArrowsKeys.Children[i] as TextBox;
                tb.Text = ManagerGame.arrowsKeys[tb.Name.Split('_')[1]];
            }
        }


        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            //שינוי על פי הגדרת המשתמש
            var value = (sender as TextBox).Name.Split('_')[1];
            (sender as TextBox).Text = e.Key.ToString();
            ManagerGame.arrowsKeys[value] = e.Key.ToString();
            e.Handled = true;
            //שמירת ישום גם אם האפליקציה נסגרת הערכים נשמרים
            ApplicationData.Current.RoamingSettings.Values[value] = e.Key.ToString();

        }
    }
}
