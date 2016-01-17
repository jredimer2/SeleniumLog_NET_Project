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


        public bool AreNotEqual(
            Object notExpected,
            Object actual,
            string message = "",
            bool throwException = false,
            bool silent = false
        ) 
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, message: message);
                if (!silent) PassAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        public bool AreNotEqual(
            double notExpected,
            double actual,
            double delta,
            string message = "",
            bool throwException = false,
            bool silent = false            
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, delta: delta, message: message);
                if (!silent) PassAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}]- PASS", notExpected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}] - FAIL", notExpected, actual, delta));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        public bool AreNotEqual(
            float notExpected,
            float actual,
            float delta,
            string message = "",
            bool throwException = false,
            bool silent = false
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, delta: delta, message: message);
                if (!silent) PassAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}]- PASS", notExpected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}] - FAIL", notExpected, actual, delta));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        public bool AreNotEqual(
	        string notExpected,
	        string actual,
	        bool ignoreCase,
            string message = "",
            bool throwException = false,
            bool silent = false
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, ignoreCase: ignoreCase, message: message);
                if (!silent) PassAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        public bool AreNotEqual(
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
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, message: message, parameters: parameters);
                if (!silent) PassAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        public bool AreNotEqual(
            double notExpected,
            double actual,
            double delta,
            string message = "",
            bool throwException = false,
            bool silent = false,            
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, message: message, parameters: parameters);
                if (!silent) PassAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}] - PASS", notExpected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}] - FAIL", notExpected, actual, delta));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        public bool AreNotEqual(
            float notExpected,
            float actual,
            float delta,
            string message = "",
            bool throwException = false,
            bool silent = false,            
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, message: message, parameters: parameters);
                if (!silent) PassAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}] - PASS", notExpected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}] - FAIL", notExpected, actual, delta));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        public bool AreNotEqual(
	        string notExpected,
	        string actual,
	        bool ignoreCase,
	        CultureInfo culture,
            string message = "",
            bool silent = false,
            bool throwException = false
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, ignoreCase: ignoreCase, message: message, culture: culture);
                if (!silent) PassAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        public bool AreNotEqual(
            string notExpected,
            string actual,
            bool ignoreCase,
            string message = "",
            bool throwException = false,
            bool silent = false,            
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, ignoreCase: ignoreCase, message: message, parameters: parameters);
                if (!silent) PassAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        public bool AreNotEqual(
            string notExpected,
            string actual,
            bool ignoreCase,
            CultureInfo culture,
            string message = "",
            bool throwException = false,
            bool silent = false,            
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, ignoreCase: ignoreCase, message: message, culture: culture, parameters: parameters);
                if (!silent) PassAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        public bool AreNotEqual<T>(
	        T notExpected,
	        T actual,
            string message = "",
            bool throwException = false,
            bool silent = false
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, message: message);
                if (!silent) PassAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }



        public bool AreNotEqual<T>(
            T notExpected,
            T actual,
            string message = "",
            bool throwException = false,
            bool silent = false,            
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, message: message, parameters: parameters);
                if (!silent) PassAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


    }


}
