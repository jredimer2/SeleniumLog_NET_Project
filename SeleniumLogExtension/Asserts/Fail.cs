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
        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public void Fail(
            string message = ""
        ) 
        {
            Assert.Fail(message: message);
            FailAssert(string.Format(message + " Fail assert."));
            Result = false;
            if (Config.ForceThrowExceptionOnAssertFail) throw new AssertFailedException();
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <param name="parameters">An array of parameters to use when formatting message.</param>
        /// <returns></returns>
        public void Fail(
            string message = "",
            params Object[] parameters
        )
        {
            Assert.Fail(message: message, parameters: parameters);
            FailAssert(string.Format(message + " Fail assert."));
            Result = false;
            if (Config.ForceThrowExceptionOnAssertFail) throw new AssertFailedException();
        }

    }


}
