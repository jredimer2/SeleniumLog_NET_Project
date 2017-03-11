using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;
using SeleniumLogger;

namespace Example_Automation_of_Dummy_Airline_Booking_Project
//namespace Dummysite
{
    public class PAGE_OBJECT_MODEL
    {
        public class Page1_Welcome
        {
            private IWebDriver driver;

            [FindsBy(How = How.XPath, Using = "//select[@name='fromPort']")]
            public IWebElement FromCountry;

            [FindsBy(How = How.XPath, Using = "//select[@name='toPort']")]
            public IWebElement ToCountry;

            [FindsBy(How = How.XPath, Using = "//input[@value='Find Flights']")]
            public IWebElement FindFlights;

            public Page1_Welcome(IWebDriver driver)
            {
                this.driver = driver;
                PageFactory.InitElements(driver, this);
            }
        }

        public class Page2_ChooseFlights
        {
            private IWebDriver driver;

            public Page2_ChooseFlights(IWebDriver driver)
            {
                this.driver = driver;
                PageFactory.InitElements(driver, this);
            }

            public IWebElement GetChooseFlightButton(int FlightId)
            {
                try
                {
                    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
                    string XPATH = "//td[contains(text(),'" + FlightId + "')]/../td/input[@value='Choose This Flight']";
                    IWebElement elem = driver.FindElement(By.XPath(XPATH));
                    return elem;
                }
                catch
                {
                    throw new Exception("GetChooseButton :: Exception :: " + "//td[contains(text(),'" + FlightId + "')]/../td/input[@value='Choose This Flight']  NOT FOUND");
                }
                finally
                {
                    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                }

            }
        }

        public class Page3_PaymentDetails
        {
            private IWebDriver driver;

            [FindsBy(How = How.XPath, Using = "//input[@name='inputName']")]
            public IWebElement Name;

            [FindsBy(How = How.XPath, Using = "//input[@id='address']")]
            public IWebElement Address;

            [FindsBy(How = How.XPath, Using = "//input[@id='city']")]
            public IWebElement City;

            [FindsBy(How = How.XPath, Using = "//input[@id='state']")]
            public IWebElement State;

            [FindsBy(How = How.XPath, Using = "//input[@id='zipCode']")]
            public IWebElement ZipCode;

            [FindsBy(How = How.XPath, Using = "//select[@id='cardType']")]
            public IWebElement CardType;

            [FindsBy(How = How.XPath, Using = "//input[@id='creditCardNumber']")]
            public IWebElement CardNumber;

            [FindsBy(How = How.XPath, Using = "//input[@id='creditCardMonth']")]
            public IWebElement Month;

            [FindsBy(How = How.XPath, Using = "//input[@id='creditCardYear']")]
            public IWebElement Year;

            [FindsBy(How = How.XPath, Using = "//input[@id='nameOnCard']")]
            public IWebElement NameOnCard;

            [FindsBy(How = How.XPath, Using = "//p[contains(text(),'Price:')]")]
            public IWebElement Price;

            [FindsBy(How = How.XPath, Using = "//input[@id='purchase']")]
            public IWebElement Purchase;

            [FindsBy(How = How.XPath, Using = "//a[@id='home_button']")]
            public IWebElement Home;

            public Page3_PaymentDetails(IWebDriver driver)
            {
                this.driver = driver;
                PageFactory.InitElements(driver, this);
            }
        }

        private IWebDriver driver;
        public Page1_Welcome p1;
        public Page2_ChooseFlights p2;
        public Page3_PaymentDetails p3;

        public PAGE_OBJECT_MODEL(IWebDriver driver) {
            this.driver = driver;
            p1 = new Page1_Welcome(this.driver);
            p2 = new Page2_ChooseFlights(this.driver);
            p3 = new Page3_PaymentDetails(this.driver);
        }
    }

    public class DUMMYSITE_TEST_CASES
    {
        public PAGE_OBJECT_MODEL pom;

        private IWebDriver driver1;

        public DUMMYSITE_TEST_CASES()
        {
            IWebDriver driver0 = new ChromeDriver(@"C:\Selenium Drivers");
            //IWebDriver driver0 = new InternetExplorerDriver(@"C:\Selenium Drivers");
            driver1 = new SeleniumLogEventListener(driver0); //Bind Selenium Event Listener to driver1
            SeleniumLog log = SeleniumLog.Instance(driver0);  //Initialise SeleniumLog with driver1 instance (containing an Event Listener already)
            pom = new PAGE_OBJECT_MODEL(driver1);
            driver1.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(60));
            driver1.Navigate().GoToUrl("http://www.dummysite.net");
        }

        private void SelectFlight(string FROM, string TO, int FLIGHTNUM)
        {
            SeleniumLog log = SeleniumLog.Instance();

            log.Info("Choose " + FROM + " to " + TO);
            SelectElement fromSelect = new SelectElement(pom.p1.FromCountry);
            fromSelect.SelectByValue(FROM);

            SelectElement toSelect = new SelectElement(pom.p1.ToCountry);
            toSelect.SelectByValue(TO);

            log.Info("Click on Choose Flights button");
            pom.p1.FindFlights.Click();

            log.Info("Choose Flight number " + FLIGHTNUM);
            pom.p2.GetChooseFlightButton(FLIGHTNUM).Click();

            log.Info("Enter payment details");
            pom.p3.Name.SendKeys("James Ni");

            log.Info("Click on Purchase button");
            pom.p3.Purchase.Click();

            log.Info("OK Alert");
            log.Config.OnWebdriverExceptionThrown_LogEvent = false;
            WebDriverWait wait = new WebDriverWait(driver1, TimeSpan.FromSeconds(10));
            IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());
            alert.Accept();
            log.Config.OnWebdriverExceptionThrown_LogEvent = true;

            log.Info("Click Home button");
            pom.p3.Home.Click();

        }
        public void TestCase1()
        {
            SeleniumLog log = SeleniumLog.Instance();

            log.Path("Test Case 1");
            SelectFlight(
                FROM: "Boston",
                TO: "Rome",
                FLIGHTNUM: 622
             );
        }

        public void TestCase2()
        {
            SeleniumLog log = SeleniumLog.Instance();

            log.Path("Test Case 2");
            SelectFlight(
                FROM: "Paris",
                TO: "Berlin",
                FLIGHTNUM: 15
             );
        }

        public void TestCase3()
        {
            SeleniumLog log = SeleniumLog.Instance();

            log.Path("Test Case 3");
            SelectFlight(
                FROM: "Paris",
                TO: "London",
                FLIGHTNUM: 711
             );
        }

    }
        
    //}
}
