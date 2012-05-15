// -----------------------------------------------------------------------
// <copyright file="MINMAXINFO.cs" company="">
// Microsoft Public License (Ms-PL)
// http://www.opensource.org/licenses/MS-PL
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace Handle.WPF.Native
{
  [StructLayout(LayoutKind.Sequential)]
  public struct MINMAXINFO
  {
    public POINT ptReserved;
    public POINT ptMaxSize;
    public POINT ptMaxPosition;
    public POINT ptMinTrackSize;
    public POINT ptMaxTrackSize;
  };
}
