
package SeleniumLogger;

import XMLConfig.XmlConfigurationClass;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.regex.Pattern;

import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.Keys;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.support.events.EventFiringWebDriver;
import org.openqa.selenium.support.events.WebDriverEventListener;

/**
 *
 * @author
 */
public class SeleniumLogEventListener extends EventFiringWebDriver {
    
    private class XPathTester {
        private class Result {
            public boolean result;
            public int matches;
            public String cummulative_xpath;
            public Result(boolean result, int matches, String cummulative_xpath) {
                this.result = result;
                this.matches = matches;
                this.cummulative_xpath = cummulative_xpath;
            }
        }
        
        private ArrayList<Result> Results = new ArrayList<>();
        
        public XPathTester() {
        }

        private void Reset() {
            Results.clear();
        }
        
        public void Test(WebDriver driver, String XPath)
        {            
            //String XPath2 = Regex.Replace(input: XPath, pattern: @"^\/\/", replacement: "");
            String XPath2 = XPath.replaceAll("^\\/\\/", "");
            String[] components = XPath2.split("/");
            String cummulative_xpath = "";
            int i = 0;
            boolean res = false;

            Reset();

            for (String comp : components) {
                i++;
                if (i == 1) {
                    cummulative_xpath = "//" + comp;
                }
                else {
                    cummulative_xpath = cummulative_xpath + "/" + comp;
                }

                //Test component
                //ReadOnlyCollection<IWebElement> foundelements = driver.findElements(By.XPath(cummulative_xpath));
                List<WebElement> foundelements = Collections.unmodifiableList(driver.findElements(By.xpath(cummulative_xpath)));
                Results.add(new Result((foundelements.size() > 0), foundelements.size(), cummulative_xpath));
            }
        }
        
        public void DisplayResults() {
            SeleniumLog log = SeleniumLog.Instance();

            for (Result result : Results) {
                if (result.result) {
                    log.Green().Pass().WriteLine(result.cummulative_xpath + "  - XPath Found - " + result.matches + " matches.");
                } else {
                    log.Red().Fail().WriteLine(result.cummulative_xpath + "  - XPath Not Found!");
                }
            }
        }
    }
    
        private class SeleniumLogEventHandler implements WebDriverEventListener{    
        //++
        @Override
        public void beforeNavigateTo(String string, WebDriver wd) {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnNavigating_LogBeforeEvent)
            {
                //log.SaveIndent("OnNavigating");
                log.Indent();
                log.Orange().WriteLine("Navigating To: " + string, log.Config.OnNavigating_TakeScreenshotBeforeEvent);
                //log.RestoreIndent("OnNavigating");
                log.Unindent();
            }
        }
        //++
        @Override
        public void afterNavigateTo(String string, WebDriver wd) {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnNavigating_LogAfterEvent)
            {
                //log.SaveIndent("OnNavigated");
                log.Indent();
                log.Pass().Green().WriteLine("... Navigation Success!", log.Config.OnNavigating_TakeScreenshotBeforeEvent);
                //log.RestoreIndent("OnNavigated");
                log.Unindent();
            }
        }
        //++
        @Override
        public void beforeNavigateBack(WebDriver wd) {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnNavigatingBack_LogBeforeEvent)
            {
                log.Indent();
                log.Orange().WriteLine("Navigating Back from: " + wd.getCurrentUrl(), log.Config.OnNavigatingBack_TakeScreenshotBeforeEvent);
                log.Unindent();
            }
        }
        //++
        @Override
        public void afterNavigateBack(WebDriver wd) {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnNavigatingBack_LogAfterEvent)
            {
                log.Indent();
                log.Pass().Green().WriteLine("... Navigate Back Success!", log.Config.OnNavigatingBack_TakeScreenshotAfterEvent);
                log.Unindent();
            }
        }
        //++
        @Override
        public void beforeNavigateForward(WebDriver wd) {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnNavigatingForward_LogBeforeEvent)
            {
                log.Indent();
                log.Orange().WriteLine("Navigating Forward from: " + wd.getCurrentUrl(), log.Config.OnNavigatingForward_TakeScreenshotBeforeEvent);
                log.Unindent();
            }
        }
        //++
        @Override
        public void afterNavigateForward(WebDriver wd) {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnNavigatingForward_LogAfterEvent)
            {
                log.Indent();
                log.Pass().Green().WriteLine("... Navigate Forward Success!", log.Config.OnNavigatingForward_TakeScreenshotAfterEvent);
                log.Unindent();
            }
        }
        //++
        @Override
        public void beforeFindBy(By by, WebElement we, WebDriver wd) {
            SeleniumLog log = SeleniumLog.Instance();
            //System.out.println("ByToString: '" + by.toString() + "'");
            String[] FindMethodStr = by.toString().split(":");
            _by = FindMethodStr[0].split(Pattern.quote("."))[1].trim();
            _locator = FindMethodStr[1].trim();

            if (log.Config.OnFindElement_LogBeforeEvent)
            {
                log.Indent();
                log.Orange().WriteLine("Finding Element: " + by.toString(), log.Config.OnFindElement_TakeScreenshotBeforeEvent);
                log.Unindent();
            }
        }
        //++
        @Override
        public void afterFindBy(By by, WebElement we, WebDriver wd) {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnFindElement_LogAfterEvent)
            {
                log.Indent();
                log.Pass().Green().WriteLine("... Element Found!", log.Config.OnFindElement_TakeScreenshotAfterEvent);
                log.Unindent();
            }
        }
        //++
        @Override
        public void beforeClickOn(WebElement we, WebDriver wd) {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnClick_LogBeforeEvent)
            {
                log.Indent();
                log.Orange().WriteLine("Clicking Element: " + _by + " " + _locator, log.Config.OnClick_TakeScreenshotBeforeEvent);
                log.Unindent();
            }
        }
        //++
        @Override
        public void afterClickOn(WebElement we, WebDriver wd) {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnClick_LogAfterEvent)
            {
                log.Indent();
                log.Pass().Green().WriteLine("... Click Success!", log.Config.OnClick_TakeScreenshotAfterEvent);
                log.Unindent();
            }
        }
        //++
        @Override
        public void beforeChangeValueOf(WebElement we, WebDriver wd) {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnChangeValue_LogBeforeEvent)
            {
                log.SaveIndent("OnElementValueChanging");
                log.Indent();
                log.Orange().WriteLine("Changing value ...");
                if ((_keyInput != null) && !_keyInput.isEmpty())
                {
                    log.Orange().WriteLine(" [" + _keyInput + "]", log.Config.OnChangeValue_TakeScreenshotBeforeEvent);
                }
                log.RestoreIndent("OnElementValueChanging");
            }
        }
        //++
        @Override
        public void afterChangeValueOf(WebElement we, WebDriver wd) {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnChangeValue_LogAfterEvent)
            {
                log.SaveIndent("OnElementValueChanged");
                log.Indent();
                log.Pass().Green().WriteLine("... Successfully changed value [" + we.getAttribute("value") + "]", log.Config.OnChangeValue_TakeScreenshotAfterEvent);
                log.Indent();

                if ((_keyInput != null) && !_keyInput.isEmpty())
                {
                    //log.WriteLine("Input was: " + _keyInput);
                }
                log.RestoreIndent("OnElementValueChanged");
            }
        }
        //++
        @Override
        public void beforeScript(String string, WebDriver wd) {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnScriptExecute_LogBeforeEvent)
            {
                log.Indent();
                log.Orange().WriteLine("Script Executing: " + string, log.Config.OnScriptExecute_TakeScreenshotBeforeEvent);
                log.Unindent();
            }
        }
        //++
        @Override
        public void afterScript(String string, WebDriver wd) {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnScriptExecute_LogAfterEvent)
            {
                log.Indent();
                log.Pass().Green().WriteLine("... Script Executed Successfully!", log.Config.OnScriptExecute_TakeScreenshotAfterEvent);
                log.Unindent();
            }
        }
        //++
        @Override
        public void onException(Throwable thrwbl, WebDriver wd) {
            SeleniumLog log = SeleniumLog.Instance();
            log.Indent();
            log.Red().WriteLine("Exception Thrown: " + thrwbl.getMessage(), true);
            if (log.Config.OnWebdriverExceptionThrown_LogEvent)
            {
                if (_by.equals("XPath"))
                {
                    log.Red().WriteLine("Running XPath Diagnostics: Expand to see which part of XPath failed.");
                    log.Indent();
                    XPathTest.Test(driver, _locator);
                    XPathTest.DisplayResults();
                    log.Unindent();
                }
            }
            log.Unindent();        }
    }
    
    private final XPathTester XPathTest = new XPathTester();
    private String _keyInput;
    private String _by = "";
    private String _locator = "";
    private final WebDriver driver;
    private boolean _enabled;
    private SeleniumLogEventHandler _handler;
    
    /// <summary>
    /// Send 'true (boolen)' to listen to selenium events
    /// </summary>
    /// <param name="enable"></param>
    public void EnableEventListening(boolean enable) {
        if (enable) {
            SubscribeEvents();
        }
        else {
            UnsubscribeEvents();
        }
        _enabled = enable;
    }
    
    // XPath Generator JavaScript
    private String GenerateElementXPath(WebDriver driver, WebElement element)
    {
        JavascriptExecutor js = (JavascriptExecutor)driver;
        return (String)js.executeScript("gPt=function(c){if(c.id!==''){return'id(\"'+c.id+'\")'}if(c===document.body){return c.tagName}var a=0;var e=c.parentNode.childNodes;for(var b=0;b<e.length;b++){var d=e[b];if(d===c){return gPt(c.parentNode)+'/'+c.tagName+'['+(a+1)+']'}if(d.nodeType===1&&d.tagName===c.tagName){a++}}};return gPt(arguments[0]).toLowerCase();", element);
    }
    
    public SeleniumLogEventListener(WebDriver driver) {
        super(driver);
        XmlConfigurationClass Config = XmlConfigurationClass.Instance();
        this.driver = driver;
        EnableEventListening(Config.EnableLoggingOfLowLevelSeleniumWebdriverEvents);
    }
    
    /// <summary>
    /// Subscribes all Selenium Events
    /// </summary>
    private void SubscribeEvents() {
        _handler = new SeleniumLogEventHandler();
        this.register(_handler);
    }
    
    /// <summary>
    /// Un-subscribes all registered Selenium Events
    /// </summary>
    private void UnsubscribeEvents() {
        try {
            this.unregister(_handler);
        }
        catch (Exception ex) {}
    }
    
    /// <summary>
    /// Sends required key to the given element keeping track of the input used
    /// </summary>
    /// <param name="element">Element to which to send the key</param>
    /// <param name="key">Key to send</param>
    public void SendKeys(WebElement element, String key) {
        // Save the key input to be used for future reference
        _keyInput = KeyInputName(key);

        // Send the key to element to perform action
        element.sendKeys(key);
    }
    
    /// <summary>
    /// Converts the keyboard keys to a readable string name
    /// </summary>
    /// <param name="key">Key to convert to string name</param>
    /// <returns></returns>
    private String KeyInputName(String key) {
        if (key.equals(Keys.ADD.toString()))
        {
            return "Add";
        }
        else if (key.equals(Keys.ALT.toString()))
        {
            return "Alt";
        }
        else if (key.equals(Keys.ARROW_DOWN.toString()))
        {
            return "ArrowDown";
        }
        else if (key.equals(Keys.ARROW_LEFT.toString()))
        {
            return "ArrowLeft";
        }
        else if (key.equals(Keys.ARROW_RIGHT.toString()))
        {
            return "ArrowRight";
        }
        else if (key.equals(Keys.ARROW_UP.toString()))
        {
            return "ArrowUp";
        }
        else if (key.equals(Keys.BACK_SPACE.toString()))
        {
            return "Backspace";
        }
        else if (key.equals(Keys.CANCEL.toString()))
        {
            return "Cancel";
        }
        else if (key.equals(Keys.CLEAR.toString()))
        {
            return "Clear";
        }
        else if (key.equals(Keys.COMMAND.toString()))
        {
            return "Command";
        }
        else if (key.equals(Keys.CONTROL.toString()))
        {
            return "Control";
        }
        else if (key.equals(Keys.DECIMAL.toString()))
        {
            return "Decimal";
        }
        else if (key.equals(Keys.DELETE.toString()))
        {
            return "Delete";
        }
        else if (key.equals(Keys.DIVIDE.toString()))
        {
            return "Divide";
        }
        else if (key.equals(Keys.DOWN.toString()))
        {
            return "Down";
        }
        else if (key.equals(Keys.END.toString()))
        {
            return "End";
        }
        else if (key.equals(Keys.ENTER.toString()))
        {
            return "Enter";
        }
        else if (key.equals(Keys.EQUALS.toString()))
        {
            return "Equal";
        }
        else if (key.equals(Keys.ESCAPE.toString()))
        {
            return "Escape";
        }
        else if (key.equals(Keys.F1.toString()))
        {
            return "F1";
        }
        else if (key.equals(Keys.F10.toString()))
        {
            return "F10";
        }
        else if (key.equals(Keys.F11.toString()))
        {
            return "F11";
        }
        else if (key.equals(Keys.F12.toString()))
        {
            return "F12";
        }
        else if (key.equals(Keys.F2.toString()))
        {
            return "F2";
        }
        else if (key.equals(Keys.F3.toString()))
        {
            return "F3";
        }
        else if (key.equals(Keys.F4.toString()))
        {
            return "F4";
        }
        else if (key.equals(Keys.F5.toString()))
        {
            return "F5";
        }
        else if (key.equals(Keys.F6.toString()))
        {
            return "F6";
        }
        else if (key.equals(Keys.F7.toString()))
        {
            return "F7";
        }
        else if (key.equals(Keys.F8.toString()))
        {
            return "F8";
        }
        else if (key.equals(Keys.F9.toString()))
        {
            return "F9";
        }
        else if (key.equals(Keys.HELP.toString()))
        {
            return "Help";
        }
        else if (key.equals(Keys.HOME.toString()))
        {
            return "Home";
        }
        else if (key.equals(Keys.INSERT.toString()))
        {
            return "Insert";
        }
        else if (key.equals(Keys.LEFT.toString()))
        {
            return "Left";
        }
        else if (key.equals(Keys.LEFT_ALT.toString()))
        {
            return "LeftAlt";
        }
        else if (key.equals(Keys.LEFT_CONTROL.toString()))
        {
            return "LeftControl";
        }
        else if (key.equals(Keys.LEFT_SHIFT.toString()))
        {
            return "LeftShift";
        }
        else if (key.equals(Keys.META.toString()))
        {
            return "Meta";
        }
        else if (key.equals(Keys.MULTIPLY.toString()))
        {
            return "Multiply";
        }
        else if (key.equals(Keys.NULL.toString()))
        {
            return "Null";
        }
        else if (key.equals(Keys.NUMPAD0.toString()))
        {
            return "NumberPad0";
        }
        else if (key.equals(Keys.NUMPAD1.toString()))
        {
            return "NumberPad1";
        }
        else if (key.equals(Keys.NUMPAD2.toString()))
        {
            return "NumberPad2";
        }
        else if (key.equals(Keys.NUMPAD3.toString()))
        {
            return "NumberPad3";
        }
        else if (key.equals(Keys.NUMPAD4.toString()))
        {
            return "NumberPad4";
        }
        else if (key.equals(Keys.NUMPAD5.toString()))
        {
            return "NumberPad5";
        }
        else if (key.equals(Keys.NUMPAD6.toString()))
        {
            return "NumberPad6";
        }
        else if (key.equals(Keys.NUMPAD7.toString()))
        {
            return "NumberPad7";
        }
        else if (key.equals(Keys.NUMPAD8.toString()))
        {
            return "NumberPad8";
        }
        else if (key.equals(Keys.NUMPAD9.toString()))
        {
            return "NumberPad9";
        }
        else if (key.equals(Keys.PAGE_DOWN.toString()))
        {
            return "PageDown";
        }
        else if (key.equals(Keys.PAGE_UP.toString()))
        {
            return "PageUp";
        }
        else if (key.equals(Keys.PAUSE.toString()))
        {
            return "Pause";
        }
        else if (key.equals(Keys.RETURN.toString()))
        {
            return "Return";
        }
        else if (key.equals(Keys.RIGHT.toString()))
        {
            return "Right";
        }
        else if (key.equals(Keys.SEMICOLON.toString()))
        {
            return "Semicolon";
        }
        else if (key.equals(Keys.SEPARATOR.toString()))
        {
            return "Separator";
        }
        else if (key.equals(Keys.SHIFT.toString()))
        {
            return "Shift";
        }
        else if (key.equals(Keys.SPACE.toString()))
        {
            return "Space";
        }
        else if (key.equals(Keys.SUBTRACT.toString()))
        {
            return "Subtract";
        }
        else if (key.equals(Keys.TAB.toString()))
        {
            return "Tab";
        }
        else if (key.equals(Keys.UP.toString()))
        {
            return "Up";
        }
        else
        {
            return key;
        }
    }
}
