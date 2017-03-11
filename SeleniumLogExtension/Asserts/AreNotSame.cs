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
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotSame().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="notExpected">The not expected object</param>
        /// <param name="actual">Actual object</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public static bool AreNotSame(
            Object notExpected,
            Object actual,
            string message = ""
        ) 
        {
            try
            {
                Assert.AreNotSame(notExpected: notExpected, actual: actual, message: message);
                PassAssert(string.Format(message + " AreNotSame: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreNotSame: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotSame().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="notExpected">The not expected object</param>
        /// <param name="actual">Actual object</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <param name="parameters">An array of parameters to use when formatting message.</param>
        /// <returns></returns>
        public static bool AreNotSame(
            Object notExpected,
            Object actual,
            string message = "",
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreNotSame(notExpected: notExpected, actual: actual, message: message, parameters: parameters);
                PassAssert(string.Format(message + " AreNotSame: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreNotSame: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        
    }


}
