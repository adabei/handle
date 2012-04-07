// -----------------------------------------------------------------------
// <copyright file="NickConverter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Handle.WPF.Converters
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Windows.Data;
  using IrcDotNet;
  using IrcDotNet.Collections;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class NickConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      var nick = (value as IrcChannelUser).User.NickName;
      var modes = (value as IrcChannelUser).Modes;

      if (modes.Contains('o'))
      {
        return '@' + nick;
      }
      else if (modes.Contains('v'))
      {
        return '+' + nick;
      }
      else
      {
        return nick;
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
