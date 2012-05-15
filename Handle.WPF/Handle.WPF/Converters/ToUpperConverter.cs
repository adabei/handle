// -----------------------------------------------------------------------
// <copyright file="ToUpperConverter.cs" company="">
// Microsoft Public License (Ms-PL)
// http://www.opensource.org/licenses/MS-PL
// </copyright>
// -----------------------------------------------------------------------

namespace Handle.WPF.Converters
{
  using System;
  using System.Globalization;
  using System.Windows.Data;

  public class ToUpperConverter : MarkupConverter
  {
    protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is string)
        return ((string)value).ToUpper();

      return value;
    }

    protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return Binding.DoNothing;
    }
  }

  public class ToLowerConverter : MarkupConverter
  {
    protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is string)
        return ((string)value).ToLower();

      return value;
    }

    protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return Binding.DoNothing;
    }
  }
}
