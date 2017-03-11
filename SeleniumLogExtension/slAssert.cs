using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SeleniumLogger
{
    /// <summary>
    /// Encapsulate MS Asserts
    /// </summary>
    public static partial class slAssert
    {
        private static bool Result {get; set;}
        private static SeleniumLog log = SeleniumLog.Instance();

        public static void ResetResult()
        {
            Result = true;
        }

        public static void PublishResult()
        {
            if (Result == false)
            {
                Result = true; //reset result
                throw new AssertFailedException();
            }
        }

        private static void PassAssert(string message)
        {
            log.Pass().WriteLine(message);
        }

        private static void FailAssert(string message = "")
        {
            log.Fail().WriteLine(message);
        }
        /*
        public static void Fail(string message = "")
        {
            if (log.Config.ForceThrowExceptionOnAssertFail)
            {
                Assert.Fail(message: message + " - FAIL");
            }
            else
            {
                FailAssert(string.Format(message + " Fail assert."));
                Result = false;
            }
        }
        */

         
    }
}
