// -----------------------------------------------------------------------
// <copyright file="InputBindingCommand.cs" company="">
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
  using System.Windows.Input;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class InputBindingCommand : ICommand
  {
#pragma warning disable // never used
    public event EventHandler CanExecuteChanged;
#pragma warning restore

    private readonly Action<object> executeDelegate;
    private Func<object, bool> canExecutePredicate;

    public Key GestureKey { get; set; }
    public ModifierKeys GestureModifier { get; set; }
    public MouseAction MouseGesture { get; set; }

    public bool CanExecute(object parameter)
    {
      return canExecutePredicate(parameter);
    }

    public InputBindingCommand(Action executeDelegate)
    {
      this.executeDelegate = x => executeDelegate();
      this.canExecutePredicate = x => true;
    }

    public InputBindingCommand(Action<object> executeDelegate)
    {
      this.executeDelegate = executeDelegate;
      this.canExecutePredicate = x => true;
    }

    public void Execute(object parameter)
    {
      this.executeDelegate(parameter);
    }

    public InputBindingCommand If(Func<bool> canExecutePredicate)
    {
      this.canExecutePredicate = x => canExecutePredicate();

      return this;
    }

    public InputBindingCommand If(Func<object, bool> canExecutePredicate)
    {
      this.canExecutePredicate = canExecutePredicate;

      return this;
    }
  }
}
