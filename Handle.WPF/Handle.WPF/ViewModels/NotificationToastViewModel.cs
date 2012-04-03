using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Handle.WPF
{
  class NotificationToastViewModel : ViewModelBase
  {
    public string Network { get; set; }
    public string Channel { get; set; }
    public string User { get; set; }
    public string TimeStamp { get; set; }
    public string Message { get; set; }

    public NotificationToastViewModel(MessageFilterEventArgs e) 
    {
      this.Network = e.Network;
      this.Channel = e.Channel;
      this.TimeStamp = e.Timestamp;
      this.User = e.Name;
      this.Message = e.Message;
    }
  }
}
