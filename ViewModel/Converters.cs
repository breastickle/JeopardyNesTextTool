using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace JeopardyNesTextTool.ViewModel
{
    public class TerminatedToUnterminatedStringConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value)?.TrimEnd('\r');
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value + '\r';
        }
    }

    public class ShortPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            const int trimLength = 64;
            var fullPath = (string)value;
            if (fullPath is null)
            {
                return string.Empty;
            }
            return fullPath.Length > trimLength ? $"...{fullPath.Substring(fullPath.Length - trimLength)}" : fullPath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BudgetStateToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BudgetState state)
            {
                return state switch
                {
                    BudgetState.Green => Brushes.LimeGreen,
                    BudgetState.Yellow => Brushes.Gold,
                    BudgetState.Orange => Brushes.DarkOrange,
                    BudgetState.Red => Brushes.Crimson,
                    _ => Brushes.Gray
                };
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
