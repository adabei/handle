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
    }

    private void btnclose_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
