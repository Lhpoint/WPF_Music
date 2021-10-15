using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Music.Converter
{
    public class IsMyLoveConverter : IValueConverter
    {
        // 当值从绑定源传播给绑定目标时，调用方法 Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine(value.ToString());
            if(value.ToString() == "True")
            {
                return "\xe61d";
            }
            else
            {
                return "\xe7fe";
            }
        }
        // 当值从绑定目标传播给绑定源时，调用此方法
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
