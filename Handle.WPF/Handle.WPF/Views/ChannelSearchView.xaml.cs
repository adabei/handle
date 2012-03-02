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
using Handle.WPF.Controls;

namespace Handle.WPF
{
  /// <summary>
  /// Interaction logic for ChannelSearchView.xaml
  /// </summary>
  public partial class ChannelSearchView : MetroWindow
  {
    public ChannelSearchView()
    {
      InitializeComponent();
    }

    protected override void OnInitialized(EventArgs e)
    {
      base.OnInitialized(e);
      this.Pattern.Focus();
    }
  }
}
