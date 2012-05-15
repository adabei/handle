// -----------------------------------------------------------------------
// <copyright file="LayoutInvalidationCatcher.cs" company="">
// Microsoft Public License (Ms-PL)
// http://www.opensource.org/licenses/MS-PL
// </copyright>
// -----------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace Handle.WPF.Controls
{
  public class LayoutInvalidationCatcher : Decorator
  {
    public Planerator PlaParent
    {
      get { return Parent as Planerator; }
    }

    protected override Size MeasureOverride(Size constraint)
    {
      Planerator pl = PlaParent;
      if (pl != null)
      {
        pl.InvalidateMeasure();
      }
      return base.MeasureOverride(constraint);
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
      Planerator pl = PlaParent;
      if (pl != null)
      {
        pl.InvalidateArrange();
      }
      return base.ArrangeOverride(arrangeSize);
    }
  }
}
