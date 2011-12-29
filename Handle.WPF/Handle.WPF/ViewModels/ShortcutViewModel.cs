namespace Handle.WPF.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using Handle.WPF.Views;
  using Caliburn.Micro;

  public class ShortcutViewModel
  {

    public void OK() 
    {
      var svm = GetView() as ShortcutView;
      svm.DialogResult = true;
    }
    public void Cancel() 
    { 
    
    }
  }
}
