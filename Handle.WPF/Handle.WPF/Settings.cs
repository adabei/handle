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
  public class Settings
  {
    /// <summary>
    /// Initializes a new instance of the Settings class.
    /// </summary>
    public Settings() 
    {
      this.Notifications = new Dictionary<string, bool>();
      this.Notifications["Toast"] = true;
      this.Notifications["Sound"] = true;
      this.Notifications["Taskbar"] = true;
      this.Shortcuts = new Dictionary<string, string>();
      this.Shortcuts["ActivateLastChannel"] = "ja";
      this.Shortcuts["SwitchBetweenNeighbourChannelForward"] = "ja";
      this.Shortcuts["SwitchBetweenNeighbourChannelBack"] = "ja";
      this.Shortcuts["ActivateLastActiveChannelFromTheLastNetwork"] = "ja";
      this.Shortcuts["DisconnectWithCurrentNetwork"] = "ja";
      this.Shortcuts["OpenChannelSearch"] = "ja";
      this.Shortcuts["OpenNetworklist"] = "ja";
      this.Shortcuts["ActivateLastActiveChannelFromNetworkFirstKey"] = "ja";
      this.Shortcuts["ActivateLastActiveChannelFromNetworkSecondKey"] = "ja";
      this.Shortcuts["ChannelSwitchFirstKey"] = "ja";
      this.Shortcuts["ChannelSwitchSecondKey"] = "ja"; 
      this.Shortcuts["LeaveCurrentChannel"] = "ja";
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
    public int FontSize { get; set; }

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
    public Dictionary<string,bool> Notifications { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether ...
    /// </summary>
    public bool NotificationToast
    {
      get 
      {
        return this.Notifications["Toast"];
      }
      
      set  
      {
        this.Notifications["Toast"] = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether ...
    /// </summary>
    public bool NotificationSound
    {
      get
      {
        return this.Notifications["Sound"];
      }

      set
      {
        this.Notifications["Sound"] = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether ...
    /// </summary>
    public bool NotificationTaskbar
    {
      get
      {
        return this.Notifications["Taskbar"];
      }
      
      set
      {
        this.Notifications["Taskbar"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the Shortcuts
    /// </summary>
    public Dictionary<string, string> Shortcuts { get; set; }

    public string SActivateLastChannel
    {
      get
      {
        return this.Shortcuts["ActivateLastChannel"];
      }
      
      set
      {
        this.Shortcuts["ActivateLastChannel"] = value;
      }
    }

    public string SSwitchBetweenNeighbourChannelForward
    {
      get
      {
        return this.Shortcuts["SwitchBetweenNeighbourChannelForward"];
      }

      set
      {
        this.Shortcuts["SwitchBetweenNeighbourChannelForward"] = value;
      }
    }

    public string SSwitchBetweenNeighbourChannelBack
    {
      get
      {
        return this.Shortcuts["SwitchBetweenNeighbourChannelBack"];
      }

      set
      {
        this.Shortcuts["SwitchBetweenNeighbourChannelBack"] = value;
      }
    }

    public string SActivateLastActiveChannelFromTheLastNetwork
    {
      get
      {
        return this.Shortcuts["ActivateLastActiveChannelFromTheLastNetwork"];
      }

      set
      {
        this.Shortcuts["ActivateLastActiveChannelFromTheLastNetwork"] = value;
      }
    }

    public string SDisconnectWithCurrentNetwork
    {
      get
      {
        return this.Shortcuts["DisconnectWithCurrentNetwork"];
      }

      set
      {
        this.Shortcuts["DisconnectWithCurrentNetwork"] = value;
      }
    }

    public string SOpenChannelSearch
    {
      get
      {
        return this.Shortcuts["OpenChannelSearch"];
      }

      set
      {
        this.Shortcuts["OpenChannelSearch"] = value;
      }
    }

    public string SOpenNetworklist
    {
      get
      {
        return this.Shortcuts["OpenNetworklist"];
      }

      set
      {
        this.Shortcuts["OpenNetworklist"] = value;
      }
    }

    public string SLeaveCurrentChannel
    {
      get
      {
        return this.Shortcuts["LeaveCurrentChannel"];
      }

      set
      {
        this.Shortcuts["LeaveCurrentChannel"] = value;
      }
    }

    public string SActivateLastActiveChannelFromNetworkFirstKey
    {
      get
      {
        return this.Shortcuts["ActivateLastActiveChannelFromNetworkFirstKey"];
      }

      set
      {
        this.Shortcuts["ActivateLastActiveChannelFromNetworkFirstKey"] = value;
      }
    }

    public string SActivateLastActiveChannelFromNetworkSecondKey
    {
      get
      {
        return this.Shortcuts["ActivateLastActiveChannelFromNetworkSecondKey"];
      }

      set
      {
        this.Shortcuts["ActivateLastActiveChannelFromNetworkSecondKey"] = value;
      }
    }

    public string SChannelSwitchFirstKey
    {
      get
      {
        return this.Shortcuts["ChannelSwitchFirstKey"];
      }

      set
      {
        this.Shortcuts["ChannelSwitchFirstKey"] = value;
      }
    }

    public string SChannelSwitchSecondKey
    {
      get
      {
        return this.Shortcuts["ChannelSwitchSecondKey"];
      }

      set
      {
        this.Shortcuts["ChannelSwitchSecondKey"] = value;
      }
    }

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
