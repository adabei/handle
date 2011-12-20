﻿namespace Handle.WPF {
  using System;
  using System.ComponentModel.Composition;
  using System.Windows;
  using Caliburn.Micro;
  using System.Windows.Input;

  [Export(typeof(IShell))]
  public class ShellViewModel : Conductor<object>.Collection.OneActive, IShell {
    private WindowState _windowState;
    private double _left = 500;
    private double _top = 50;

    public ShellViewModel() {
      Left = 10.0;
      Top = 100.0;
      ActivateItem(new SettingsViewModel());
    }

    public double Left {
      get { return _left; }
      set {
        _left = value;
        NotifyOfPropertyChange(() => Left);
      }
    }

    public double Top {
      get { return _top; }
      set {
        _top = value;
        NotifyOfPropertyChange(() => Top);
      }
    }

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
  }
}
