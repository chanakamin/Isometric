using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Isometric.SavedGame
{
    class SavedGame : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        string userName;

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }


        string gameName;

        public string GameName
        {
            get { return gameName; }
            set
            {
                gameName = value;
                OnPropertyChanged("GameName");
            }
        }

        private ImageSource imageSrc;

        public ImageSource ImageSrc
        {
            get { return imageSrc; }
            set
            {
                if (imageSrc != value)
                {
                    this.OnPropertyChanged("ImageSrc");
                    imageSrc = value;
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        string type;

        public string Type
        {
            get { return type; }
            set
            {
                if (type != value)
                {
                    type = value;
                    this.OnPropertyChanged("Type");
                }
            }
        }       
        
        string date;

        public string Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    this.OnPropertyChanged("Date");
                }
            }
        }
    }
}
