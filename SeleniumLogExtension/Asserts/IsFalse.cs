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
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="condition">The boolean condition to be evaluated whether false of not</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
         public bool IsFalse(
            bool condition,
            string message = ""
        ) 
        {
            try
            {
                Assert.IsFalse(condition: condition, message: message);
                PassAssert(string.Format(message + " IsFalse: Condition [{0}] - PASS", condition));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " IsFalse: Condition [{0}] - FAIL", condition));
                Result = false;
                if (Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

         /// <summary>
         /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse().
         /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
         /// </summary>
         /// <param name="condition">The boolean condition to be evaluated whether false of not</param>
         /// <param name="message">Optional message string displayed in the log</param>
         /// <param name="parameters">An array of parameters to use when formatting message.</param>
         /// <returns></returns>
         public bool IsFalse(
            bool condition,
            string message = "",
            params Object[] parameters
        ) 
        {
            try
            {
                Assert.IsFalse(condition: condition, message: message, parameters: parameters);
                PassAssert(string.Format(message + " IsFalse: Condition [{0}] - PASS", condition));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " IsFalse: Condition [{0}] - FAIL", condition));
                Result = false;
                if (Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

    }


}
