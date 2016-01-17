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

         public bool IsNotInstanceOfType(
	        Object value,
	        Type wrongType,
            string message = "",
            bool silent = false,
            bool throwException = false
        ) 
        {
            try
            {
                Assert.IsNotInstanceOfType(value: value, wrongType: wrongType, message: message);
                if (!silent) PassAssert(string.Format(message + "IsNotInstanceOfType: Value [{0}]   WrongType [{1}] - PASS", value, wrongType));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "IsNotInstanceOfType: Value [{0}]   WrongType [{1}] - FAIL", value, wrongType));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }



         public bool IsNotInstanceOfType(
            Object value,
            Type wrongType,
            string message = "",
            bool silent = false,
            bool throwException = false,
            params Object[] parameters
        )
         {
             try
             {
                 Assert.IsNotInstanceOfType(value: value, wrongType: wrongType, message: message, parameters: parameters);
                 if (!silent) PassAssert(string.Format(message + "IsNotInstanceOfType: Value [{0}]   WrongType [{1}] - PASS", value, wrongType));
                 return true;
             }
             catch (AssertFailedException e)
             {
                 if (!silent) FailAssert(string.Format(message + "IsNotInstanceOfType: Value [{0}]   WrongType [{1}] - FAIL", value, wrongType));
                 Result = false;
                 if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                 return false;
             }
         }


    }


}
