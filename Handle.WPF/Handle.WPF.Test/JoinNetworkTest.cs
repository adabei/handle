using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using White.Core;
using White.Core.UIItems.WindowItems;
using White.Core.InputDevices;
using White.Core.WindowsAPI;
using White.Core.UIItems;
using White.Core.UIItems.Finders;
using White.Core.UIItems.TableItems;
using White.Core.UIItems.ListBoxItems;
using System.Text;
using White.Core.UIItems.TabItems;
using System.Threading;

namespace Handle.WPF.Test
{
  class JoinNetworkTest
  {
    Application Application;
    Window MainWindow;
    Keyboard Keyboard = Keyboard.Instance;
    Window NetworkWindow;
    Tab NetworksTab;

    [Test]
    public void JoinNetworksTest()
    {
      Start();
      OpenNetworkWindow();
      SelectItem("freenode");
      JoinNetwork();
      LeaveNetwork();
      Exit();
    }

    [Test]
    public void StatusTabExistsTest()
    {
      Start();
      OpenNetworkWindow();
      SelectItem("freenode");
      JoinNetwork();
      Thread.Sleep(10000);
      CheckStatusTab();
      LeaveNetwork();
      Exit();
    }

    public void LeaveNetwork() 
    {
      Button CloseNetwork = MainWindow.Get<Button>(SearchCriteria.ByText("X"));
      Assert.IsNotNull(CloseNetwork);
      CloseNetwork.Click();
    }

    public void CheckStatusTab() 
    {
      Tab ChannelsTab = MainWindow.Get<Tab>("Channels");
      Assert.IsNotNull(ChannelsTab);
      TabPage StatusTab = MainWindow.Get<TabPage>(SearchCriteria.ByText("Handle.WPF.IrcStatusTabViewModel"));
      Assert.IsNotNull(StatusTab);
    }

    public void JoinNetwork() 
    {
      Button join = NetworkWindow.Get<Button>("Connect");
      join.Click();
      NetworkWindow.Close();
      MainWindow.Focus();
      NetworksTab = MainWindow.Get<Tab>("Networks");
      Assert.IsNotNull(NetworksTab);
      TabPage FirstNetwork = MainWindow.Get<TabPage>(SearchCriteria.ByText("Handle.WPF.IrcNetworkViewModel"));
      Assert.IsNotNull(FirstNetwork);
      FirstNetwork.Click();     
    }

    public void Start()
    {
      Application = Application.Launch(@"C:\Users\Flotschi\git\handle\Handle.WPF\Handle.WPF\bin\Debug\Handle.WPF.exe");
      Assert.IsNotNull(Application);
      MainWindow = Application.GetWindow("Handle");
      Assert.IsNotNull(MainWindow);
      MainWindow.Focus();
    }

    public void Exit()
    {
      Application.Kill();
    }

    public void SelectItem(string value)
    {
      ListBox networks = NetworkWindow.Get<ListBox>("Networks");
      Assert.IsNotNull(networks);
      networks.Select(value);
      ListItem item = networks.SelectedItem;
      Assert.AreEqual(value, item.Text);
    }

    public void OpenNetworkWindow()
    {
      Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
      Keyboard.Enter("n");
      Keyboard.LeaveAllKeys();
      NetworkWindow = MainWindow.ModalWindow("Networks");
      Assert.IsNotNull(NetworkWindow);
    }
  }
}
