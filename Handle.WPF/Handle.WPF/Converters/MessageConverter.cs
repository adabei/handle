// -----------------------------------------------------------------------
// <copyright file="StringToMessageConverter.cs" company="">
// Copyright (c) 2011-2012 Bernhard Schwarz, Florian Lembeck
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// -----------------------------------------------------------------------

namespace Handle.WPF.Converters
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Text.RegularExpressions;
  using System.Windows.Controls;
  using System.Windows.Data;
  using System.Windows.Documents;
  using System.Windows.Media;
  using Caliburn.Micro;
  using System.Windows;

  /// <summary>
  /// Represents a concrete ValueConverter to convert strings, possibly containing links, to TextBlocks
  /// </summary>
  public class MessageConverter : IValueConverter
  {
    private readonly bool displayLinks;
    private readonly FilterService filterService;
    private const string uriPattern = @"^(?i)\b((?:https?://|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'"".,<>?«»“”‘’]))$";

    public MessageConverter()
      : base()
    {
      try
      {
        this.displayLinks = IoC.Get<Settings>().DisplayURLAsLink;
        this.filterService = IoC.Get<FilterService>();
      }
      catch
      {
        this.displayLinks = true;
        this.filterService = new FilterService();
      }
    }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      string text = (value as Handle.WPF.Message).Text;
      List<Inline> inlines = new List<Inline>();
      Hyperlink hl;
      foreach (string word in Regex.Split(text, @"(?=(?<=[^\s])\s+)"))
      {
        if (!Regex.IsMatch(word, uriPattern, RegexOptions.Compiled) || !displayLinks)
        {
          inlines.Add(new Run(word));
        }
        else
        {
          hl = new Hyperlink();
          try
          {
            hl.NavigateUri = new Uri(word);
          }
          catch
          {
            hl.NavigateUri = new Uri("http://" + word);
          }

          hl.RequestNavigate += delegate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
          {
            System.Diagnostics.Process.Start(e.Uri.OriginalString);
          };

          hl.Inlines.Add(word);
          inlines.Add(hl);
        }
      }

      TextBlock tb = new TextBlock();
      var msg = value as Handle.WPF.Message;
      foreach (var inline in inlines)
      {
        if (inline is Run)
        {
          if (msg.Levels.Contains(MessageLevels.Highlight))
          {
            inline.FontWeight = FontWeights.DemiBold;
          }
          else if (msg.Levels.Contains(MessageLevels.Join))
          {
            inline.Foreground = Brushes.Green;
          }
          else if (msg.Levels.Contains(MessageLevels.Part))
          {
            inline.Foreground = Brushes.Red;
          }
        }

        tb.Inlines.Add(inline);
        tb.Inlines.Add(" ");
      }
      tb.TextWrapping = System.Windows.TextWrapping.Wrap;
      return tb;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      StringBuilder stripped = new StringBuilder();
      foreach (var inline in (value as TextBlock).Inlines)
      {
        if (inline is Run)
        {
          stripped.Append((inline as Run).Text);
        }
        else
        {
          stripped.Append(((inline as Hyperlink).Inlines.FirstInline as Run).Text);
        }
      }
      return stripped.ToString();
    }
  }
}