// -----------------------------------------------------------------------
// <copyright file="MONITORINFO.cs" company="">
// Microsoft Public License (Ms-PL)
// http://www.opensource.org/licenses/MS-PL
// </copyright>
// -----------------------------------------------------------------------


namespace Handle.WPF.Native
{
  using System.Runtime.InteropServices;

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  public class MONITORINFO
  {
    public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
    public RECT rcMonitor = new RECT();
    public RECT rcWork = new RECT();
    public int dwFlags = 0;
  }
}
