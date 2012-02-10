using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using White.Core;
using White.Core.UIItems.WindowItems;
using White.Core.InputDevices;
using White.Core.UIItems.TabItems;
using NUnit.Framework;
using White.Core.UIItems.ListBoxItems;
using White.Core.Configuration;
using White.Core.WindowsAPI;
using White.Core.UIItems;
using White.Core.UIItems.Finders;

namespace Handle.WPF.Test
{
  class ChannelTest
  {
    Application Application;
    Window MainWindow;
    Keyboard Keyboard = Keyboard.Instance;
    Window NetworkWindow;
    Window ChannelWindow;
    Tab NetworksTab;
    int channelzaehler;

    [Test]
    public void JoinChannelInNetwork()
    {
      channelzaehler = 0;
      Start();
      OpenNetworkWindow();
      SelectItem("freenode");
      JoinNetwork();
      channelzaehler = 1;
      CheckStatusTab();
      OpenChannelWindow();
      JoinChannel("#test");
      channelzaehler++;
      CheckNewChannelExists();
      LeaveNetwork();
      Exit();
    }

    [Test]
    public void JoinMultipleChannels() 
    {
      //TODO
    }

    [Test]
    public void SendMessage() 
    {
      //TODO
    }

    [Test]
    public void ReceiveMessage() 
    {
      //TODO
    }

    [Test]
    public void FilterChannels() 
    {
      //TODO
    }

    private void CheckNewChannelExists()
    {
      Tab ChannelsTab = MainWindow.Get<Tab>("Channels");
      Assert.IsNotNull(ChannelsTab);
      MainWindow.WaitTill(() => (ChannelsTab = MainWindow.Get<Tab>("Channels")).TabCount == channelzaehler);
      TabPage FirstChannelTab = MainWindow.Get<TabPage>(SearchCriteria.ByText("Handle.WPF.IrcChannelViewModel"));
      Assert.IsNotNull(FirstChannelTab);
    }

    private void OpenChannelWindow()
    {
      Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
      Keyboard.Enter("t");
      Keyboard.LeaveAllKeys();
      ChannelWindow = MainWindow.ModalWindow("Join Channel");
      Assert.IsNotNull(ChannelWindow);
    }

    private void JoinChannel(string channel)
    {
      TextBox name = ChannelWindow.Get<TextBox>("Pattern");
      name.Text = channel;
      Button join = ChannelWindow.Get<Button>("Join");
      join.Click();
      ChannelWindow.Close();
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
      MainWindow.WaitTill(() => (ChannelsTab = MainWindow.Get<Tab>("Channels")).TabCount == channelzaehler);
      TabPage StatusTab = MainWindow.Get<TabPage>(SearchCriteria.ByText("Handle.WPF.IrcStatusTabViewModel"));
      Assert.IsNotNull(StatusTab);
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
