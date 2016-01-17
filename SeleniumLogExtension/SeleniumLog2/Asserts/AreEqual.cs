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
        private void PassAssert(string message) {
                Pass().WriteLine(message);
        }

        private void FailAssert(string message = "")
        {
            Fail().WriteLine(message);
        }

        public bool AreEqual(
            Object expected,
            Object actual,
            string message = "",
            bool silent = false,
            bool throwException = false
        ) 
        {
            try
            {
                Assert.AreEqual(expected: expected, actual: actual, message: message);
                if (!silent) PassAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}] - PASS", expected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}] - FAIL", expected, actual));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        public bool AreEqual(
	        double expected,
	        double actual,
	        double delta,
            string message = "",
            bool silent = false,
            bool throwException = false
        )
        {
            try
            {
                Assert.AreEqual(expected: expected, actual: actual, delta: delta, message: message);
                if (!silent) PassAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - PASS", expected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - FAIL", expected, actual, delta));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        public bool AreEqual(
	        float expected,
	        float actual,
	        float delta,
            string message = "",
            bool silent = false,
            bool throwException = false
        )
        {
            try
            {
                Assert.AreEqual(expected: expected, actual: actual, delta: delta, message: message);
                if (!silent) PassAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - PASS", expected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - FAIL", expected, actual, delta));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        public bool AreEqual(
            string expected,
            string actual,
            bool ignoreCase,
            string message = "",
            bool silent = false,
            bool throwException = false
        )
        {
            try
            {
                Assert.AreEqual(expected: expected, actual: actual, ignoreCase: ignoreCase, message: message);
                if (!silent) PassAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}] - PASS", expected, actual, ignoreCase));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}] - FAIL", expected, actual, ignoreCase));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        public bool AreEqual(
	        Object expected,
	        Object actual,
	        string message = "",
            bool throwException = false,
            bool silent = false,
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreEqual(expected: expected, actual: actual, message: message, parameters: parameters);
                if (!silent) PassAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}] - PASS", expected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}] - FAIL", expected, actual));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        public bool AreEqual(
	        string expected,
	        string actual,
	        bool ignoreCase,
	        CultureInfo culture,
            string message = "",
            bool throwException = false,
            bool silent = false
)
        {
            try
            {
                Assert.AreEqual(expected: expected, actual: actual, message: message, ignoreCase: ignoreCase, culture: culture);
                if (!silent) PassAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}] - PASS", expected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}] - FAIL", expected, actual));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        public bool AreEqual(
            double expected,
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
                Assert.AreEqual(expected: expected, actual: actual, message: message, delta: delta, parameters: parameters);
                if (!silent) PassAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - PASS", expected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - FAIL", expected, actual, delta));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        public bool AreEqual(
            float expected,
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
                Assert.AreEqual(expected: expected, actual: actual, message: message, delta: delta, parameters: parameters);
                if (!silent) PassAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - PASS", expected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - FAIL", expected, actual, delta));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        public bool AreEqual<T>(
            T expected,
            T actual,
            string message = "",
            bool throwException = false,
            bool silent = false
        )
        {
            try
            {
                Assert.AreEqual<T>(expected: expected, actual: actual, message: message);
                if (!silent) PassAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}] - PASS", expected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}] - FAIL", expected, actual));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail)  throw;
                return false;
            }
        }

        public bool AreEqual<T>(
            T expected,
            T actual,
            string message = "",
            bool throwException = false,
            bool silent = false,
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreEqual<T>(expected: expected, actual: actual, message: message, parameters: parameters);
                if (!silent) PassAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}] - PASS", expected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                if (!silent) FailAssert(string.Format(message + "AreEqual: Expected [{0}]   Actual [{1}] - FAIL", expected, actual));
                Result = false;
                if (throwException || Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

    }


}
