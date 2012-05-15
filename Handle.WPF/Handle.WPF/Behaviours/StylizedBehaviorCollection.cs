// -----------------------------------------------------------------------
// <copyright file="StylizedBehaviorCollection.xaml.cs" company="">
// Microsoft Public License (Ms-PL)
// http://www.opensource.org/licenses/MS-PL
// </copyright>
// -----------------------------------------------------------------------

namespace Handle.WPF.Behaviours
{
  using System.Windows;
  using System.Windows.Interactivity;

  public class StylizedBehaviorCollection : FreezableCollection<Behavior>
  {
    protected override Freezable CreateInstanceCore()
    {
      return new StylizedBehaviorCollection();
    }
  }
}
