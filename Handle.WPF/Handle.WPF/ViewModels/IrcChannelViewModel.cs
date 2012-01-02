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
  using Caliburn.Micro;
  using IrcDotNet;
  using System.Windows.Input;
  using System.Collections.Generic;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class IrcChannelViewModel : Screen
  {
    public IrcChannel IrcChannel { get; set; }

    public BindableCollection<Message> Messages { get; set; }

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

    public bool CanSend
    {
      get
      {
        return !string.IsNullOrWhiteSpace(this.Message);
      }
    }

    /// <summary>
    /// Initializes a new instance of the IrcChannelViewModel class
    /// </summary>
    public IrcChannelViewModel(IrcChannel channel)
    {
      this.Messages = new BindableCollection<Message>();
      this.DisplayName = channel.Name;
      this.IrcChannel = channel;
      this.IrcChannel.MessageReceived += this.channelMessageReceived;
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

    private void channelMessageReceived(object sender, IrcMessageEventArgs e)
    {
      this.Messages.Add(new Message(e.Text, DateTime.Now.ToString("HH:mm"), e.Source.Name));
    }

    public void Send()
    {
      this.IrcChannel.Client.LocalUser.SendMessage(this.IrcChannel, this.Message);
    }

    private void LeaveChannel()
    {
      Console.WriteLine("left");
    }
  }
}
