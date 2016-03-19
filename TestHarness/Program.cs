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
using ExampleAsserts;
using ExampleExplore;

namespace TestHarness
{

    public class Class1 : BaseClass
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [SeleniumLogTrace]
        public Class1(string fn, string ln)
        {
            FirstName = fn;
            LastName = ln;
        }

        [SeleniumLogTrace]
        public string FName()
        {
            return FirstName;
        }

        [SeleniumLogTrace]
        public string LName()
        {
            return LastName;
        }

        [SeleniumLogTrace]
        public void DisplayName()
        {
            Console.WriteLine("{0} {1}", FName(), LName());
        }
    }

    partial class Program
    {
        public static void TestComplexStructures()
        {
            SeleniumLog log = SeleniumLog.Instance();
            string str = "Hello World";
            Hashtable ht = new Hashtable();
            ht.Add("k1", "v1");
            ht.Add("k2", "v2");
            TestClass tc = new TestClass();

            datastruct1 DS = new datastruct1();
            datastruct2 DS2 = new datastruct2();
            //tc.Method1(100, 250, DS);
            //Console.ReadLine();
            //log.WriteLine("Exploring DS");
            ///log.Indent();
            //log.Explore(DS2, comment: "Exporing DS structure");
            //log.Unindent();

            //log.WriteLine("Exploring DS2");
            //log.Indent();

            // It is the combination of these two calls that causes the indentation error

            log.Explore(ht, comment: "Exporing ht structure");
            log.Explore(DS, comment: "Exploring DS structure");
            log.Explore(DS2, comment: "Exploring DS2 structure");
            //log.Explore(DS, comment: "Exploring DS structure");
                //log.Unindent();

            //log.Explore(DS2, comment: "Exploring DS2 structure again");
            log.Explore(str, comment: "Exploring string");

        }


        static void Main(string[] args)
        {
            SeleniumLog log = SeleniumLog.Instance();

           //TestCase test = new TestCase();
           //test.Run();
            //TestIndentations_Java_1();
            //TestIndentations();
            TestSimpleText();
            
 

        }
    }
}
