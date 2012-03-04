// -----------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="">
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
  using System.Linq;
  using System.Text;
  using System.Windows;
  using Caliburn.Micro;

  /// <summary>
  /// Represents a base class for all Screen-like ViewModels
  /// </summary>
  public class ViewModelBase : Screen, IShell, ISupportShortcuts
  {
    /// <summary>
    /// To keep track of the defined shortcuts
    /// </summary>
    private InputBindings inputBindings;

    /// <summary>
    /// Private backing for the Settings property
    /// </summary>
    private Settings settings;

    /// <summary>
    /// Gets or sets the settings
    /// </summary>
    public Settings Settings
    {
      get
      {
        return this.settings;
      }

      set
      {
        this.settings = value;
        NotifyOfPropertyChange(() => this.Settings);
      }
    }

    /// <summary>
    /// Let's you define your shortcuts.
    /// </summary>
    /// <returns>Defined shortcuts</returns>
    public virtual IEnumerable<InputBindingCommand> GetInputBindingCommands()
    {
      yield break;
    }

    /// <summary>
    /// Deregisters all commands upon deactivating.
    /// </summary>
    /// <param name="close">Whether to close the ViewModel or not</param>
    protected override void OnDeactivate(bool close)
    {
      base.OnDeactivate(close);
      this.inputBindings.DeregisterCommands();
    }

    /// <summary>
    /// Register all shortcuts on the View
    /// </summary>
    /// <param name="view">The corosponding view</param>
    protected override void OnViewLoaded(object view)
    {
      base.OnViewLoaded(view);

      var window = (view as FrameworkElement).GetWindow() ?? this.GetWindowViewModel(this).GetView() as Window;
      if (window != null)
      {
        this.inputBindings = new InputBindings(window);
        this.inputBindings.RegisterCommands(this.GetInputBindingCommands());
      }
    }

    /// <summary>
    /// Recurisve method to obtain a Window, where we can set our shortcuts.
    /// </summary>
    /// <param name="vm">Previously tried ViewModel</param>
    /// <returns>A Window ViewModel</returns>
    protected Screen GetWindowViewModel(dynamic vm)
    {
      if (vm == null)
      {
        return null;
      }

      if (vm.GetView() is Window)
      {
        return vm;
      }

      if (vm.Parent == null)
      {
        return null;
      }

      return this.GetWindowViewModel(vm.Parent as Screen);
    }
  }
}
