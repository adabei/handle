﻿namespace Handle.WPF
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Data;
  using System.Windows.Documents;
  using System.Windows.Input;
  using System.Windows.Interop;
  using System.Windows.Media;
  using System.Windows.Media.Imaging;
  using System.Windows.Shapes;

  /// <summary>
  /// Interaction logic for ShellView.xaml
  /// </summary>
  public partial class ShellView : Window
  {
    public ShellView()
    {
      InitializeComponent();
    }

    public void HeaderAreaRectangle_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed)
      {
        DragMove();
        if (e.ClickCount == 2)
        {
          if (WindowState == WindowState.Normal)
          {
            WindowState = WindowState.Maximized;
          }
          else
          {
            WindowState = WindowState.Normal;
          }
        }
      }
    }
  }
}