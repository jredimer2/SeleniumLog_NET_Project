using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace SeleniumLogger
{
    public sealed partial class SeleniumLog
    {
        private Stack<string> pnStack = new Stack<string>();
        private string _ParentNode = "";

        private class _Config
        {
            public string TimestampFormat { get; set; }
            public string OutputFilePath { get; set; }
            public string ScreenshotsFolder { get; set; }
            public bool ScreenshotOnEveryMessage { get; set; }
            public bool ScreenshotOnEveryPass { get; set; }
            public bool ScreenshotOnEveryFail { get; set; }
            public bool ScreenshotOnEveryError { get; set; }
            public bool ScreenshotOnEveryWarning { get; set; }
            public bool ForceThrowExceptionOnAssertFail { get; set; }
            public bool LogLowLevelSeleniumCommands { get; set; }

            public bool OnClick_LogFunctionStart { get; set; }
            public bool OnClick_LogFunctionEnd { get; set; }
            public bool OnClick_ScreenshotOnStart { get; set; }
            public bool OnClick_ScreenshotOnEnd { get; set; }


            public _Config() { }

            public void Display()
            {
                bool debug = false;

                if (debug) Console.WriteLine("TimestampFormat = " + TimestampFormat  ); 
                if (debug) Console.WriteLine("OutputFilePath = " + OutputFilePath  ); 
                if (debug) Console.WriteLine("ScreenshotsFolder = " + ScreenshotsFolder  ); 
                if (debug) Console.WriteLine("ScreenshotOnEveryMessage = " + ScreenshotOnEveryMessage  ); 
                if (debug) Console.WriteLine("ScreenshotOnEveryPass = " +  ScreenshotOnEveryPass ); 
                if (debug) Console.WriteLine("ScreenshotOnEveryFail = " +  ScreenshotOnEveryFail ); 
                if (debug) Console.WriteLine("ScreenshotOnEveryError = " + ScreenshotOnEveryError  ); 
                if (debug) Console.WriteLine("ScreenshotOnEveryWarning = " + ScreenshotOnEveryWarning  ); 
                if (debug) Console.WriteLine("ForceThrowExceptionOnAssertFail = " + ForceThrowExceptionOnAssertFail  ); 
                if (debug) Console.WriteLine("LogLowLevelSeleniumCommands = " + LogLowLevelSeleniumCommands  );
                if (debug) Console.WriteLine("");
                if (debug) Console.WriteLine("OnClick_LogFunctionStart = " + OnClick_LogFunctionStart  ); 
                if (debug) Console.WriteLine("OnClick_LogFunctionEnd = " + OnClick_LogFunctionEnd  ); 
                if (debug) Console.WriteLine("OnClick_ScreenshotOnStart = " + OnClick_ScreenshotOnStart  );
                if (debug) Console.WriteLine("OnClick_ScreenshotOnEnd = " + OnClick_ScreenshotOnEnd);

                if (debug) Console.WriteLine("Press ENTER to exit");
                //if (debug) Console.ReadLine();
            }
        }

        _Config Config = new _Config();

        private void ParseXML(bool debug = false)
        {

            XPathDocument xmlPathDoc = new XPathDocument("SeleniumLog.config");
            XPathNavigator xNav = xmlPathDoc.CreateNavigator();

            xNav.MoveToRoot();

            TraverseChildren(xNav);

            Config.Display();


        }

        /// <summary>
        /// Reference: https://support.microsoft.com/en-us/kb/308343
        /// </summary>
        /// <param name="xNav"></param>
        public void TraverseChildren(XPathNavigator xNav)
        {
            xNav.MoveToFirstChild();

            do
            {
                //Find the first element.
                if (xNav.NodeType == XPathNodeType.Element)
                {

                    //Determine whether children exist.
                    if (xNav.HasChildren == true)
                    {
                        //Console.Write("The XML string for this child ");
                        //Console.WriteLine("is {0} = {1}", xNav.Name, xNav.Value);

                        switch (xNav.Name) {
                            case "timestamp_format" :
                                Config.TimestampFormat = xNav.Value;
                                break;
                            case "output_file_path":
                                Config.OutputFilePath = xNav.Value;
                                break;
                            case "screenshots_folder":
                                Config.ScreenshotsFolder = xNav.Value;
                                break;
                            case "screenshot_on_every_message":
                                Config.ScreenshotOnEveryMessage = Convert.ToBoolean(xNav.Value);
                                break;
                            case "screenshot_on_every_pass":
                                Config.ScreenshotOnEveryPass = Convert.ToBoolean(xNav.Value);
                                break;
                            case "screenshot_on_every_fail":
                                Config.ScreenshotOnEveryFail = Convert.ToBoolean(xNav.Value);
                                break;
                            case "screenshot_on_every_error":
                                Config.ScreenshotOnEveryError = Convert.ToBoolean(xNav.Value);
                                break;
                            case "force_throw_exception_on_assert_fail":
                                Config.ScreenshotOnEveryFail = Convert.ToBoolean(xNav.Value);
                                break;
                            case "on_click_event":
                                _ParentNode = "OnClick";
                                break;
                            case "log_function_start":
                                if (_ParentNode == "OnClick")
                                    Config.OnClick_LogFunctionStart = Convert.ToBoolean(xNav.Value);
                                break;
                            case "log_function_end":
                                if (_ParentNode == "OnClick")
                                    Config.OnClick_LogFunctionEnd = Convert.ToBoolean(xNav.Value);
                                break;
                            case "screenshot_on_start":
                                if (_ParentNode == "OnClick")
                                    Config.OnClick_ScreenshotOnStart = Convert.ToBoolean(xNav.Value);
                                break;
                            case "screenshot_on_end":
                                if (_ParentNode == "OnClick")
                                    Config.OnClick_ScreenshotOnEnd = Convert.ToBoolean(xNav.Value);
                                break;                             

                            default:
                                break;

                        } // end switch

                        TraverseChildren(xNav);
                        xNav.MoveToParent();

                    }
                }
            } while (xNav.MoveToNext());
        }

    }
}
