﻿// -----------------------------------------------------------------------
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
  using Caliburn.Micro;
  using Newtonsoft.Json;

  /// <summary>
  /// ViewModel for the network list.
  /// </summary>
  public class NetworkSelectionViewModel : Screen
  {
    /// <summary>
    /// Initializes a new instance of the NetworkSelectionViewModel class.
    /// </summary>
    public NetworkSelectionViewModel()
    {
      this.initializeNetworks();
    }

    /// <summary>
    /// Gets or sets the global identity.
    /// </summary>
    public Identity GlobalIdentity { get; set; }

    /// <summary>
    /// Gets or sets a list of networks.
    /// </summary>
    public List<Network> Networks { get; set; }

    /// <summary>
    /// Opens a dialog to add new networks.
    /// </summary>
    public void Add()
    {
      IWindowManager wm;
      try
      {
        wm = IoC.Get<IWindowManager>();
      }
      catch
      {
        wm = new WindowManager();
      }

      wm.ShowDialog(new NetworkNewViewModel());
    }

    /// <summary>
    /// Loads networks from a config file in IsolatedStorage.
    /// </summary>
    private void initializeNetworks()
    {
      var store = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
      IsolatedStorageFileStream isolatedStream;
      try
      {
        isolatedStream = new IsolatedStorageFileStream("networks.json", FileMode.Open, store);
      }
      catch
      {
        isolatedStream = new IsolatedStorageFileStream("networks.json", FileMode.Create, store);
      }

      this.Networks = JsonConvert.DeserializeObject<List<Network>>(new StreamReader(isolatedStream).ReadToEnd());
    }
  }
}