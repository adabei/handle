// -----------------------------------------------------------------------
// <copyright file="IrcChannelUserComparer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Handle.WPF
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using IrcDotNet;
  using IrcDotNet.Collections;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class IrcChannelUserComparison
  {
    public static int Compare(IrcChannelUser x, IrcChannelUser y)
    {
      return string.Compare(x.User.NickName, y.User.NickName);
    }
  }
}
