// -----------------------------------------------------------------------
// <copyright file="ShellViewModel.cs" company="">
// Copyright (c) 2011 Bernhard Schwarz, Florian Lembeck
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// -----------------------------------------------------------------------

namespace Handle.WPF
{
  using System.Collections.Generic;
  using System.ComponentModel.Composition;
  using System.Windows;
  using System.Windows.Input;
  using Caliburn.Micro;
  using System.IO.IsolatedStorage;
  using System.IO;
  using Newtonsoft.Json;

  /// <summary>
  /// Represents a ViewModel for ShellViews
  /// </summary>
  [Export(typeof(IShell))]
  public class ShellViewModel : Conductor<object>.Collection.OneActive, IShell
  {
    private WindowState windowState;
    private double left = 500;
    private double top = 50;
    private IrcMainViewModel ircMainViewModel;
    private InputBindings inputBindings;

    public IrcMainViewModel IrcMainViewModel
    {
      get { return this.ircMainViewModel; }
      set { this.ircMainViewModel = value; }
    }
    public Settings Settings { get; set; }

    /// <summary>
    /// Initializes a new instance of the ShellViewModel class
    /// </summary>
    public ShellViewModel()
    {
      this.Left = 10.0;
      this.Top = 100.0;
      this.IrcMainViewModel = new IrcMainViewModel();
      this.Settings = this.deserializeSettings();
      ActivateItem(this.IrcMainViewModel);
    }

    private void Connect(Network network)
    {
      this.IrcMainViewModel.Networks.Add(new IrcNetworkViewModel(network));
    }

    private void ShowNetworkSelection()
    {
      NetworkSelectionViewModel nsvm = new NetworkSelectionViewModel();
      nsvm.ConnectButtonPressed += Connect;
      ActivateItem(nsvm);
    }

    public Thickness WindowStateCorrection
    {
      get
      {
        if (this.IsNormal)
        {
          return new Thickness(7, 2, 0, 0);
        }
        else if (this.IsMaximized)
        {
          return new Thickness(10, 10, 10, 0);
        }

        return new Thickness(0, 0, 0, 0);
      }
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
        NotifyOfPropertyChange(() => this.WindowStateCorrection);
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

    public void ShowSettings()
    {
      var svm = new SettingsViewModel(this.Settings.ShallowCopy());
      svm.SaveButtonPressed += SaveSettings;
      ActivateItem(svm);
    }

    private void SaveSettings(Settings settings)
    {
      this.Settings = settings;
    }

    protected override void OnViewLoaded(object view)
    {
      base.OnViewLoaded(view);
      var window = GetView() as ShellView;
      this.inputBindings = new InputBindings(window);
      inputBindings.RegisterCommands(GetInputBindingCommands());
    }

    protected IEnumerable<InputBindingCommand> GetInputBindingCommands()
    {
      yield return new InputBindingCommand(ShowNetworkSelection)
      {
        GestureModifier = ModifierKeys.Control,
        GestureKey = Key.N
      };
    }

    /// <summary>
    /// Loads settings from a config file in IsolatedStorage.
    /// </summary>
    private Settings deserializeSettings()
    {
      
      var store = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
      IsolatedStorageFileStream isolatedStream;
      isolatedStream = new IsolatedStorageFileStream("settings.json", FileMode.OpenOrCreate, store);
      var settings = JsonConvert.DeserializeObject<Settings>(new StreamReader(isolatedStream).ReadToEnd());
      if (settings == null)
      {
        settings = new Settings();
      }
      isolatedStream.Close();
      return settings;
    }
  }
}
