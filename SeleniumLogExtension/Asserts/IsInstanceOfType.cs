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
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="value">The input object to be evaluated</param>
        /// <param name="expectedType">Expected type of value</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
         public bool IsInstanceOfType(
	        Object value,
	        Type expectedType,
            string message = ""
        ) 
        {
            try
            {
                Assert.IsInstanceOfType(value: value, expectedType: expectedType, message: message);
                PassAssert(string.Format(message + " IsInstanceOfType: Value [{0}]   ExpectedType [{1}] - PASS", value, expectedType));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " IsInstanceOfType: Value [{0}]   ExpectedType [{1}] - FAIL", value, expectedType));
                Result = false;
                if (Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


         /// <summary>
         /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType().
         /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
         /// </summary>
         /// <param name="value">The input object to be evaluated</param>
         /// <param name="expectedType">Expected type of value</param>
         /// <param name="message">Optional message string displayed in the log</param>
         /// <param name="parameters">An array of parameters to use when formatting message.</param>
         /// <returns></returns>
         public bool IsInstanceOfType(
            Object value,
            Type expectedType,
            string message = "",
            params Object[] parameters
        )
         {
             try
             {
                 Assert.IsInstanceOfType(value: value, expectedType: expectedType, message: message, parameters: parameters);
                 PassAssert(string.Format(message + " IsInstanceOfType: Value [{0}]   ExpectedType [{1}] - PASS", value, expectedType));
                 return true;
             }
             catch (AssertFailedException e)
             {
                 FailAssert(string.Format(message + " IsInstanceOfType: Value [{0}]   ExpectedType [{1}] - FAIL", value, expectedType));
                 Result = false;
                 if (Config.ForceThrowExceptionOnAssertFail) throw;
                 return false;
             }
         }


    }


}
