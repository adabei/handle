using System;
using System.Collections.Generic;
using System.Text;
using System.Media;
using System.Threading;
using System.Windows.Threading;
namespace Handle.WPF
{
  class ExtendedSoundPlayer : SoundPlayer
  {
    private bool myBoolIsplaying = false;

    public bool bIsPlaying
    {
      get { return myBoolIsplaying; }
    }

    public ExtendedSoundPlayer(String strFilename) : base(strFilename) { }

    public void PlaySound()
    {
      if (!bIsPlaying)
      {
        Thread threadSound = new Thread(new ThreadStart(PlaySoundThread));
        threadSound.Start();
      }
    }

    protected virtual void PlaySoundThread()
    {
      myBoolIsplaying = true;
      //PlaySync plays the sound in the same thread and doesn't return till it is finished.
      try
      {
        PlaySync();
      }catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      myBoolIsplaying = false;
    }
  }
}
