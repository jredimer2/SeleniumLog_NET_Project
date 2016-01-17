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
        public static void TestIndentations()
        {
            SeleniumLog log = SeleniumLog.Instance();
            int L = 0;

            log.WriteLine("line 1");
            log.WriteLine("line 2");
            log.WriteLine("line 3");
            log.WriteLine("line 4");
            log.WriteLine("line 5");
            log.Indent();
            log.Indent();
            log.WriteLine("line 6");
            log.WriteLine("line 7");
            log.WriteLine("line 8");
            log.WriteLine("line 9");
            log.WriteLine("line 10");
            log.WriteLine("line 11");
            log.WriteLine("line 12");
            log.WriteLine("line 13");
            log.WriteLine("line 14");
            log.WriteLine("line 15");
            log.Indent();
            log.WriteLine("line 16");
            log.WriteLine("line 17");
            log.WriteLine("line 18");
            log.WriteLine("line 19");
            log.WriteLine("line 20");
            log.WriteLine("line 21");
            log.WriteLine("line 22");
            log.WriteLine("line 23");
            log.WriteLine("line 24");
            log.WriteLine("line 25");
            log.WriteLine("line 26");
            log.WriteLine("line 27");
            log.WriteLine("line 28");
            log.SaveIndent("id"); //
            log.Indent();
            log.WriteLine("line 29");
            log.WriteLine("line 30");
            log.WriteLine("line 31");
            log.WriteLine("line 32");
            log.WriteLine("line 33");
            log.WriteLine("line 34");
            log.WriteLine("line 35");
            log.WriteLine("line 36");
            log.Unindent().Unindent();
            log.WriteLine("line 37");
            log.WriteLine("line 38");
            log.Indent().WriteLine("line 39");
            log.WriteLine("line 40");
            log.Indent().WriteLine("line 41");
            log.WriteLine("line 42");
            log.WriteLine("line 43");
            log.Indent().WriteLine("line 44");
            log.Indent().WriteLine("line 45");
            log.WriteLine("line 46");
            log.WriteLine("line 47");
            log.Indent();
            log.WriteLine("line 48");
            log.WriteLine("line 49");
            log.WriteLine("line 50");
            log.WriteLine("line 51");
            log.WriteLine("line 52");
            log.WriteLine("line 53");
            log.WriteLine("line 54");
            log.WriteLine("line 55");
            log.WriteLine("line 56");
            log.WriteLine("line 57");
            log.WriteLine("line 58");
            log.WriteLine("line 59");
            log.WriteLine("line 60");
            log.WriteLine("line 61");
            log.Unindent().Unindent().Unindent();
            log.WriteLine("line 62");
            log.WriteLine("line 63");
            log.WriteLine("line 64");
            log.WriteLine("line 65");
            log.WriteLine("line 66");
            log.WriteLine("line 67");
            log.WriteLine("line 68");
            log.WriteLine("line 69");
            log.WriteLine("line 70");
            log.RestoreIndent("id");  //
            log.WriteLine("line 71");
            log.WriteLine("line 72");
            log.WriteLine("line 73");
            log.WriteLine("line 74");
            log.WriteLine("line 75");
            log.WriteLine("line 76");
            log.WriteLine("line 77");
            log.WriteLine("line 78");
            log.WriteLine("line 79");
            log.WriteLine("line 80");
            log.WriteLine("line 81");
            log.WriteLine("line 82");
            log.WriteLine("line 83");
            log.WriteLine("line 84");
            log.WriteLine("line 85");
            log.WriteLine("line 86");
            log.WriteLine("line 87");
            log.WriteLine("line 88");
            log.WriteLine("line 89");
            log.WriteLine("line 90");
            log.WriteLine("line 91");
            log.WriteLine("line 92");
            log.WriteLine("line 93");
            log.WriteLine("line 94");
            log.WriteLine("line 95");
            log.WriteLine("line 96");
            log.WriteLine("line 97");
            log.WriteLine("line 98");
            log.WriteLine("line 99");
            log.WriteLine("line 100");


        }
    }
}
