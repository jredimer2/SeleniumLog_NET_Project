using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;
using SeleniumLogger;

namespace ExampleInstantiations
{
    class TestClass
    {
        public void Message(string msg)
        {
            try
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.WriteLine(string.Format("TestClass :: Message :: {0}", msg));
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Error().WriteLine(string.Format("TestClass :: Exception :: {0}", e.Message));
            }
        }
        public TestClass() { }
    }

    class Program
    {
        static void Foo()
        {
            SeleniumLog log = SeleniumLog.Instance();
            log.WriteLine("Message from Foo");
        }

        static void Main(string[] args)
        {
            FirefoxDriver driver0 = new FirefoxDriver();
            SeleniumLogEventListener driver = new SeleniumLogEventListener(driver0);
            SeleniumLog log = SeleniumLog.Instance(driver);

            TestClass tobj = new TestClass();

            log.WriteLine("Main");
            Foo();
            tobj.Message("hi there");
        }
    }
}
