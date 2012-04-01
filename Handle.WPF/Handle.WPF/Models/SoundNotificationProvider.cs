using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Handle.WPF
{
  class SoundNotificationProvider : INotificationProvider
  {
    Settings Settings;
    public SoundNotificationProvider(Settings settings) 
    {
      this.Settings = settings;
    }
    public void Notify(MessageFilterEventArgs args) 
    {
      ExtendedSoundPlayer sp = new ExtendedSoundPlayer(this.Settings.SoundPath);
      sp.PlaySound();
    }
  }
}
