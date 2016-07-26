
package TestExampleInstantiations;

import SeleniumLogger.SeleniumLog;
import SeleniumLogger.SeleniumLogEventListener;
import java.text.ParseException;

import org.openqa.selenium.By;
import org.openqa.selenium.By.ByXPath;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.firefox.FirefoxDriver;

class TestClass
{
    public void Message(String msg)
    {
        try
        {
            SeleniumLog log = SeleniumLog.Instance();
            log.WriteLine(String.format("TestClass :: Message :: %s", msg));
        }
        catch (Exception e)
        {
            SeleniumLog log = SeleniumLog.Instance();
            log.Error().WriteLine(String.format("TestClass :: Exception :: %s", e.getMessage()));
        }
    }
    public TestClass() { }
}

public class ExampleInstantiationsTest {
    
    static void TestIndents()
    {
        SeleniumLog log = SeleniumLog.Instance();
        log.WriteLine("line 1");
        log.WriteLine("line 2");
        log.Indent().Indent().WriteLine("line 3");
        log.SaveIndent("ID1");
        log.WriteLine("line 4");
        log.Indent().Indent().Indent().Indent().Indent().WriteLine("line 5");
        log.Unindent().Unindent().WriteLine("line 6");
        log.WriteLine("line 7");
        log.WriteLine("line 8");
        log.WriteLine("line 9");
        log.WriteLine("line 10");
        log.WriteLine("line 11");
        log.WriteLine("line 12");
        log.WriteLine("line 13");
        log.WriteLine("line 14");
        log.WriteLine("line 15");
        log.WriteLine("line 16");
        log.WriteLine("line 17");
        log.WriteLine("line 18");
        log.WriteLine("line 19");
        log.RestoreIndent("ID1");
        log.WriteLine("line 20");
        log.WriteLine("line 21");
        log.WriteLine("line 22");
        log.WriteLine("line 23");
        log.WriteLine("line 24");
        log.WriteLine("line 25");
        log.WriteLine("line 26");
        log.WriteLine("line 27");
        log.WriteLine("line 28");
        log.WriteLine("line 29");
        log.WriteLine("line 30");
    }
    
    static void TestColors() {
    	SeleniumLog log = SeleniumLog.Instance();
    	log.Blue().WriteLine("Blue");
    	log.BlueGreen("BlueGrn");
    	log.Orange("Orange");
    	log.Red().WriteLine("Red");    	
    }
    
    static void TestSeleniumWebdriverIntegration() {
        FirefoxDriver driver0 = new FirefoxDriver();
        SeleniumLogEventListener driver = new SeleniumLogEventListener(driver0);        
        
        // This works
        driver0.get("http://google.com/");

        // Test PASS - This also works fine. So SeleniumLogEvenListener() is probably returning a proper Selenium object.
        // Test FAIL - However, if you leave it running for about a minute, exceptions are thrown. Need to fix this.
        driver.get("http://upwork.com/");
        
        // Test FAIL - This somehow causes some exceptions when I pass in driver or driver0 object to Instance().
        SeleniumLog log = SeleniumLog.Instance(driver);
        log.WriteLine("Testing Selenium Webdriver Integration");
        log.Screenshot();
        log.Pass();
    }
    
    static void TestScreenshots() {
    	FirefoxDriver driver0 = new FirefoxDriver();
    	SeleniumLog log = SeleniumLog.Instance();
    	
    	log.WriteLine("Testing Screenshots");
    	driver0.get("http://google.com");
    	//log.Screenshot();
    	log.WriteLine("Screenshot complete");
    	
    }
    
    static void TestSeleniumLogEventListener() {
    	//ChromeDriver driver0 = new ChromeDriver();
    	FirefoxDriver driver0 = new FirefoxDriver();
        SeleniumLogEventListener driver = new SeleniumLogEventListener(driver0); 
    	SeleniumLog log = SeleniumLog.Instance(driver);
    	
        log.WriteLine("Step 1: Goto Seleniumlog.com website");
        driver.get("http://seleniumlog.com");

        log.WriteLine("Step 2: Goto Downloads page");
        driver.findElement(By.xpath("//li/a[@href='/downloads.html']")).click();
        
        log.WriteLine("Step 3: Goto Contact Us page");
        driver.findElement(By.xpath("//li/a[@href='/contact-us.html']")).click();

        log.WriteLine("Step 4: Enter First Name");
        driver.findElement(By.id("input-780482698489254089")).sendKeys("Harold");

        log.WriteLine("Step 5: Enter Last Name");
        driver.findElement(By.id("input-780482698489254089-1")).sendKeys("Chung");

        log.WriteLine("Step 6: Enter Email address");
        driver.findElement(By.id("input-465062639798639934")).sendKeys("jredimer@yahoo.com.au");

        log.WriteLine("Step 7: Enter Comments");
        driver.findElement(By.id("input-417418761492911507")).sendKeys("JAVA Extension");

        log.WriteLine("Step 8: Click on Submit button");
        driver.findElement(By.xpath("//span[contains(text(),'Submit')]/..")).click();    

    }
    
    public static void main(String[] args) throws ParseException {
    	
    	//TestIndents();
    	//TestColors();
    	//TestSeleniumWebdriverIntegration();
    	//TestScreenshots();
    	TestSeleniumLogEventListener();
    	
    	/*
        FirefoxDriver driver0 = new FirefoxDriver();
        SeleniumLogEventListener driver = new SeleniumLogEventListener(driver0);
        SeleniumLog log = SeleniumLog.Instance(driver);

        TestClass tobj = new TestClass();

        log.WriteLine("Main");
        Foo();
        tobj.Message("hi there");
        
        driver.get("http://www.google.com");
        driver.navigate().to("http://www.upwork.com");
        driver.navigate().back();
        */
    }
}
