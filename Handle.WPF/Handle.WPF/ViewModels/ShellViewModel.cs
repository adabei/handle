namespace Handle.WPF {
  using System.ComponentModel.Composition;
  using System;
  using System.Windows;
  using System.Windows.Input;

  [Export(typeof(IShell))]
  public class ShellViewModel : IShell {

    public void Exit() {
      Application.Current.Shutdown();
    }
  }
}
