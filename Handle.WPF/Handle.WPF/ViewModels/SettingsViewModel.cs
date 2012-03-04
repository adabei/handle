namespace Handle.WPF
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Windows.Input;
  using Caliburn.Micro;

  public class SettingsViewModel : ViewModelBase
  {
    public delegate void SaveEventHandler(Settings settings);
    public event SaveEventHandler SaveButtonPressed;

    public SettingsViewModel(Settings settings)
    {
      this.Settings = settings;
      this.DisplayName = "Settings";
    }

    public void Save()
    {
      this.SaveButtonPressed(this.Settings);
      this.TryClose();
    }

    public void Cancel()
    {
      this.TryClose();
    }

    public override IEnumerable<InputBindingCommand> GetInputBindingCommands()
    {
      yield return new InputBindingCommand(Cancel)
      {
        GestureKey = Key.Escape
      };
    }
  }
}