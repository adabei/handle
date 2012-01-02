// -----------------------------------------------------------------------
// <copyright file="NetworkNewViewModel.cs" company="">
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
  using Caliburn.Micro;
  using System.Windows.Input;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class NetworkNewViewModel : ViewModelBase
  {
    private Network network;

    /// <summary>
    /// Initializes a new instance of the NetworkNewViewModel class
    /// </summary>
    public NetworkNewViewModel()
    {
      this.Network = new Network();
    }

    public Network Network
    {
      get
      {
        return this.network;
      }

      set
      {
        this.network = value;
        NotifyOfPropertyChange(() => this.Network);
      }
    }

    public void Ok()
    {
      var nnv = GetView() as NetworkNewView;
      nnv.DialogResult = true;
    }

    public void Cancel()
    {
      var nnv = GetView() as NetworkNewView;
      nnv.DialogResult = false;
    }

    protected override IEnumerable<InputBindingCommand> GetInputBindingCommands()
    {
      yield return new InputBindingCommand(Ok)
      {
        GestureKey = Key.Enter
      };
      yield return new InputBindingCommand(Cancel)
      {
        GestureKey = Key.Escape
      };
    }
  }
}
