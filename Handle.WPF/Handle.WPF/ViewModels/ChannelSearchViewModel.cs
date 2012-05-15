// -----------------------------------------------------------------------
// <copyright file="ChannelSearchViewModel.cs" company="">
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
  using System.Text.RegularExpressions;
  using System.Windows.Input;
  using Caliburn.Micro;
  using IrcDotNet;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class ChannelSearchViewModel : ViewModelBase
  {

    private IrcClient client;

    public struct BindableChannelInfo
    {
      public string Name { get; set; }
      public int? VisibleUsersCount { get; set; }
      public string Topic { get; set; }
    }

    private BindableCollection<BindableChannelInfo> channels;

    public BindableCollection<BindableChannelInfo> Channels
    {
      get
      {
        return this.channels;
      }
      set
      {
        this.channels = value;
      }
    }
    private List<BindableChannelInfo> allChannels;
    public ChannelSearchViewModel(IrcClient client)
    {
      this.client = client;
      this.allChannels = new List<BindableChannelInfo>();
      this.Channels = new BindableCollection<BindableChannelInfo>();
      this.client.ChannelListReceived += ircClient_ChannelListReceived;
      this.DisplayName = "Join Channel";
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
      var csv = GetView() as ChannelSearchView;
      if (csv.Channels.SelectedIndex == -1)
      {
        this.client.Channels.Join(this.Pattern);
      }
      else
      {
        this.client.Channels.Join(this.Channels[csv.Channels.SelectedIndex].Name);
      }
    }

    public void Filter()
    {
      if (this.allChannels.Count > 0)
      {
        this.Channels.Clear();
        foreach (var channelInfo in this.allChannels.Where(info => Regex.IsMatch(info.Name, this.Pattern)))
        {
          this.Channels.Add(channelInfo);
        }
      }
      else
      {
        this.client.ListChannels();
      }
    }

    public void Cancel()
    {
      // TODO
      this.TryClose();
    }

    protected void ircClient_ChannelListReceived(object sender, IrcChannelListReceivedEventArgs e)
    {
      this.allChannels.Clear();
      foreach (var channelInfo in e.Channels)
      {
        BindableChannelInfo info = new BindableChannelInfo()
        {
          Name = channelInfo.Name,
          VisibleUsersCount = channelInfo.VisibleUsersCount,
          Topic = channelInfo.Topic
        };
        this.allChannels.Add(info);
        if (Regex.IsMatch(channelInfo.Name, this.Pattern, RegexOptions.Compiled))
        {
          this.Channels.Add(info);
        }
      }
    }

    public override System.Collections.Generic.IEnumerable<InputBindingCommand> GetInputBindingCommands()
    {
      yield return new InputBindingCommand(Cancel)
      {
        GestureKey = Key.Escape
      };
      yield return new InputBindingCommand(Filter)
      {
        GestureKey = Key.Enter,
        GestureModifier = ModifierKeys.Shift
      };
      yield return new InputBindingCommand(Join)
      {
        GestureKey = Key.Enter
      };
    }
  }
}