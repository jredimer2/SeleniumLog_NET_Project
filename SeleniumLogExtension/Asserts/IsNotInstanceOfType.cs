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
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotInstanceOfType().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="value">The input object to be evaluated</param>
        /// <param name="wrongType">The wrong type of value</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
         public static bool IsNotInstanceOfType(
	        Object value,
	        Type wrongType,
            string message = ""
        ) 
        {
            try
            {
                Assert.IsNotInstanceOfType(value: value, wrongType: wrongType, message: message);
                PassAssert(string.Format(message + " IsNotInstanceOfType: Value [{0}]   WrongType [{1}] - PASS", value, wrongType));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " IsNotInstanceOfType: Value [{0}]   WrongType [{1}] - FAIL", value, wrongType));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


         /// <summary>
         /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotInstanceOfType().
         /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
         /// </summary>
         /// <param name="value">The input object to be evaluated</param>
         /// <param name="wrongType">The wrong type of value</param>
         /// <param name="message">Optional message string displayed in the log</param>
         /// <param name="parameters">An array of parameters to use when formatting message.</param>
         /// <returns></returns>
         public static bool IsNotInstanceOfType(
            Object value,
            Type wrongType,
            string message = "",
            params Object[] parameters
        )
         {
             try
             {
                 Assert.IsNotInstanceOfType(value: value, wrongType: wrongType, message: message, parameters: parameters);
                 PassAssert(string.Format(message + " IsNotInstanceOfType: Value [{0}]   WrongType [{1}] - PASS", value, wrongType));
                 return true;
             }
             catch (AssertFailedException e)
             {
                 FailAssert(string.Format(message + " IsNotInstanceOfType: Value [{0}]   WrongType [{1}] - FAIL", value, wrongType));
                 Result = false;
                 if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                 return false;
             }
         }


    }


}
