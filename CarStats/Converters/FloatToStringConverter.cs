using System;
using Windows.UI.Xaml.Data;
using HSDHelper.Models;

namespace HSDHelper.Converters {
    public class DoubleToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var val = (double)value;
            return val.ToString("F1");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new System.NotImplementedException();
        }
    }
    public class HSDStatusToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var val = (CarState.HSDStateEnum)value;
            switch (val) {
                case CarState.HSDStateEnum.None:
                    return "None";
                case CarState.HSDStateEnum.S0:
                    return "S0";
                case CarState.HSDStateEnum.S1:
                    return "S1";
                case CarState.HSDStateEnum.S1A:
                    return "S1A";
                case CarState.HSDStateEnum.End_S1A:
                    return "S1A-end";
                case CarState.HSDStateEnum.S1B:
                    return "S1B";
                case CarState.HSDStateEnum.S2:
                    return "S2";
                case CarState.HSDStateEnum.S3:
                    return "S3";
                case CarState.HSDStateEnum.S4:
                    return "S4";
            }
            return "error";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new System.NotImplementedException();
        }
    }
}