using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Interop;

namespace Handle.WPF
{
  class TaskBarBlinkingNotificationProvider : INotificationProvider
  {
    Screen screen;
    public TaskBarBlinkingNotificationProvider(Screen s) 
    {
      this.screen = s;
    }

    public void Notify(MessageFilterEventArgs args) 
    {
      Window x = screen.GetView() as Window;
      x.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
      {
        IntPtr h = new WindowInteropHelper(x).Handle;
        TaskBarBlinking.Flash(h);
      }));
    }
  }
}
