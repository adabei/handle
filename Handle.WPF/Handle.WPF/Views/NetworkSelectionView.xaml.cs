// -----------------------------------------------------------------------
// <copyright file="NetworkSelectionView.xaml.cs" company="">
// Copyright (c) 2011 Bernhard Schwarz, Florian Lembeck
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
  using System.Windows.Navigation;
  using System.Windows.Shapes;
  using Handle.WPF.Controls;
using System.ComponentModel;

  /// <summary>
  /// Interaction logic for NetworkSelectionView.xaml
  /// </summary>
  public partial class NetworkSelectionView : MetroWindow
  {
    private GridViewColumnHeader _CurSortCol = null;
    private SortAdorner _CurAdorner = null;

    /// <summary>
    /// Initializes a new instance of the NetworkSelectionView class.
    /// </summary>
    public NetworkSelectionView()
    {
      InitializeComponent();
    }

    private void SortClick(object sender, RoutedEventArgs e)
    {
      GridViewColumnHeader column = sender as GridViewColumnHeader;
      String field = column.Tag as String;

      if (_CurSortCol != null)
      {
        AdornerLayer.GetAdornerLayer(_CurSortCol).Remove(_CurAdorner);
        NetworkList.Items.SortDescriptions.Clear();
      }

      ListSortDirection newDir = ListSortDirection.Ascending;
      if (_CurSortCol == column && _CurAdorner.Direction == newDir)
        newDir = ListSortDirection.Descending;

      _CurSortCol = column;
      _CurAdorner = new SortAdorner(_CurSortCol, newDir);
      AdornerLayer.GetAdornerLayer(_CurSortCol).Add(_CurAdorner);
      NetworkList.Items.SortDescriptions.Add(
          new SortDescription(field, newDir));
    }
  }

  public class SortAdorner : Adorner
  {
    private readonly static Geometry _AscGeometry =
        Geometry.Parse("M 0,0 L 10,0 L 5,5 Z");
    private readonly static Geometry _DescGeometry =
        Geometry.Parse("M 0,5 L 10,5 L 5,0 Z");

    public ListSortDirection Direction { get; private set; }

    public SortAdorner(UIElement element, ListSortDirection dir)
      : base(element)
    { Direction = dir; }

    protected override void OnRender(DrawingContext drawingContext)
    {
      base.OnRender(drawingContext);

      if (AdornedElement.RenderSize.Width < 20)
        return;

      drawingContext.PushTransform(
          new TranslateTransform(
            AdornedElement.RenderSize.Width - 15,
            (AdornedElement.RenderSize.Height - 5) / 2));

      drawingContext.DrawGeometry(Brushes.Black, null,
          Direction == ListSortDirection.Ascending ?
            _AscGeometry : _DescGeometry);

      drawingContext.Pop();
    }
  }
}
