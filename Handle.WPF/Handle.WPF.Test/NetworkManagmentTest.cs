using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using White.Core;
using White.Core.UIItems.WindowItems;
using White.Core.InputDevices;
using White.Core.WindowsAPI;
using White.Core.UIItems;
using White.Core.UIItems.Finders;
using White.Core.UIItems.TableItems;
using White.Core.UIItems.ListBoxItems;
using System.Threading;

namespace Handle.WPF.Test
{
  [TestFixture]
  class NetworkManagmentTest
  {
    Application Application;
    Window MainWindow;
    Keyboard Keyboard = Keyboard.Instance;
    Window NetworkWindow;

    [Test]
    public void NewNetworkTest() 
    {
      Start();
      OpenNetworkWindow();
      NewNetwork();
      SelectItem("Test");
      RemoveNetwork();
      Exit();
    }

    [Test]
    public void RemoveNetworkTest()
    {
      Start();
      OpenNetworkWindow();
      NewNetwork();
      SelectItem("Test");
      RemoveNetwork();
      Exit();
    }

    [Test]
    public void EditNetworkTest()
    {
      Start();
      OpenNetworkWindow();
      NewNetwork();
      SelectItem("Test");
      EditNetwork();
      SelectItem("Test2");
      RemoveNetwork();
      Exit();
    }

    public void EditNetwork() 
    {
      Button edit = NetworkWindow.Get<Button>("Edit");
      edit.Click();
      Window NewNetworkWindow = NetworkWindow.ModalWindow("Edit Network");
      Button ok = NewNetworkWindow.Get<Button>("Ok");
      TextBox name = NetworkWindow.Get<TextBox>("Network_Name");
      TextBox address = NetworkWindow.Get<TextBox>("Network_Address");
      name.Text = "Test2";
      address.Text = "test.irc.at";
      ok.Click();
      NetworkWindow.Focus();
    }

    public void SelectItem(string value) 
    {
      ListBox networks = NetworkWindow.Get<ListBox>("Networks");
      Assert.IsNotNull(networks);
      Thread.Sleep(1000);
      networks.Select(value);
      ListItem item = networks.SelectedItem;
      Assert.AreEqual(value, item.Text);
    }

    public void NewNetwork() 
    {
      Button add = NetworkWindow.Get<Button>("Add");
      add.Click();
      Window NewNetworkWindow = NetworkWindow.ModalWindow("New network");
      Assert.IsNotNull(NewNetworkWindow);
      Button ok = NewNetworkWindow.Get<Button>("Ok");
      TextBox name = NetworkWindow.Get<TextBox>("Network_Name");
      TextBox address = NetworkWindow.Get<TextBox>("Network_Address");
      name.Text = "Test";
      address.Text = "test.irc.at";
      ok.Click();
      NetworkWindow.Focus();
    }

    public void OpenNetworkWindow() 
    {
      Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
      Keyboard.Enter("n");
      Keyboard.LeaveAllKeys();
      NetworkWindow = MainWindow.ModalWindow("Networks");
      Assert.IsNotNull(NetworkWindow);
    }

    public void RemoveNetwork() 
    {
      Button remove = NetworkWindow.Get<Button>("Remove");
      remove.Click();
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
  }
}
