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

    public void Notify(MessageFilterEventArgs args)
    {
      IWindowManager wm;
      var csvm = new NotificationToastViewModel();
      try
      {
        wm = IoC.Get<IWindowManager>();
      }
      catch
      {
        wm = new WindowManager();
      }
      Window x = screen.GetView() as Window;
      x.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
      {
        wm.ShowWindow(csvm);
        var view = csvm.GetView() as NotificationToastView;
        view.network.Text = args.Network;
        view.channel.Text = args.Channel;
        view.user.Text = args.Name;
        view.message.Text = args.Message;
        view.timestamp.Text = args.Timestamp;
      }));
    }
  }
}
