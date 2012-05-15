// -----------------------------------------------------------------------
// <copyright file="FilterService.cs" company="">
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
  using System.ComponentModel.Composition;
  using System.Linq;
  using System.Text;
  using System.Text.RegularExpressions;
  using Caliburn.Micro;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class FilterService : IHandle<MessageFilterEventArgs>
  {
    public List<Regex> Patterns { get; set; }

    public List<INotificationProvider> NotificationProviders { get; set; }

    public Settings Settings { get; set; }

    public FilterService()
    {
      this.Patterns = new List<Regex>();
      this.NotificationProviders = new List<INotificationProvider>();
    }

    /// <summary>
    /// Check whether the given message is important or not
    /// </summary>
    /// <param name="message">The message</param>
    /// <returns></returns>
    public bool IsImportant(string message)
    {
      return this.Patterns.Exists(regex => regex.IsMatch(message));
    }

    public void Handle(MessageFilterEventArgs message)
    {
      if (this.IsImportant(message.ToString()))
      {
        foreach (var provider in this.NotificationProviders)
        {
          provider.Notify(message);
        }
      }
    }

    public void PopulatePatterns()
    {
      lock (this.Patterns)
      {
        this.Patterns.Clear();
        foreach (var p in this.Settings.FilterPatterns)
        {
          try
          {
            this.Patterns.Add(new Regex(p, RegexOptions.Compiled));
          }
          catch (InvalidCastException e)
          {
            Console.WriteLine(e.Message);
          }
        }
      }
    }
  }
}