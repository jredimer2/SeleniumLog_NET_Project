
package XMLConfig;

import java.io.File;
import java.io.IOException;
import javax.xml.XMLConstants;
import javax.xml.namespace.QName;
import javax.xml.parsers.*;
import javax.xml.xpath.*;
import org.w3c.dom.*;
import org.xml.sax.SAXException;

public final class XmlConfigurationClass {
    
    public boolean EnableSeleniumLog;
    public String TimestampFormat;
    public String LogFilePath;
    public boolean WriteLineNumbers;

    public boolean AutoLaunchSeleniumLogDesktop;
    public String SeleniumLogDesktopInstallationFolder;

    public String ScreenshotsFolder;
    public boolean TakeScreenshotOnEveryWriteline;
    public boolean TakeScreenshotOnEveryPass;
    public boolean TakeScreenshotOnEveryFail;
    public boolean TakeScreenshotOnEveryError;
    public boolean TakeScreenshotOnEveryWarning;
    public boolean TakeScreenshotOnEveryException;

    public boolean ForceThrowExceptionOnAssertFail;
    public boolean EnableFunctionInterceptor;
    public boolean FunctionEntry_AutoExploreComplexFunctionInputParams;
    public boolean FunctionEntry_DisplayNullInputs;
    public boolean FunctionEntry_TakeScreenshotOnEntry;
    public boolean FunctionExit_AutoExploreComplexFunctionOutputs;
    public boolean FunctionExit_DisplayNullOutputs;
    public boolean FunctionExit_TakeScreenshotOnExit;
    public boolean FunctionExit_EnablePerformanceMeasurement;
    public String PerformanceMeasurementUnit;

    public boolean EnableLoggingOfLowLevelSeleniumWebdriverEvents;
    public boolean OnNavigating_LogBeforeEvent;
    public boolean OnNavigating_TakeScreenshotBeforeEvent;
    public boolean OnNavigating_LogAfterEvent;
    public boolean OnNavigating_TakeScreenshotAfterEvent;
    public boolean OnNavigatingBack_LogBeforeEvent;
    public boolean OnNavigatingBack_TakeScreenshotBeforeEvent;
    public boolean OnNavigatingBack_LogAfterEvent;
    public boolean OnNavigatingBack_TakeScreenshotAfterEvent;
    public boolean OnNavigatingForward_LogBeforeEvent;
    public boolean OnNavigatingForward_TakeScreenshotBeforeEvent;
    public boolean OnNavigatingForward_LogAfterEvent;
    public boolean OnNavigatingForward_TakeScreenshotAfterEvent;
    public boolean OnClick_LogBeforeEvent;
    public boolean OnClick_TakeScreenshotBeforeEvent;
    public boolean OnClick_LogAfterEvent;
    public boolean OnClick_TakeScreenshotAfterEvent;
    public boolean OnChangeValue_LogBeforeEvent;
    public boolean OnChangeValue_TakeScreenshotBeforeEvent;
    public boolean OnChangeValue_LogAfterEvent;
    public boolean OnChangeValue_TakeScreenshotAfterEvent;
    public boolean OnFindElement_LogBeforeEvent;
    public boolean OnFindElement_TakeScreenshotBeforeEvent;
    public boolean OnFindElement_LogAfterEvent;
    public boolean OnFindElement_TakeScreenshotAfterEvent;
    public boolean OnScriptExecute_LogBeforeEvent;
    public boolean OnScriptExecute_TakeScreenshotBeforeEvent;
    public boolean OnScriptExecute_LogAfterEvent;
    public boolean OnScriptExecute_TakeScreenshotAfterEvent;
    public boolean OnWebdriverExceptionThrown_LogEvent;

    //public boolean EnableSeleniumLog;
    public boolean Enable_Selenium_Webdriver_Trace;
    public boolean Enable_Generic_Function_Trace;
    public boolean AutoStartSeleniumLogApp;
    public String SeleniumLogAppInstallationFolder;
    //public String TimestampFormat;
    public String OutputFilePath;
    //public String ScreenshotsFolder;
    public boolean ScreenshotOnEveryMessage;
    public boolean ScreenshotOnEveryPass;
    public boolean ScreenshotOnEveryFail;
    public boolean ScreenshotOnEveryError;
    public boolean ScreenshotOnEveryWarning;

    public boolean LogLowLevelSeleniumCommands;

    public boolean OnClick_LogFunctionStart;
    public boolean OnClick_LogFunctionEnd;
    public boolean OnClick_ScreenshotOnStart;
    public boolean OnClick_ScreenshotOnEnd;
    public boolean FunctionTrace_DisplayNullInputs;

    public String ExceptionMessageBuffer = "";

    private static XmlConfigurationClass instance = null;
    
    public static XmlConfigurationClass Instance() {
        if (instance == null) {
            instance = new XmlConfigurationClass();
        }
        return instance;
    }
    
    private XmlConfigurationClass() {
        ParseXML();
    }
    
    public class XPathReader {

        private String xmlFile;
        private Document xmlDocument;
        private XPath xPath;

        public XPathReader(String xmlFile) {
          this.xmlFile = xmlFile;
          initObjects();
        }

        private void initObjects(){
            try {
                xmlDocument = DocumentBuilderFactory.newInstance().newDocumentBuilder().parse(xmlFile);
                xPath =  XPathFactory.newInstance().newXPath();
            } catch (IOException | SAXException | ParserConfigurationException ex) {
                ex.printStackTrace();
            }
        }

        public Object read(String expression, QName returnType){
            try {
                XPathExpression xPathExpression = xPath.compile(expression);
                return xPathExpression.evaluate(xmlDocument, returnType);
            } catch (XPathExpressionException ex) {
                ex.printStackTrace();
                return null;
            }
        }
    }
    
    private void ParseXML() {
        ParseXML(false);
    }
    
    private void ParseXML(boolean debug) {
        XPathReader reader = new XPathReader("SeleniumLog.config");
        String expression = "/configuration";
        NodeList nodes = (NodeList)reader.read(expression, XPathConstants.NODESET);
        traverse(nodes);
    }
    
    private boolean TrueOrFalse(String tagname, String val) {
        try {
            return val.equalsIgnoreCase("true") || val.equalsIgnoreCase("yes");
        } catch (Exception e) {
            System.err.println("Error: <" + tagname + "> value cannot be converted to boolean. Accepted values are Yes/No or True/False.");
            throw e;
        }
    }
    
    private void traverse(NodeList rootNode){
        try {
            for (int index = 0; index < rootNode.getLength(); index ++){
                Node aNode = rootNode.item(index);
                if (aNode.getNodeType() == Node.ELEMENT_NODE){
                    NodeList childNodes = aNode.getChildNodes();
                    if (childNodes.getLength() > 0){
                        NamedNodeMap attrs = aNode.getAttributes();
                        if ((attrs != null) && (attrs.getLength() > 0)) {
                            String attr = attrs.item(0).getNodeValue();
                            String val = aNode.getFirstChild().getNodeValue();
                            System.out.println(aNode.getNodeName() + ", ATTRIB [" + attr + "]");
                            switch (attr) {
                                case "Enable SeleniumLog":
                                    EnableSeleniumLog = TrueOrFalse("Enable SeleniumLog", val);
                                    break;

                                case "Timestamp format":
                                    TimestampFormat = val;
                                    break;

                                case "Log file path":
                                    LogFilePath = val;
                                    break;

                                case "Write line numbers":
                                    WriteLineNumbers = TrueOrFalse("Write line numbers", val);
                                    break;


                                case "Auto-launch SeleniumLog Desktop":
                                    AutoLaunchSeleniumLogDesktop = TrueOrFalse("Auto-launch SeleniumLog Desktop", val);
                                    break;

                                case "SeleniumLog Desktop Installation Folder":
                                    SeleniumLogAppInstallationFolder = val;
                                    break;

                                case "Screenshots folder":
                                    ScreenshotsFolder = val;
                                    break;

                                case "Take screenshot on every log.WriteLine()":
                                    TakeScreenshotOnEveryWriteline = TrueOrFalse("Take screenshot on every log.WriteLine()", val);
                                    break;

                                case "Take screenshot on every log.Pass()":
                                    TakeScreenshotOnEveryPass = TrueOrFalse("Take screenshot on every log.Pass()", val);
                                    break;

                                case "Take screenshot on every log.Fail()":
                                    TakeScreenshotOnEveryFail = TrueOrFalse("Take screenshot on every log.Fail()", val);
                                    break;

                                case "Take screenshot on every log.Error()":
                                    TakeScreenshotOnEveryError = TrueOrFalse("Take screenshot on every log.Error()", val);
                                    break;

                                case "Take screenshot on every log.Warning()":
                                    TakeScreenshotOnEveryWarning = TrueOrFalse("Take screenshot on every log.Warning()", val);
                                    break;

                                case "Take screenshot on every Exception":
                                    TakeScreenshotOnEveryException = TrueOrFalse("Take screenshot on every Exception", val);
                                    break;

                                case "Force throw exception when assert fails":
                                    ForceThrowExceptionOnAssertFail = TrueOrFalse("Force throw exception when assert fails", val);
                                    break;

                                case "Enable Function Interceptor":
                                    EnableFunctionInterceptor = TrueOrFalse("Enable Function Interceptor", val);
                                    break;

                                case "Function Entry : Auto-explore complex function input params":
                                    FunctionEntry_AutoExploreComplexFunctionInputParams = TrueOrFalse("Function Entry : Auto-explore complex function input params", val);
                                    break;

                                case "Function Entry : Display NULL inputs":
                                    FunctionEntry_DisplayNullInputs = TrueOrFalse("Function Entry : Display NULL inputs", val);
                                    break;

                                case "Function Entry : Take screenshot on entry":
                                    FunctionEntry_TakeScreenshotOnEntry = TrueOrFalse("Function Entry : Take screenshot on entry", val);
                                    break;

                                case "Function Exit : Auto-explore complex function outputs":
                                    FunctionExit_AutoExploreComplexFunctionOutputs = TrueOrFalse("Function Exit : Auto-explore complex function outputs", val);
                                    break;

                                case "Function Exit : Display NULL outputs":
                                    FunctionExit_DisplayNullOutputs = TrueOrFalse("Function Exit : Display NULL outputs", val);
                                    break;

                                case "Function Exit : Take screenshot on exit":
                                    FunctionExit_TakeScreenshotOnExit = TrueOrFalse("Function Exit : Take screenshot on exit", val);
                                    break;

                                case "Function Exit : Enable performance measurement":
                                    FunctionExit_EnablePerformanceMeasurement = TrueOrFalse("Function Exit : Enable performance measurement", val);
                                    break;

                                case "Performance measurement unit":
                                    PerformanceMeasurementUnit = val;
                                    break;

                                case "Enable Logging of low-level Selenium Webdriver Events":
                                    EnableLoggingOfLowLevelSeleniumWebdriverEvents = TrueOrFalse("Enable Logging of low-level Selenium Webdriver Events", val);
                                    break;

                                case "OnNavigating : log before event":
                                    OnNavigating_LogBeforeEvent = TrueOrFalse("OnNavigating : log before event", val);
                                    break;

                                case "OnNavigating : take screenshot before event":
                                    OnNavigating_TakeScreenshotBeforeEvent = TrueOrFalse("OnNavigating : take screenshot before event", val);
                                    break;

                                case "OnNavigating : log after event":
                                    OnNavigating_LogAfterEvent = TrueOrFalse("OnNavigating : log after event", val);
                                    break;

                                case "OnNavigating : take screenshot after event":
                                    OnNavigating_TakeScreenshotAfterEvent = TrueOrFalse("OnNavigating : take screenshot after event", val);
                                    break;


                                case "OnNavigatingForward : log before event":
                                    OnNavigatingForward_LogBeforeEvent = TrueOrFalse("OnNavigatingForward : log before event", val);
                                    break;

                                case "OnNavigatingForward : take screenshot before event":
                                    OnNavigatingForward_TakeScreenshotBeforeEvent = TrueOrFalse("OnNavigatingForward : take screenshot before event", val);
                                    break;

                                case "OnNavigatingForward : log after event":
                                    OnNavigatingForward_LogAfterEvent = TrueOrFalse("OnNavigatingForward : log after event", val);
                                    break;

                                case "OnNavigatingForward : take screenshot after event":
                                    OnNavigatingForward_TakeScreenshotAfterEvent = TrueOrFalse("OnNavigatingForward : take screenshot after event", val);
                                    break;


                                case "OnNavigatingBack : log before event":
                                    OnNavigatingBack_LogBeforeEvent = TrueOrFalse("OnNavigatingBack : log before event", val);
                                    break;

                                case "OnNavigatingBack : take screenshot before event":
                                    OnNavigatingBack_TakeScreenshotBeforeEvent = TrueOrFalse("OnNavigatingBack : take screenshot before event", val);
                                    break;

                                case "OnNavigatingBack : log after event":
                                    OnNavigatingBack_LogAfterEvent = TrueOrFalse("OnNavigatingBack : log after event", val);
                                    break;

                                case "OnNavigatingBack : take screenshot after event":
                                    OnNavigatingBack_TakeScreenshotAfterEvent = TrueOrFalse("OnNavigatingBack : take screenshot after event", val);
                                    break;


                                case "OnClick : log before event":
                                    OnClick_LogBeforeEvent = TrueOrFalse("OnClick : log before event", val);
                                    break;

                                case "OnClick : take screenshot before event":
                                    OnClick_TakeScreenshotBeforeEvent = TrueOrFalse("OnClick : take screenshot before event", val);
                                    break;

                                case "OnClick : log after event":
                                    OnClick_LogAfterEvent = TrueOrFalse("OnClick : log after event", val);
                                    break;

                                case "OnClick : take screenshot after event":
                                    OnClick_TakeScreenshotAfterEvent = TrueOrFalse("OnClick : take screenshot after event", val);
                                    break;

                                case "OnChangeValue : log before event":
                                    OnChangeValue_LogBeforeEvent = TrueOrFalse("OnChangeValue : log before event", val);
                                    break;

                                case "OnChangeValue : take screenshot before event":
                                    OnChangeValue_TakeScreenshotBeforeEvent = TrueOrFalse("OnChangeValue : take screenshot before event", val);
                                    break;

                                case "OnChangeValue : log after event":
                                    OnChangeValue_LogAfterEvent = TrueOrFalse("OnChangeValue : log after event", val);
                                    break;

                                case "OnChangeValue : take screenshot after event":
                                    OnChangeValue_TakeScreenshotAfterEvent = TrueOrFalse("OnChangeValue : take screenshot after event", val);
                                    break;

                                case "OnFindElement : log before event":
                                    OnFindElement_LogBeforeEvent = TrueOrFalse("OnFindElement : log before event", val);
                                    break;

                                case "OnFindElement : take screenshot before event":
                                    OnFindElement_TakeScreenshotBeforeEvent = TrueOrFalse("OnFindElement : take screenshot before event", val);
                                    break;

                                case "OnFindElement : log after event":
                                    OnFindElement_LogAfterEvent = TrueOrFalse("OnFindElement : log after event", val);
                                    break;

                                case "OnFindElement : take screenshot after event":
                                    OnFindElement_TakeScreenshotAfterEvent = TrueOrFalse("OnFindElement : take screenshot after event", val);
                                    break;

                                case "OnScriptExecute : log before event":
                                    OnScriptExecute_LogBeforeEvent = TrueOrFalse("OnScriptExecute : log before event", val);
                                    break;

                                case "OnScriptExecute : take screenshot before event":
                                    OnScriptExecute_TakeScreenshotBeforeEvent = TrueOrFalse("OnScriptExecute : take screenshot before event", val);
                                    break;

                                case "OnScriptExecute : log after event":
                                    OnScriptExecute_LogAfterEvent = TrueOrFalse("OnScriptExecute : log after event", val);
                                    break;

                                case "OnScriptExecute : take screenshot after event":
                                    OnScriptExecute_TakeScreenshotAfterEvent = TrueOrFalse("OnScriptExecute : take screenshot after event", val);
                                    break;

                                case "OnWebdriverExceptionThrown : log event":
                                    OnWebdriverExceptionThrown_LogEvent = TrueOrFalse("OnWebdriverExceptionThrown : log event", val);
                                    break;

                                case "enable_seleniumlog":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    EnableSeleniumLog = Boolean.parseBoolean(val);
                                    break;
                                case "enable_selenium_webdriver_trace":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    Enable_Selenium_Webdriver_Trace = Boolean.parseBoolean(val);
                                    break;
                                case "enable_general_function_trace":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    Enable_Generic_Function_Trace = Boolean.parseBoolean(val);
                                    break;
                                case "auto_start_seleniumlog_app":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    AutoStartSeleniumLogApp = Boolean.parseBoolean(val);
                                    break;
                                case "seleniumlog_app_installation_folder":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    SeleniumLogAppInstallationFolder = val;
                                    break;
                                case "timestamp_format":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    TimestampFormat = val;
                                    break;
                                case "output_file_path":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    OutputFilePath = val;
                                    break;
                                case "screenshots_folder":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    ScreenshotsFolder = val;
                                    break;
                                case "screenshot_on_writeline":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    ScreenshotOnEveryMessage = Boolean.parseBoolean(val);
                                    break;
                                case "screenshot_on_pass":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    ScreenshotOnEveryPass = Boolean.parseBoolean(val);
                                    break;
                                case "screenshot_on_fail":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    ScreenshotOnEveryFail = Boolean.parseBoolean(val);
                                    break;
                                case "screenshot_on_error":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    ScreenshotOnEveryError = Boolean.parseBoolean(val);
                                    break;
                                case "screenshot_on_warning":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    ScreenshotOnEveryWarning = Boolean.parseBoolean(val);
                                    break;
                                case "force_throw_exception_on_assert_fail":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    ScreenshotOnEveryFail = Boolean.parseBoolean(val);
                                    break;
                                case "on_click_event":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    //_ParentNode = "OnClick";
                                    break;
                                case "log_function_start":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    //if (_ParentNode == "OnClick")
                                    //    OnClick_LogFunctionStart = Convert.ToBoolean(val);
                                    break;
                                case "log_function_end":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    //if (_ParentNode == "OnClick")
                                    //    OnClick_LogFunctionEnd = Convert.ToBoolean(val);
                                    break;
                                case "screenshot_on_start":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    //if (_ParentNode == "OnClick")
                                    //    OnClick_ScreenshotOnStart = Convert.ToBoolean(val);
                                    break;
                                case "screenshot_on_end":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    //if (_ParentNode == "OnClick")
                                    //    OnClick_ScreenshotOnEnd = Convert.ToBoolean(val);
                                    break;
                                case "general_function_trace_display_null_inputs":
                                    ExceptionMessageBuffer = aNode.getNodeName();
                                    FunctionTrace_DisplayNullInputs = Boolean.parseBoolean(val);
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                    traverse(childNodes);
                }
            }
        }
        catch (Exception e)
        {
            System.err.println("Exception raised while reading <" + ExceptionMessageBuffer + "> in XML file - " + e.getMessage());
            System.err.println("Press ENTER to close this console window.");
            System.console().readLine();
        }
    }
    
}
