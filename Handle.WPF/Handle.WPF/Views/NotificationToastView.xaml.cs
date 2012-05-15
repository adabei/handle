// -----------------------------------------------------------------------
// <copyright file="NotificationToastView.xaml.cs" company="">
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

namespace Handle.WPF
{
  using System;
  using System.Windows;
  using System.Windows.Threading;
  using Handle.WPF.Controls;

  /// <summary>
  /// Interaktionslogik für NotificationToastView.xaml
  /// </summary>
  public partial class NotificationToastView : MetroWindow
  {
    public NotificationToastView()
    {
      InitializeComponent();
      this.Top = SystemParameters.PrimaryScreenHeight - this.Height - 35;
      this.Left = SystemParameters.PrimaryScreenWidth - this.Width;
      this.ShowInTaskbar = false;
      DispatcherTimer dt = new DispatcherTimer();
      dt.Interval = new TimeSpan(0, 0,10);
      dt.IsEnabled = true;
      dt.Tick += delegate(object sender, EventArgs e) { this.Close(); };
    }

    protected void CloseButtonClick(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
