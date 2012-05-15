// -----------------------------------------------------------------------
// <copyright file="NetworkSelectionViewModel.cs" company="">
// Copyright (c) 2011-2012 Bernhard Schwarz, Florian Lembeck
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
      nnvm.Parent = this;
      if (this.windowManager.ShowDialog(nnvm) == true)
      {
        this.Networks.Add(nnvm.Network);
        this.saveNetworks();
      }
    }

    public void Connect()
    {
      var nsv = GetView() as NetworkSelectionView;
      List<Network> nets = nsv.NetworkList.SelectedItems.Cast<Network>().ToList<Network>();
      foreach (Network n in nets)
      {
        this.ConnectButtonPressed(this.Networks[networks.IndexOf(n)]);
      }
      SaveGlobalIdentity();
    }

    public void Join(Network n, MouseButtonEventArgs e) 
    {
      if (e.ClickCount == 2)
      {
        this.ConnectButtonPressed(n);
      }
    }

    public void Remove()
    {
      var nsv = GetView() as NetworkSelectionView;
      List<Network> nets = nsv.NetworkList.SelectedItems.Cast<Network>().ToList<Network>();
      foreach (Network n in nets)
      {
        this.Networks.Remove(n);
      }
      this.saveNetworks();
    }

    public void Edit()
    {
      var nsv = GetView() as NetworkSelectionView;
      int index = nsv.NetworkList.SelectedIndex;
      if (index == -1)
      {
        return;
      }
      Network n = (Network)nsv.NetworkList.SelectedItem;
      index = this.networks.IndexOf(n);
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
      this.TryClose();
    }

    public void Export()
    {
      var nsv = GetView() as NetworkSelectionView;
      Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
      dlg.DefaultExt = ".json";
      dlg.Filter = "JSON (.json)|*.json";
      if (dlg.ShowDialog() == true)
      {
        string filename = dlg.FileName;

        List<Network> nets = new List<Network>();

        foreach (Network n in nsv.NetworkList.SelectedItems)
        {
          for (int i = 0; i < this.Networks.Count; i++)
          {
            if (n.Name == this.Networks[i].Name && n.Address == this.Networks[i].Address)
            {
              nets.Add(n);
            }
          }
        }
        FileStream fs = new FileStream(filename, FileMode.Create);
        try
        {
          JsonSerializer.SerializeToStream(nets, fs);
        }
        finally
        {
          fs.Close();
        }
      }
    }

    public void Import()
    {
      var nsv = GetView() as NetworkSelectionView;
      Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
      dlg.DefaultExt = ".json";
      dlg.Filter = "JSON (.json)|*.json";
      if (dlg.ShowDialog() == true)
      {
        string filename = dlg.FileName;
        List<Network> nets = new List<Network>();

        FileStream fs = new FileStream(filename, FileMode.Open);
        try
        {
          nets = JsonSerializer.DeserializeFromStream<List<Network>>(fs);
        }
        finally
        {
          fs.Close();
        }

        foreach (Network n in nets)
        {
          Boolean insert = true;
          foreach (Network x in this.Networks)
          {
            if (n.Name == x.Name && n.Address == x.Address)
            {
              insert = false;
              break;
            }
          }
          if (insert)
          {
            this.Networks.Add(n);
          }
        }
      }
      this.saveNetworks();
    }

    public void SaveGlobalIdentity()
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

    public void ChangeFavF(Network net)
    {
      var nsv = GetView() as NetworkSelectionView;
      if (net.IsFavorite)
      {
        net.IsFavorite = false;
      }
      else
      {
        net.IsFavorite = true;
      }

      nsv.NetworkList.Items.Refresh();
      this.saveNetworks();
    }

    public void ChangeFavT(Network net)
    {
      var nsv = GetView() as NetworkSelectionView;
      net.IsFavorite = true;
      nsv.NetworkList.Items.Refresh();
      this.saveNetworks();
    }
  }
}