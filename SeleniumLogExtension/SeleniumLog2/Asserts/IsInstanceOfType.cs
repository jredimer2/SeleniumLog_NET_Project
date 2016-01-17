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

         public bool IsInstanceOfType(
	        Object value,
	        Type expectedType,
            string message = "",
            bool silent = false,
            bool throwException = false
        ) 
        {
            try
            {
                Assert.IsInstanceOfType(value: value, expectedType: expectedType, message: message);
                if (!silent) PassAssert(string.Format(message + "IsInstanceOfType: Value [{0}]   ExpectedType [{1}] - PASS", value, expectedType));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "IsInstanceOfType: Value [{0}]   ExpectedType [{1}] - FAIL", value, expectedType));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }



         public bool IsInstanceOfType(
            Object value,
            Type expectedType,
            string message = "",
            bool silent = false,
            bool throwException = false,
            params Object[] parameters
        )
         {
             try
             {
                 Assert.IsInstanceOfType(value: value, expectedType: expectedType, message: message, parameters: parameters);
                 if (!silent) PassAssert(string.Format(message + "IsInstanceOfType: Value [{0}]   ExpectedType [{1}] - PASS", value, expectedType));
                 return true;
             }
             catch (AssertFailedException e)
             {
                 if (!silent) FailAssert(string.Format(message + "IsInstanceOfType: Value [{0}]   ExpectedType [{1}] - FAIL", value, expectedType));
                 Result = false;
                 if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                 return false;
             }
         }


    }


}
