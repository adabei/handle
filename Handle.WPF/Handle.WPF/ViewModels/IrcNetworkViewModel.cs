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
  using System.Collections.Generic;
  using System.ComponentModel.Composition;
  using System.ComponentModel.Composition.Hosting;
  using System.Threading;
  using System.Windows.Input;
  using Caliburn.Micro;
  using IrcDotNet;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class IrcNetworkViewModel : ConductorBase<dynamic>, IHaveClosableTabControl
  {
    private IrcClient client;

    public bool Closable { get; set; }

    public IProgressService ProgressService { get; set; }

    /// <summary>
    /// Initializes a new instance of the IrcNetworkViewModel class
    /// </summary>
    public IrcNetworkViewModel(Network network, Settings settings)
    {
      this.Closable = true;
      this.DisplayName = network.Name;
      this.Settings = settings;
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
      this.ProgressService = IoC.Get<IProgressService>();

      this.Client = new IrcClient();
      this.Client.Registered += this.clientRegistered;

      // TODO Display Popup
      this.Client.ConnectFailed += delegate(object sender, IrcErrorEventArgs e)
      {
        this.ProgressService.Hide();
        Console.WriteLine("Couldn't connect to server");
      };

      using (var connectedEvent = new ManualResetEventSlim(false))
      {
        this.Client.Connected += (sender, e) => connectedEvent.Set();
        this.ProgressService.Show();
        client.Connect(network.Address, false, info);

        if (!connectedEvent.Wait(1000))
        {
          this.Client.Dispose();
          return;
        }
      }
    }

    private void localUserMessageReceived(object sender, IrcMessageEventArgs e)
    {
      IrcPrivateConversationViewModel ipcvm = null;
      foreach (var item in this.Items)
      {
        if (item.DisplayName == e.Source.Name)
        {
          ipcvm = item;
        }
      }

      if (ipcvm == null)
      {
        ipcvm = new IrcPrivateConversationViewModel(e.Source as IrcUser, this.Client, this.Settings);
        this.Items.Add(ipcvm);
        ipcvm.Messages.Add(new Message(e.Text,
                              DateTime.Now.ToString(this.Settings.TimestampFormat),
                              e.Source.Name, MessageLevels.Private));
      }
    }

    private void clientRegistered(object sender, EventArgs e)
    {
      this.ProgressService.Hide();
      this.Client.LocalUser.JoinedChannel += this.localUserJoinedChannel;
      this.Client.LocalUser.InviteReceived += this.localUserInviteReceived;
      var istvm = new IrcStatusTabViewModel(this.Client);
      istvm.Parent = this;
      istvm.Settings = this.Settings;
      istvm.JoinChannelClicked += this.JoinChannel;
      this.Client.LocalUser.MessageReceived += this.localUserMessageReceived;
      this.Items.Add(istvm);
    }

    private void localUserInviteReceived(object sender, IrcChannelInvitationEventArgs e)
    {
      // TODO Ask the user before joining the channel
      this.Client.Channels.Join(e.Channel.Name);
    }

    private void localUserJoinedChannel(object sender, IrcChannelEventArgs e)
    {
      var icvm = new IrcChannelViewModel(e.Channel, this.DisplayName, this.Settings);
      icvm.Parent = this;
      icvm.JoinChannelClicked += this.JoinChannel;
      this.Items.Add(icvm);
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
      string message = null;
      try
      {
        message = this.Settings.PartMessage;
      }
      catch
      {
        message = string.Empty;
      }

      if (sender is IrcChannelViewModel)
      {
        (sender as IrcChannelViewModel).LeaveChannel(message);
      }

      this.Items.Remove(sender);
    }

    public override IEnumerable<InputBindingCommand> GetInputBindingCommands()
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
      csvm.Parent = this;
      csvm.Settings = this.Settings;
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