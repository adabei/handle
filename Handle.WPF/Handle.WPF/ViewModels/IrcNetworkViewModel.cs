// -----------------------------------------------------------------------
// <copyright file="IrcNetworkViewModel.cs" company="">
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
  using System.Threading;
  using Caliburn.Micro;
  using IrcDotNet;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class IrcNetworkViewModel : Screen
  {
    private string displayName;

    private IrcClient client;

    public BindableCollection<IrcChannelViewModel> Channels { get; set; }

    /// <summary>
    /// Initializes a new instance of the IrcNetworkViewModel class
    /// </summary>
    public IrcNetworkViewModel(Network network)
    {
      this.Channels = new BindableCollection<IrcChannelViewModel>();
      this.DisplayName = network.Name;
      IrcRegistrationInfo info = new IrcUserRegistrationInfo()
      {
        NickName = network.Identity.Name,
        UserName = network.Identity.Name,
        RealName = network.Identity.RealName
      };

      this.Client = new IrcClient();
      this.Client.Registered += new EventHandler<EventArgs>(this.clientRegistered);

      using (var connectedEvent = new ManualResetEventSlim(false))
      {
        this.Client.Connected += (sender, e) => connectedEvent.Set();
        client.Connect(network.Address, false, info);

        if (!connectedEvent.Wait(10000))
        {
          this.Client.Dispose();
          return;
        }
      }
    }

    private void clientRegistered(object sender, EventArgs e)
    {
      this.Client.LocalUser.JoinedChannel += localUserJoinedChannel;
      this.Client.Channels.Join("#bots");
    }

    private void localUserJoinedChannel(object sender, IrcChannelEventArgs e)
    {
      var icvm = new IrcChannelViewModel(e.Channel);
      this.Channels.Add(icvm);
    }

    public override string DisplayName
    {
      get
      {
        return this.displayName;
      }

      set
      {
        this.displayName = value;
      }
    }

    public IrcClient Client
    {
      get
      {
        return this.client;
      }

      set
      {
        this.client = value;
      }
    }
  }
}
