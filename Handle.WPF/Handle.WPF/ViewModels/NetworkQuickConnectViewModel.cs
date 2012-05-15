// -----------------------------------------------------------------------
// <copyright file="NetworkQuickConnectViewModel.cs" company="">
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
  using System.Windows.Input;
  using Caliburn.Micro;

  public class NetworkQuickConnectViewModel : ViewModelBase
  {
    public NetworkQuickConnectViewModel()
    {
      this.GlobalIdentity = Identity.GlobalIdentity();
    }

    public delegate void ConnectEventHandler(Network network);
    public event ConnectEventHandler ConnectButtonPressed;

    public Identity GlobalIdentity { get; set; }

    public override IEnumerable<InputBindingCommand> GetInputBindingCommands()
    {
      yield return new InputBindingCommand(Cancel)
      {
        GestureKey = Key.Escape
      };
    }

    public void Cancel()
    {
      this.TryClose();
    }

    public void Connect()
    {
      var nqcv = this.GetView() as NetworkQuickConnectView;
      Network quick;
      if (nqcv.Network_UseCustomIdentity.IsChecked == true)
      {
        quick = new Network(nqcv.Network_Name.Text, nqcv.Network_Address.Text, false, "",new Identity(nqcv.Network_Identity_Name.Text,"",nqcv.Network_Identity_Alternative.Text,nqcv.Network_Identity_RealName.Text),true);
      }
      else 
      {
        quick = new Network(nqcv.Network_Name.Text, nqcv.Network_Address.Text, false, "", this.GlobalIdentity);
      }
      this.ConnectButtonPressed(quick);
    }
  }
}
