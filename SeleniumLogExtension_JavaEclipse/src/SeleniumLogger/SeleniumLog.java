/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package SeleniumLogger;

/// <summary>

import XMLConfig.XmlConfigurationClass;

import java.awt.AWTException;
import java.awt.Color;
import java.awt.Rectangle;
import java.awt.Robot;
import java.awt.Toolkit;
import java.awt.image.BufferedImage;
import java.io.ByteArrayInputStream;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.nio.file.Files;
import static java.nio.file.StandardCopyOption.REPLACE_EXISTING;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map.Entry;
import java.util.Stack;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.util.regex.Pattern;
import javax.imageio.ImageIO;
import org.openqa.selenium.OutputType;
import org.openqa.selenium.TakesScreenshot;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebDriverException;

/// This is the main class that the user instantiates.
/// </summary>
/// 
public final class SeleniumLog {
    
    private String _LogFilePath;
    private String _ScreenshotsPath;
    private Stack PathStack = new Stack();
    private boolean _FileCreated;
    
    private Stack _CurrentIndentLevelStack = new Stack();
    private SaveIndents _SavedIndents = new SaveIndents();
    
    private MessageSettings _MessageSettings = new MessageSettings();

    private boolean Result = true;

    public String OutputFilePath() { return _LogFilePath; }
    public String ScreenshotsPath() { return _ScreenshotsPath; }
    private int ActualIndentLevel() { return _MessageSettings.indentModel.getCurrentLevel(); }
    private int PendingIndentLevel() { return _MessageSettings.GetPendingLevel(); }

    private ArrayList<Integer> wdlist = new ArrayList<Integer>();
    private WebDriver driver;
    //public _Config Config = new _Config();
    public XmlConfigurationClass Config = XmlConfigurationClass.Instance();
    
    private SeleniumLog() {
        this(null, false, false);
    }
    
    //private SeleniumLog(IWebDriver webdriver = null, bool overwrite = false, bool debug = false)
    private SeleniumLog(WebDriver webdriver, boolean overwrite, boolean debug)
    {

        if (webdriver != null)
        {
            driver = webdriver;
        }
        else
        {
            driver = null;
        }

        if (overwrite)
        {
            _LogFilePath = Config.LogFilePath;
            NewFile();
            try {
                Thread.sleep(500);
            } catch (InterruptedException ex) { }
            if (overwrite)
                Clear();

            _MessageSettings.TimestampFormat = Config.TimestampFormat;
            _MessageSettings.EnableLogging = Config.EnableSeleniumLog;
        }
        else
        {
        }

        if (Config.AutoLaunchSeleniumLogDesktop)
        {
            try {
                //Process myProcess = new ProcessBuilder(command, arg).start();
                Process logger = new ProcessBuilder(Config.SeleniumLogAppInstallationFolder +  "\\SeleniumLog Desktop.exe",
                        Config.LogFilePath).start();
                /*
                Process logger = new Process();
                logger.StartInfo.FileName = Config.SeleniumLogAppInstallationFolder +  "\\SeleniumLog Desktop.exe";
                logger.StartInfo.Arguments = Config.LogFilePath;
                logger.Start();
                */
            } catch (IOException ex) {
                //Logger.getLogger(SeleniumLog.class.getName()).log(Level.SEVERE, null, ex);
            }
        }

    }
    

    private static SeleniumLog instance = null;
    public static SeleniumLog Instance() {
        return Instance(null, true, false);
    }
    
    public static SeleniumLog Instance(WebDriver webdriver) {
        return Instance(webdriver, true, false);
    }
    
    public static SeleniumLog Instance(WebDriver webdriver, boolean overwrite, boolean debug) {
        if (instance == null)
        {
            instance = new SeleniumLog(webdriver, overwrite, debug);
        }
        else
        {
            if (webdriver != null)
            {
                instance.driver = webdriver;
            }
        }
        return instance;
    }
    
    public void SaveIndent(String Name)
    {
        _SavedIndents.Set(Name, PendingIndentLevel());
    }

    public void RestoreIndent(String Name)
    {
        _MessageSettings.GetPendingLevel();

        int[] irestore = _SavedIndents.Get(Name);
        if (irestore[1] < 0)
            IndentTo(0);
            //Error().Red().WriteLine("ERROR: Cannot restore unknown indent name [" + Name + "]");
        IndentTo(irestore[1]);
        _SavedIndents.DeleteKey(Name, irestore[0]);
    }
    
    public void ResetResult()
    {
        Result = true;
    }

    public void PublishResult()
    {
        if (!Result)
            throw new RuntimeException("Result = false");
            //throw new AssertFailedException();
    }
    
    /// <summary>
    /// Create an empty file
    /// </summary>
    private void NewFile()
    {
        //File.writeAllText(_LogFilePath, "");
        try (PrintWriter out = new PrintWriter(_LogFilePath)) {
            out.print("");
        } catch (FileNotFoundException ex) {
            //Logger.getLogger(SeleniumLog.class.getName()).log(Level.SEVERE, null, ex);
        }
        _FileCreated = true;
    }
    
    /// <summary>
    /// Clear file
    /// </summary>
    private void Clear()
    {
        NewFile();
    }
    
    private String FormPathString()
    {
        String ReturnString = "";
        for (Object obj : PathStack)
        {
            ReturnString = (obj.toString() + "/" + ReturnString).replaceAll("^/+", "").replaceAll("/+$", "");
        }
        return ReturnString;
    }
    
    /// <summary>
    /// Write msg string to file.
    /// </summary>
    /// <param name="msg"></param>
    public void WriteLine(String msg, boolean take_screenshot)
    {
        if (!_MessageSettings.EnableLogging) return;
        
        if (Config.TakeScreenshotOnEveryWriteline)
        {
            Screenshot();
        }
        else
        {
            if (take_screenshot)
                Screenshot();
        }
        int sleepTime = 50;
        _MessageSettings.MessageStr = msg;
        String StrToWrite = _MessageSettings.FormMessageString();
        while (true) {
            try (FileWriter out = new FileWriter(_LogFilePath, true)) {
                out.append(StrToWrite + "\n");
                if (_MessageSettings.indentModel.getEmptyTree())
                    _MessageSettings.indentModel.setEmptyTree(false);
                break;
            } catch (IOException ex) {
                System.err.println("..... Exception: File locked. Message - " + ex.getMessage());
                try {
                    Thread.sleep(sleepTime);
                } catch (InterruptedException ex1) { }
                sleepTime *= 2;
                if (sleepTime > 50000) {
                    System.err.println("File lock timeout. WriteLine() failed");
                    break;
                }
            }
        }
    }
    
    public void WriteLine(String msg) {
        WriteLine(msg, false);
    }
    
    /// <summary>
    /// true - Display line numbers. false - turn off line numbers.
    /// </summary>
    /// <param name="AddLN"></param>
    /// <returns></returns>
    public SeleniumLog DisplayLineNumbers()
    {
        _MessageSettings.ShowLineNumbers = true;
        return this;
    }

    public SeleniumLog RemoveLineNumbers()
    {
        _MessageSettings.ShowLineNumbers = false;
        return this;
    }
    
    /// <summary>
    /// Indent by one level.
    /// </summary>
    /// <returns></returns>
    public SeleniumLog Indent()
    {
        _MessageSettings.Indent = _MessageSettings.Indent + 1;
        //MessageSettings.RunningIndentLevel++;
        //MessageSettings.CalculatePendingLevel();
        return this;
    }
    
    /// <summary>
    /// Set indent to any level specified by SetLevel. Note that SetLevel value has to be an existing level.
    /// </summary>
    /// <param name="SetLevel">Set the level. It will calculate the number of unindents required to set the indent level to this. Base level is 0.</param>
    /// <returns></returns>
    public SeleniumLog IndentTo(int SetLevel)
    {
        int Delta = SetLevel - _MessageSettings.CurrentIndentLevel();
        if (Delta > 0)
        {
            _MessageSettings.Indent = 0;
            _MessageSettings.Unindent = 0;
            _MessageSettings.Indent++;
        }
        else
        {
            _MessageSettings.Indent = 0;
            _MessageSettings.Unindent = 0;
            Unindent(Math.abs(Delta));
        }
        return this;
    }
    
    /// <summary>
    /// Indent by one level then write message buffer to file.
    /// </summary>
    /// <param name="WriteNow">Set to true to write message buffer to the file now.</param>
    /// <returns></returns>
    public void Indent(boolean WriteNow)
    {
        _MessageSettings.Indent++;
        if (WriteNow)
        {
            String StrToWrite = _MessageSettings.FormMessageString();
            WriteLine(StrToWrite);
        }
    }
    
    /// <summary>
    /// Unindent by one level. 
    /// </summary>
    /// <returns></returns>
    public SeleniumLog Unindent()
    {
        if ((_MessageSettings.CurrentIndentLevel() - 1) >= 0)
            _MessageSettings.Unindent = _MessageSettings.Unindent + 1;
        return this;
    }
    
    /// <summary>
    /// Unindent by multiple levels.
    /// </summary>
    /// <param name="Number">Number of times to unindent.</param>
    /// <returns></returns>
    public SeleniumLog Unindent(int Number)
    {
        for (int i = 0; i < Math.abs(Number); i++ )
        {
            Unindent();
        }
        return this;
    }
    
    /// <summary>
    /// Unindent then write message buffer to file.
    /// </summary>
    /// <param name="WriteNow">Set to true to write message buffer to the file now.</param>
    /// <returns></returns>
    public void Unindent(boolean WriteNow)
    {
        _MessageSettings.Unindent++;
        if (WriteNow)
        {
            String StrToWrite = _MessageSettings.FormMessageString();
            WriteLine(StrToWrite);
        }
    }
    
    /// <summary>
    /// Unindent to specified SetLevel. Actually, this is just a wrapper to IndentTo().
    /// </summary>
    /// <param name="SetLevel"></param>
    /// <returns></returns>
    public SeleniumLog UnindentTo(int SetLevel)
    {
        IndentTo(SetLevel);
        return this;
    }

    /// <summary>
    /// Reset indentation to root level.
    /// </summary>
    /// <returns></returns>
    public SeleniumLog Root()
    {
        _MessageSettings.Root = true;
        return this;
    }
    
    /// <summary>
    /// Reset indentation to root level.
    /// </summary>
    /// <param name="WriteNow">Set to true to write message buffer to the file now.</param>
    /// <returns></returns>
    public void Root(boolean WriteNow)
    {
        _MessageSettings.Root = true;
        if (WriteNow)
        {
            String StrToWrite = _MessageSettings.FormMessageString();
            WriteLine(StrToWrite);
        }
    }
    
    /// <summary>
    /// Initiate watchdog feature.
    /// </summary>
    /// <returns></returns>
    private SeleniumLog WatchdogStart()
    {
        _MessageSettings.WatchdogStart = true;
        boolean containsItem = wdlist.contains(_MessageSettings.CurrentIndentLevel());
        if (containsItem)
        {
            WriteLine(">>>>>>>>>> wdlist already contains this indentation level!");
        }
        else
        {
            wdlist.add(_MessageSettings.CurrentIndentLevel());
        }
        return this;
    }
    
    /// <summary>
    /// Initiate watchdog feature.
    /// </summary>
    /// <param name="WriteNow">Set to true to write message buffer to the file now.</param>
    /// <returns></returns>
    private SeleniumLog WatchdogStart(boolean WriteNow)
    {
        _MessageSettings.WatchdogStart = true;
        if (WriteNow)
        {
            String StrToWrite = _MessageSettings.FormMessageString();
            WriteLine(StrToWrite);
            return null;
        }
        else
        {
            return this;
        }
    }
    
    /// <summary>
    /// Terminate current watchdog which was previously initiated by WatchdogStart.
    /// </summary>
    /// <returns></returns>
    private SeleniumLog WatchdogEnd()
    {
        _MessageSettings.WatchdogEnd = true;
        boolean containsItem = wdlist.contains(_MessageSettings.CurrentIndentLevel());
        if (containsItem)
        {
            _MessageSettings.WatchdogEnd = true;
        }
        else
        {
        }
        return this;
    }
    
    /// <summary>
    /// Terminate current watchdog which was previously initiated by WatchdogStart.
    /// </summary>
    /// <param name="WriteNow">Set to true to write message buffer to the file now.</param>
    /// <returns>SeleniumLog object</returns>
    private SeleniumLog WatchdogEnd(boolean WriteNow)
    {
        _MessageSettings.WatchdogEnd = true;
        if (WriteNow)
        {
            String StrToWrite = _MessageSettings.FormMessageString();
            //File.AppendAllText(_LogFilePath, StrToWrite + "\n");
            WriteLine(StrToWrite);
            return null;
        }
        else
        {
            return this;
        }
    }
    
    /// <summary>
    /// Pass the step.
    /// </summary>
    /// <returns></returns>
    public SeleniumLog Pass()
    {
        if (Config.TakeScreenshotOnEveryPass)
            Screenshot();
        Green();
        _MessageSettings.Pass = true;
        return this;
    }

    /// <summary>
    /// Fail the step.
    /// </summary>
    /// <returns></returns>
    public SeleniumLog Fail()
    {
        if (Config.TakeScreenshotOnEveryFail)
            Screenshot();
        Red();
        _MessageSettings.Fail = true;
        return this;
    }
    
    /// <summary>
    /// Put a warning icon on the step.
    /// </summary>
    /// <returns></returns>
    public SeleniumLog Warning()
    {
        if (Config.TakeScreenshotOnEveryWarning)
            Screenshot();
        _MessageSettings.Warning = true;
        return this;
    }


    /// <summary>
    /// Put an error icon on the step.
    /// </summary>
    /// <returns></returns>
    public SeleniumLog Error()
    {
        if (Config.TakeScreenshotOnEveryError)
            Screenshot();
        _MessageSettings.Error = true;
        return this;
    }
    
    /// <summary>
    /// Set custom font color. Each component range is 0 to 255.
    /// </summary>
    /// <param name="red">between 0 - 255</param>
    /// <param name="green">between 0 - 255</param>
    /// <param name="blue">between 0 - 255</param>
    /// <returns></returns>
    public SeleniumLog CRGB(int red, int green, int blue) {
        _MessageSettings.CRGB = new Color(red, green, blue);
        return this;
    }
    
    /// <summary>
    /// Attach a picture file to a step. 
    /// </summary>
    /// <param name="PicturePath"></param>
    /// <returns></returns>
    public SeleniumLog AttachPicture(String PicturePath)
    {
        _MessageSettings.Image = PicturePath;
        return this;
    }

    /// <summary>
    /// Attach a file of any format to the step. Can also use for pictures.
    /// </summary>
    /// <param name="_FilePath"></param>
    /// <returns></returns>
    private SeleniumLog AttachFile(String FilePath)
    {
        _MessageSettings.File = FilePath;
        return this;
    }
    
    public SeleniumLog Path(String PathStr) {
        return Path(PathStr, false);
    }
    
    /// <summary>
    /// Change the tree path on the left panel. New lines will be added to this path.
    /// </summary>
    /// <param name="PathStr">The tree path separated by / character. Eg., LoginFeature/TestCase1/Setup. If path already exist, then switch to this path.</param>
    /// <param name="WriteNow">Optional. If true, create the path now without having to call Message(str) after it.</param>
    /// <returns></returns>
    public SeleniumLog Path(String PathStr, boolean WriteNow)
    {
        _MessageSettings.Path = PathStr;
        if (WriteNow)
        {
            String StrToWrite = _MessageSettings.FormMessageString();
            WriteLine(StrToWrite);
            return null;
        }
        else
        {
            return this;
        }
    }
    
    public SeleniumLog Tab(String TabStr) {
        return Tab(TabStr, false);
    }
    
    /// <summary>
    /// Change or create new tab. New lines will be added to this tab.
    /// </summary>
    /// <param name="TabStr"></param>
    /// <returns></returns>
    /// 
    /// <summary>
    /// Change or create new tab. New lines will be added to this tab.
    /// </summary>
    /// <param name="TabStr">The name of the tab to be created. If tab already exists, then switch to this tab.</param>
    /// <param name="WriteNow">Optional. If true, create the path now without having to call Message(str) after it.</param>
    /// <returns></returns>
    public SeleniumLog Tab(String TabStr, boolean WriteNow)
    {
        _MessageSettings.Tab = TabStr;
        if (WriteNow)
        {
            String StrToWrite = _MessageSettings.FormMessageString();
            WriteLine(StrToWrite);
            return null;
        }
        else
        {
            return this;
        }
    }
    
    /// <summary>
    /// Set timestamp format.
    /// </summary>
    /// <param name="Format"></param>
    /// <returns></returns>
    private SeleniumLog TimestampFormat(String Format)
    {
        _MessageSettings.TimestampFormat = Format;
        return this;
    }
    
    public String GetUniqueFilename() {
        return GetUniqueFilename("JPG");
    }
    
    /// <summary>
    /// Generates a unique filename based on current date/time. This can be used by end-user for screenshots.
    /// </summary>
    /// <returns></returns>
    public String GetUniqueFilename(String Extension)
    {
        String Filename;
        Date Now = new Date();
        Calendar cal = Calendar.getInstance();
        cal.setTime(Now);

        String Extension2 = Extension.replaceAll("^ *\\.* *", "");
        Filename = "scn_" + Integer.toString(cal.get(Calendar.YEAR))
                + "_" + Integer.toString(cal.get(Calendar.MONTH + 1))
                + "_" + Integer.toString(cal.get(Calendar.DAY_OF_MONTH))
                + "_" + Integer.toString(cal.get(Calendar.HOUR_OF_DAY))
                + "_" + Integer.toString(cal.get(Calendar.MINUTE))
                + "_" + Integer.toString(cal.get(Calendar.SECOND))
                + "_" + Integer.toString(cal.get(Calendar.MILLISECOND))
                + "." + Extension2.toLowerCase();

        return Filename;
    }
    
    public void Test(String msg)
    {
        try {
            Thread.sleep(50);
            WriteLine(msg);
        } catch (InterruptedException ex) { }
    }
    
    public SeleniumLog Screenshot() {
        SeleniumLog res = null;
        try {
            res = Screenshot(null);
        } catch (Exception ex) {
            Logger.getLogger(SeleniumLog.class.getName()).log(Level.SEVERE, null, ex);
        }
        return res;
    }
    
    /// <summary>
    /// Pass the step.
    /// </summary>
    /// <returns></returns>
    public SeleniumLog Screenshot(WebDriver _driver) throws Exception
    {
        try
        {
            WebDriver scrndriver ;
            if (_driver != null)
                scrndriver = _driver;
            else
                scrndriver = driver;
            
            if (scrndriver == null)
                return this;

            // only take a screenshot if scrndriver is not null. This is so that no exceptions are raised if
            // user sets the config to true in the SeleniumLog.config, but has not set the Selenium Webdriver pointer.
            try
            {
                String PICTURE_PATH = Config.ScreenshotsFolder + "/" + GetUniqueFilename("jpg");
                TakeScreenshot(scrndriver, PICTURE_PATH);
                //File srcFile = ((TakesScreenshot)scrndriver).getScreenshotAs(OutputType.FILE);
                //Files.copy(srcFile.toPath(), (new File(PICTURE_PATH)).toPath(), REPLACE_EXISTING);
                //Files.delete(srcFile.toPath());

                //byte[] ssb = ((TakesScreenshot)scrndriver).getScreenshotAs(OutputType.BYTES);
                //BufferedImage bi = ImageIO.read(new ByteArrayInputStream(ssb));
                //ImageIO.write(bi, "jpg", new File(PICTURE_PATH));
                //AttachPicture(PICTURE_PATH);
                _MessageSettings.Image = PICTURE_PATH;
            }
            catch (WebDriverException | IOException | AWTException e)
            {
                throw e;
            }
            return this;
        }
        catch (WebDriverException | IOException | AWTException e)
        {
            Error().WriteLine(e.getMessage());
            throw e;
        }
    }
    
    private void TakeScreenshot(WebDriver drv, String path) throws IOException, AWTException
    { 
    	Rectangle bounds = new Rectangle();
    	org.openqa.selenium.Point loc = drv.manage().window().getPosition();
    	org.openqa.selenium.Dimension dim = drv.manage().window().getSize();
    	bounds.setBounds(loc.getX(), loc.getY(), dim.getWidth(), dim.getHeight());
    	
    		Robot robot = new Robot();
            BufferedImage screenShot = robot.createScreenCapture(bounds);
            ImageIO.write(screenShot, "JPG", new File(path));
    }
    
    public SeleniumLog Red() {
        if (Config.ScreenshotOnEveryMessage)
            Screenshot().CRGB(255, 0, 0);
        else
            CRGB(255, 0, 0);
        return this;
    }
    
    public void Red(String msg)
    {
        Red().WriteLine(msg);
    }
    
    public SeleniumLog Green() {
        CRGB(0, 150, 0);
        return this;
    }
    
    public void Green(String msg)
    {
        Green().WriteLine(msg);
    }
    
    public SeleniumLog Blue() {
        CRGB(0, 0, 250);
        return this;
    }
    
    public void Blue(String msg)
    {
        Blue().WriteLine(msg);
    }
    
    public SeleniumLog Pink() {
        CRGB(247, 91, 208);
        return this;
    }
    
    public void Pink(String msg)
    {
        Pink().WriteLine(msg);
    }
    
    public SeleniumLog Magenta() {
        CRGB(233, 12, 177);
        return this;
    }
    
    public void Magenta(String msg)
    {
        Magenta().WriteLine(msg);
    }
    
    public SeleniumLog Orange() {
        CRGB(250, 97, 5);
        return this;
    }
    
    public void Orange(String msg)
    {
        Orange().WriteLine(msg);
    }
    
    public SeleniumLog Purple() {
        CRGB(97, 0, 193);
        return this;
    }
    
    public void Purple(String msg)
    {
        Purple().WriteLine(msg);
    }
    
    public SeleniumLog Gray() {
        CRGB(135, 135, 135);
        return this;
    }
    
    public void Gray(String msg)
    {
        Gray().WriteLine(msg);
    }
    
    public SeleniumLog BlueGreen() {
        CRGB(13, 115, 71);
        return this;
    }
    
    public void BlueGreen(String msg)
    {
        BlueGreen().WriteLine(msg);
    }
    
    public SeleniumLog Brown() {
        CRGB(113, 0, 0);
        return this;
    }
    
    public void Brown(String msg)
    {
        Brown().WriteLine(msg);
    }
    
    public SeleniumLog Olive() {
        CRGB(128, 128, 0);
        return this;
    }
    
    public void Olive(String msg)
    {
        Olive().WriteLine(msg);
    }
    
    
    public void Explore(Object arg) {
        Explore(arg, "");
    }
    
    /// <summary>
    /// Explores any data structure and display the entire tree of elements and values in the log. Works with Lists, Stacks, Queues, etc., or any combination thereof.
    /// </summary>
    /// <param name="arg">The data structure to be explored.</param>
    /// <param name="comment">Optional string to be displayed in the log.</param>
    public void Explore(Object arg, String comment)
    {
        SeleniumLog log = SeleniumLog.Instance();
        log.SaveIndent("ExploreParams");
        PrintParameters printParameters = new PrintParameters();
        printParameters.PrintValues(arg, comment);
        log.RestoreIndent("ExploreParams");
        //log.MessageSettings.indentModel.SimulateIndentations(MessageSettings.MessageStr);
        log._MessageSettings.GetPendingLevel();
    }

}
