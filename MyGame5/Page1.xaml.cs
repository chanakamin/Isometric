//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices.WindowsRuntime;
//using System.Threading.Tasks;
//using Windows.Foundation;
//using Windows.Foundation.Collections;
//using Windows.Storage;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Controls.Primitives;
//using Windows.UI.Xaml.Data;
//using Windows.UI.Xaml.Input;
//using Windows.UI.Xaml.Media;
//using Windows.UI.Xaml.Navigation;

//// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

//namespace Isometric
//{
//    /// <summary>
//    /// An empty page that can be used on its own or navigated to within a Frame.
//    /// </summary>
//    public sealed partial class Page1
//    {
//        private readonly MyGame5 game;

//        public Page1()
//        {
//            this.InitializeComponent();
//            game = new MyGame5();
//            this.Loaded += (sender, args) => game.Run(this);
//        }

//        private async void  Button_Click(object sender, RoutedEventArgs e)
//        {
//            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
//            Stream s = await storageFolder.OpenStreamForWriteAsync("bsiata_dishmaya1", new CreationCollisionOption());
//            game.bsd.Save(s, SharpDX.Toolkit.Graphics.ImageFileType.Wmp);
////bsd.Source=game.bsd
//        }

      
//    }
//}
