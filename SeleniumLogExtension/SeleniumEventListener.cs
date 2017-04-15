using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.Events;
using System;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using XMLConfig;

namespace SeleniumLogger
{
    public class SeleniumLogEventListener : EventFiringWebDriver
    {
        private class XPathTester
        {
            private class Result
            {
                public bool result { get; set; }
                public int matches { get; set; }
                public string cummulative_xpath { get; set; }
                public Result(bool result, int matches, string cummulative_xpath) {
                    this.result = result;
                    this.matches = matches;
                    this.cummulative_xpath = cummulative_xpath;
                }
            }

            private List<Result> Results = new List<Result>();
            public XPathTester() {
            }

            private void Reset()
            {
                Results.Clear();
            }

            public void Test(IWebDriver driver, string XPath)
            {
                try
                {
                    string XPath2 = Regex.Replace(input: XPath, pattern: @"^\/\/", replacement: "");
                    string[] components = XPath2.Split('/');
                    string cummulative_xpath = "";
                    int i = 0;
                    bool res = false;

                    Reset();

                    foreach (string comp in components)
                    {
                        i++;
                        if (i == 1)
                        {
                            cummulative_xpath = "//" + comp;
                        }
                        else
                        {
                            cummulative_xpath = cummulative_xpath + "/" + comp;
                        }

                        //Test component
                        try
                        {
                            ReadOnlyCollection<IWebElement> foundelements = driver.FindElements(By.XPath(cummulative_xpath));
                            if (foundelements.Count > 0)
                            {
                                Results.Add(new Result(result: true, matches: foundelements.Count, cummulative_xpath: cummulative_xpath));
                            }
                            else
                            {
                                Results.Add(new Result(result: false, matches: foundelements.Count, cummulative_xpath: cummulative_xpath));
                            }
                        }
                        catch (InvalidSelectorException e) { }

                    }
                }
                catch
                { }
                finally
                {
                }

            }
            public void DisplayResults() {

                SeleniumLog log = SeleniumLog.Instance();

                foreach (Result result in Results) {
                    if (result.result == true) {
                        log.Green().Pass().Debug(result.cummulative_xpath + "  - XPath Found - " + result.matches + " matches.");
                    } else {
                        log.Red().Fail().Debug(result.cummulative_xpath + "  - XPath Not Found!");
                    }
                }
            }  
        } // end class XPathTester

        private XPathTester XPathTest = new XPathTester();
        private string _keyInput;
        private string _by = "";
        private string _locator = "";
        private IWebDriver driver;
        private bool _enabled;

        /// <summary>
        /// Send 'true (boolen)' to listen to selenium events
        /// </summary>
        /// <param name="enable"></param>
        public void EnableEventListening(bool enable)
        {
            if (enable)
            {
                SubscribeEvents();
            }
            else
            {
                UnsubscribeEvents();
            }

            _enabled = enable;
        }

        // XPath Generator JavaScript
        private String GenerateElementXPath(IWebDriver driver, IWebElement element)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            return (string)js.ExecuteScript("gPt=function(c){if(c.id!==''){return'id(\"'+c.id+'\")'}if(c===document.body){return c.tagName}var a=0;var e=c.parentNode.childNodes;for(var b=0;b<e.length;b++){var d=e[b];if(d===c){return gPt(c.parentNode)+'/'+c.tagName+'['+(a+1)+']'}if(d.nodeType===1&&d.tagName===c.tagName){a++}}};return gPt(arguments[0]).toLowerCase();", element);
        }

        public SeleniumLogEventListener(IWebDriver driver)
            : base(driver)
        {
            XmlConfigurationClass Config = XmlConfigurationClass.Instance();
            this.driver = driver;
            EnableEventListening(Convert.ToBoolean(Config.EnableLoggingOfLowLevelSeleniumWebdriverEvents));
        }



        /// <summary>
        /// Subscribes all Selenium Events
        /// </summary>
        private void SubscribeEvents()
        {
            // Makes sure that events are only subscribed once
            UnsubscribeEvents();

            ElementClicked += new EventHandler<WebElementEventArgs>(OnElementClicked);

            ElementClicking += new EventHandler<WebElementEventArgs>(OnElementClicking);

            ElementValueChanged += new EventHandler<WebElementEventArgs>(OnElementValueChanged);

            ElementValueChanging += new EventHandler<WebElementEventArgs>(OnElementValueChanging);

            ExceptionThrown += new EventHandler<WebDriverExceptionEventArgs>(OnExceptionThrown);

            FindingElement += new EventHandler<FindElementEventArgs>(OnFindingElement);

            FindElementCompleted += new EventHandler<FindElementEventArgs>(OnFindingElementCompleted);

            Navigated += new EventHandler<WebDriverNavigationEventArgs>(OnNavigated);

            NavigatedBack += new EventHandler<WebDriverNavigationEventArgs>(OnNavigatedBack);

            NavigatedForward += new EventHandler<WebDriverNavigationEventArgs>(OnNavigatedForward);

            Navigating += new EventHandler<WebDriverNavigationEventArgs>(OnNavigating);

            NavigatingBack += new EventHandler<WebDriverNavigationEventArgs>(OnNavigatingBack);

            NavigatingForward += new EventHandler<WebDriverNavigationEventArgs>(OnNavigatingForward);

            ScriptExecuted += new EventHandler<WebDriverScriptEventArgs>(OnScriptExecuted);

            ScriptExecuting += new EventHandler<WebDriverScriptEventArgs>(OnScriptExecuting);
        }


        /// <summary>
        /// Un-subscribes all registered Selenium Events
        /// </summary>
        private void UnsubscribeEvents()
        {
            ElementClicked -= new EventHandler<WebElementEventArgs>(OnElementClicked);

            ElementClicking -= new EventHandler<WebElementEventArgs>(OnElementClicking);

            ElementValueChanged -= new EventHandler<WebElementEventArgs>(OnElementValueChanged);

            ElementValueChanging -= new EventHandler<WebElementEventArgs>(OnElementValueChanging);

            ExceptionThrown -= new EventHandler<WebDriverExceptionEventArgs>(OnExceptionThrown);

            FindingElement -= new EventHandler<FindElementEventArgs>(OnFindingElement);

            FindElementCompleted -= new EventHandler<FindElementEventArgs>(OnFindingElementCompleted);

            Navigated -= new EventHandler<WebDriverNavigationEventArgs>(OnNavigated);

            NavigatedBack -= new EventHandler<WebDriverNavigationEventArgs>(OnNavigatedBack);

            NavigatedForward -= new EventHandler<WebDriverNavigationEventArgs>(OnNavigatedForward);

            Navigating -= new EventHandler<WebDriverNavigationEventArgs>(OnNavigating);

            NavigatingBack -= new EventHandler<WebDriverNavigationEventArgs>(OnNavigatingBack);

            NavigatingForward -= new EventHandler<WebDriverNavigationEventArgs>(OnNavigatingForward);

            ScriptExecuted -= new EventHandler<WebDriverScriptEventArgs>(OnScriptExecuted);

            ScriptExecuting -= new EventHandler<WebDriverScriptEventArgs>(OnScriptExecuting);
        }


        private void OnElementClicking(object sender, WebElementEventArgs webElementEventArgs)
        { 
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnClick_LogBeforeEvent)
            {
                log.Indent();
                log.Debug("[Selenium Event]  Clicking Element: " + _locator, take_screenshot: log.Config.OnClick_TakeScreenshotBeforeEvent);
                log.Unindent();
            }
        }

        private void OnElementClicked(object sender, WebElementEventArgs webElementEventArgs)
        {
            //Console.WriteLine("ElementClicked: " + webElementEventArgs.Element);
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnClick_LogAfterEvent)
            {
                log.Indent();
                log.Debug("[Selenium Event]  Click Success!", take_screenshot: log.Config.OnClick_TakeScreenshotAfterEvent);
                log.Unindent();
            }
        }


        private void OnElementValueChanging(object sender, WebElementEventArgs webElementEventArgs)
        {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnChangeValue_LogBeforeEvent)
            {
                log.SaveIndent("OnElementValueChanging");
                log.Indent();
                if (!string.IsNullOrEmpty(_keyInput))
                {
                    log.Debug("[Selenium Event]  Element value changing to [" + _keyInput + "]", take_screenshot: log.Config.OnChangeValue_TakeScreenshotBeforeEvent);
                }
                else
                {
                    log.Debug("[Selenium Event]  Element value changing to - NULL or EMPTY - ", take_screenshot: log.Config.OnChangeValue_TakeScreenshotBeforeEvent);
                }
                log.RestoreIndent("OnElementValueChanging", debug: true);
            }
        }

        private void OnElementValueChanged(object sender, WebElementEventArgs webElementEventArgs)
        {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnChangeValue_LogAfterEvent)
            {
                log.SaveIndent("OnElementValueChanged");
                log.Indent();
                log.Debug("[Selenium Event]  Successfully changed value [" + webElementEventArgs.Element.GetAttribute("value") + "]", take_screenshot: log.Config.OnChangeValue_TakeScreenshotAfterEvent);
                log.Unindent();

                if (!string.IsNullOrEmpty(_keyInput))
                {
                    //log.WriteLine("Input was: " + _keyInput);
                }
                log.RestoreIndent("OnElementValueChanged");
            }
        }

        
        private void OnExceptionThrown(object sender, WebDriverExceptionEventArgs webDriverExceptionEventArgs)
        {
            SeleniumLog log = SeleniumLog.Instance();      

            if (log.Config.OnWebdriverExceptionThrown_LogEvent)
            {
                log.Indent();  
                log.Error().Red().Debug("[Selenium Event]  Exception Thrown: " + webDriverExceptionEventArgs.ThrownException, take_screenshot: true);
                if (_by == "XPath")
                {
                    log.Red().Debug("Running XPath Diagnostics: Expand to see which part of XPath failed.");
                    log.Indent();
                    XPathTest.Test(driver, _locator);
                    XPathTest.DisplayResults();
                    log.Unindent();
                }
                log.Unindent();
            }
            
        }

        private void OnFindingElement(object sender, FindElementEventArgs findElementEventArgs)
        {
            SeleniumLog log = SeleniumLog.Instance();
            string[] FindMethodStr = findElementEventArgs.FindMethod.ToString().Split(':');
            _by = FindMethodStr[0].Split('.')[1].Trim();
            _locator = FindMethodStr[1].Trim();

            if (log.Config.OnFindElement_LogBeforeEvent)
            {
                log.Indent();
                log.Debug("[Selenium Event]  Finding Element: " + _locator, take_screenshot: log.Config.OnFindElement_TakeScreenshotBeforeEvent);
                log.Unindent();
            }
        }

        private void OnFindingElementCompleted(object sender, FindElementEventArgs findElementEventArgs)
        {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnFindElement_LogAfterEvent)
            {
                log.Indent();
                log.Debug("[Selenium Event]  Element Found!", take_screenshot: log.Config.OnFindElement_TakeScreenshotAfterEvent);
                log.Unindent();
            }
        }

        private void OnNavigating(object sender, WebDriverNavigationEventArgs webDriverNavigationEventArgs)
        {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnNavigating_LogBeforeEvent)
            {
                log.Indent();
                log.Debug("[Selenium Event]  Navigating To: " + webDriverNavigationEventArgs.Url, take_screenshot: log.Config.OnNavigating_TakeScreenshotBeforeEvent);
                log.Unindent();
            }
        }

        private void OnNavigated(object sender, WebDriverNavigationEventArgs webDriverNavigationEventArgs)
        {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnNavigating_LogAfterEvent)
            {
                log.Indent();
                log.Debug("[Selenium Event]  Navigation Success!", take_screenshot: log.Config.OnNavigating_TakeScreenshotBeforeEvent);
                log.Unindent();
            }
        }

        private void OnNavigatingBack(object sender, WebDriverNavigationEventArgs webDriverNavigationEventArgs)
        {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnNavigatingBack_LogBeforeEvent)
            {
                log.Indent();
                log.Debug("[Selenium Event]  Navigating Back: " + webDriverNavigationEventArgs.Url, take_screenshot: log.Config.OnNavigatingBack_TakeScreenshotBeforeEvent);
                log.Unindent();
            }
        }

        private void OnNavigatedBack(object sender, WebDriverNavigationEventArgs webDriverNavigationEventArgs)
        {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnNavigatingBack_LogAfterEvent)
            {
                log.Indent();
                log.Debug("[Selenium Event]  Navigate Back Success!", take_screenshot: log.Config.OnNavigatingBack_TakeScreenshotAfterEvent);
                log.Unindent();
            }
        }


        private void OnNavigatingForward(object sender, WebDriverNavigationEventArgs webDriverNavigationEventArgs)
        {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnNavigatingForward_LogBeforeEvent)
            {
                log.Indent();
                log.Debug("[Selenium Event]  Navigating Forward: " + webDriverNavigationEventArgs.Url, take_screenshot: log.Config.OnNavigatingForward_TakeScreenshotBeforeEvent);
                log.Unindent();
            }
        }

        private void OnNavigatedForward(object sender, WebDriverNavigationEventArgs webDriverNavigationEventArgs)
        {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnNavigatingForward_LogAfterEvent)
            {
                log.Indent();
                log.Debug("[Selenium Event]  Navigate Forward Success!", take_screenshot: log.Config.OnNavigatingForward_TakeScreenshotAfterEvent);
                log.Unindent();
            }
        }


        private void OnScriptExecuting(object sender, WebDriverScriptEventArgs webDriverScriptEventArgs)
        {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnScriptExecute_LogBeforeEvent)
            {
                log.Indent();
                log.Debug("[Selenium Event]  Script Executing: " + webDriverScriptEventArgs.Script, take_screenshot: log.Config.OnScriptExecute_TakeScreenshotBeforeEvent);
                log.Unindent();
            }
        }

        private void OnScriptExecuted(object sender, WebDriverScriptEventArgs webDriverScriptEventArgs)
        {
            SeleniumLog log = SeleniumLog.Instance();
            if (log.Config.OnScriptExecute_LogAfterEvent)
            {
                log.Indent();
                log.Debug("[Selenium Event]  Script Executed Successfully!", take_screenshot: log.Config.OnScriptExecute_TakeScreenshotAfterEvent);
                log.Unindent();
            }
        }




        //=========================================================================================
        /// <summary>
        /// Sends required key to the given element keeping track of the input used
        /// </summary>
        /// <param name="element">Element to which to send the key</param>
        /// <param name="key">Key to send</param>
        public void SendKeys(IWebElement element, string key)
        {
            // Save the key input to be used for future reference
            _keyInput = KeyInputName(key);

            // Send the key to element to perform action
            element.SendKeys(key);
        }

        /// <summary>
        /// Converts the keyboard keys to a readable string name
        /// </summary>
        /// <param name="key">Key to convert to string name</param>
        /// <returns></returns>
        private string KeyInputName(string key)
        {
            if (key.Equals(Keys.Add))
            {
                return "Add";
            }
            else if (key.Equals(Keys.Alt))
            {
                return "Alt";
            }
            else if (key.Equals(Keys.ArrowDown))
            {
                return "ArrowDown";
            }
            else if (key.Equals(Keys.ArrowLeft))
            {
                return "ArrowLeft";
            }
            else if (key.Equals(Keys.ArrowRight))
            {
                return "ArrowRight";
            }
            else if (key.Equals(Keys.ArrowUp))
            {
                return "ArrowUp";
            }
            else if (key.Equals(Keys.Backspace))
            {
                return "Backspace";
            }
            else if (key.Equals(Keys.Cancel))
            {
                return "Cancel";
            }
            else if (key.Equals(Keys.Clear))
            {
                return "Clear";
            }
            else if (key.Equals(Keys.Command))
            {
                return "Command";
            }
            else if (key.Equals(Keys.Control))
            {
                return "Control";
            }
            else if (key.Equals(Keys.Decimal))
            {
                return "Decimal";
            }
            else if (key.Equals(Keys.Delete))
            {
                return "Delete";
            }
            else if (key.Equals(Keys.Divide))
            {
                return "Divide";
            }
            else if (key.Equals(Keys.Down))
            {
                return "Down";
            }
            else if (key.Equals(Keys.End))
            {
                return "End";
            }
            else if (key.Equals(Keys.Enter))
            {
                return "Enter";
            }
            else if (key.Equals(Keys.Equal))
            {
                return "Equal";
            }
            else if (key.Equals(Keys.Escape))
            {
                return "Escape";
            }
            else if (key.Equals(Keys.F1))
            {
                return "F1";
            }
            else if (key.Equals(Keys.F10))
            {
                return "F10";
            }
            else if (key.Equals(Keys.F11))
            {
                return "F11";
            }
            else if (key.Equals(Keys.F12))
            {
                return "F12";
            }
            else if (key.Equals(Keys.F2))
            {
                return "F2";
            }
            else if (key.Equals(Keys.F3))
            {
                return "F3";
            }
            else if (key.Equals(Keys.F4))
            {
                return "F4";
            }
            else if (key.Equals(Keys.F5))
            {
                return "F5";
            }
            else if (key.Equals(Keys.F6))
            {
                return "F6";
            }
            else if (key.Equals(Keys.F7))
            {
                return "F7";
            }
            else if (key.Equals(Keys.F8))
            {
                return "F8";
            }
            else if (key.Equals(Keys.F9))
            {
                return "F9";
            }
            else if (key.Equals(Keys.Help))
            {
                return "Help";
            }
            else if (key.Equals(Keys.Home))
            {
                return "Home";
            }
            else if (key.Equals(Keys.Insert))
            {
                return "Insert";
            }
            else if (key.Equals(Keys.Left))
            {
                return "Left";
            }
            else if (key.Equals(Keys.LeftAlt))
            {
                return "LeftAlt";
            }
            else if (key.Equals(Keys.LeftControl))
            {
                return "LeftControl";
            }
            else if (key.Equals(Keys.LeftShift))
            {
                return "LeftShift";
            }
            else if (key.Equals(Keys.Meta))
            {
                return "Meta";
            }
            else if (key.Equals(Keys.Multiply))
            {
                return "Multiply";
            }
            else if (key.Equals(Keys.Null))
            {
                return "Null";
            }
            else if (key.Equals(Keys.NumberPad0))
            {
                return "NumberPad0";
            }
            else if (key.Equals(Keys.NumberPad1))
            {
                return "NumberPad1";
            }
            else if (key.Equals(Keys.NumberPad2))
            {
                return "NumberPad2";
            }
            else if (key.Equals(Keys.NumberPad3))
            {
                return "NumberPad3";
            }
            else if (key.Equals(Keys.NumberPad4))
            {
                return "NumberPad4";
            }
            else if (key.Equals(Keys.NumberPad5))
            {
                return "NumberPad5";
            }
            else if (key.Equals(Keys.NumberPad6))
            {
                return "NumberPad6";
            }
            else if (key.Equals(Keys.NumberPad7))
            {
                return "NumberPad7";
            }
            else if (key.Equals(Keys.NumberPad8))
            {
                return "NumberPad8";
            }
            else if (key.Equals(Keys.NumberPad9))
            {
                return "NumberPad9";
            }
            else if (key.Equals(Keys.PageDown))
            {
                return "PageDown";
            }
            else if (key.Equals(Keys.PageUp))
            {
                return "PageUp";
            }
            else if (key.Equals(Keys.Pause))
            {
                return "Pause";
            }
            else if (key.Equals(Keys.Return))
            {
                return "Return";
            }
            else if (key.Equals(Keys.Right))
            {
                return "Right";
            }
            else if (key.Equals(Keys.Semicolon))
            {
                return "Semicolon";
            }
            else if (key.Equals(Keys.Separator))
            {
                return "Separator";
            }
            else if (key.Equals(Keys.Shift))
            {
                return "Shift";
            }
            else if (key.Equals(Keys.Space))
            {
                return "Space";
            }
            else if (key.Equals(Keys.Subtract))
            {
                return "Subtract";
            }
            else if (key.Equals(Keys.Tab))
            {
                return "Tab";
            }
            else if (key.Equals(Keys.Up))
            {
                return "Up";
            }
            else
            {
                return key;
            }
        }
    }
}