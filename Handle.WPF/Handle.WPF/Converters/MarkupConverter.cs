﻿namespace Handle.WPF.Converters
{
  using System;
  // -----------------------------------------------------------------------
  // <copyright file="MarkupConverter.cs" company="">
  // Microsoft Public License (Ms-PL)
  // http://www.opensource.org/licenses/MS-PL
  // </copyright>
  // -----------------------------------------------------------------------

  using System.Globalization;
  using System.Windows;
  using System.Windows.Data;
  using System.Windows.Markup;

  [MarkupExtensionReturnType(typeof(IValueConverter))]
  public abstract class MarkupConverter : MarkupExtension, IValueConverter
  {
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return this;
    }

    protected abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
    protected abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

    object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      try
      {
        return Convert(value, targetType, parameter, culture);
      }
      catch
      {
        return DependencyProperty.UnsetValue;
      }
    }

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      try
      {
        return ConvertBack(value, targetType, parameter, culture);
      }
      catch
      {
        return DependencyProperty.UnsetValue;
      }
    }
  }
}
