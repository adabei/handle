using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using White.Core;
using White.Core.UIItems.WindowItems;
using White.Core.InputDevices;
using White.Core.WindowsAPI;
using System.Threading;

namespace Handle.WPF.Test
{
  [TestFixture]
  class ShortcutTest
  {
    Application Application;
    Window MainWindow;
    Keyboard Keyboard = Keyboard.Instance;
    Window NetworkWindow;

    [Test]
    public void OpenNetworkWindow()
    {
      Application = Application.Launch(@"C:\Users\Flotschi\git\handle\Handle.WPF\Handle.WPF\bin\Debug\Handle.WPF.exe");
      Assert.IsNotNull(Application);
      MainWindow = Application.GetWindow("Handle");
      Assert.IsNotNull(MainWindow);
      MainWindow.Focus();
      Keyboard.LeaveAllKeys();
      Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
      Keyboard.Enter("n");
      NetworkWindow = MainWindow.ModalWindow("Networks");
      Assert.IsNotNull(NetworkWindow);
      Keyboard.LeaveAllKeys();
      NetworkWindow.Close();
      Application.Kill();
    }
  }
}
