// -----------------------------------------------------------------------
// <copyright file="Identity.cs" company="">
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
  using System.Security.Cryptography;
  using System.Text;
  using ServiceStack.Text;
  using System.IO;

  /// <summary>
  /// A class representing an identity used to authenticate with NickServ.
  /// </summary>
  public class Identity
  {
    /// <summary>
    /// The encrypted password.
    /// </summary>
    private string password;

    /// <summary>
    /// Initializes a new instance of the Identity class.
    /// </summary>
    /// <param name="name">The name displayed to other users.</param>
    /// <param name="password">The password used to connect with NickServ</param>
    /// <param name="alternative">An alternative name if the prefered one is already taken.</param>
    /// <param name="realName">The user's real name</param>
    public Identity(string name, string password, string alternative, string realName)
    {
      this.Name = name;
      this.Password = password;
      this.Alternative = alternative;
      this.RealName = realName;
    }

    /// <summary>
    /// Gets or sets the users nickname.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the nickname that is used, when Name is already taken.
    /// </summary>
    public string Alternative { get; set; }

    /// <summary>
    /// Gets or sets the users real name.
    /// </summary>
    public string RealName { get; set; }

    /// <summary>
    /// Gets or sets the password used to authenticate with NickServ.
    /// TODO: Actually encrypt the password.
    /// </summary>
    public string Password
    {
      get
      {
        return Convert.FromBase64String(this.password).ToString();
      }

      set
      {
        var data = Encoding.Unicode.GetBytes(value);
        this.password = Convert.ToBase64String(data, 0, data.Length);
      }
    }

    public static Identity GlobalIdentity()
    {
      Identity identity;

      using (var fs = new FileStream(Settings.PATH + "identity.json", FileMode.OpenOrCreate))
      {
        try
        {
          identity = JsonSerializer.DeserializeFromStream<Identity>(fs);
        }
        catch
        {
          identity = null;
        }
      }

      return identity;
    }
  }
}
