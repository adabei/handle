namespace Handle.WPF
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using Caliburn.Micro;
  using System.IO.IsolatedStorage;
  using System.IO;
  using Newtonsoft.Json;
  using System.Windows.Input;

  public class SettingsViewModel : ViewModelBase
  {
    public delegate void SaveEventHandler(Settings settings);
    public event SaveEventHandler SaveButtonPressed;
    public Settings Settings { get; set; }

    public SettingsViewModel(Settings settings)
    {
      this.Settings = settings;
    }

    private void serializeSettings()
    {
      string json = JsonConvert.SerializeObject(this.Settings, Formatting.Indented);
      var store = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
      IsolatedStorageFileStream isolatedStream;
      try
      {
        isolatedStream = new IsolatedStorageFileStream("settings.json", FileMode.Truncate, store);
      }
      catch
      {
        isolatedStream = new IsolatedStorageFileStream("settings.json", FileMode.Create, store);
      }
      StreamWriter sw = new StreamWriter(isolatedStream);
      sw.Write(json);
      sw.Close();
    }

    public void Save()
    {
      this.SaveButtonPressed(this.Settings);
      var parent = this.Parent as ShellViewModel;
      serializeSettings();
      parent.ActivateItem(parent.IrcMainViewModel);
    }

    public void Cancel()
    {
      var parent = this.Parent as ShellViewModel;
      parent.ActivateItem(parent.IrcMainViewModel);
    }

    public void LogBrowse()
    {
      var sv = GetView() as SettingsView;
      System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
      System.Windows.Forms.DialogResult result = dlg.ShowDialog();
      if (result.ToString() == "OK")
      {
        this.Settings.LogSavePath = dlg.SelectedPath;
        sv.Settings_LogSavePath.Text = this.Settings.LogSavePath;
      }
    }

    public void SliderValueChanged()
    {
      var sv = GetView() as SettingsView;
      this.Settings.FontSize = sv.Settings_FontSize.Value;
    }

    public void FontChanging()
    {
      var sv = GetView() as SettingsView;
      this.Settings.FontFamily = sv.FontFamilyListView.SelectedValue.ToString();
    }

    public BindableCollection<string> SChannelSwitchFirstKey
    {
      get
      {
        return new BindableCollection<string>(
                         new string[] { "Alt", "Strg" });
      }
    }

    public string SelectedSChannelSwitchFirstKey
    {
      get { return this.Settings.Shortcuts["SChannelSwitchFirstKey"]; }
      set
      {
        this.Settings.Shortcuts["SChannelSwitchFirstKey"] = value;
        NotifyOfPropertyChange(() => SelectedSChannelSwitchFirstKey);
      }
    }

    public BindableCollection<string> SChannelSwitchSecondKey
    {
      get
      {
        return new BindableCollection<string>(
                         new string[] { "1-8", "F1-F8" });
      }
    }

    public string SelectedSChannelSwitchSecondKey
    {
      get { return this.Settings.Shortcuts["SChannelSwitchSecondKey"]; }
      set
      {
        this.Settings.Shortcuts["SChannelSwitchSecondKey"] = value;
        NotifyOfPropertyChange(() => SelectedSChannelSwitchSecondKey);
      }
    }

    public BindableCollection<string> SActivateLastActiveChannelFromNetworkFirstKey
    {
      get
      {
        return new BindableCollection<string>(
                         new string[] { "Alt", "Strg" });
      }
    }


    public BindableCollection<string> SActivateLastActiveChannelFromNetworkSecondKey
    {
      get
      {
        return new BindableCollection<string>(
                         new string[] { "1-8", "F1-F8" });
      }
    }
    public string SelectedSActivateLastActiveChannelFromNetworkSecondKey
    {
      get { return this.Settings.Shortcuts["SActivateLastActiveChannelFromNetworkSecondKey"]; }
      set
      {
        this.Settings.Shortcuts["SActivateLastActiveChannelFromNetworkSecondKey"] = value;
        NotifyOfPropertyChange(() => SelectedSActivateLastActiveChannelFromNetworkSecondKey);
      }
    }


    /// <summary>
    /// Gets or sets a value indicating whether ...
    /// </summary>
    public bool NotificationToast
    {
      get
      {
        return this.Settings.Notifications["Toast"];
      }

      set
      {
        this.Settings.Notifications["Toast"] = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether ...
    /// </summary>
    public bool NotificationSound
    {
      get
      {
        return this.Settings.Notifications["Sound"];
      }

      set
      {
        this.Settings.Notifications["Sound"] = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether ...
    /// </summary>
    public bool NotificationTaskbar
    {
      get
      {
        return this.Settings.Notifications["Taskbar"];
      }

      set
      {
        this.Settings.Notifications["Taskbar"] = value;
      }
    }

    protected override IEnumerable<InputBindingCommand> GetInputBindingCommands()
    {
      yield return new InputBindingCommand(Save)
      {
        GestureKey = Key.Enter
      };
      yield return new InputBindingCommand(Cancel)
      {
        GestureKey = Key.Escape
      };
    }
  }
}