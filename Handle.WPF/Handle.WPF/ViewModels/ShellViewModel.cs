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
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.Composition;
  using System.IO;
  using System.Reflection;
  using System.Windows;
  using System.Windows.Input;
  using Caliburn.Micro;

  /// <summary>
  /// Represents a ViewModel for ShellViews
  /// </summary>
  [Export(typeof(IShell))]
  public class ShellViewModel : ConductorBase<object>, IShell
  {
    private double left = 500;
    private double top = 50;
    private IrcMainViewModel ircMainViewModel;

    public IrcMainViewModel IrcMainViewModel
    {
      get { return this.ircMainViewModel; }
      set { this.ircMainViewModel = value; }
    }

    /// <summary>
    /// Initializes a new instance of the ShellViewModel class
    /// </summary>
    [ImportingConstructor]
    public ShellViewModel(Settings settings)  
    {
      this.Settings = settings;
      this.Left = 10.0;
      this.Top = 100.0;
      this.IrcMainViewModel = new IrcMainViewModel();
      this.IrcMainViewModel.Parent = this;
      //this.IrcMainViewModel.Settings = this.Settings;
      DirectoryInfo di = new DirectoryInfo(Settings.PATH);
      if (!di.Exists)
        di.Create();
      if (this.Settings.CanLog)
      {
        di = new DirectoryInfo(Settings.PATH + @"logs\");
        if (!di.Exists)
          di.Create();
      }

      this.DisplayName = "Handle";
      ActivateItem(this.IrcMainViewModel);
    }

    private void Connect(Network network)
    {
      var invm = new IrcNetworkViewModel(network);
      // invm.Settings = this.Settings;
      invm.Parent = this;
      this.IrcMainViewModel.Items.Add(invm);
    }

    private void ShowNetworkSelection()
    {
      IWindowManager wm;
      var nsvm = new NetworkSelectionViewModel();
      nsvm.ConnectButtonPressed += Connect;
      try
      {
        wm = IoC.Get<IWindowManager>();
      }
      catch
      {
        wm = new WindowManager();
      }

      wm.ShowWindow(nsvm);
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

    public void ShowSettings()
    {
      var svm = new SettingsViewModel(this.Settings.ShallowCopy());
      svm.SaveButtonPressed += SaveSettings;
      ActivateItem(svm);
    }

    private void SaveSettings(Settings settings)
    {
      PropertyUpdater.Update(this.Settings).With(settings);
      this.Settings.Save();
    }

    public override IEnumerable<InputBindingCommand> GetInputBindingCommands()
    {
      yield return new InputBindingCommand(ShowNetworkSelection)
      {
        GestureModifier = ModifierKeys.Control,
        GestureKey = Key.N
      };
    }
  }
}
