using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumLogger;
using System.Collections;
using SeleniumTest;

namespace TestHarness
{


    partial class Program
    {
        public static void TestSimpleText()
        {
            SeleniumLog log = SeleniumLog.Instance();

            log.WriteLine("line 1");
            log.Indent().Indent().WriteLine("line 2");
            log.WriteLine("line 3");
            log.Indent().WriteLine("line 4");
            log.WriteLine("line 5");
            log.WriteLine("line 6");
            log.Indent().Pass().Blue().WriteLine("line 7");
            log.WriteLine("line 8");
            log.Unindent().Unindent().Unindent().WriteLine("line 9");
            log.WriteLine("line 10");
        }

    }
}
