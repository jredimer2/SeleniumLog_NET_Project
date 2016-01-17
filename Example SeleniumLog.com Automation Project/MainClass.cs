using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumLogger;
using System.Drawing.Imaging;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;


namespace SeleniumTest
{

    public class HomePage  
    {
        private IWebDriver driver;

        [FindsBy(How = How.XPath, Using = "//li/a[@href='/index.html']")]
        public IWebElement Home;

        [FindsBy(How = How.XPath, Using = "//li/a[@href='/screenshots.html']")]
        public IWebElement Screenshots;

        [FindsBy(How = How.XPath, Using = "//li/a[@href='/contact-us.html']")]
        public IWebElement ContactUs;

        [FindsBy(How = How.XPath, Using = "//li/a[@href='/buy-now.html']")]
        public IWebElement BuyNow;

        [FindsBy(How = How.XPath, Using = "//li/a[@href='/documentation.html']")]
        public IWebElement Documentation;

        public HomePage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }
        
        /// <summary>
        /// Goto seleniumlog.com URL
        /// </summary>
        public void Goto()
        {
            SeleniumLog log = SeleniumLog.Instance();
            log.SaveIndent("Function-goto");
            log.Blue().WriteLine("Function: Goto()");
            log.Indent();

            driver.Navigate().GoToUrl("http://seleniumlog.com/");

            log.RestoreIndent("Function-goto"); //Ensures unindent back to correct level
        }
    }

    public class ContactUsPage 
    {
        private IWebDriver driver;

        [FindsBy(How = How.Id, Using = "input-780482698489254089")]
        public IWebElement FirstName;

        [FindsBy(How = How.Id, Using = "input-780482698489254089-1")]
        public IWebElement LastName;

        [FindsBy(How = How.Id, Using = "input-465062639798639934")]
        public IWebElement Email;
        
        [FindsBy(How = How.Id, Using = "input-417418761492911507")]
        public IWebElement Comment;

        [FindsBy(How = How.XPath, Using = "//span[contains(text(),'Submit')]/..")]
        public IWebElement Submit;

        private const string ErrorXPath = "//div[contains(text(),'Please correct the highlighted fields')]";
        [FindsBy(How = How.XPath, Using = ErrorXPath)]
        public IWebElement Error;

        private const string ThankYouXPath = "//div[contains(text(),'Thank you. Your information has been submitted')]";
        [FindsBy(How = How.XPath, Using = ThankYouXPath)]
        public IWebElement ThankYou;


        public ContactUsPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        /// <summary>
        /// Tests if element exists on the page
        /// </summary>
        public bool IsExist(string XPATH)
        {
            try
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Config.OnWebdriverExceptionThrown_LogEvent = false; //temporary disable screenshots and diagnosis on assert
                IWebElement elem = driver.FindElement(By.XPath(XPATH));
                log.Config.OnWebdriverExceptionThrown_LogEvent = true;
                return true;
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Config.OnWebdriverExceptionThrown_LogEvent = true;
                return false;
            }
        }

        /// <summary>
        /// Verifies if Comment is submitted in ContactUs page
        /// </summary>
        public void VerifySubmitted() 
        {
            SeleniumLog log = SeleniumLog.Instance();
            log.SaveIndent("Function 1");
            log.Blue().WriteLine("Function: MY_CUSTOM_VerifySubmitted()");
            log.Indent();

            log.ResetResult();
            log.AreNotEqual(notExpected: true, actual: IsExist(ErrorXPath), message: "Verify that Error message is not displayed");
            log.AreEqual(expected: true, actual: IsExist(ThankYouXPath), message: "Verify that Thank You message is submitted");
            log.PublishResult(); //In case one of the above assert fails, it doesn't exist straight away

            log.RestoreIndent("Function 1");  //Ensures unindent back to correct level
        }

        /// <summary>
        /// Verifies if Comment is NOT submitted in ContactUs page
        /// </summary>
        public void VerifyNotSubmitted() 
        {
            SeleniumLog log = SeleniumLog.Instance();
            log.SaveIndent("Function 2");
            log.Blue().WriteLine("Function: VerifyNotSubmitted()");
            log.Indent();            

            log.ResetResult();
            log.AreEqual(expected: true, actual: IsExist(ErrorXPath), message: "Verify that Error message is displayed");
            log.AreNotEqual(notExpected: true, actual: IsExist(ThankYouXPath), message: "Verify that Thank You message is not submitted");
            log.PublishResult(); //In case one of the above assert fails, it doesn't exist straight away

            log.RestoreIndent("Function 2");  //Ensures unindent back to correct level
        }
    }

    public class TestCase
    {
        private IWebDriver driver {get;set;}
        private HomePage Home {get;set;}
        private ContactUsPage Contact {get;set;}

        public TestCase() {
            FirefoxDriver driver0 = new FirefoxDriver();
            driver = new SeleniumLogEventListener(driver0);
            Home = new HomePage(driver);
            Contact = new ContactUsPage(driver);
            SeleniumLog log = SeleniumLog.Instance(driver);  //Only need to pass in driver once as it is a Singleton object
        }


        public void Run()
        {
            SeleniumLog log = SeleniumLog.Instance();

            log.Path("TestCase 1: Site navigation");

            log.WriteLine("Step 1: Goto Seleniumlog.com website");
            log.Indent();
            Home.Goto();
            log.Unindent();

            log.WriteLine("Step 2: Goto Buy Now page");
            Home.BuyNow.Click();

            log.WriteLine("Step 3: Goto Contact Us page");
            Home.ContactUs.Click();

            log.WriteLine("Step 4: Enter First Name");
            Contact.FirstName.SendKeys("James");

            log.WriteLine("Step 5: Enter Last Name");
            Contact.LastName.SendKeys("Smith");

            log.WriteLine("Step 6: Enter Email address");
            Contact.Email.SendKeys("james.smith.seleniumlog.com");

            log.WriteLine("Step 7: Enter Comments");
            Contact.Comment.SendKeys("Hello World!");

            log.WriteLine("Step 8: Click on Submit button");
            Contact.Submit.Click();            

            log.WriteLine("Step 9: Verify not submitted");
            log.Indent();
            Contact.VerifyNotSubmitted();
            log.Unindent();
          
            log.WriteLine("Step 10: Re-Enter Email address");
            Contact.Email.SendKeys("james.smith@seleniumlog.com");

            log.WriteLine("Step 11: Click on Submit button");
            Contact.Submit.Click();            

            log.WriteLine("Step 12: Verify submitted successfully");
            log.Indent();
            Contact.VerifySubmitted();
            log.Unindent();

        }
    }
}

