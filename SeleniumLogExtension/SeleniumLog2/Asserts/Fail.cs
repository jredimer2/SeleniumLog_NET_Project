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

        public void Fail(
            string message = "",
            bool throwException = false,
            bool silent = false
        ) 
        {
            Assert.Fail(message: message);
            if (!silent) FailAssert(string.Format(message + "Fail assert."));
            Result = false;
            if (throwException || Config.ForceThrowExceptionOnAssertFail) throw new AssertFailedException();
        }


        public void Fail(
            string message = "",
            bool throwException = false,
            bool silent = false,
            params Object[] parameters
        )
        {
            Assert.Fail(message: message, parameters: parameters);
            if (!silent) FailAssert(string.Format(message + "Fail assert."));
            Result = false;
            if (throwException || Config.ForceThrowExceptionOnAssertFail) throw new AssertFailedException();
        }

    }


}
