// -----------------------------------------------------------------------
// <copyright file="MessageFilterEventArgs.cs" company="">
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

  public class MessageFilterEventArgs : EventArgs
  {
    public string Channel;
    public string Network;
    public string Timestamp;
    public string Name;
    public string Message;

    public MessageFilterEventArgs(string channel, string network, string timestamp, string name, string message)
    {
      this.Channel = channel;
      this.Network = network;
      this.Timestamp = timestamp;
      this.Name = name;
      this.Message = message;
    }

    public override string ToString()
    {
      return this.Channel + "." + this.Network + "<" + this.Timestamp + ">" + this.Name + ":" + this.Message;
    }
  }
}