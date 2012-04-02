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

namespace Handle.WPF
{
  /// <summary>
  /// Interaktionslogik für NotificationToastView.xaml
  /// </summary>
  public partial class NotificationToastView : Window
  {
    public NotificationToastView()
    {
      InitializeComponent();
      this.Top = SystemParameters.PrimaryScreenHeight - this.Height;
      this.Left = SystemParameters.PrimaryScreenWidth - this.Width;
      DispatcherTimer mIdle = new DispatcherTimer();
      mIdle.Interval = new TimeSpan(0, 0, 10);
      mIdle.IsEnabled = true;
      mIdle.Tick += Idle_Tick;
    }

    private void btnclose_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    public void Idle_Tick(object sender, EventArgs e) {
      this.Close();
    }
  }
}
