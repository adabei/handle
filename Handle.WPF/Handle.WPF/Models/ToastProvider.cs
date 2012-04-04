namespace Handle.WPF
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading;
  using System.Windows;
  using System.Windows.Threading;
  using Caliburn.Micro;

  class ToastProvider : INotificationProvider
  {

    Screen screen;

    public ToastProvider(Screen s) 
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
