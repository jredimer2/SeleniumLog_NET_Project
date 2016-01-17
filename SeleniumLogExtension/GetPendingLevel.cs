using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumLogger
{

    public sealed partial class SeleniumLog
    {
        public partial class _MessageSettings
        {
            /// <summary>
            /// Aggregate all settings and finally generate the string that will be written to the text log.
            /// </summary>
            /// <returns></returns>
            public int GetPendingLevel()
            {

                Stack tmpMessageStack = new Stack();
                string ReturnString = "";
                string PathString = "";
                string FormattingString = "";


                if (Indent > 0)
                {
                    for (int i = 0; i < Indent; i++)
                    {
                        tmpMessageStack.Push("INDENT");
                    }

                    //if (Indent > 1)
                    //{
                    //    CurrentIndentLevel = CurrentIndentLevel - Indent + 1;  //adjust, because INDENT;INDENT;INDENT really only causes one indent.
                    //}
                    //Indent = 0;
                }

                if (Unindent > 0)
                {
                    for (int i = 0; i < Unindent; i++)
                    {
                        tmpMessageStack.Push("UNINDENT");
                    }
                    //Unindent = 0;
                }

                if (Root == true)
                {
                    tmpMessageStack.Push("ROOT");
                    //Root = false;
                }



                // Form the FormattingString    
                foreach (Object StackObj in tmpMessageStack)
                {
                    //if (obj.ToString
                    FormattingString = (StackObj.ToString() + ";" + FormattingString).TrimStart(';').TrimEnd(';');
                }


                // Form ReturnString
                if (FormattingString == "" || FormattingString == null)
                {
                    ReturnString = "<" + "TIMESTAMP:" + DateTime.Now.ToString(_TimestampFormat) + ">" + MessageStr;
                }
                else
                {
                    ReturnString = "<" + FormattingString + ";TIMESTAMP:" + DateTime.Now.ToString(_TimestampFormat) + ">" + MessageStr;
                }

                //ResetDefaultValues();

                indentModel.CalculatePendingLevel(ReturnString);
                //Console.WriteLine("GET_PENDING2: indentModel.CurrentLevel = " + indentModel.CurrentLevel + ", indentModel.PendingLevel = " + indentModel.PendingLevel);
                return indentModel.CurrentLevel + indentModel.PendingDelta; //NET PENDING LEVEL
                //return indentModel.PendingLevel;
            }
            // Stack Code:
            // http://msdn.microsoft.com/en-us/library/system.collections.stack%28v=vs.110%29.aspx

        }
    }
}
