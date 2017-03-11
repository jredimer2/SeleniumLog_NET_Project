using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace SeleniumLogger
{
    //public sealed partial class SeleniumLog
    public static partial class slAssert
    {
        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Equals().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="objA">First comparison object</param>
        /// <param name="objB">Second comparison object</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public static bool Equals(
	        Object objA,
	        Object objB,
            string message = ""
        ) 
        {
            try
            {
                Assert.Equals(objA: objA, objB: objB);
                PassAssert(string.Format(message + " Equals: objA [{0}]   objB [{1}] - PASS", objA, objB));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " Equals: objA [{0}]   objB [{1}] - FAIL", objA, objB));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }
    }


}
