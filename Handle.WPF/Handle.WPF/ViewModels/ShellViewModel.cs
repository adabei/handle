// -----------------------------------------------------------------------
// <copyright file="ShellViewModel.cs" company="">
// Copyright (c) 2011-2012 Bernhard Schwarz, Florian Lembeck
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
  using System.ComponentModel.Composition.Hosting;
  using System.IO;
  using System.Reflection;
  using System.Windows;
  using System.Windows.Input;
  using Caliburn.Micro;
  using System.Windows.Controls;

  /// <summary>
  /// Represents a ViewModel for View of our main Window.
  /// </summary>
  [Export(typeof(IShell))]
  public class ShellViewModel : ConductorBase<object>, IShell
  {
    /// <summary>
    /// The private backing for the IrcMainViewModel property.
    /// </summary>
    private IrcMainViewModel ircMainViewModel;

    public IrcMainViewModel IrcMainViewModel
    {
      get { return this.ircMainViewModel; }
      set { this.ircMainViewModel = value; }
    }

    [Import]
    public FilterService FilterService { get; set; }

    [Import(RequiredCreationPolicy = CreationPolicy.Shared)]
    public IProgressService ProgressService { get; set; }

    /// <summary>
    /// Initializes a new instance of the ShellViewModel class
    /// </summary>
    /// <param name="settings">The Settings singleton, filled in by MEF</param>
    [ImportingConstructor]
    public ShellViewModel(Settings settings)
    {
      this.Settings = settings;

      this.IrcMainViewModel = new IrcMainViewModel();
      this.IrcMainViewModel.Parent = this;

      var svm = new StartupViewModel();
      svm.Parent = this;
 
      if (this.Settings.CanLog)
      {
        DirectoryInfo di = new DirectoryInfo(Settings.PATH + @"logs\");
        if (!di.Exists)
          di.Create();
      }

      this.DisplayName = "Handle";
      ActivateItem(svm);
    }

    private void initializeNotificationProviders()
    {
      lock (this.FilterService.NotificationProviders)
      {
        this.FilterService.NotificationProviders.Clear();
        if (this.Settings.MakeSound)
          this.FilterService.NotificationProviders.Add(new SoundProvider(this.Settings));
        if (this.Settings.TaskbarBlinking)
          this.FilterService.NotificationProviders.Add(new TaskbarBlinkProvider(this));
        if (this.Settings.NotificationToast)
          this.FilterService.NotificationProviders.Add(new ToastProvider(this));
      }
    }

    private void Connect(Network network)
    {
      var invm = new IrcNetworkViewModel(network, this.Settings);
      invm.Parent = this;
      this.IrcMainViewModel.Items.Add(invm);
      if (!this.IrcMainViewModel.IsActive)
      {
        ActivateItem(this.IrcMainViewModel);
      }
      this.initializeNotificationProviders();
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

    public void ShowNetworkQuickConnect()
    {
      IWindowManager wm;
      var nqcvm = new NetworkQuickConnectViewModel();
      nqcvm.ConnectButtonPressed += Connect;
      try
      {
        wm = IoC.Get<IWindowManager>();
      }
      catch
      {
        wm = new WindowManager();
      }
      wm.ShowWindow(nqcvm);
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
      this.FilterService.PopulatePatterns();
      initializeNotificationProviders();
    }

    public override IEnumerable<InputBindingCommand> GetInputBindingCommands()
    {
      yield return new InputBindingCommand(ShowNetworkSelection)
      {
        GestureModifier = ModifierKeys.Control,
        GestureKey = Key.N
      };
      yield return new InputBindingCommand(ShowNetworkQuickConnect)
      {
        GestureModifier = ModifierKeys.Control,
        GestureKey = Key.Q
      };
    }
  }
}
