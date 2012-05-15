// -----------------------------------------------------------------------
// <copyright file="MetroImage.cs" company="">
// Microsoft Public License (Ms-PL)
// http://www.opensource.org/licenses/MS-PL
// </copyright>
// -----------------------------------------------------------------------

namespace Handle.WPF.Controls
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Media;

  public class MetroImage : Control
  {
    public MetroImage()
    {
      DefaultStyleKey = typeof(MetroImage);
    }

    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Visual), typeof(MetroImage), new PropertyMetadata(default(Visual)));

    public Visual Source
    {
      get { return (Visual)GetValue(SourceProperty); }
      set { SetValue(SourceProperty, value); }
    }
  }
}
