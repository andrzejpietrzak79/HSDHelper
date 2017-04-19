using System;
using Windows.UI.Xaml.Data;
using HSDHelper.Services;

namespace HSDHelper.Converters {
    public class ConnectionStateToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            switch ((ConnectionStateEnum)value) {
                case ConnectionStateEnum.Connected:
                    return "Connected";
                case ConnectionStateEnum.Connecting:
                    return "Connecting";
                case ConnectionStateEnum.Initializing:
                    return "Initializing";
                default:
                    return "Disconnected";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new System.NotImplementedException();
        }
    }
}