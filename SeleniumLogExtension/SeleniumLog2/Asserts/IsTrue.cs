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

        public bool IsTrue(
           bool condition,
           string message = "",
           bool silent = false,
           bool throwException = false
       ) 
        {
            try
            {
                Assert.IsTrue(condition: condition, message: message);
                if (!silent) PassAssert(string.Format(message + "IsTrue: Condition [{0}] - PASS", condition));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "IsTrue: Condition [{0}] - FAIL", condition));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        public bool IsTrue(
            bool condition,
            string message = "",
            bool silent = false,
            bool throwException = false,
            params Object[] parameters
        ) 
        {
            try
            {
                Assert.IsTrue(condition: condition, message: message, parameters: parameters);
                if (!silent) PassAssert(string.Format(message + "IsTrue: Condition [{0}] - PASS", condition));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "IsTrue: Condition [{0}] - FAIL", condition));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

    }


}
