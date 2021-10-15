using Music.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // IsMyLove
        private bool _isMyLove;

        public bool IsMyLove
        {
            get { return _isMyLove; }
            set
            {
                _isMyLove = value;
                //OnPropertyChanged("IsMyLove");
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsMyLove"));

                }
            }
        }

        // Name
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                //OnPropertyChanged("Name");
                if (PropertyChanged != null)
                {
                    Console.WriteLine("V-sModel中属性变更被捕获");
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));

                }
            }
        }

        //  创建一个可观察集合
        //private ObservableCollection<MusicMenu> _musicMenueList;
        public ObservableCollection<MusicMenu> MusicMenueList;
        //{
        //    get
        //    {
        //        return _musicMenueList;
        //    }
        //    set
        //    {
        //        _musicMenueList = value;
        //        if (_musicMenueList != null)
        //        {
        //            _musicMenueList.CollectionChanged += MusicMenueList_CollectionChanged;
        //        }
        //        OnPropertyChanged("MusicMenueList");
        //    }
            
        //}
        //public void MusicMenueList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    // 第一种插入方法： 每次更新 _MusicMenuList.MusicMenueList 后 clear 一下，重新对ui进行渲染
        //    // 在此之前需要清除 Ui然后 再更新 Refresh_MusicMenuList();

        //    // 第二种  定位到对改变项， 进行专门的更新

        //    //更改已存在项目 更、删
        //    if (e.OldItems != null)
        //    {
        //        foreach (MusicMenu item in e.OldItems)
        //        {
        //            Console.WriteLine("已删除歌单：" + item.Name);
        //        }
        //    }
        //    // 插入新的歌单 增
        //    if (e.NewItems != null)
        //    {
        //        foreach (MusicMenu item in e.NewItems)
        //        {
        //            AddMusicMenu_TextBlock(item.Name);
        //            Console.WriteLine("已添加歌单：" + item.Name);
        //        }
        //    }
        //}

        private void AddMusicMenu_TextBlock(string name)
        {
            throw new NotImplementedException();
        }



        // 先做增删呗

        public event PropertyChangedEventHandler PropertyChanged;
        
        //属性变更通知
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                Console.WriteLine("属性变更被监听到了");
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        // 构造函数
        public MainViewModel()
        {
            //  初始化默认的内容
            var NewMenu = new ObservableCollection<MusicMenu>();
            // 添加一个新的菜单
            NewMenu.Add( new MusicMenu { Name="原有有菜单1",IsMyLove = false} );
            NewMenu.Add( new MusicMenu { Name="原有的菜单2",IsMyLove = true} );
            MusicMenueList = NewMenu;
        }

    }
}
