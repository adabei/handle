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
  using System.Windows.Input;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class IrcNetworkViewModel : ViewModelBase, IHaveClosableTabControl
  {
    private string displayName;

    private IrcClient client;

    public BindableCollection<dynamic> Channels { get; set; }
    public IrcStatusTabViewModel StatusTab { get; set; }

    public bool Closable { get; set; }

    /// <summary>
    /// Initializes a new instance of the IrcNetworkViewModel class
    /// </summary>
    public IrcNetworkViewModel(Network network)
    {
      this.Channels = new BindableCollection<dynamic>();
      this.Closable = true;
      this.DisplayName = network.Name;
      IrcRegistrationInfo info;
      if (network.UseCustomIdentity)
      {
        info = new IrcUserRegistrationInfo()
        {
          NickName = network.Identity.Name ?? Environment.UserName,
          UserName = Environment.UserName,
          RealName = network.Identity.RealName ?? "Rumpelstilzchen",
        };
      }
      else
      {
        Identity id = Identity.GlobalIdentity();
        info = new IrcUserRegistrationInfo()
        {
          NickName = id.Name ?? Environment.UserName,
          UserName = Environment.UserName,
          RealName = id.RealName ?? "Rumpelstilzchen",
        };
      }


      this.Client = new IrcClient();
      this.Client.Registered += this.clientRegistered;

      using (var connectedEvent = new ManualResetEventSlim(false))
      {
        this.Client.Connected += (sender, e) => connectedEvent.Set();
        client.Connect(network.Address, false, info);

        if (!connectedEvent.Wait(1000))
        {
          this.Client.Dispose();
          return;
        }
      }
    }

    private void clientRegistered(object sender, EventArgs e)
    {
      this.Client.LocalUser.JoinedChannel += this.localUserJoinedChannel;
      this.Client.LocalUser.InviteReceived += this.localUserInviteReceived;
      var istvm = new IrcStatusTabViewModel(this.Client);
      istvm.Parent = this;
      istvm.JoinChannelClicked += this.JoinChannel;
      this.Channels.Add(istvm);
    }

    private void localUserInviteReceived(object sender, IrcChannelInvitationEventArgs e)
    {
      // TODO Ask the user before joining the channel
      this.Client.Channels.Join(e.Channel.Name);
    }

    private void localUserJoinedChannel(object sender, IrcChannelEventArgs e)
    {
      var icvm = new IrcChannelViewModel(e.Channel);
      icvm.Parent = this.Parent;
      icvm.JoinChannelClicked += this.JoinChannel;
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

    public void CloseItem(object sender)
    {
      // TODO Leave message
      (sender as IrcChannelViewModel).LeaveChannel();
      this.Channels.Remove(sender as IrcChannelViewModel);
    }

    protected override System.Collections.Generic.IEnumerable<InputBindingCommand> GetInputBindingCommands()
    {
      yield return new InputBindingCommand(JoinChannel)
      {
        GestureModifier = ModifierKeys.Control,
        GestureKey = Key.T
      };
    }

    public void JoinChannel()
    {
      IWindowManager wm;
      var csvm = new ChannelSearchViewModel(this.Client);
      try
      {
        wm = IoC.Get<IWindowManager>();
      }
      catch
      {
        wm = new WindowManager();
      }

      if (wm.ShowDialog(csvm) == true)
      {
        foreach (var item in csvm.Channels)
        {
          this.Client.Channels.Join(item.Name);
        }
      }
    }
  }

}