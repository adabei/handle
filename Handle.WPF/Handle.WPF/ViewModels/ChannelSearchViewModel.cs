// -----------------------------------------------------------------------
// <copyright file="ChannelSearchViewModel.cs" company="">
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
  using IrcDotNet;
  using System.Text.RegularExpressions;
  using System.Windows.Input;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class ChannelSearchViewModel : ViewModelBase
  {

    private IrcClient ircClient;

    public struct ChannelInfo
    {
      public string Name { get; set; }
      public int? VisibleUserCount { get; set; }
      public string Topic { get; set; }
    }

    public BindableCollection<ChannelInfo> Channels { get; set; }

    public ChannelSearchViewModel(IrcClient ircClient)
    {
      this.ircClient = ircClient;
      this.Channels = new BindableCollection<ChannelInfo>();
      this.ircClient.ChannelListReceived += new EventHandler<IrcChannelListReceivedEventArgs>(ircClient_ChannelListReceived);
    }

    private string pattern;

    public string Pattern
    {
      get
      {
        return this.pattern;
      }
      set
      {
        this.pattern = value;
        NotifyOfPropertyChange(() => Pattern);
        NotifyOfPropertyChange(() => CanJoin);
        NotifyOfPropertyChange(() => CanFilter);
      }
    }

    public bool CanJoin{
      get { return !string.IsNullOrWhiteSpace(Pattern); }
    }

    public bool CanFilter{
      get { return !string.IsNullOrWhiteSpace(Pattern); }
    }

    public void Join()
    {
      this.ircClient.Channels.Join(this.Pattern);
    }

    public void Filter()
    {
      this.ircClient.ListChannels();
    }

    public void Cancel()
    {
      // TODO
      this.TryClose();
    }

    protected void ircClient_ChannelListReceived(object sender, IrcChannelListReceivedEventArgs e)
    {
      this.Channels.Clear();
      foreach (var channelInfo in e.Channels.Where(info => Regex.IsMatch(info.Name, this.Pattern)))
      {
        this.Channels.Add(new ChannelInfo()
        {
          Name = channelInfo.Name,
          VisibleUserCount = channelInfo.VisibleUsersCount,
          Topic = channelInfo.Topic
        });
      }

      NotifyOfPropertyChange(() => this.Channels);
    }

    protected override System.Collections.Generic.IEnumerable<InputBindingCommand> GetInputBindingCommands()
    {
      yield return new InputBindingCommand(Cancel)
      {
        GestureKey = Key.Escape
      };
    }
  }
}