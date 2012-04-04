using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.IO;

namespace Handle.WPF
{
  class SoundProvider : INotificationProvider
  {
    private Settings settings;
    [ImportingConstructor]
    public SoundProvider(Settings settings)
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
