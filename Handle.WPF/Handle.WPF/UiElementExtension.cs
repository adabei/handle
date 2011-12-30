// -----------------------------------------------------------------------
// <copyright file="UiElementExtension.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Handle.WPF
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Windows;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public static class UiElementExtension
  {
    public static Window GetWindow(this FrameworkElement element)
    {
      if (element == null)
        return null;

      if (element is Window)
        return (Window)element;

      if (element.Parent == null)
        return null;

      return GetWindow(element.Parent as FrameworkElement);
    }
  }
}
