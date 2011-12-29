// -----------------------------------------------------------------------
// <copyright file="Network.cs" company="">
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
  using System.Linq;
  using System.Text;

  /// <summary>
  /// A class representing an IRC network.
  /// </summary>
  public class Network
  {
    public Network()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Network class.
    /// </summary>
    /// <param name="name">The name of the network.</param>
    /// <param name="address">The address of the network</param>
    /// <param name="isFavorite">Whether the network is a favorite or not</param>
    /// <param name="connectCommands">The commands to be executed upon connecting.</param>
    public Network(string name, string address, bool isFavorite, string connectCommands)
    {
      this.Name = name;
      this.Address = address;
      this.IsFavorite = isFavorite;
      this.ConnectCommands = connectCommands;
    }

    /// <summary>
    /// Gets or sets the network's name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the network's address.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the network is a favorite.
    /// </summary>
    public bool IsFavorite { get; set; }

    /// <summary>
    /// Gets or sets the commands to be executed upon connecting.
    /// </summary>
    public string ConnectCommands { get; set; }

    /// <summary>
    /// Returns the name of the network.
    /// </summary>
    /// <returns>The name of the network</returns>
    public override string ToString()
    {
      return this.Name;
    }

    public Network ShallowCopy()
    {
      return (Network)this.MemberwiseClone();
    }
  }
}
