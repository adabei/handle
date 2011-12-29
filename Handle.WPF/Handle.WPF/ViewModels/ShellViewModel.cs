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
  using System.ComponentModel.Composition;
  using System.Windows;
  using System.Windows.Input;
  using System.Windows.Media;
  using Caliburn.Micro;

  /// <summary>
  /// Represents a ViewModel for ShellViews
  /// </summary>
  [Export(typeof(IShell))]
  public class ShellViewModel : Conductor<object>.Collection.OneActive, IShell
  {
    private WindowState windowState;
    private double left = 500;
    private double top = 50;

    /// <summary>
    /// Initializes a new instance of the ShellViewModel class
    /// </summary>
    public ShellViewModel()
    {
      this.Left = 10.0;
      this.Top = 100.0;
      ActivateItem(new IrcMainViewModel());
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

    public void DisplaySettings()
    {
      ActivateItem(new SettingsViewModel());
    }
  }
}