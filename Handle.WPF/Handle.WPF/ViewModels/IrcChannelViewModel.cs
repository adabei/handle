// -----------------------------------------------------------------------
// <copyright file="IrcChannelViewModel.cs" company="">
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
  using System.IO;
  using System.Text;
  using System.Windows.Input;
  using Caliburn.Micro;
  using IrcDotNet;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class IrcChannelViewModel : ViewModelBase
  {
    public delegate void JoinChannelClickedEventHandler();
    public event JoinChannelClickedEventHandler JoinChannelClicked;

    public IrcChannel Channel { get; set; }

    public BindableCollection<Message> Messages { get; set; }

    private Logger logger;
    private string message;
    public string Message
    {
      get
      {
        return this.message;
      }
      set
      {
        this.message = value;
        NotifyOfPropertyChange(() => this.Message);
        NotifyOfPropertyChange(() => this.CanSend);
      }
    }
    private string topic;

    public string Topic
    {
      get
      {
        return topic;
      }
      set
      {
        this.topic = value;
        NotifyOfPropertyChange(() => this.Topic);
      }
    }

    public bool CanSend
    {
      get
      {
        return !string.IsNullOrWhiteSpace(this.Message);
      }
    }

    public bool Closable { get; set; }

    /// <summary>
    /// Initializes a new instance of the IrcChannelViewModel class
    /// </summary>
    public IrcChannelViewModel(IrcChannel channel, string networkName, Settings settings)
    {
      this.Settings = settings;
      this.Messages = new BindableCollection<Message>();
      this.Closable = true;
      this.DisplayName = channel.Name;
      this.Channel = channel;
      this.Channel.ModesChanged += this.channelModesChanged;
      this.Channel.UsersListReceived += this.channelUsersListReceived;
      this.Channel.MessageReceived += this.channelMessageReceived;
      this.Channel.UserJoined += this.channelUserJoined;
      this.Channel.UserLeft += this.channelUserLeft;
      this.Channel.NoticeReceived += this.channelNoticeReceived;
      this.Channel.TopicChanged += this.channelTopicChanged;
      DirectoryInfo di = new DirectoryInfo(Settings.PATH + "\\logs\\");
      if (!di.Exists)
        di.Create();
      if (this.Settings.CanLog)
        this.logger = new Logger(String.Format("{0}\\logs\\{1}.{2}.txt", 
                                 Settings.PATH, 
                                 channel.Name, 
                                 networkName));

      this.Channel.GetTopic();
    }

    private void channelUsersListReceived(object sender, EventArgs e)
    {
      StringBuilder sb = new StringBuilder();
      foreach (var user in this.Channel.Users)
      {
        sb.Append(user.User.NickName + " | ");
      }

      if (sb[sb.Length - 2] == '|') 
        sb.Length -= 3;
      this.Users = sb.ToString();
    }

    private void channelModesChanged(object sender, EventArgs e)
    {
    }

    private string users;

    public string Users
    {
      get
      {
        return this.users;
      }
      set
      {
        this.users = value;
        NotifyOfPropertyChange(() => this.Users);
      }
    }

    private string displayName;

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

    private void channelTopicChanged(object sender, EventArgs e)
    {
      this.Topic = this.Channel.Topic;
    }

    private void channelMessageReceived(object sender, IrcMessageEventArgs e)
    {
      Message m = new Message(e.Text,
                              DateTime.Now.ToString(this.Settings.TimestampFormat),
                              e.Source.Name);
      this.Messages.Add(m);
      if (this.Settings.CanLog && this.logger != null)
        logger.Append(String.Format("{0} {1}: {2}",
                                    m.Received,
                                    m.Sender,
                                    m.Text));
    }

    private void channelNoticeReceived(object sender, IrcMessageEventArgs e)
    {
      this.Messages.Add(new Message(e.Text, DateTime.Now.ToString(this.Settings.TimestampFormat), "=!="));
    }

    private void channelUserJoined(object sender, IrcChannelUserEventArgs e)
    {
      this.Messages.Add(new Message(String.Format("{0} [{1}] has joined {2}",
                                                  e.ChannelUser.User.NickName,
                                                  e.ChannelUser.User.HostName,
                                                  e.ChannelUser.Channel.Name),
                                    DateTime.Now.ToString(this.Settings.TimestampFormat),
                                    "=!="));
    }

    private void channelUserLeft(object sender, IrcChannelUserEventArgs e)
    {
      this.Messages.Add(new Message(String.Format("{0} [{1}] has left {2} [{3}]",
                                                  e.ChannelUser.User.NickName,
                                                  e.ChannelUser.User.HostName,
                                                  e.ChannelUser.Channel.Name,
                                                  e.Comment),
                                    DateTime.Now.ToString(this.Settings.TimestampFormat),
                                    "=!="));
    }

    public void Send()
    {
      this.Channel.Client.LocalUser.SendMessage(this.Channel, this.Message);
      Message m = new Message(this.Message.Trim(),
                              DateTime.Now.ToString(this.Settings.TimestampFormat),
                              this.Channel.Client.LocalUser.NickName);
      this.Messages.Add(m);
      if (this.Settings.CanLog && this.logger != null)
        logger.Append(String.Format("{0} {1}: {2}", m.Received, m.Sender, m.Text));
      this.Message = String.Empty;
    }

    public void LeaveChannel(string message)
    {
      // Unsubscribe from all events
      this.Channel.ModesChanged -= this.channelModesChanged;
      this.Channel.UsersListReceived -= this.channelUsersListReceived;
      this.Channel.MessageReceived -= this.channelMessageReceived;
      this.Channel.UserJoined -= this.channelUserJoined;
      this.Channel.UserLeft -= this.channelUserLeft;
      this.Channel.NoticeReceived -= this.channelNoticeReceived;
      this.Channel.TopicChanged -= this.channelTopicChanged;

      this.Channel.Leave(message);

      if (this.Settings.CanLog && this.logger != null)
        this.logger.Dispose();
    }

    public void LeaveChannel()
    {
      this.LeaveChannel(string.Empty);
    }

    public void JoinChannel()
    {
      if (this.JoinChannelClicked != null)
        this.JoinChannelClicked();
    }

    public override System.Collections.Generic.IEnumerable<InputBindingCommand> GetInputBindingCommands()
    {
      yield return new InputBindingCommand(LeaveChannel)
      {
        GestureModifier = ModifierKeys.Control,
        GestureKey = Key.W
      };
    }

    public void OpenContextMenu() 
    {
      var view = GetView() as IrcChannelView;
      view.CoMenu.PlacementTarget = view;
      view.CoMenu.IsOpen = true;
    }

    public void ClearMessages() 
    {
      this.Messages.Clear();
    }

    public void SaveMessages() 
    {
      var nsv = GetView() as IrcChannelView;
      Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
      dlg.DefaultExt = ".txt";
      dlg.Filter = "TXT (.txt)|*.txt";
      if (dlg.ShowDialog() == true)
      {
        string filename = dlg.FileName;
        FileStream fs = new FileStream(filename, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        try
        {
          foreach (Message m in this.Messages) 
          {
            sw.WriteLine(m.Received + ":" + m.Sender + ":" + m.Text);
          }
        }
        finally
        {
          sw.Close();
          fs.Close();
        }
      }   
    }
  }
}