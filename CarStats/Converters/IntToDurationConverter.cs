using System;
using Windows.UI.Xaml.Data;

namespace HSDHelper.Converters {
    public class IntToDurationConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var ret = TimeSpan.FromSeconds((int)value).ToString();
            return ret.StartsWith("00:") ? ret.Substring(3) : ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new System.NotImplementedException();
        }
    }
}