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
using System.Windows.Shapes;

namespace Music.Dialogs
{
    /// <summary>
    /// AddMusicMenu.xaml 的交互逻辑
    /// </summary>
    public partial class AddMusicMenu : Window
    {
        public AddMusicMenu()
        {
            InitializeComponent();
        }
        //  取消按钮响应
        private void LabelESC(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void LabelOK(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
