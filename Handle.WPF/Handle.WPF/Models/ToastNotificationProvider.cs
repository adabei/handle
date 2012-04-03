using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Windows;
using System.Windows.Threading;
using System.Threading;

namespace Handle.WPF
{
  class ToastNotificationProvider : INotificationProvider
  {

    Screen screen;

    public ToastNotificationProvider(Screen s) 
    {
      this.screen = s;
    }

    public void Notify(MessageFilterEventArgs e)
    {
      IWindowManager wm;
      try
      {
        wm = IoC.Get<IWindowManager>();
      }
      catch
      {
        wm = new WindowManager();
      }
      var ntvm = new NotificationToastViewModel(e);
      Window x = screen.GetView() as Window;
      x.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
      {
        wm.ShowWindow(ntvm);
      }));
    }
  }
}
