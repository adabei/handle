// -----------------------------------------------------------------------
// <copyright file="POINT.cs" company="">
// Microsoft Public License (Ms-PL)
// http://www.opensource.org/licenses/MS-PL
// </copyright>
// -----------------------------------------------------------------------

namespace Handle.WPF.Native
{
  using System.Runtime.InteropServices;

  [StructLayout(LayoutKind.Sequential)]
  public struct POINT
  {
    public int x;
    public int y;
    public POINT(int x, int y)
    {
      this.x = x;
      this.y = y;
    }
  }
}
