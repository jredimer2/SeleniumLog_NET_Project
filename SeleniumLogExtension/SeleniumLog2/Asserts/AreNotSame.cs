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
        
        public bool AreNotSame(
            Object notExpected,
            Object actual,
            string message = "",
            bool throwException = false,
            bool silent = false
        ) 
        {
            try
            {
                Assert.AreNotSame(notExpected: notExpected, actual: actual, message: message);
                if (!silent) PassAssert(string.Format(message + "AreNotSame: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreNotSame: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        public bool AreNotSame(
            Object notExpected,
            Object actual,
            string message = "",
            bool throwException = false,
            bool silent = false,
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreNotSame(notExpected: notExpected, actual: actual, message: message, parameters: parameters);
                if (!silent) PassAssert(string.Format(message + "AreNotSame: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreNotSame: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        
    }


}
