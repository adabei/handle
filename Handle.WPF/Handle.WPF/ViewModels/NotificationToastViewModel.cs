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

  class NotificationToastViewModel : ViewModelBase
  {
    public string Network { get; set; }
    public string Channel { get; set; }
    public string TimeStamp { get; set; }
    public string User { get; set; }
    public string Message { get; set; }

    public NotificationToastViewModel(MessageFilterEventArgs e) 
    {
      this.Network = e.Network;
      this.Channel = e.Channel;
      this.TimeStamp = e.Timestamp;
      this.User = e.Name;
      this.Message = e.Message;
    }

    public void ShowTab() 
    {
      //var invm = GetView() as IrcNetworkView;
      //var imvm = GetView() as IrcMainView;
      //TabItem ti = (TabItem)imvm.Items.Items[imvm.Items.Items.IndexOf(this.Network)];
      //ti.IsSelected = true;
      //TabItem tii = (TabItem)invm.Items.Items[invm.Items.Items.IndexOf(this.Channel)];
      //tii.IsSelected = true;
    }
  }
}
