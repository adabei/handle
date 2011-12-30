// -----------------------------------------------------------------------
// <copyright file="Message.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Handle.WPF
{
  using System;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class Message
  {
    public string Text { get; private set; }
    public string Received { get; private set; }
    public string Sender { get; private set; }

    public Message(string text, string received, string sender)
    {
      Text = text;
      Received = received;
      Sender = sender;
    }
  }
}
