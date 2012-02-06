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
using White.Core.Configuration;

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
    public void JoinSingleNetworkTest()
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
      CheckStatusTab();
      LeaveNetwork();
      Exit();
    }

    [Test]
    public void JoinMultipleNetworksTest() 
    {
      Start();
      OpenNetworkWindow();
      SelectItem("freenode");
      JoinNetwork();
      OpenNetworkWindow();
      NewNetwork("Vienna", "vienna.irc.at");
      SelectItem("Vienna");
      JoinNetwork();
      OpenNetworkWindow();
      NewNetwork("Quakenet", "clanserver4u.de.quakenet.org");
      SelectItem("Quakenet");
      JoinNetwork();
      CheckNetworksStatusTab();
      LeaveNetwork();
      CheckNetworksStatusTab();
      LeaveNetwork();
      CheckNetworksStatusTab();
      LeaveNetwork();
      OpenNetworkWindow();
      SelectItem("Quakenet");
      RemoveNetwork();
      SelectItem("Vienna");
      RemoveNetwork();
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
      MainWindow.WaitTill(() => (ChannelsTab = MainWindow.Get<Tab>("Channels")).TabCount == 1);
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

    public void CheckNetworksStatusTab()
    {
      TabPage FirstNetwork = MainWindow.Get<TabPage>(SearchCriteria.ByText("Handle.WPF.IrcNetworkViewModel"));
      Assert.IsNotNull(FirstNetwork);
      FirstNetwork.Click();
      CheckStatusTab();
    }

    public void Start()
    {
      Console.WriteLine(CoreAppXmlConfiguration.Instance.UIAutomationZeroWindowBugTimeout);
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

    public void NewNetwork(string nname, string addresse)
    {
      Button add = NetworkWindow.Get<Button>("Add");
      add.Click();
      Window NewNetworkWindow = NetworkWindow.ModalWindow("New network");
      Assert.IsNotNull(NewNetworkWindow);
      Button ok = NewNetworkWindow.Get<Button>("Ok");
      TextBox name = NetworkWindow.Get<TextBox>("Network_Name");
      TextBox address = NetworkWindow.Get<TextBox>("Network_Address");
      name.Text = nname;
      address.Text = addresse;
      ok.Click();
      NetworkWindow.Focus();
    }

    public void RemoveNetwork()
    {
      Button remove = NetworkWindow.Get<Button>("Remove");
      remove.Click();
    }
  }
}
