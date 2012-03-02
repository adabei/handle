// -----------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="">
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
  using System.Linq;
  using System.Text;
  using System.Windows;
  using Caliburn.Micro;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class ViewModelBase : Screen, IShell, ISupportShortcuts
  {
    private InputBindings inputBindings;
    private Settings settings;

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

    protected override void OnViewLoaded(object view)
    {
      base.OnViewLoaded(view);

      var window = (view as FrameworkElement).GetWindow() ?? this.GetWindowViewModel(this).GetView() as Window;
      if (window != null)
      {
        this.inputBindings = new InputBindings(window);
        this.inputBindings.RegisterCommands(GetInputBindingCommands());
      }
    }

    public virtual IEnumerable<InputBindingCommand> GetInputBindingCommands()
    {
      yield break;
    }

    protected override void OnDeactivate(bool close)
    {
      base.OnDeactivate(close);
      this.inputBindings.DeregisterCommands();
    }

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

      return GetWindowViewModel(vm.Parent as Screen);
    }
  }
}
