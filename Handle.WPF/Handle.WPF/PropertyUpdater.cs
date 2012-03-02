// -----------------------------------------------------------------------
// <copyright file="PropertyUpdater.cs" company="">
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
// Original by StackOverflow user Joel V. Earnest-DeYoung
// </copyright>
// -----------------------------------------------------------------------

namespace Handle.WPF
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Reflection;

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class PropertyUpdater
  {
    public static PropertyUpdater<T> Update<T>(T objectToUpdate)
    {
      return PropertyUpdater<T>.Update(objectToUpdate);
    }
  }

  public class PropertyUpdater<T>
  {
    /// <summary>
    /// Which object will be updated
    /// </summary>
    private readonly T destination;

    private PropertyUpdater(T destination)
    {
      this.destination = destination;
    }

    public static PropertyUpdater<T> Update(T objectToUpdate)
    {
      return new PropertyUpdater<T>(objectToUpdate);
    }

    public void With(T objectToCopyFrom)
    {
      var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
      foreach (var prop in properties)
      {
        // Only copy properties with public get and set methods
        if (!prop.CanRead || !prop.CanWrite || prop.GetGetMethod(false) == null || prop.GetSetMethod(false) == null) continue;
        // Skip indexers
        if (prop.GetGetMethod(false).GetParameters().Length > 0) continue;

        prop.SetValue(this.destination, prop.GetValue(objectToCopyFrom, null), null);
      }
    }
  }
}
