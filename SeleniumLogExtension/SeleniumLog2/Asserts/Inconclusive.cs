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

        public void Inconclusive(
            string message = "",
            bool throwException = false,
            bool silent = false
        ) 
        {
            Assert.Inconclusive(message: message);
            if (!silent) Warning().WriteLine(string.Format(message + "Inconclusive assert."));
            if (throwException || Config.ForceThrowExceptionOnAssertFail)  throw new AssertInconclusiveException();
        }


        public void Inconclusive(
            string message = "",
            bool throwException = false,
            bool silent = false,
            params Object[] parameters
        )
        {
            Assert.Inconclusive(message: message, parameters: parameters);
            if (!silent) Warning().WriteLine(string.Format(message + "Inconclusive assert."));
            if (throwException || Config.ForceThrowExceptionOnAssertFail)  throw new AssertInconclusiveException();
        }


    }


}
