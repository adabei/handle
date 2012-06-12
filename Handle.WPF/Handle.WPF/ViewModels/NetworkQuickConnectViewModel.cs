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
  using System.Collections.Generic;
  using System.Windows.Input;

  public class NetworkQuickConnectViewModel : ViewModelBase
  {
    private readonly bool forceCustomIdentity;
    private bool useCustomIdentity;

    public bool UseCustomIdentity
    {
      get
      {
        if (this.forceCustomIdentity)
        {
          return true;
        }
        else
        {
          return this.useCustomIdentity;
        }
      }
      set
      {
        this.useCustomIdentity = value;
        this.NotifyOfPropertyChange(() => this.UseCustomIdentity);
      }
    }

    public string NetworkName { get; set; }
    public string NetworkAddress { get; set; }

    public string IdentityName { get; set; }
    public string IdentityAlternative { get; set; }
    public string IdentityRealName { get; set; }

    public delegate void ConnectEventHandler(Network network);
    public event ConnectEventHandler ConnectButtonPressed;

    public NetworkQuickConnectViewModel()
    {
      this.NetworkName = string.Empty;
      this.NetworkAddress = string.Empty;

      this.IdentityName = string.Empty;
      this.IdentityAlternative = string.Empty;
      this.IdentityRealName = string.Empty;

      this.UseCustomIdentity = this.forceCustomIdentity = Identity.GlobalIdentity() == null;
    }

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
      if (this.UseCustomIdentity)
      {
        this.ConnectButtonPressed(new Network(this.NetworkName,
                                              this.NetworkAddress,
                                              false, string.Empty,
                                              new Identity(this.IdentityName, string.Empty,
                                                           this.IdentityAlternative,
                                                           this.IdentityRealName),
                                              true));
      }
      else
      {
        this.ConnectButtonPressed(new Network(this.NetworkName, this.NetworkAddress, false, string.Empty, Identity.GlobalIdentity(), false));
      }
    }
  }
}
