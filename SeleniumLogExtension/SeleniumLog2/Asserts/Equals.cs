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

        public bool Equals(
	        Object objA,
	        Object objB,
            string message = "",
            bool silent = false
        ) 
        {
            bool result = Assert.Equals(objA: objA, objB: objB);
            if (result) {
                if (!silent) PassAssert(string.Format(message + "Equals: Expected [{0}]   Actual [{1}] - PASS", objA, objB));
            } else {
                if (!silent) FailAssert(string.Format(message + "Equals: Expected [{0}]   Actual [{1}] - FAIL", objA, objB));
            }
            return result;
        }


    }


}
