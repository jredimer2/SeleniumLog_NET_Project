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
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="value">The input object to be evaluated</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
         public static bool IsNotNull(
	        Object value,
            string message = ""
        ) 
        {
            try
            {
                Assert.IsNotNull(value: value, message: message);
                PassAssert(string.Format(message + " IsNotNull: Value [{0}] - PASS", value));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " IsNotNull: Value [{0}]  - FAIL", value));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


         /// <summary>
         /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull().
         /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
         /// </summary>
         /// <param name="value">The input object to be evaluated</param>
         /// <param name="message">Optional message string displayed in the log</param>
         /// <param name="parameters">An array of parameters to use when formatting message.</param>
         /// <returns></returns>
         public static bool IsNotNull(
            Object value,
            string message = "",
            params Object[] parameters
        )
         {
             try
             {
                 Assert.IsNotNull(value: value, message: message, parameters: parameters);
                 PassAssert(string.Format(message + " IsNotNull: Value [{0}] - PASS", value));
                 return true;
             }
             catch (AssertFailedException e)
             {
                 FailAssert(string.Format(message + " IsNotNull: Value [{0}]  - FAIL", value));
                 if (log.Config.ForceThrowExceptionOnAssertFail)  throw;
                 return false;
             }
         }


    }


}
