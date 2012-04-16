using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Handle.WPF.Controls;

namespace Handle.WPF
{
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
