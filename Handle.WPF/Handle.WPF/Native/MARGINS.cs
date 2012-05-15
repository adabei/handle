// -----------------------------------------------------------------------
// <copyright file="MARGINS.cs" company="">
// Microsoft Public License (Ms-PL)
// http://www.opensource.org/licenses/MS-PL
// </copyright>
// -----------------------------------------------------------------------

namespace Handle.WPF.Native
{
  using System.Runtime.InteropServices;

  [StructLayout(LayoutKind.Sequential)]
  public struct MARGINS
  {
    public int leftWidth;
    public int rightWidth;
    public int topHeight;
    public int bottomHeight;
  }
}
