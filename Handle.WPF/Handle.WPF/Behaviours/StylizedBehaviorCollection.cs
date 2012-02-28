using System.Windows;
using System.Windows.Interactivity;

namespace Handle.WPF.Behaviours
{
  public class StylizedBehaviorCollection : FreezableCollection<Behavior>
  {
    protected override Freezable CreateInstanceCore()
    {
      return new StylizedBehaviorCollection();
    }
  }
}
