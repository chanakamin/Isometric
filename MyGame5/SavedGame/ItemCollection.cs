using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Xml.Linq;
using Windows.UI.Xaml.Media.Imaging;

namespace Isometric.SavedGame
{
     class ItemCollection : IEnumerable<SavedGame>
    {
        ObservableCollection<SavedGame> itemCollection = new ObservableCollection<SavedGame>();
        //מממש את הממשק מחזיר את הליסט בשני צורות ג'נרי...
        public IEnumerator<SavedGame> GetEnumerator()
        {
            return itemCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(SavedGame item)
        {
            itemCollection.Add(item);
        }

        public SavedGame GetSavedGame(int index)
        {
            if (index < itemCollection.Count)
            {
                return itemCollection[index];
            }
            return null;
        }

        public List<SavedGame> GetitemCollection(string Group)
        {
            List<SavedGame> items = new List<SavedGame>();
            foreach (SavedGame item in itemCollection)
            {     
                if (item.Type == Group)
                {
                    items.Add(item);
                }
            }
            return items;
        }


       
    }

    public class GroupInfoList<T> : List<object>
    {

        public object Key { get; set; }


        public new IEnumerator<object> GetEnumerator()
        {
            return (IEnumerator<object>)base.GetEnumerator();
        }
    }

    public class SavedGameData
    {
         ItemCollection _Collection = new ItemCollection();

         ItemCollection Collection
         {
             get { return _Collection; }
         }
          public   SavedGameData()
         {
           //   openFiles();
         }
        async public Task openFiles()
        {
             StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            var x= await storageFolder.GetFilesAsync();
           // x.GetResults();
             
            foreach (var file in x.ToList())
            {
               // var bsd= file.OpenReadAsync();
                  //IRandomAccessStream fileStream =
                  //  await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                if (file.FileType.Equals(".xml"))
                { 
                    try
                    {
                        XDocument Document = XDocument.Load(file.Path.ToString());
                        Collection.Add(new SavedGame()
                        {
                            GameName = file.Name,
                            UserName = Document.Descendants("UserName").ToList().First().Value,
                            Date = Document.Descendants("Date").ToList().First().Value,
                            Type ="1",// Document.Descendants("Type").ToList().First().Value,
                            ImageSrc = new BitmapImage(new Uri(file.Path.Substring(0, file.Path.IndexOf(".xml")) + ".jpg"))
                        });
                    }
                    catch (Exception ex)
                    {
                        int b;
                    }
                }
                //Document.

                  //XElement.Load(bsd);
               // Windows.Foundation
            }
            // var x= StorageFolder.GetFolderFromPathAsync("ms-appx:///");
          //   StorageFile file =  StorageFile.GetFileFromApplicationUriAsync(storageFolder.Path.));
            // var file =  StorageFile.GetFileFromPathAsync(storageFolder.Path.ToString());
           //  string jsonText =  FileIO.ReadTextAsync(file);
            int v;
         }
        internal List<GroupInfoList<object>> GetGroupsByUserName()
        {
            List<GroupInfoList<object>> groups = new List<GroupInfoList<object>>();

            var query = from item in Collection
                        orderby ((SavedGame)item).UserName
                        group item by ((SavedGame)item).UserName into g
                        select new { GroupName = g.Key, Items = g };
            foreach (var g in query)
            {
                GroupInfoList<object> info = new GroupInfoList<object>();
                info.Key = g.GroupName;
                foreach (var item in g.Items)
                {
                    info.Add(item);
                }
                groups.Add(info);
                
            }
            //Collection.GroupBy(g => g.Type);
            return groups;

        }

        //internal List<GroupInfoList<object>> GetGroupsByLetter()
        //{
        //    List<GroupInfoList<object>> groups = new List<GroupInfoList<object>>();

        //    var query = from item in Collection
        //                orderby ((MyRecipes)item).Name
        //                group item by ((MyRecipes)item).Name[0] into g
        //                select new { GroupName = g.Key, Items = g };
        //    foreach (var g in query)
        //    {
        //        GroupInfoList<object> info = new GroupInfoList<object>();
        //        info.Key = g.GroupName;
        //        foreach (var item in g.Items)
        //        {
        //            info.Add(item);
        //        }
        //        groups.Add(info);
        //    }

        //    return groups;

        //}
    }
}
