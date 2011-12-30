// -----------------------------------------------------------------------
// <copyright file="Message.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Handle.WPF
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class Message
  {
    public string Text { get; private set; }
    public DateTime Received { get; private set; }
    public string Sender { get; private set; }

    public Message(string text, DateTime received, string sender)
    {
      Text = text;
      Received = received;
      Sender = sender;
    }
  }
}
