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
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="notExpected">The not expected object</param>
        /// <param name="actual">Actual object</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public static bool AreNotEqual(
            Object notExpected,
            Object actual,
            string message = ""
        ) 
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, message: message);
                 PassAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="notExpected">The not expected double-precision floating point number</param>
        /// <param name="actual">Actual double-precision floating point number</param>
        /// <param name="delta">Tolerance value</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public static bool AreNotEqual(
            double notExpected,
            double actual,
            double delta,
            string message = ""            
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, delta: delta, message: message);
                PassAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}]- PASS", notExpected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}] - FAIL", notExpected, actual, delta));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="notExpected">The not expected single-precision floating point number</param>
        /// <param name="actual">Actual single-precision floating point number</param>
        /// <param name="delta">Tolerance value</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public static bool AreNotEqual(
            float notExpected,
            float actual,
            float delta,
            string message = ""
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, delta: delta, message: message);
                PassAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}]- PASS", notExpected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}] - FAIL", notExpected, actual, delta));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="notExpected">The not expected string</param>
        /// <param name="actual">Actual string</param>
        /// <param name="ignoreCase">Set to true to ignore case in string comparison.</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public static bool AreNotEqual(
	        string notExpected,
	        string actual,
	        bool ignoreCase,
            string message = ""
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, ignoreCase: ignoreCase, message: message);
                PassAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="notExpected">The not expected object</param>
        /// <param name="actual">Actual object</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <param name="parameters">An array of parameters to use when formatting message.</param>
        /// <returns></returns>
        public static bool AreNotEqual(
            Object notExpected,
            Object actual,
            string message = "",           
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, message: message, parameters: parameters);
                PassAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="notExpected">The not expected double-precision floating point number</param>
        /// <param name="actual">Actual double-precision floating point number</param>
        /// <param name="delta">Tolerance value</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <param name="parameters">An array of parameters to use when formatting message.</param>
        /// <returns></returns>
        public static bool AreNotEqual(
            double notExpected,
            double actual,
            double delta,
            string message = "",          
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, message: message, parameters: parameters);
                PassAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}] - PASS", notExpected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}] - FAIL", notExpected, actual, delta));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="notExpected">The not expected single-precision floating point number</param>
        /// <param name="actual">Actual single-precision floating point number</param>
        /// <param name="delta">Tolerance value</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <param name="parameters">An array of parameters to use when formatting message.</param>
        /// <returns></returns>
        public static bool AreNotEqual(
            float notExpected,
            float actual,
            float delta,
            string message = "",           
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, message: message, parameters: parameters);
                PassAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}] - PASS", notExpected, actual, delta));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}]   Delta [{2}] - FAIL", notExpected, actual, delta));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="notExpected">The not expected string</param>
        /// <param name="actual">Actual string</param>
        /// <param name="ignoreCase">Set to true to ignore case in string comparison.</param>
        /// <param name="culture">A CultureInfo object that supplies culture-specific comparison information.</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public static bool AreNotEqual(
	        string notExpected,
	        string actual,
	        bool ignoreCase,
	        CultureInfo culture,
            string message = ""
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, ignoreCase: ignoreCase, message: message, culture: culture);
                PassAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="notExpected">The not expected string</param>
        /// <param name="actual">Actual string</param>
        /// <param name="ignoreCase">Set to true to ignore case in string comparison.</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <param name="parameters">An array of parameters to use when formatting message.</param>
        /// <returns></returns>
        public static bool AreNotEqual(
            string notExpected,
            string actual,
            bool ignoreCase,
            string message = "",         
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, ignoreCase: ignoreCase, message: message, parameters: parameters);
                PassAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="notExpected">The not expected string</param>
        /// <param name="actual">Actual string</param>
        /// <param name="ignoreCase">Set to true to ignore case in string comparison.</param>
        /// <param name="culture">A CultureInfo object that supplies culture-specific comparison information.</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <param name="parameters">An array of parameters to use when formatting message.</param>
        /// <returns></returns>
        public static bool AreNotEqual(
            string notExpected,
            string actual,
            bool ignoreCase,
            CultureInfo culture,
            string message = "",           
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, ignoreCase: ignoreCase, message: message, culture: culture, parameters: parameters);
                PassAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="expected">Expected generic data type</param>
        /// <param name="actual">Actual generic data type</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public static bool AreNotEqual<T>(
	        T notExpected,
	        T actual,
            string message = ""
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, message: message);
                PassAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="expected">Expected generic data type</param>
        /// <param name="actual">Actual generic data type</param>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <param name="parameters">An array of parameters to use when formatting message.</param>
        /// <returns></returns>
        public static bool AreNotEqual<T>(
            T notExpected,
            T actual,
            string message = "",           
            params Object[] parameters
        )
        {
            try
            {
                Assert.AreNotEqual(notExpected: notExpected, actual: actual, message: message, parameters: parameters);
                PassAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - PASS", notExpected, actual));
                return true;
            }
            catch (AssertFailedException e)
            {
                FailAssert(string.Format(message + " AreNotEqual: Input1 [{0}]   Input2 [{1}] - FAIL", notExpected, actual));
                Result = false;
                if (log.Config.ForceThrowExceptionOnAssertFail) throw;
                return false;
            }
        }


    }


}
