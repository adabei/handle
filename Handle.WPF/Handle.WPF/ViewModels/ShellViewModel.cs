﻿namespace Handle.WPF
{
  using System;
  using System.ComponentModel.Composition;
  using System.Windows;
  using System.Windows.Input;
  using System.Windows.Media;
  using Caliburn.Micro;
  using System.IO.IsolatedStorage;
  using System.IO;
  using Newtonsoft.Json;

  [Export(typeof(IShell))]
  public class ShellViewModel : Conductor<object>.Collection.OneActive, IShell
  {
    private WindowState windowState;
    private double left = 500;
    private double top = 50;
    public Settings Settings { get; set; }

    public ShellViewModel()
    {
      this.Left = 10.0;
      this.Top = 100.0;
      initializeSettings();
      ActivateItem(new SettingsViewModel(this.Settings.ShallowCopy()));

    }

    public double Left
    {
      get
      {
        return this.left;
      }

      set
      {
        this.left = value;
        NotifyOfPropertyChange(() => this.Left);
      }
    }

    public double Top
    {
      get
      {
        return this.top;
      }

      set
      {
        this.top = value;
        NotifyOfPropertyChange(() => this.Top);
      }
    }

    public WindowState WindowState
    {
      get
      {
        return this.windowState;
      }

      set
      {
        this.windowState = value;
        NotifyOfPropertyChange(() => this.WindowState);
        NotifyOfPropertyChange(() => this.IsMaximized);
        NotifyOfPropertyChange(() => this.IsNormal);
      }
    }

    public bool IsNormal
    {
      get { return this.WindowState == WindowState.Normal; }
    }

    public bool IsMaximized
    {
      get { return this.WindowState == WindowState.Maximized; }
    }

    public void Exit()
    {
      Application.Current.Shutdown();
    }

    public void Minimize()
    {
      this.WindowState = WindowState.Minimized;
    }

    public void Maximize()
    {
      this.WindowState = WindowState.Maximized;
    }

    public void Restore()
    {
      this.WindowState = WindowState.Normal;
    }

    /// <summary>
    /// Loads settings from a config file in IsolatedStorage.
    /// </summary>
    private void initializeSettings()
    {
      var store = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
      IsolatedStorageFileStream isolatedStream;
      isolatedStream = new IsolatedStorageFileStream("settings.json", FileMode.OpenOrCreate, store);
      this.Settings = JsonConvert.DeserializeObject<Settings>(new StreamReader(isolatedStream).ReadToEnd());
      if (this.Settings == null)
      {
        this.Settings = new Settings();
      }
      isolatedStream.Close();
    }
  }
}