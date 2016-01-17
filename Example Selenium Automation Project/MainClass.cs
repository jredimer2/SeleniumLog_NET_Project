using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using SeleniumLogger;
using System.Drawing.Imaging;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;


namespace SeleniumTest
{

    public class SeleniumHomePage : BaseClass
    {
        private IWebDriver driver;

        [FindsBy(How = How.XPath, Using = "//li[@id='menu_projects']/a")]
        public IWebElement ProjectsTab;

        [FindsBy(How = How.XPath, Using = "//li[@id='menu_download']/a")]
        public IWebElement DownloadTab;

        [FindsBy(How = How.XPath, Using = "//li[@id='menu_documentation']/a")]
        public IWebElement DocumentationTab;

        [FindsBy(How = How.XPath, Using = "//li[@id='menu_support']/a")]
        public IWebElement SupportTab;

        [FindsBy(How = How.XPath, Using = "//li[@id='menu_about']/a")]
        public IWebElement AboutTab;

        [SeleniumLogTrace]
        public SeleniumHomePage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        [SeleniumLogTrace]
        public void Greeting1(string message)
        {
            SeleniumLog log = SeleniumLog.Instance();
            log.WriteLine(message);
        }
    }

    public class SeleniumDocumentationPage : BaseClass
    {
        private IWebDriver driver;

        [FindsBy(How = How.XPath, Using = "//a[contains(text(),'Introduction')]")]
        public IWebElement Introduction;

        [FindsBy(How = How.XPath, Using = "//ul[@class='treeview']/li/a[contains(text(),'Test Automation For Web Applications')]")]
        public IWebElement TestAutomationForWebApplications;

        [FindsBy(How = How.XPath, Using = "//ul[@class='treeview']/li/a[contains(text(),'Selenium’s Tool Suite')]")]
        public IWebElement SeleniumToolSuite;

        [FindsBy(How = How.XPath, Using = "//ul[@class='treeview']/li/a[contains(text(),'Introducing Selenium')]")]
        public IWebElement IntroducingSelenium;

        [FindsBy(How = How.XPath, Using = "//ul[@class='treeview']/li/a[contains(text(),'Supported Browsers and Platforms')]")]
        public IWebElement SupportedBrowsersAndPlatforms;

        [SeleniumLogTrace]
        public SeleniumDocumentationPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        [SeleniumLogTrace]
        public void Greeting2(string message)
        {
            SeleniumLog log = SeleniumLog.Instance();
            log.WriteLine(message);
        }
    }

    public class MainTest
    {
        public MainTest() { }

        public void Run()
        {
            SeleniumLog log = SeleniumLog.Instance();
            log.WriteLine("Run: Start");
        }

        public void Run2()
        {
            
            FirefoxDriver driver0 = new FirefoxDriver();
            SeleniumLogEventListener driver = new SeleniumLogEventListener(driver0);
            //SeleniumEventListener driver = SeleniumLogEventListener(driver0);
            SeleniumHomePage Home = new SeleniumHomePage(driver);
            SeleniumDocumentationPage Doc = new SeleniumDocumentationPage(driver);
            SeleniumLog log = SeleniumLog.Instance(driver);

            log.WriteLine("Step 0: Display message");
            log.Indent();
            Home.Greeting1("Hi there");
            log.Unindent();

            log.WriteLine("Step 0.5: Display message");
            log.Indent();
            Doc.Greeting2("Hi there 2");
            log.Unindent();
            
            log.WriteLine("Step 1: Go to SeleniumHQ home page");
            driver.Navigate().GoToUrl("http://seleniumhq.org/");

            log.WriteLine("Step 2: Click on Documentation tab");
            Home.DocumentationTab.Click();

            log.WriteLine("Step 3: Click on Introduction link");
            Doc.Introduction.Click();

            log.WriteLine("Step 4: Click on Selenium Tool Suite link");
            Doc.SeleniumToolSuite.Click();

            log.WriteLine("Step 5: Click on Supported Browsers and Platforms link");
            Doc.SupportedBrowsersAndPlatforms.Click();

            log.WriteLine("complete.");
        }
    }
}
