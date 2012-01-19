using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using White.Core;
using White.Core.UIItems.WindowItems;

namespace Handle.WPF.Test
{
  [TestFixture]
  class ShortcutTest
  {
    Application application;
    [Test]
    public void FindMainWindow() 
    {
      application = Application.Launch(@"C:\Users\Flotschi\git\handle\Handle.WPF\Handle.WPF\bin\Debug\Handle.WPF.exe");
      Assert.IsNotNull(application);
    }
  }
}
