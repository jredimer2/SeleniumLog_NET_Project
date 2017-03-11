using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;

namespace XMLConfig
{
    public sealed class XmlConfigurationClass
    {
        public bool EnableSeleniumLog { get; set; }
        public bool RichTextOutput { get; set; }
        public string TimestampFormat { get; set; }
        public string LogFilePath { get; set; }
        public bool WriteLineNumbers { get; set; }

        public bool AutoLaunchSeleniumLogDesktop { get; set; }
        public string SeleniumLogDesktopInstallationFolder { get; set; }

        public string ScreenshotsFolder { get; set; }
        public bool UseFastScreenshot { get; set; }
        public bool TakeScreenshotOnEveryWriteline { get; set; }
        public bool TakeScreenshotOnEveryInfo { get; set; }
        public bool TakeScreenshotOnEveryInfo2 { get; set; }
        public bool TakeScreenshotOnEveryDebug { get; set; }
        public bool TakeScreenshotOnEveryDebug2 { get; set; }
        public bool TakeScreenshotOnEveryPass { get; set; }
        public bool TakeScreenshotOnEveryPass2 { get; set; }
        public bool TakeScreenshotOnEveryFail { get; set; }
        public bool TakeScreenshotOnEveryFail2 { get; set; }
        public bool TakeScreenshotOnEveryError { get; set; }
        public bool TakeScreenshotOnEveryError2 { get; set; }
        public bool TakeScreenshotOnEveryWarning { get; set; }
        public bool TakeScreenshotOnEveryWarning2 { get; set; }
        public bool TakeScreenshotOnEveryFatal { get; set; }
        public bool TakeScreenshotOnEveryFatal2 { get; set; }
        public bool TakeScreenshotOnEveryException { get; set; }

        public bool ForceThrowExceptionOnAssertFail { get; set; }
        public bool EnableFunctionInterceptor { get; set; }
        public bool FunctionEntry_AutoExploreComplexFunctionInputParams { get; set; }
        public bool FunctionEntry_DisplayNullInputs { get; set; }
        public bool FunctionEntry_TakeScreenshotOnEntry { get; set; }
        public bool FunctionExit_AutoExploreComplexFunctionOutputs { get; set; }
        public bool FunctionExit_DisplayNullOutputs { get; set; }
        public bool FunctionExit_TakeScreenshotOnExit { get; set; }
        public bool FunctionExit_EnablePerformanceMeasurement { get; set; }
        public string PerformanceMeasurementUnit { get; set; }

        public bool EnableLoggingOfLowLevelSeleniumWebdriverEvents { get; set; }
        public bool OnNavigating_LogBeforeEvent { get; set; }
        public bool OnNavigating_TakeScreenshotBeforeEvent { get; set; }
        public bool OnNavigating_LogAfterEvent { get; set; }
        public bool OnNavigating_TakeScreenshotAfterEvent { get; set; }
        public bool OnNavigatingBack_LogBeforeEvent { get; set; }
        public bool OnNavigatingBack_TakeScreenshotBeforeEvent { get; set; }
        public bool OnNavigatingBack_LogAfterEvent { get; set; }
        public bool OnNavigatingBack_TakeScreenshotAfterEvent { get; set; }
        public bool OnNavigatingForward_LogBeforeEvent { get; set; }
        public bool OnNavigatingForward_TakeScreenshotBeforeEvent { get; set; }
        public bool OnNavigatingForward_LogAfterEvent { get; set; }
        public bool OnNavigatingForward_TakeScreenshotAfterEvent { get; set; }
        public bool OnClick_LogBeforeEvent { get; set; }
        public bool OnClick_TakeScreenshotBeforeEvent { get; set; }
        public bool OnClick_LogAfterEvent { get; set; }
        public bool OnClick_TakeScreenshotAfterEvent { get; set; }
        public bool OnChangeValue_LogBeforeEvent { get; set; }
        public bool OnChangeValue_TakeScreenshotBeforeEvent { get; set; }
        public bool OnChangeValue_LogAfterEvent { get; set; }
        public bool OnChangeValue_TakeScreenshotAfterEvent { get; set; }
        public bool OnFindElement_LogBeforeEvent { get; set; }
        public bool OnFindElement_TakeScreenshotBeforeEvent { get; set; }
        public bool OnFindElement_LogAfterEvent { get; set; }
        public bool OnFindElement_TakeScreenshotAfterEvent { get; set; }
        public bool OnScriptExecute_LogBeforeEvent { get; set; }
        public bool OnScriptExecute_TakeScreenshotBeforeEvent { get; set; }
        public bool OnScriptExecute_LogAfterEvent { get; set; }
        public bool OnScriptExecute_TakeScreenshotAfterEvent { get; set; }
        public bool OnWebdriverExceptionThrown_LogEvent { get; set; }

        //public bool EnableSeleniumLog { get; set; }
        public bool Enable_Selenium_Webdriver_Trace { get; set; }
        public bool Enable_Generic_Function_Trace { get; set; }
        public bool AutoStartSeleniumLogApp { get; set; }
        public string SeleniumLogAppInstallationFolder { get; set; }
        //public string TimestampFormat { get; set; }
        public string OutputFilePath { get; set; }
        //public string ScreenshotsFolder { get; set; }
        public bool ScreenshotOnEveryMessage { get; set; }
        public bool ScreenshotOnEveryPass { get; set; }
        public bool ScreenshotOnEveryFail { get; set; }
        public bool ScreenshotOnEveryError { get; set; }
        public bool ScreenshotOnEveryWarning { get; set; }
        
        public bool LogLowLevelSeleniumCommands { get; set; }

        public bool OnClick_LogFunctionStart { get; set; }
        public bool OnClick_LogFunctionEnd { get; set; }
        public bool OnClick_ScreenshotOnStart { get; set; }
        public bool OnClick_ScreenshotOnEnd { get; set; }
        public bool FunctionTrace_DisplayNullInputs { get; set; }

        public string ExceptionMessageBuffer = "";

        private static XmlConfigurationClass instance = null;

        public static XmlConfigurationClass Instance()
        {
            //get 
            {
                //lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new XmlConfigurationClass();
                    }
                    return instance;
                }
            }
        }

        private XmlConfigurationClass() 
        {

            ParseXML();
        }

        private void ParseXML(bool debug = false)
        {

            XPathDocument xmlPathDoc = new XPathDocument("SLConfig.xml");
            XPathNavigator xNav = xmlPathDoc.CreateNavigator();

            xNav.MoveToRoot();

            TraverseChildren(xNav);
        }

        private bool TrueOrFalse(string tagname, string val) {
            try {
                if ((val.ToLower() == "true") || (val.ToLower() == "yes"))
                    return true;
                else
                    return false;
            } catch (Exception e) {
                Console.WriteLine("Error: <" + tagname + "> value cannot be converted to boolean. Accepted values are Yes/No or True/False.");
                throw;
            }
        }

        /// <summary>
        /// Reference: https://support.microsoft.com/en-us/kb/308343
        /// </summary>
        /// <param name="xNav"></param>
        public void TraverseChildren(XPathNavigator xNav)
        {
            try
            {
                //string option2;
                string test;
                xNav.MoveToFirstChild();
                string option1 = xNav.GetAttribute("option", "");

                do
                {
                    //Find the first element.
                    if (xNav.NodeType == XPathNodeType.Element)
                    {

                        //Determine whether children exist.
                        if (xNav.HasChildren == true)
                        {
                            //Console.WriteLine("ATTRIB [" + xNav.GetAttribute("option","") + "]");

                            switch(xNav.GetAttribute("option",""))
                            {
                                case "Enable SeleniumLog":
                                    EnableSeleniumLog = TrueOrFalse("Enable SeleniumLog", xNav.Value);
                                    break;

                                case "Rich text output":
                                    RichTextOutput = TrueOrFalse("Rich text output", xNav.Value);
                                    break;

                                case "Timestamp format":
                                    TimestampFormat = xNav.Value;
                                    break;

                                case "Log file path":
                                    if (!Directory.Exists(Path.GetDirectoryName(xNav.Value))) {
                                        MessageBox.Show("ERROR: Log file path '" + xNav.Value + "' folder does not exist or you do not have write permission. Exiting. Please update SeleniumgLog.config file's 'Log file path' property with a folder path that exists.");
                                        throw new Exception("ERROR: Log file path '" + xNav.Value + "' folder does not exist or you do not have write permission. Exiting. Please update SeleniumgLog.config file's 'Log file path' property with a folder path that exists.");
                                    }
                                    LogFilePath = xNav.Value;
                                    break;

                                case "Write line numbers":
                                    WriteLineNumbers = TrueOrFalse("Write line numbers", xNav.Value);
                                    break;

                                case "Auto-launch SeleniumLog Viewer":
                                    AutoLaunchSeleniumLogDesktop = TrueOrFalse("Auto-launch SeleniumLog Viewer", xNav.Value);
                                    break;

                                case "SeleniumLog Viewer Installation Folder":
                                    SeleniumLogAppInstallationFolder = xNav.Value;
                                    break;

                                case "Screenshots folder":
                                    if (!Directory.Exists(Path.GetDirectoryName(xNav.Value)))
                                    {
                                        MessageBox.Show("ERROR: Screenshots folder '" + xNav.Value + "' folder does not exist or you do not have write permission. Exiting. Please update SeleniumgLog.config file's 'Screenshots folder' property with a folder path that exists.");
                                        throw new Exception("ERROR: Screenshots folder '" + xNav.Value + "' folder does not exist or you do not have write permission. Exiting. Please update SeleniumgLog.config file's 'Screenshots folder' property with a folder path that exists.");
                                    }
                                    ScreenshotsFolder = xNav.Value;
                                    break;

                                case "Use fast screenshot":
                                    UseFastScreenshot = TrueOrFalse("Use fast screenshot", xNav.Value);
                                    break;

                                case "Take screenshot on every log.WriteLine()":
                                    TakeScreenshotOnEveryWriteline = TrueOrFalse("Take screenshot on every log.WriteLine()", xNav.Value);
                                    break;

                                case "Take screenshot on every log.Info()":
                                    TakeScreenshotOnEveryInfo = TrueOrFalse("Take screenshot on every log.Info()", xNav.Value);
                                    break;

                                case "Take screenshot on every log.Info2()":
                                    TakeScreenshotOnEveryInfo2 = TrueOrFalse("Take screenshot on every log.Info2()", xNav.Value);
                                    break;

                                case "Take screenshot on every log.Debug()":
                                    TakeScreenshotOnEveryDebug = TrueOrFalse("Take screenshot on every log.Debug()", xNav.Value);
                                    break;

                                case "Take screenshot on every log.Debug2()":
                                    TakeScreenshotOnEveryDebug2 = TrueOrFalse("Take screenshot on every log.Debug2()", xNav.Value);
                                    break;

                                case "Take screenshot on every log.Pass()":
                                    TakeScreenshotOnEveryPass = TrueOrFalse("Take screenshot on every log.Pass()", xNav.Value);
                                    break;

                                case "Take screenshot on every log.Pass2()":
                                    TakeScreenshotOnEveryPass2 = TrueOrFalse("Take screenshot on every log.Pass2()", xNav.Value);
                                    break;

                                case "Take screenshot on every log.Fail()":
                                    TakeScreenshotOnEveryFail = TrueOrFalse("Take screenshot on every log.Fail()", xNav.Value);
                                    break;

                                case "Take screenshot on every log.Fail2()":
                                    TakeScreenshotOnEveryFail2 = TrueOrFalse("Take screenshot on every log.Fail2()", xNav.Value);
                                    break;

                                case "Take screenshot on every log.Error()":
                                    TakeScreenshotOnEveryError = TrueOrFalse("Take screenshot on every log.Error()", xNav.Value);
                                    break;

                                case "Take screenshot on every log.Error2()":
                                    TakeScreenshotOnEveryError2 = TrueOrFalse("Take screenshot on every log.Error2()", xNav.Value);
                                    break;

                                case "Take screenshot on every log.Warning()":
                                    TakeScreenshotOnEveryWarning = TrueOrFalse("Take screenshot on every log.Warning()", xNav.Value);
                                    break;

                                case "Take screenshot on every log.Warning2()":
                                    TakeScreenshotOnEveryWarning2 = TrueOrFalse("Take screenshot on every log.Warning2()", xNav.Value);
                                    break;

                                case "Take screenshot on every log.Fatal()":
                                    TakeScreenshotOnEveryFatal = TrueOrFalse("Take screenshot on every log.Fatal()", xNav.Value);
                                    break;

                                case "Take screenshot on every log.Fatal2()":
                                    TakeScreenshotOnEveryFatal2 = TrueOrFalse("Take screenshot on every log.Fatal2()", xNav.Value);
                                    break;

                                case "Take screenshot on every Exception":
                                    TakeScreenshotOnEveryException = TrueOrFalse("Take screenshot on every Exception", xNav.Value);
                                    break;

                                case "Force throw exception when assert fails":
                                    ForceThrowExceptionOnAssertFail = TrueOrFalse("Force throw exception when assert fails", xNav.Value);
                                    break;

                                case "Enable Function Interceptor":
                                    EnableFunctionInterceptor = TrueOrFalse("Enable Function Interceptor", xNav.Value);
                                    break;

                                case "Function Entry : Auto-explore complex function input params":
                                    FunctionEntry_AutoExploreComplexFunctionInputParams = TrueOrFalse("Function Entry : Auto-explore complex function input params", xNav.Value);
                                    break;

                                case "Function Entry : Display NULL inputs":
                                    FunctionEntry_DisplayNullInputs = TrueOrFalse("Function Entry : Display NULL inputs", xNav.Value);
                                    break;

                                case "Function Entry : Take screenshot on entry":
                                    FunctionEntry_TakeScreenshotOnEntry = TrueOrFalse("Function Entry : Take screenshot on entry", xNav.Value);
                                    break;

                                case "Function Exit : Auto-explore complex function outputs":
                                    FunctionExit_AutoExploreComplexFunctionOutputs = TrueOrFalse("Function Exit : Auto-explore complex function outputs", xNav.Value);
                                    break;

                                case "Function Exit : Display NULL outputs":
                                    FunctionExit_DisplayNullOutputs = TrueOrFalse("Function Exit : Display NULL outputs", xNav.Value);
                                    break;

                                case "Function Exit : Take screenshot on exit":
                                    FunctionExit_TakeScreenshotOnExit = TrueOrFalse("Function Exit : Take screenshot on exit", xNav.Value);
                                    break;

                                case "Function Exit : Enable performance measurement":
                                    FunctionExit_EnablePerformanceMeasurement = TrueOrFalse("Function Exit : Enable performance measurement", xNav.Value);
                                    break;

                                case "Performance measurement unit":
                                    PerformanceMeasurementUnit = xNav.Value;
                                    break;

                                case "Enable Logging of low-level Selenium Webdriver Events":
                                    EnableLoggingOfLowLevelSeleniumWebdriverEvents = TrueOrFalse("Enable Logging of low-level Selenium Webdriver Events", xNav.Value);
                                    break;

                                case "OnNavigating : log before event":
                                    OnNavigating_LogBeforeEvent = TrueOrFalse("OnNavigating : log before event", xNav.Value);
                                    break;

                                case "OnNavigating : take screenshot before event":
                                    OnNavigating_TakeScreenshotBeforeEvent = TrueOrFalse("OnNavigating : take screenshot before event", xNav.Value);
                                    break;

                                case "OnNavigating : log after event":
                                    OnNavigating_LogAfterEvent = TrueOrFalse("OnNavigating : log after event", xNav.Value);
                                    break;

                                case "OnNavigating : take screenshot after event":
                                    OnNavigating_TakeScreenshotAfterEvent = TrueOrFalse("OnNavigating : take screenshot after event", xNav.Value);
                                    break;


                                case "OnNavigatingForward : log before event":
                                    OnNavigatingForward_LogBeforeEvent = TrueOrFalse("OnNavigatingForward : log before event", xNav.Value);
                                    break;

                                case "OnNavigatingForward : take screenshot before event":
                                    OnNavigatingForward_TakeScreenshotBeforeEvent = TrueOrFalse("OnNavigatingForward : take screenshot before event", xNav.Value);
                                    break;

                                case "OnNavigatingForward : log after event":
                                    OnNavigatingForward_LogAfterEvent = TrueOrFalse("OnNavigatingForward : log after event", xNav.Value);
                                    break;

                                case "OnNavigatingForward : take screenshot after event":
                                    OnNavigatingForward_TakeScreenshotAfterEvent = TrueOrFalse("OnNavigatingForward : take screenshot after event", xNav.Value);
                                    break;


                                case "OnNavigatingBack : log before event":
                                    OnNavigatingBack_LogBeforeEvent = TrueOrFalse("OnNavigatingBack : log before event", xNav.Value);
                                    break;

                                case "OnNavigatingBack : take screenshot before event":
                                    OnNavigatingBack_TakeScreenshotBeforeEvent = TrueOrFalse("OnNavigatingBack : take screenshot before event", xNav.Value);
                                    break;

                                case "OnNavigatingBack : log after event":
                                    OnNavigatingBack_LogAfterEvent = TrueOrFalse("OnNavigatingBack : log after event", xNav.Value);
                                    break;

                                case "OnNavigatingBack : take screenshot after event":
                                    OnNavigatingBack_TakeScreenshotAfterEvent = TrueOrFalse("OnNavigatingBack : take screenshot after event", xNav.Value);
                                    break;


                                case "OnClick : log before event":
                                    OnClick_LogBeforeEvent = TrueOrFalse("OnClick : log before event", xNav.Value);
                                    break;

                                case "OnClick : take screenshot before event":
                                    OnClick_TakeScreenshotBeforeEvent = TrueOrFalse("OnClick : take screenshot before event", xNav.Value);
                                    break;

                                case "OnClick : log after event":
                                    OnClick_LogAfterEvent = TrueOrFalse("OnClick : log after event", xNav.Value);
                                    break;

                                case "OnClick : take screenshot after event":
                                    OnClick_TakeScreenshotAfterEvent = TrueOrFalse("OnClick : take screenshot after event", xNav.Value);
                                    break;

                                case "OnChangeValue : log before event":
                                    OnChangeValue_LogBeforeEvent = TrueOrFalse("OnChangeValue : log before event", xNav.Value);
                                    break;

                                case "OnChangeValue : take screenshot before event":
                                    OnChangeValue_TakeScreenshotBeforeEvent = TrueOrFalse("OnChangeValue : take screenshot before event", xNav.Value);
                                    break;

                                case "OnChangeValue : log after event":
                                    OnChangeValue_LogAfterEvent = TrueOrFalse("OnChangeValue : log after event", xNav.Value);
                                    break;

                                case "OnChangeValue : take screenshot after event":
                                    OnChangeValue_TakeScreenshotAfterEvent = TrueOrFalse("OnChangeValue : take screenshot after event", xNav.Value);
                                    break;

                                case "OnFindElement : log before event":
                                    OnFindElement_LogBeforeEvent = TrueOrFalse("OnFindElement : log before event", xNav.Value);
                                    break;

                                case "OnFindElement : take screenshot before event":
                                    OnFindElement_TakeScreenshotBeforeEvent = TrueOrFalse("OnFindElement : take screenshot before event", xNav.Value);
                                    break;

                                case "OnFindElement : log after event":
                                    OnFindElement_LogAfterEvent = TrueOrFalse("OnFindElement : log after event", xNav.Value);
                                    break;

                                case "OnFindElement : take screenshot after event":
                                    OnFindElement_TakeScreenshotAfterEvent = TrueOrFalse("OnFindElement : take screenshot after event", xNav.Value);
                                    break;

                                case "OnScriptExecute : log before event":
                                    OnScriptExecute_LogBeforeEvent = TrueOrFalse("OnScriptExecute : log before event", xNav.Value);
                                    break;

                                case "OnScriptExecute : take screenshot before event":
                                    OnScriptExecute_TakeScreenshotBeforeEvent = TrueOrFalse("OnScriptExecute : take screenshot before event", xNav.Value);
                                    break;

                                case "OnScriptExecute : log after event":
                                    OnScriptExecute_LogAfterEvent = TrueOrFalse("OnScriptExecute : log after event", xNav.Value);
                                    break;

                                case "OnScriptExecute : take screenshot after event":
                                    OnScriptExecute_TakeScreenshotAfterEvent = TrueOrFalse("OnScriptExecute : take screenshot after event", xNav.Value);
                                    break;

                                case "OnWebdriverExceptionThrown : log event":
                                    OnWebdriverExceptionThrown_LogEvent = TrueOrFalse("OnWebdriverExceptionThrown : log event", xNav.Value);
                                    break;

                                case "enable_seleniumlog":
                                    ExceptionMessageBuffer = xNav.Name;
                                    EnableSeleniumLog = Convert.ToBoolean(xNav.Value);
                                    break;
                                case "enable_selenium_webdriver_trace":
                                    ExceptionMessageBuffer = xNav.Name;
                                    Enable_Selenium_Webdriver_Trace = Convert.ToBoolean(xNav.Value);
                                    break;
                                case "enable_general_function_trace":
                                    ExceptionMessageBuffer = xNav.Name;
                                    Enable_Generic_Function_Trace = Convert.ToBoolean(xNav.Value);
                                    break;
                                case "auto_start_seleniumlog_app":
                                    ExceptionMessageBuffer = xNav.Name;
                                    AutoStartSeleniumLogApp = Convert.ToBoolean(xNav.Value);
                                    break;
                                case "seleniumlog_app_installation_folder":
                                    ExceptionMessageBuffer = xNav.Name;
                                    SeleniumLogAppInstallationFolder = xNav.Value;
                                    break;
                                case "timestamp_format":
                                    ExceptionMessageBuffer = xNav.Name;
                                    TimestampFormat = xNav.Value;
                                    break;
                                case "output_file_path":
                                    ExceptionMessageBuffer = xNav.Name;
                                    OutputFilePath = xNav.Value;
                                    break;
                                case "screenshots_folder":
                                    ExceptionMessageBuffer = xNav.Name;
                                    ScreenshotsFolder = xNav.Value;
                                    break;
                                case "screenshot_on_writeline":
                                    ExceptionMessageBuffer = xNav.Name;
                                    ScreenshotOnEveryMessage = Convert.ToBoolean(xNav.Value);
                                    break;
                                case "screenshot_on_pass":
                                    ExceptionMessageBuffer = xNav.Name;
                                    ScreenshotOnEveryPass = Convert.ToBoolean(xNav.Value);
                                    break;
                                case "screenshot_on_fail":
                                    ExceptionMessageBuffer = xNav.Name;
                                    ScreenshotOnEveryFail = Convert.ToBoolean(xNav.Value);
                                    break;
                                case "screenshot_on_error":
                                    ExceptionMessageBuffer = xNav.Name;
                                    ScreenshotOnEveryError = Convert.ToBoolean(xNav.Value);
                                    break;
                                case "screenshot_on_warning":
                                    ExceptionMessageBuffer = xNav.Name;
                                    ScreenshotOnEveryWarning = Convert.ToBoolean(xNav.Value);
                                    break;
                                case "force_throw_exception_on_assert_fail":
                                    ExceptionMessageBuffer = xNav.Name;
                                    ScreenshotOnEveryFail = Convert.ToBoolean(xNav.Value);
                                    break;
                                case "on_click_event":
                                    ExceptionMessageBuffer = xNav.Name;
                                    //_ParentNode = "OnClick";
                                    break;
                                case "log_function_start":
                                    ExceptionMessageBuffer = xNav.Name;
                                    //if (_ParentNode == "OnClick")
                                    //    OnClick_LogFunctionStart = Convert.ToBoolean(xNav.Value);
                                    break;
                                case "log_function_end":
                                    ExceptionMessageBuffer = xNav.Name;
                                    //if (_ParentNode == "OnClick")
                                    //    OnClick_LogFunctionEnd = Convert.ToBoolean(xNav.Value);
                                    break;
                                case "screenshot_on_start":
                                    ExceptionMessageBuffer = xNav.Name;
                                    //if (_ParentNode == "OnClick")
                                    //    OnClick_ScreenshotOnStart = Convert.ToBoolean(xNav.Value);
                                    break;
                                case "screenshot_on_end":
                                    ExceptionMessageBuffer = xNav.Name;
                                    //if (_ParentNode == "OnClick")
                                    //    OnClick_ScreenshotOnEnd = Convert.ToBoolean(xNav.Value);
                                    break;
                                case "general_function_trace_display_null_inputs":
                                    ExceptionMessageBuffer = xNav.Name;
                                    FunctionTrace_DisplayNullInputs = Convert.ToBoolean(xNav.Value);
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
            catch (Exception e)
            {
                Console.WriteLine("Exception raised while reading <" + ExceptionMessageBuffer + "> in XML file - " + e.Message);
                Console.WriteLine("Press ENTER to close this console window.");
                Console.ReadLine();
            }
        }

    }
}
