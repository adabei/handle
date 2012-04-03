using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.IO;

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
      if (this.settings.SoundPath != "")
      {
        try
        {
          ExtendedSoundPlayer sp = new ExtendedSoundPlayer(this.settings.SoundPath);
          sp.PlaySound();
        }
        catch (FileNotFoundException ex) 
        {
          Console.WriteLine(ex.ToString());
        }
      }
      else 
      {
        Console.WriteLine("Keine Sounddatei ausgewählt");
      }
    }
  }
}
