using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeleniumLogger;

namespace ExampleAsserts
{
    public class ExampleAssertTests
    {

        public void Test1() {

            SeleniumLog log = SeleniumLog.Instance();
            int test = 3;

            //Check if it matches any of the numbers
            log.ResetResult();
            log.AreEqual(1, test);  
            log.AreEqual(2, test);
            log.AreEqual(3, test);
            log.AreEqual(4, test);
            log.AreEqual(5, test);
            log.PublishResult();   //You can choose when to evaluate the test and exit the test, so all comparisons above will be made and logged.
        }
    }
}
