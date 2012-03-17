namespace Handle.WPF
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Windows.Input;
  using Caliburn.Micro;
  using System.Drawing;
  using System.Drawing.Text;

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

    public void OnSelectionChangedFontFamily()
    {
      var sv = GetView() as SettingsView;
      this.Settings.FontFamily = sv.lstFonts.SelectedValue.ToString();
    }

    protected override void OnViewLoaded(object view)
    {
      base.OnViewLoaded(view);
      var sv = GetView() as SettingsView;
      if (sv != null)
      {
        int x = 0;
        foreach (System.Windows.Media.FontFamily ff in sv.lstFonts.Items) 
        {
          if (ff.Source == this.Settings.FontFamily) 
          {
            sv.lstFonts.SelectedIndex = x;
          }
          x++;
        }
        sv.lstFontSize.SelectedIndex = (int)(this.Settings.FontSize - 1);
      }
    }

    public void OnSelectionChangedFontSize()
    {
      var sv = GetView() as SettingsView;
      this.Settings.FontSize = (double)sv.lstFontSize.SelectedValue;
    }
  }
}