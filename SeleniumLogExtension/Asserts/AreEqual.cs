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

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="expected">Expected object</param>
        /// <param name="actual">Actual object</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public bool AreEqual(
            Object expected,
            Object actual,
            string message = ""
        ) 
        {
            try
            {
                Assert.AreEqual(expected: expected, actual: actual, message: message);
                PassAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - PASS", expected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - FAIL", expected, actual));
                Result = false;
                if (Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="expected">Expected double-precision floating point number</param>
        /// <param name="actual">Actual double-precision floating point number</param>
        /// <param name="delta">Tolerance value</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public bool AreEqual(
	        double expected,
	        double actual,
	        double delta,
            string message = ""
        )
        {
            try
            {
                Assert.AreEqual(expected: expected, actual: actual, delta: delta, message: message);
                PassAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - PASS", expected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - FAIL", expected, actual, delta));
                Result = false;
                if (Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="expected">Expected single-precision floating point number</param>
        /// <param name="actual">Actual single-precision floating point number</param>
        /// <param name="delta">Tolerance value</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public bool AreEqual(
	        float expected,
	        float actual,
	        float delta,
            string message = ""
        )
        {
            try
            {
                Assert.AreEqual(expected: expected, actual: actual, delta: delta, message: message);
                PassAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - PASS", expected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - FAIL", expected, actual, delta));
                Result = false;
                if (Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="expected">Expected string</param>
        /// <param name="actual">Actual string</param>
        /// <param name="ignoreCase">Set to true to ignore case in string comparison.</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public bool AreEqual(
            string expected,
            string actual,
            bool ignoreCase,
            string message = ""
        )
        {
            try
            {
                Assert.AreEqual(expected: expected, actual: actual, ignoreCase: ignoreCase, message: message);
                PassAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}] - PASS", expected, actual, ignoreCase));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}] - FAIL", expected, actual, ignoreCase));
                Result = false;
                if (Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="expected">Expected object</param>
        /// <param name="actual">Actual object</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <param name="parameters">An array of parameters to use when formatting message.</param>
        /// <returns></returns>
        public bool AreEqual(
	        Object expected,
	        Object actual,
	        string message = "",
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreEqual(expected: expected, actual: actual, message: message, parameters: parameters);
                PassAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}] - PASS", expected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}] - FAIL", expected, actual));
                Result = false;
                if (Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="expected">Expected string</param>
        /// <param name="actual">Actual string</param>
        /// <param name="ignoreCase">Set to true to ignore case in string comparison.</param>
        /// <param name="culture">A CultureInfo object that supplies culture-specific comparison information.</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public bool AreEqual(
	        string expected,
	        string actual,
	        bool ignoreCase,
	        CultureInfo culture,
            string message = ""
        )
        {
            try
            {
                Assert.AreEqual(expected: expected, actual: actual, message: message, ignoreCase: ignoreCase, culture: culture);
                PassAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}] - PASS", expected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}] - FAIL", expected, actual));
                Result = false;
                if (Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="expected">Expected double-precision floating point number</param>
        /// <param name="actual">Actual double-precision floating point number</param>
        /// <param name="delta">Tolerance value</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <param name="parameters">An array of parameters to use when formatting message.</param>
        /// <returns></returns>
        public bool AreEqual(
            double expected,
            double actual,
            double delta,
            string message = "",
            params Object[] parameters
)
        {
            try
            {
                Assert.AreEqual(expected: expected, actual: actual, message: message, delta: delta, parameters: parameters);
                PassAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - PASS", expected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - FAIL", expected, actual, delta));
                Result = false;
                if (Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="expected">Expected single-precision floating point number</param>
        /// <param name="actual">Actual single-precision floating point number</param>
        /// <param name="delta">Tolerance value</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <param name="parameters">An array of parameters to use when formatting message.</param>
        /// <returns></returns>
        public bool AreEqual(
            float expected,
            float actual,
            float delta,
            string message = "",
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreEqual(expected: expected, actual: actual, message: message, delta: delta, parameters: parameters);
                PassAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - PASS", expected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}]   Delta [{2}] - FAIL", expected, actual, delta));
                Result = false;
                if (Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="expected">Expected generic data type</param>
        /// <param name="actual">Actual generic data type</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public bool AreEqual<T>(
            T expected,
            T actual,
            string message = ""
        )
        {
            try
            {
                Assert.AreEqual<T>(expected: expected, actual: actual, message: message);
                PassAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}] - PASS", expected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}] - FAIL", expected, actual));
                Result = false;
                if (Config.ForceThrowExceptionOnAssertFail)  throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="expected">Expected generic data type</param>
        /// <param name="actual">Actual generic data type</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <param name="parameters">An array of parameters to use when formatting message.</param>
        /// <returns></returns>
        public bool AreEqual<T>(
            T expected,
            T actual,
            string message = "",
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreEqual<T>(expected: expected, actual: actual, message: message, parameters: parameters);
                PassAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}] - PASS", expected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreEqual: Expected [{0}]   Actual [{1}] - FAIL", expected, actual));
                Result = false;
                if (Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

    }


}
