using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace SeleniumLogger
{
    public sealed partial class SeleniumLog
    {

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreSame().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="expected">Expected object</param>
        /// <param name="actual">Actual object</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public bool AreSame(
            Object expected,
            Object actual,
            string message = ""
        ) 
        {
            try
            {
                Assert.AreSame(expected: expected, actual: actual, message: message);
                PassAssert(string.Format(message + " AreSame: Expected [{0}]   Actual [{1}] - PASS", expected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreSame: Expected [{0}]   Actual [{1}] - FAIL", expected, actual));
                Result = false;
                if (Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreSame().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="expected">Expected object</param>
        /// <param name="actual">Actual object</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <param name="parameters">An array of parameters to use when formatting message.</param>
        /// <returns></returns>
        public bool AreSame(
            Object expected,
            Object actual,
            string message = "",
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreSame(expected: expected, actual: actual, message: message, parameters: parameters);
                PassAssert(string.Format(message + " AreSame: Expected [{0}]   Actual [{1}] - PASS", expected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreSame: Expected [{0}]   Actual [{1}] - FAIL", expected, actual));
                Result = false;
                if (Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        
    }


}
