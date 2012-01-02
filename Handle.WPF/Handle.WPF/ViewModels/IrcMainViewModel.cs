// -----------------------------------------------------------------------
// <copyright file="IrcMainViewModel.cs" company="">
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
  using Caliburn.Micro;
  using System.Collections.Generic;
  using System.Windows.Input;

  /// <summary>
  /// Represents a ViewModel for IrcMainViews
  /// </summary>
  public class IrcMainViewModel : Screen, IHaveClosableTabControl
  {
    /// <summary>
    /// Initializes a new instance of the IrcMainViewModel class
    /// </summary>
    public IrcMainViewModel()
    {
      this.Networks = new BindableCollection<IrcNetworkViewModel>();
    }

    public BindableCollection<IrcNetworkViewModel> Networks { get; set; }

    public void CloseItem(object sender)
    {
      foreach (var item in (sender as IrcNetworkViewModel).Channels)
      {
        item.IrcChannel.Leave();
      }
      this.Networks.Remove(sender as IrcNetworkViewModel);
    }
  }
}
