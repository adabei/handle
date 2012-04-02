using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Handle.WPF
{
  class SoundNotificationProvider : INotificationProvider
  {
    private Settings settings;
    [ImportingConstructor]
    public SoundNotificationProvider(Settings settings)
    {
      this.settings = settings;
    }
    public void Notify(MessageFilterEventArgs args)
    {
      ExtendedSoundPlayer sp = new ExtendedSoundPlayer(this.settings.SoundPath);
      sp.PlaySound();
    }
  }
}
