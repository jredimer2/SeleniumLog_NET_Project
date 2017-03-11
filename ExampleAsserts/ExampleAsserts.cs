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
            float test = 3.2f;

            //Check if it matches any of the numbers
            try
            {
                log.Path("AreEual");
                slAssert.ResetResult();
                slAssert.AreEqual(1.0f, 1.5f, 0.0f);
                slAssert.AreEqual(1.1f, 3.5f, 0.0f);
                slAssert.AreEqual(3.0f, 2.5f, 1.0f);
                slAssert.AreEqual(1.0f, 1.5f, 0.7f);
                slAssert.AreEqual(1.0f, 1.5f, 0.0f);
                slAssert.PublishResult();   //You can choose when to evaluate the test and exit the test, so all comparisons above will be made and logged.
                log.Pass("AreEqual test cases passed");
            }
            catch (Exception e)
            {
                log.Fail("Are equal test cases failed");            
            }

            try
            {
                
                log.Path("AreNotEual");
                slAssert.ResetResult();
                slAssert.AreNotEqual(1.0f, 1.5f, 0.0f);
                slAssert.AreNotEqual(1.1f, 3.5f, 0.0f);
                slAssert.AreNotEqual(3.0f, 2.5f, 1.0f);
                slAssert.AreNotEqual(1.0f, 1.5f, 0.7f);
                slAssert.AreNotEqual(1.0f, 1.5f, 0.0f);
                slAssert.PublishResult();   //You can choose when to evaluate the test and exit the test, so all comparisons above will be made and logged.
                log.Pass("AreNotEqual test cases passed");
                 
            }
            catch 
            {
                log.Fail("AreNotEequal test cases failed");
            }

        }
    }
}
