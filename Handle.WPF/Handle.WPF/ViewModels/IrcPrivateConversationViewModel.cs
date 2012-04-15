// -----------------------------------------------------------------------
// <copyright file="IrcPrivateConversationViewModel.cs" company="">
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
  using Caliburn.Micro;
  using IrcDotNet;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class IrcPrivateConversationViewModel : ViewModelBase
  {
    public IrcUser User { get; set; }
    private IrcClient client;
    public BindableCollection<Message> Messages { get; set; }
    public bool Closable { get; set; }

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

    public IrcPrivateConversationViewModel(IrcUser user, IrcClient client, Settings settings)
    {
      this.Settings = settings;
      this.User = user;
      this.client = client;
      this.DisplayName = user.NickName;
      this.Messages = new BindableCollection<Message>();
      this.Closable = true;
      // TODO
      this.client.LocalUser.MessageReceived += this.messageReceived;
    }

    private void messageReceived(object sender, IrcMessageEventArgs e)
    {
      this.Messages.Add(new Message(e.Text,
                              DateTime.Now.ToString(this.Settings.TimestampFormat),
                              e.Source.Name, MessageLevels.Private)); 

    }

    public bool CanSend
    {
      get
      {
        return !string.IsNullOrWhiteSpace(this.Message);
      }
    }

    public void Send()
    {
      this.client.LocalUser.SendMessage(this.User, this.Message);
      this.Messages.Add(new Message(this.Message,
                              DateTime.Now.ToString(this.Settings.TimestampFormat),
                              this.client.LocalUser.NickName, MessageLevels.Private));
      this.Message = string.Empty;
    }

    protected override void OnDeactivate(bool close)
    {
      // Assuming this is correct
      if (close)
      {
        this.client.LocalUser.MessageReceived -= this.messageReceived;
        Console.WriteLine("I'm closing! (" + this.DisplayName + ")");
      }
      base.OnDeactivate(close);
    }
  }
}
