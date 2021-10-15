using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Music.ViewModel;
using System.Windows.Shapes;
using Music.Model;
using Music.Dialogs;
using System.Data;
using Music.Converter;

namespace Music
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel _MusicMenuList;

        public MainWindow()
        {
            InitializeComponent();

            _MusicMenuList = base.DataContext as MainViewModel;

            // 对控件进行数据绑定
            this.AddMusicMenu.DataContext = _MusicMenuList.MusicMenueList;
            // git 测试
            // 刷新
            Refresh_MusicMenuList();


            //定义一个 可观察集合 用来做歌单

            // 监听它增删的过程
            _MusicMenuList.MusicMenueList.CollectionChanged += MusicMenueList_CollectionChanged;

        }

        public void MusicMenueList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // 第一种插入方法： 每次更新 _MusicMenuList.MusicMenueList 后 clear 一下，重新对ui进行渲染
            // 在此之前需要清除 Ui然后 再更新 Refresh_MusicMenuList();

            // 第二种  定位到对改变项， 进行专门的更新

            //更改已存在项目 更、删
            if (e.OldItems != null)
            {
                foreach (MusicMenu item in e.OldItems)
                {
                    this.AddMusicMenu.Children.RemoveAt(e.OldStartingIndex);

                    Console.WriteLine("已删除歌单：" + item.Name);
                }
            }
            // 插入新的歌单 增
            if (e.NewItems != null)
            {
                foreach (MusicMenu item in e.NewItems)
                {
                    AddMusicMenu_TextBlock(e.NewStartingIndex);
                    Console.WriteLine("已添加歌单：" + item.Name);
                }
            }
        }

        // menu 展开于收缩
        private void Tab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Visibility targ = this.Tab.Visibility;
            this.Tab.Visibility = this.Tab1.Visibility;
            this.Tab1.Visibility = targ;
        }

        // 创建一个新的歌单
        private void CreateMenue_Mousedown(object sender, MouseButtonEventArgs e)
        {

            //打开弹窗
            AddMusicMenu addMusicMenu = new AddMusicMenu();
            addMusicMenu.Left = 200;
            addMusicMenu.Top = 200;
            bool? result = addMusicMenu.ShowDialog();
            // 点击确定
            if (result == true && addMusicMenu.label.Text != "")
            {

                // 拿到输入的歌单名称 和 是否为私有
                //Console.WriteLine(addMusicMenu.label.Text);
                //Console.WriteLine(addMusicMenu.IfPrivate.IsChecked);

                MusicMenu NeedAdd = new MusicMenu { Name = addMusicMenu.label.Text, IsMyLove = (bool)addMusicMenu.IfPrivate.IsChecked };
                // 创建新的歌单 并且 插入
                // 不允许插入重复项目
                bool IsRepeat = false;
                foreach (MusicMenu item in _MusicMenuList.MusicMenueList)
                {
                    if (item.Name == NeedAdd.Name)
                    {
                        IsRepeat = true;
                        break;
                    }
                }

                if (!IsRepeat)
                {
                    _MusicMenuList.MusicMenueList.Add(NeedAdd);
                }
                else
                {
                    MessageBox.Show("该歌单以及存在了");
                }
                // 更新 UI

            }

            // 尝试采用可观察集合来进行实现
            //ModifyTextDialog
            //点击然后添加
            //_MusicMenuList.MusicMenueList.Add(new MusicMenu { Name = "新添加的歌单", IsMyLove = false });

            /*
             * MessageBox.Show(_MusicMenuList.MusicMenueList.Count().ToString());
             * 拿到了 List 对 List 进行循环，创建新的 StackPanel  然后插入到队列里去
             */

            // 点击这里 应该弹出一个 窗口  然后对 新表单 进行 赋值操作，完成后 刷新 一下列表




        }

        //Refresh  第一次加载的时候，刷新列表
        private void Refresh_MusicMenuList()
        {
            for (int i = 0; i < _MusicMenuList.MusicMenueList.Count; i++)
            {
                AddMusicMenu_TextBlock(i);
            }
        }

        // 添加歌单函数
        public void AddMusicMenu_TextBlock(int id)
        {
            string MenuName = _MusicMenuList.MusicMenueList[id].Name;
            // 定义一个外标签
            StackPanel stackPanel_MusicList = new StackPanel(); // 外标签
            //设置 标签样式
            //stackPanel_MusicList.Name = "MusicMenu"+_MusicMenuList.MusicMenueList.Count.ToString();
            stackPanel_MusicList.Name = "MusicMenu" + MenuName;  // 为了方便 利用了名字来定义，实际操作利用名称可能会产生乱码而识别不到
            stackPanel_MusicList.Orientation = Orientation.Horizontal;
            stackPanel_MusicList.VerticalAlignment = VerticalAlignment.Center;
            stackPanel_MusicList.Height = 30;
            stackPanel_MusicList.Margin = new Thickness(0, 10, 0, 0);
            stackPanel_MusicList.Style = this.Resources["MouseOver"] as Style;

            // 添加右击属性菜单
            // 可以这样直接进行注册
            //stackPanel_MusicList.ContextMenu = this.Resources["MusicMenu"] as ContextMenu;
            //方法二
            ContextMenu contextMenu = new ContextMenu();
            //重命名
            MenuItem menuItem1 = new MenuItem();
            menuItem1.Name = stackPanel_MusicList.Name;
            menuItem1.Header = "编辑歌单信息";
            menuItem1.Click += ReMusicMenuName;
            contextMenu.Items.Add(menuItem1);
            // 删除  未绑定事件
            MenuItem menuItem2 = new MenuItem();
            menuItem2.Name = stackPanel_MusicList.Name;
            menuItem2.Header = "删除";
            //menuItem2.MouseDown += DeleteMusicMenu;
            contextMenu.Items.Add(menuItem2);
            // 添加为喜欢歌单 未绑定事件
            MenuItem menuItem3 = new MenuItem();
            menuItem3.Name = stackPanel_MusicList.Name;
            menuItem3.Header = "添加为喜欢歌单";
            //menuItem3.MouseDown += DeleteMusicMenu;
            contextMenu.Items.Add(menuItem3);
            // 添加右击属性
            stackPanel_MusicList.ContextMenu = contextMenu;
            // 创建里面的标签
            //icont // 若是自己喜欢的 更换 Icont 图标
            TextBlock TextBlock_Icon = new TextBlock();
            TextBlock_Icon.Style = this.Resources["Iconfont_Front"] as Style;

            // 为 Icont 创建绑定
            Binding Bingdin_Icont = new Binding()
            {
                Source = _MusicMenuList.MusicMenueList[id],// 绑定数据源
                Path = new PropertyPath("IsMyLove"),//需要绑定的数据源属性名称
                Mode = BindingMode.TwoWay,// 绑定模式
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            Bingdin_Icont.Converter = new IsMyLoveConverter();
            BindingOperations.SetBinding(
                TextBlock_Icon, //需要绑定的控件
                TextBlock.TextProperty, //需要绑定的控件属性
                Bingdin_Icont
                );


            // 创建后面的Name
            TextBlock TextBlock_Name = new TextBlock();
            TextBlock_Name.Style = this.Resources["Iconfont_Name"] as Style;
            // 为 MusicMenuName 创建绑定

            // 数据源
            Binding Binding_MusicMenuName = new Binding()
            {
                Source = _MusicMenuList.MusicMenueList[id], // 数据源
                Path = new PropertyPath("Name"),// 需绑定的数据源属性名
                Mode = BindingMode.TwoWay,// 绑定模式
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(
                TextBlock_Name, //需要绑定的控件
                TextBlock.TextProperty, //需要绑定的控件属性
                Binding_MusicMenuName
                );

            //TextBlock_Name.Text = _MusicMenuList.MusicMenueList[0].Name;


            // Dele Icont
            TextBlock TextBlock_DeleIcon = new TextBlock();
            TextBlock_DeleIcon.Name = stackPanel_MusicList.Name;
            TextBlock_DeleIcon.HorizontalAlignment = HorizontalAlignment.Right;
            TextBlock_DeleIcon.Text = "\xe6a6";
            TextBlock_DeleIcon.Style = this.Resources["Iconfont_Front"] as Style;
            // 注册一个事件
            TextBlock_DeleIcon.MouseDown += DeleteMusicMenu;

            // 将创建 好的 歌单 添加到控件中去
            stackPanel_MusicList.Children.Add(TextBlock_Icon);
            stackPanel_MusicList.Children.Add(TextBlock_Name);
            stackPanel_MusicList.Children.Add(TextBlock_DeleIcon);

            this.AddMusicMenu.Children.Add(stackPanel_MusicList);
        }
        private void DeleteMusicMenu(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("是否删除？", "系统提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            // 弹出确定的对话框
            if (dr == MessageBoxResult.OK)
            {
                // 删除 先 集合中的元素  然后 再利用可观察集合 删除ui

                // 获取选中行的ID
                var d = sender as TextBlock;
                // 拿到 Name 了
                //Console.WriteLine(d.Name);
                //进行删除操作
                foreach (StackPanel item in this.AddMusicMenu.Children)
                {
                    if (item.Name == d.Name)
                    {
                        var Children_MusicMenu = item.Children[1] as TextBlock;

                        //Console.WriteLine(Children_MusicMenu.Text);

                        // 删除可观察集合中的元素
                        foreach (MusicMenu item2 in _MusicMenuList.MusicMenueList)
                        {
                            if (item2.Name == Children_MusicMenu.Text)
                            {
                                _MusicMenuList.MusicMenueList.Remove(item2);
                                break;
                            }

                        }
                        // UI 界面上的删除
                        //this.AddMusicMenu.Children.Remove(item);
                        break;
                    }
                }
            }
        }
        // 重命名 操作
        private void ReMusicMenuName(object sender, RoutedEventArgs e)
        {

            MenuItem menu = sender as MenuItem;
            int MenuID = -1;

            // 在可观察集合中进行修改
            for (int i = 0; i < _MusicMenuList.MusicMenueList.Count; i++)
            {
                // 找到这个
                if (("MusicMenu" + _MusicMenuList.MusicMenueList[i].Name) == menu.Name)
                {
                    MenuID = i;
                    // 确定这个歌单 在可观察集合中对 该元素的Name进行修改  
                    break;
                }
            }
            // 创建一个窗口
            EditMusicMenu editMusicMenu = new EditMusicMenu(_MusicMenuList.MusicMenueList[MenuID].Name, (bool)_MusicMenuList.MusicMenueList[MenuID].IsMyLove);
            editMusicMenu.Left = 200;
            editMusicMenu.Top = 200;
            bool? result = editMusicMenu.ShowDialog();
            // 点击确定
            if (result == true && editMusicMenu.label.Text != "")
            {
                _MusicMenuList.MusicMenueList[MenuID].Name = editMusicMenu.label.Text;
                // 修改是否为喜欢对象
                _MusicMenuList.MusicMenueList[MenuID].IsMyLove = (bool)editMusicMenu.IfPrivate.IsChecked;
            }
        }

        // 关闭当前窗口
        private void WindowsClose(object sender, MouseButtonEventArgs e)
        {
            //冒泡事件的路由机制
            // 让响应停下来
            e.Handled = true;

            MessageBoxResult dr = MessageBox.Show("确定退出该应用？", "系统提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (dr == MessageBoxResult.OK)
            {
                // 关闭全部窗体并退出程序
                Application.Current.Shutdown();
            }
        }
        // 窗口最小化
        private void WindowsMin(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        // 窗口最大化 // 全屏
        private void WindowsMax(object sender, MouseButtonEventArgs e)
        {
            // 如果窗口的状态是最大化 则点击化小
            if(this.WindowState == WindowState.Maximized)
            {
                this.Topmost = false;
                this.WindowState = WindowState.Normal;
                this.WindowStyle = WindowStyle.None;

                this.ResizeMode = ResizeMode.CanResizeWithGrip;// 设置为可调整窗体大小
                this.Width = 1075;
                this.Height = 670;
            } else
            {
                this.WindowState = WindowState.Maximized;
            }
        }


        // 按住拖拽
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
