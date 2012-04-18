// -----------------------------------------------------------------------
// <copyright file="ArrayExtension.cs" company="">
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

  /// <summary>
  /// Provides extension methods for Arrays.
  /// </summary>
  public static class ArrayExtension
  {
    /// <summary>
    /// Returns a range given start and end. If the last parameter is omitted or false the return value will include the end object, otherwise it will be excluded.
    /// </summary>
    /// <typeparam name="T">The type of the Array</typeparam>
    /// <param name="sourceArray">Returns a range of an array</param>
    /// <param name="start">Start index of the range</param>
    /// <param name="end">End index of the range</param>
    /// <param name="exclude">If true the range will include the end object</param>
    /// <returns></returns>
    public static T[] Range<T>(this T[] sourceArray, int start, int end, bool exclude = false)
    {
      if (end < 0)
        end = sourceArray.Length + end;

      int length = end - start;
      if (!exclude)
        length++;

      T[] result = new T[length];
      Array.Copy(sourceArray, start, result, 0, length);
      return result;
    }
  }
}
