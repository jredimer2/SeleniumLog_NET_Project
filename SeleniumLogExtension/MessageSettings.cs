using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SeleniumLogger
{

    public sealed partial class SeleniumLog
    {
        public partial class _MessageSettings
        {
            private bool _ShowLineNumbers;
            private bool _WatchdogStart;
            private bool _WatchdogEnd;
            private int _PrevLineNum;
            private int _LineNum;
            private bool _Pass;
            private bool _Fail;
            private bool _Warning;
            private bool _Error;
            private int _PendingIndent;
            private int _PendingUnindent;
            private int _RunningIndentLevel;
            private bool _Root;
            private Color _RGB;
            private Color _DefaultRGB;
            private string _Image;
            private string _File;
            private string _Path;
            private string _Tab;
            private string _TimestampFormat = "HH:mm:ss.fff";
            private string _MessageStr;
            private string _FormattingStr;
            private Stack _BranchStack = new Stack(); // stack of branches which form a path
            private Stack _PathsStack = new Stack(); // stack of paths
            public IndentModel indentModel = new IndentModel();
            public bool EnableLogging;

            /// <summary>
            /// Constructor
            /// </summary>
            public _MessageSettings()
            {
                ShowLineNumbers = false;
                _PrevLineNum = 0;
                LineNum = 1;
                _RunningIndentLevel = 0;
                //Indent = 0;
                //Unindent = 0;
                ResetDefaultValues();
                EnableLogging = true;
            }

            private void ResetDefaultValues()
            {
                _WatchdogStart = false;
                _WatchdogEnd = false;
                _Pass = false;
                _Fail = false;
                _Warning = false;
                _Error = false;
                _PendingIndent = 0;
                _PendingUnindent = 0;
                _Root = false;
                _RGB = null;
                _DefaultRGB = null;
                _Image = null;
                _File = null;
                //_TimestampFormat = "HH:mm:ss.fff";
                _Path = null;
                _Tab = null;
                _MessageStr = null;
            }

            public bool ShowLineNumbers
            {
                get { return _ShowLineNumbers; }
                set { _ShowLineNumbers = value; }
            }

            public int LineNum
            {
                get { return _LineNum; }
                set { _LineNum = value; }
            }

            public bool WatchdogStart
            {
                get { return _WatchdogStart; }
                set { _WatchdogStart = value; }
            }

            public bool WatchdogEnd
            {
                get { return _WatchdogEnd; }
                set { _WatchdogEnd = value; }
            }

            public bool Pass
            {
                get { return _Pass; }
                set { _Pass = value; }
            }

            public bool Fail
            {
                get { return _Fail; }
                set { _Fail = value; }
            }

            public bool Warning
            {
                get { return _Warning; }
                set { _Warning = value; }
            }

            public bool Error
            {
                get { return _Error; }
                set { _Error = value; }
            }

            public int Indent
            {
                get { return _PendingIndent; }
                set { _PendingIndent = value; }
            }

            public int Unindent
            {
                get { return _PendingUnindent; }
                set { _PendingUnindent = value; }
            }

            public int CurrentIndentLevel
            {
                get { return indentModel.CurrentLevel; }
                //get { return CalculatePendingLevel(); }
                set { }
            }

            public bool Root
            {
                get { return _Root; }
                set { _Root = value; }
            }

            public Color RGB
            {
                get { return _RGB; }
                set { _RGB = value; }
            }

            public Color DefaultRGB
            {
                get { return _DefaultRGB; }
                set { _DefaultRGB = value; }
            }

            public string Image
            {
                get { return _Image; }
                set { _Image = value; }
            }

            public string File
            {
                get { return _File; }
                set { _File = value; }
            }

            public string TimestampFormat
            {
                get { return _TimestampFormat; }
                set { _TimestampFormat = value; }
            }

            public string Tab
            {
                get { return _Tab; }
                set { _Tab = value; }
            }
            public string MessageStr
            {
                get { return _MessageStr; }
                set { _MessageStr = value; }
            }
            public string FormattingStr
            {
                get { return _FormattingStr; }
                set { _FormattingStr = value; }
            }
            public string Path
            {
                get { return _Path; }
                set
                {
                    _Path = value;
                    //PathStack.Push(value);
                    //_Path = FormPathString();
                }
            }
            /*
            public int CountIndents(string RETURN_STR)
            {
                string res0 = Regex.Replace(input: RETURN_STR, pattern: @"\<.*\>", replacement: "_____FORMATING_STRING_____");
                string res1 = Regex.Replace(input: RETURN_STR, pattern: @"\<.*\>", replacement: "_____FORMATING_STRING_____");

            }
            */

        }
    }
}
