using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace MidiKeyboardSoundboard.Converters
{
    public class DictionaryItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dict = value as Dictionary<string, bool>;
            if (dict != null)
            {
                return dict[parameter as string];
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
