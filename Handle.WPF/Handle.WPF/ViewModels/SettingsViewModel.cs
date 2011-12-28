namespace Handle.WPF
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using Caliburn.Micro;

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
      // in das wirds später gespeichert
      this.settingsReference = settings;
      // das ist das, dass du bindest
      this.Settings = settings.ShallowCopy();
    }
  }
}