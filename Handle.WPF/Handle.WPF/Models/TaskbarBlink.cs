﻿// -----------------------------------------------------------------------
// <copyright file="TaskbarBlink.cs" company="">
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
  using System.Linq;
  using System.Runtime.InteropServices;
  using System.Text;
  using System.Windows;
  using System.Windows.Interop;

  public static class TaskbarBlink
  {
    [DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

		[StructLayout(LayoutKind.Sequential)]
		private struct FLASHWINFO
		{
			/// <summary>
			/// The size of the structure in bytes.
			/// </summary>
			public uint cbSize;
			/// <summary>
			/// A Handle to the Window to be Flashed. The window can be either opened or minimized.
			/// </summary>
			public IntPtr hwnd;
			/// <summary>
			/// The Flash Status.
			/// </summary>
			public uint dwFlags;
			/// <summary>
			/// The number of times to Flash the window.
			/// </summary>
			public uint uCount;
			/// <summary>
			/// The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.
			/// </summary>
			public uint dwTimeout;
		}

		/// <summary>
		/// Stop flashing. The system restores the window to its original stae.
		/// </summary>
		public const uint FLASHW_STOP = 0;

		/// <summary>
		/// Flash the window caption.
		/// </summary>
		public const uint FLASHW_CAPTION = 1;

		/// <summary>
		/// Flash the taskbar button.
		/// </summary>
		public const uint FLASHW_TRAY = 2;

		/// <summary>
		/// Flash both the window caption and taskbar button.
		/// This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags.
		/// </summary>
		public const uint FLASHW_ALL = 3;

		/// <summary>
		/// Flash continuously, until the FLASHW_STOP flag is set.
		/// </summary>
		public const uint FLASHW_TIMER = 4;

		/// <summary>
		/// Flash continuously until the window comes to the foreground.
		/// </summary>
		public const uint FLASHW_TIMERNOFG = 12;


		/// <summary>
		/// Flash the spacified Window (Form) until it recieves focus.
		/// </summary>
		/// <param name="form">The Form (Window) to Flash.</param>
		/// <returns></returns>
		public static bool Flash(IntPtr hwnd)
		{
			// Make sure we're running under Windows 2000 or later
			if (Win2000OrLater)
			{
				FLASHWINFO fi = Create_FLASHWINFO(hwnd, FLASHW_ALL | FLASHW_TIMERNOFG, uint.MaxValue, 0);

				return FlashWindowEx(ref fi);
			}

			return false;
		}

		private static FLASHWINFO Create_FLASHWINFO(IntPtr handle, uint flags, uint count, uint timeout)
		{
			FLASHWINFO fi = new FLASHWINFO();
			fi.cbSize = Convert.ToUInt32(Marshal.SizeOf(fi));
			fi.hwnd = handle;
			fi.dwFlags = flags;
			fi.uCount = count;
			fi.dwTimeout = timeout;
			return fi;
		}

		/// <summary>
		/// Flash the specified Window (form) for the specified number of times
		/// </summary>
		/// <param name="hwnd">The handle of the Window to Flash.</param>
		/// <param name="count">The number of times to Flash.</param>
		/// <returns></returns>
		public static bool Flash(IntPtr hwnd, uint count)
		{
			if (Win2000OrLater)
			{
				FLASHWINFO fi = Create_FLASHWINFO(hwnd, FLASHW_ALL, count, 0);

				return FlashWindowEx(ref fi);
			}

			return false;
		}

		/// <summary>
		/// Start Flashing the specified Window (form)
		/// </summary>
		/// <param name="form">The Form (Window) to Flash.</param>
		/// <returns></returns>
		public static bool Start(System.Windows.Forms.Form form)
		{
			if (Win2000OrLater)
			{
				FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_ALL, uint.MaxValue, 0);
				return FlashWindowEx(ref fi);
			}

			return false;
		}

		/// <summary>
		/// Stop Flashing the specified Window (form)
		/// </summary>
		/// <param name="form"></param>
		/// <returns></returns>
		public static bool Stop(System.Windows.Forms.Form form)
		{
			if (Win2000OrLater)
			{
				FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_STOP, uint.MaxValue, 0);
				return FlashWindowEx(ref fi);
			}

			return false;
		}

		/// <summary>
		/// A boolean value indicating whether the application is running on Windows 2000 or later.
		/// </summary>
		private static bool Win2000OrLater
		{
			get { return System.Environment.OSVersion.Version.Major >= 5; }
		}
  }
}
