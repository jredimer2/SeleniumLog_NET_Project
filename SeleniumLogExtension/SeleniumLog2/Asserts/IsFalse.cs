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

         public bool IsFalse(
            bool condition,
            string message = "",
            bool silent = false,
            bool throwException = false
        ) 
        {
            try
            {
                Assert.IsFalse(condition: condition, message: message);
                if (!silent) PassAssert(string.Format(message + "IsFalse: Condition [{0}] - PASS", condition));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "IsFalse: Condition [{0}] - FAIL", condition));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }
        

         public bool IsFalse(
            bool condition,
            string message = "",
            bool silent = false,
            bool throwException = false,
            params Object[] parameters
        ) 
        {
            try
            {
                Assert.IsFalse(condition: condition, message: message, parameters: parameters);
                if (!silent) PassAssert(string.Format(message + "IsFalse: Condition [{0}] - PASS", condition));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "IsFalse: Condition [{0}] - FAIL", condition));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

    }


}
