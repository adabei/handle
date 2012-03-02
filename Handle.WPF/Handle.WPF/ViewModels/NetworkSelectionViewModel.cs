// -----------------------------------------------------------------------
// <copyright file="NetworkSelectionViewModel.cs" company="">
// Copyright (c) 2011 Bernhard Schwarz, Florian Lembeck
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// -----------------------------------------------------------------------

namespace Handle.WPF
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.IO.IsolatedStorage;
  using System.Linq;
  using System.Text;
  using System.Windows.Input;
  using Caliburn.Micro;
  using ServiceStack.Text;

  /// <summary>
  /// Represents a ViewModel for NetworkSelectionViews
  /// </summary>
  public class NetworkSelectionViewModel : ViewModelBase
  {
    private BindableCollection<Network> networks;

    /// <summary>
    /// Initializes a new instance of the NetworkSelectionViewModel class.
    /// </summary>
    public NetworkSelectionViewModel()
    {
      this.loadNetworks();
      this.DisplayName = "Networks";
      try
      {
        this.windowManager = IoC.Get<IWindowManager>();
      }
      catch
      {
        this.windowManager = new WindowManager();
      }
      this.GlobalIdentity = Identity.GlobalIdentity();
    }

    private IWindowManager windowManager;

    public delegate void ConnectEventHandler(Network network);
    public event ConnectEventHandler ConnectButtonPressed;

    /// <summary>
    /// Gets or sets a list of networks.
    /// </summary>
    public BindableCollection<Network> Networks
    {
      get
      {
        return this.networks;
      }

      set
      {
        this.networks = value;
        NotifyOfPropertyChange(() => this.Networks);
      }
    }

    public Identity GlobalIdentity { get; set; }

    private void saveNetworks()
    {
      FileStream fs = new FileStream(Settings.PATH + "networks.json", FileMode.Create);
      try
      {
        JsonSerializer.SerializeToStream<BindableCollection<Network>>(this.Networks, fs);
      }
      finally
      {
        fs.Close();
      }
    }

    /// <summary>
    /// Loads networks from a config file in IsolatedStorage.
    /// </summary>
    private void loadNetworks()
    {
      FileStream fs = new FileStream(Settings.PATH + "networks.json", FileMode.OpenOrCreate);
      try
      {
        this.Networks = JsonSerializer.DeserializeFromStream<BindableCollection<Network>>(fs) ?? new BindableCollection<Network>();
      }
      finally
      {
        fs.Close();
      }
    }

    public override IEnumerable<InputBindingCommand> GetInputBindingCommands()
    {
      yield return new InputBindingCommand(Cancel)
      {
        GestureKey = Key.Escape
      };
    }

    /// <summary>
    /// Opens a dialog to add new networks.
    /// </summary>
    public void Add()
    {
      var nnvm = new NetworkNewViewModel();
      if (this.windowManager.ShowDialog(nnvm) == true)
      {
        this.Networks.Add(nnvm.Network);
        this.saveNetworks();
      }
    }

    public void Connect()
    {
      var nsv = GetView() as NetworkSelectionView;
      if (nsv.Networks.SelectedIndex != -1)
      {
        this.ConnectButtonPressed(this.Networks[nsv.Networks.SelectedIndex]);
      }
      saveGlobalIdentity();
    }

    public void Remove()
    {
      var nsv = GetView() as NetworkSelectionView;
      while (nsv.Networks.SelectedIndex != -1)
      {
        this.Networks.RemoveAt(nsv.Networks.SelectedIndex);
      }
      this.saveNetworks();
    }

    public void Edit()
    {
      var nsv = GetView() as NetworkSelectionView;
      int index = nsv.Networks.SelectedIndex;
      if (index == -1)
      {
        return;
      }

      NetworkEditViewModel nevm = new NetworkEditViewModel(this.Networks[index].ShallowCopy());

      if (this.windowManager.ShowDialog(nevm) == true)
      {
        this.Networks.RemoveAt(index);
        this.Networks.Insert(index, nevm.Network);
        this.saveNetworks();
      }
    }

    public void Cancel()
    {
      saveGlobalIdentity();
      this.TryClose();
    }

    private void saveGlobalIdentity()
    {
      FileStream fs = new FileStream(Settings.PATH + "identity.json", FileMode.Create);
      try
      {
        JsonSerializer.SerializeToStream<Identity>(this.GlobalIdentity, fs);
      }
      finally
      {
        fs.Close();
      }
    }
  }
}