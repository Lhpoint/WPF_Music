using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Model
{
    public class MusicMenu : INotifyPropertyChanged
    {
        private string _Name;
        public string Name
        {
            
            get { return _Name; }
            set
        {
                _Name = value;
                //OnPropertyChanged("IsMyLove");
                if (PropertyChanged != null)
                {
                    Console.WriteLine("属性变更被捕获class中");
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));

                }
            }
            
        }

        private bool _IsMyLove;
        public bool IsMyLove
        {
            get { return _IsMyLove; }
            set
            {
                _IsMyLove = value;
                //OnPropertyChanged("IsMyLove");
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsMyLove"));

                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
