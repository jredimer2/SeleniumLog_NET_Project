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

         public bool IsNotNull(
	        Object value,
            string message = "",
            bool silent = false,
            bool throwException = false
        ) 
        {
            try
            {
                Assert.IsNotNull(value: value, message: message);
                if (!silent) PassAssert(string.Format(message + "IsNotNull: Value [{0}] - PASS", value));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "IsNotNull: Value [{0}]  - FAIL", value));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }



         public bool IsNotNull(
            Object value,
            string message = "",
            bool silent = false,
            bool throwException = false,
            params Object[] parameters
        )
         {
             try
             {
                 Assert.IsNotNull(value: value, message: message, parameters: parameters);
                 if (!silent) PassAssert(string.Format(message + "IsNotNull: Value [{0}] - PASS", value));
                 return true;
             }
             catch (AssertFailedException e)
             {
                 if (!silent) FailAssert(string.Format(message + "IsNotNull: Value [{0}]  - FAIL", value));
                 if (throwException || Config.ForceThrowExceptionOnAssertFail)  throw;
                 return false;
             }
         }


    }


}
