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
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Inconclusive().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <returns></returns>
        public void Inconclusive(
            string message = ""
        ) 
        {
            Assert.Inconclusive(message: message);
            Warning().WriteLine(string.Format(message + " Inconclusive assert."));
            if (Config.ForceThrowExceptionOnAssertFail)  throw new AssertInconclusiveException();
        }

        /// <summary>
        /// Wrapper to Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Inconclusive().
        /// Takes screenshot on passed or failed assert. Can be forced not to throw exception (via SeleniumLog.config) if assert fails.
        /// </summary>
        /// <param name="message">Optional message string displayed in the log</param>
        /// <param name="parameters">An array of parameters to use when formatting message.</param>
        /// <returns></returns>
        public void Inconclusive(
            string message = "",
            params Object[] parameters
        )
        {
            Assert.Inconclusive(message: message, parameters: parameters);
            Warning().WriteLine(string.Format(message + " Inconclusive assert."));
            if (Config.ForceThrowExceptionOnAssertFail)  throw new AssertInconclusiveException();
        }


    }


}
