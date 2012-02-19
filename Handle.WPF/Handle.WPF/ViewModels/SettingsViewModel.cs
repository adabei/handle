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

    /* private Settings settings;
    public Settings Settings
    {
      get
      {
        return this.settings;
      }
      set
      {
        this.settings = value;
        NotifyOfPropertyChange(() => this.Settings);
      }
    } */

    public SettingsViewModel(Settings settings)
    {
      this.Settings = settings;
      this.DisplayName = "Settings";
    }

    public void Save()
    {
      this.SaveButtonPressed(this.Settings);
      var parent = this.Parent as ShellViewModel;
      parent.ActivateItem(parent.IrcMainViewModel);
    }

    public void Cancel()
    {
      var parent = this.Parent as ShellViewModel;
      parent.ActivateItem(parent.IrcMainViewModel);
    }

    protected override IEnumerable<InputBindingCommand> GetInputBindingCommands()
    {
      yield return new InputBindingCommand(Cancel)
      {
        GestureKey = Key.Escape
      };
    }
  }
}