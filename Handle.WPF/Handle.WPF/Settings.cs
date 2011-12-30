// -----------------------------------------------------------------------
// <copyright file="Settings.cs" company="">
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
  using Newtonsoft.Json;

  /// <summary>
  /// A class representing all Settings
  /// </summary>
  [JsonObject(MemberSerialization.OptIn)]
  public class Settings
  {
    /// <summary>
    /// Initializes a new instance of the Settings class.
    /// </summary>
    public Settings()
    {
      this.Notifications = new Dictionary<string, bool>();
      this.Shortcuts = new Dictionary<string, string>();
    }

    /// <summary>
    /// Gets or sets a value indicating whether logs are saved
    /// </summary>
    public bool CanLog { get; set; }

    /// <summary>
    /// Gets or sets the LogSavePath
    /// </summary>
    public string LogSavePath { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether an URL will be displayed as a link
    /// </summary>
    public bool CanDisplayURLAsLink { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a customized leave message will be sent
    /// </summary>
    public bool CanSendLeaveMessage { get; set; }

    /// <summary>
    /// Gets or sets the Leave Message
    /// </summary>
    public string LeaveMessage { get; set; }

    /// <summary>
    /// Gets or sets the FontFamily
    /// </summary>
    public string FontFamily { get; set; }

    /// <summary>
    /// Gets or sets the FontSize
    /// </summary>
    public double FontSize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether timestamps will be displayed
    /// </summary>
    public bool CanShowTimeStamps { get; set; }

    /// <summary>
    /// Gets or sets the TimeStampFormat
    /// </summary>
    public string TimeStampFormat { get; set; }

    /// <summary>
    /// Gets or sets the NotificationStyles
    /// </summary>
    public Dictionary<string, bool> Notifications { get; set; }

    /// <summary>
    /// Gets or sets the Shortcuts
    /// </summary>
    public Dictionary<string, string> Shortcuts { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether updates will be applied
    /// </summary>
    public bool CanUpdate { get; set; }

    /// <summary>
    /// Blablabla
    /// </summary>
    /// <returns></returns>
    public Settings ShallowCopy()
    {
      return (Settings)this.MemberwiseClone();
    }
  }
}
