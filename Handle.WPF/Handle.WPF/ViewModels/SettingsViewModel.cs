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

  public class SettingsViewModel : Screen
  {
    private Settings settingsReference;
    private Settings settings;

    public Settings Settings
    {
      get
      {
        return this.settings;
      }
      set
      {
        this.settings = value;
      }
    }

    public SettingsViewModel(Settings settings)
    {
      initializeSettings();
      // in das wirds später gespeichert
      this.settingsReference = settings;
      // das ist das, dass du bindest
      //this.Settings = settings.ShallowCopy();
    }

    /// <summary>
    /// Loads settings from a config file in IsolatedStorage.
    /// </summary>
    private void initializeSettings()
    {
      var store = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
      IsolatedStorageFileStream isolatedStream;
      isolatedStream = new IsolatedStorageFileStream("settings.json", FileMode.OpenOrCreate, store);
      this.Settings = JsonConvert.DeserializeObject<Settings>(new StreamReader(isolatedStream).ReadToEnd());
      if (this.Settings == null)
      {
        this.Settings = new Settings();
        
      }
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

    public void OK() 
    {
      serializeSettings();
    }

    public void Cancel() 
    {
      initializeSettings();
    }

    public void LogBrowse() 
    {

      System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
      System.Windows.Forms.DialogResult result = dlg.ShowDialog();
      if (result.ToString() == "OK")
      {
        this.Settings.LogSavePath = dlg.SelectedPath;
      }
    }

    public void KeysAreDown(System.Windows.Input.KeyEventArgs e)
    {
      var sv = GetView() as SettingsView;
      //if (e.Key == System.Windows.Input.Key.LeftCtrl)
      //{

        sv.Settings_SActivateLastChannel.Text = e.Key.ToString();
      //}
    }

    public void SliderValueChanged() 
    {
      var sv = GetView() as SettingsView;
      this.Settings.FontSize = (int)sv.Settings_FontSize.Value;
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
                         new string[] { "Alt","Strg" });
      }
    }

    public string SelectedSChannelSwitchFirstKey
    {
      get { return this.Settings.SChannelSwitchFirstKey; }
      set
      {
        this.Settings.SChannelSwitchFirstKey = value;
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
      get { return this.Settings.SChannelSwitchSecondKey; }
      set
      {
        this.Settings.SChannelSwitchSecondKey = value;
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

    public string SelectedSActivateLastActiveChannelFromNetworkFirstKey
    {
      get { return this.Settings.SActivateLastActiveChannelFromNetworkFirstKey; }
      set
      {
        this.Settings.SActivateLastActiveChannelFromNetworkFirstKey = value;
        NotifyOfPropertyChange(() => SelectedSActivateLastActiveChannelFromNetworkFirstKey);
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
      get { return this.Settings.SActivateLastActiveChannelFromNetworkSecondKey; }
      set
      {
        this.Settings.SActivateLastActiveChannelFromNetworkSecondKey = value;
        NotifyOfPropertyChange(() => SelectedSActivateLastActiveChannelFromNetworkSecondKey);
      }
    }
  }
}