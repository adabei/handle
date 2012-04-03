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
  using System.Deployment.Application;
  using System.Windows.Forms;

  public class SettingsViewModel : ViewModelBase
  {
    private BindableCollection<String> filterPatterns;
    public delegate void SaveEventHandler(Settings settings);
    public event SaveEventHandler SaveButtonPressed;
    public String OldText;

    public SettingsViewModel(Settings settings)
    {
      this.Settings = settings;
      this.DisplayName = "Settings";
      if (this.Settings.FilterPatterns == null)
      {
        this.FilterPatterns = new BindableCollection<string>();
      }
      else 
      {
        this.FilterPatterns = new BindableCollection<string>(this.Settings.FilterPatterns);
      }
    }

    public void Save()
    {
      this.Settings.FilterPatterns = new List<string>(this.FilterPatterns);
      this.SaveButtonPressed(this.Settings);
      this.TryClose();
    }

    public void Cancel()
    {
      this.TryClose();
    }

    public BindableCollection<String> FilterPatterns
    {
      get
      {
        return this.filterPatterns;
      }

      set
      {
        this.filterPatterns = value;
        NotifyOfPropertyChange(() => this.FilterPatterns);
      }
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

    public void SelectSound() 
    { 
      Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
      dlg.DefaultExt = ".wav";
      dlg.Filter = "WAV (.wav)|*.wav";
      if (dlg.ShowDialog() == true) 
      {
        this.Settings.SoundPath = dlg.FileName;
      }
    }

    public void AddFilter() 
    {
      this.FilterPatterns.Add("New Entry");
    }

    public void RemoveFilter() 
    {
      var sv = GetView() as SettingsView;
      List<String> filters = sv.FilterPatterns.SelectedItems.Cast<String>().ToList<String>();
      
      foreach (String n in filters)
      {
        this.FilterPatterns.Remove(n);
      }
    }

    public void EditFilter() 
    {
      var sv = GetView() as SettingsView;
      sv.Filter.Text = sv.FilterPatterns.SelectedItem.ToString();
      OldText = sv.Filter.Text;
    }

    public void CancelFilter() 
    {
      var sv = GetView() as SettingsView;
      sv.Filter.Text = OldText;
    }

    public void SaveFilter() 
    {
      var sv = GetView() as SettingsView;
      this.FilterPatterns.Remove(OldText);
      this.FilterPatterns.Add(sv.Filter.Text);
    }

    public void CheckForUpdate() 
    {
      UpdateCheckInfo info = null;

      if (ApplicationDeployment.IsNetworkDeployed)
      {
        ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

        try
        {
          info = ad.CheckForDetailedUpdate();

        }
        catch (DeploymentDownloadException dde)
        {
          MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
          return;
        }
        catch (InvalidDeploymentException ide)
        {
          MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
          return;
        }
        catch (InvalidOperationException ioe)
        {
          MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
          return;
        }

        if (info.UpdateAvailable)
        {
          Boolean doUpdate = true;

          if (!info.IsUpdateRequired)
          {
            DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel);
            if (!(DialogResult.OK == dr))
            {
              doUpdate = false;
            }
          }
          else
          {
            // Display a message that the app MUST reboot. Display the minimum required version.
            MessageBox.Show("This application has detected a mandatory update from your current " +
                "version to version " + info.MinimumRequiredVersion.ToString() +
                ". The application will now install the update and restart.",
                "Update Available", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
          }

          if (doUpdate)
          {
            try
            {
              ad.Update();
              MessageBox.Show("The application has been upgraded, and will now restart.");
              Application.Restart();
            }
            catch (DeploymentDownloadException dde)
            {
              MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
              return;
            }
          }
        }
      }
    }
  }
}