namespace Handle.WPF {
  using System;
  using System.ComponentModel.Composition;
  using System.Windows;
  using Caliburn.Micro;
  using System.Windows.Input;

  [Export(typeof(IShell))]
  public class ShellViewModel : PropertyChangedBase, IShell {
    private WindowState _windowState;

    public WindowState WindowState {
      get {
        return _windowState;
      }
      set {
        _windowState = value;
        NotifyOfPropertyChange(() => WindowState);
        NotifyOfPropertyChange(() => IsMaximized);
        NotifyOfPropertyChange(() => IsNormal);
      }
    }

    public bool IsNormal {
      get {
        return WindowState == WindowState.Normal;
      }
    }

    public bool IsMaximized {
      get {
        return WindowState == WindowState.Maximized;
      }
    }

    public void Exit() {
      Application.Current.Shutdown();
    }

    public void Minimize() {
      WindowState = WindowState.Minimized;
    }

    public void Maximize() {
      WindowState = WindowState.Maximized;
    }

    public void Restore() {
      WindowState = WindowState.Normal;
    }

    public void HeaderMouseDown(object sender, MouseButtonEventArgs e) {
      if (e.LeftButton == MouseButtonState.Pressed) {
        if (e.ClickCount == 2) {
          if (IsNormal) {
            Maximize();
          }
          else {
            Restore();
          }
        }
      }
    }
  }
}
