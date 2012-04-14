// -----------------------------------------------------------------------
// <copyright file="NotificationToastViewModel.cs" company="">
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
  using System.Windows.Controls;
  using System.ComponentModel.Composition;
  using Caliburn.Micro;
using System.Windows;

  class NotificationToastViewModel : ViewModelBase
  {
    public string Network { get; set; }
    public string Channel { get; set; }
    public string TimeStamp { get; set; }
    public string User { get; set; }
    public string Message { get; set; }
    public ShellView SV;

    public NotificationToastViewModel(MessageFilterEventArgs e, Window w) 
    {
      this.Network = e.Network;
      this.Channel = e.Channel;
      this.TimeStamp = e.Timestamp;
      this.User = e.Name;
      this.Message = e.Message;
      this.SV = (ShellView)w;
    }

    public void ShowTab() 
    {
      List<IrcChannelViewModel> icvm = new List<IrcChannelViewModel>();
      List<IrcNetworkViewModel> invm = new List<IrcNetworkViewModel>();
      IrcMainView imv = (IrcMainView)this.SV.ActiveItem.Content;
      IrcMainViewModel imvm = (IrcMainViewModel)imv.DataContext;
      IrcNetworkViewModel y = null;
      
      for (int i = 0; i < imv.Items.Items.Count; i++)
      {
        invm.Add((IrcNetworkViewModel)imv.Items.Items[i]);
      }
      foreach (IrcNetworkViewModel inv in invm)
      {
        if (inv.DisplayName == this.Network)
        {
          imvm.ActivateItem(inv);
          y = inv;
        }
      }
      IrcNetworkView z = y.GetView() as IrcNetworkView;
      
      for (int i = 1; i < z.Items.Items.Count; i++) 
      {
        icvm.Add((IrcChannelViewModel)z.Items.Items[i]);
      }
      foreach (IrcChannelViewModel ic in icvm) 
      {
        if (ic.Channel.Name == this.Channel) 
        {
          y.ActivateItem(ic);
        }
      }
      //TabItem ti = (TabItem)imv.Items.Items[imv.Items.Items.IndexOf(this.Network)];
      //ti.IsSelected = true;
      //TabItem tii = (TabItem)invm.Items.Items[invm.Items.Items.IndexOf(this.Channel)];
      //tii.IsSelected = true;
    }
  }
}
