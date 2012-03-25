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
  using System.ComponentModel.Composition;
  using System.IO;
  using ServiceStack.Text;

  /// <summary>
  /// A class representing all Settings
  /// </summary>
  [PartCreationPolicy (CreationPolicy.Shared)]
  [Export("Settings")]
  public class Settings
  {
    /// <summary>
    /// Initializes a new instance of the Settings class.
    /// </summary>
    public Settings()
    {
    }

    public static readonly string PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Handle.WPF\";

    /// <summary>
    /// Gets or sets a value indicating whether logs are saved
    /// </summary>
    public bool CanLog { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether an URL will be displayed as a link
    /// </summary>
    public bool DisplayURLAsLink { get; set; }

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
    /// Gets or sets the TimeStampFormat
    /// </summary>
    public string TimestampFormat { get; set; }

    public bool UseFuzzySearch { get; set; }

    public bool ShowWelcomeMessages { get; set; }

    public bool ShowLeaveMessages { get; set; }

    public bool NotificationToast { get; set; }

    public bool TaskbarBlinking { get; set; }

    public bool MakeSound { get; set; }

    public string SoundPath { get; set; }

    /// <summary>
    /// Creates a shallow copy of the instance.
    /// </summary>
    /// <returns>A shallow copy</returns>
    public Settings ShallowCopy()
    {
      return this.MemberwiseClone() as Settings;
    }

    /// <summary>
    /// Deserializes saved settings from a JSON file.
    /// </summary>
    /// <returns>Saved settings</returns>
    public static Settings Load()
    {
      FileStream fs = new FileStream(Settings.PATH + "settings.json", FileMode.OpenOrCreate);
      Settings settings;
      try
      {
        settings = JsonSerializer.DeserializeFromStream<Settings>(fs) ?? new Settings();
      }
      catch
      {
        settings = defaultSettings();
      }
      finally
      {
        fs.Close();
      }
      return settings;
    }

    /// <summary>
    /// Serializes the object using JSON.
    /// </summary>
    public void Save()
    {
      FileStream fs = new FileStream(Settings.PATH + "settings.json", FileMode.Create);
      try
      {
        JsonSerializer.SerializeToStream<Settings>(this, fs);
      }
      finally
      {
        fs.Close();
      }
    }

    /// <summary>
    /// Creates a Settings instance with default settings.
    /// </summary>
    /// <returns>Default instance of Settings</returns>
    private static Settings defaultSettings()
    {
      return new Settings()
      {
        CanLog = false,
        CanSendLeaveMessage = true,
        DisplayURLAsLink = true,
        FontFamily = "Segoe UI",
        FontSize = 11,
        LeaveMessage = "Leaving - Using the Handle client",
        TimestampFormat = "<HH:mm>",
        UseFuzzySearch = false
      };
    }
  }
}