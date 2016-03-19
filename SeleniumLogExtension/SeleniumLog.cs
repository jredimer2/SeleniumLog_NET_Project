using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using System.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XMLConfig;
using System.Diagnostics;

namespace SeleniumLogger
{
    //public class WriteNow
    //{
    //    public WriteNow() { }
    //}
    
    /// <summary>
    /// This is the main class that the user instantiates.
    /// </summary>
    /// 
    public sealed partial class SeleniumLog
    {
        private string _LogFilePath;
        private string _ScreenshotsPath;
        private Stack PathStack = new Stack();
        private bool _FileCreated { get; set; }
        /*
        private string CurrentFeaturePath = "";
        private string CurrentUserStoryPath = "";
        private string CurrentAcceptanceCriteriaPath = "";
        private string CurrentScenarioPath = "";
        private string CurrentPath = "";

        private string CurrentFeature = "";
        private string CurrentUserStory = "";
        private string CurrentTestCase = "";
        private string CurrentScenario = "";

        private int _CurrentLevel = 0;
        private int _FeatureLevel = 1;
        private int _UserStoryLevel = 2;
        private int _RequirementLevel = 2;
        private int _AcceptanceCriteriaLevel = 3;
        private int _TestCaseLevel = 3;
        private int _ScenarioLevel = 4;
         */ 
        private Stack _CurrentIndentLevelStack = new Stack();
        private SaveIndents _SavedIndents = new SaveIndents();
        //public static _MessageSettings MessageSettings = new _MessageSettings();
        private _MessageSettings MessageSettings = new _MessageSettings();

        private bool Result = true;

        public string OutputFilePath { get { return _LogFilePath; } }
        public string ScreenshotsPath { get { return _ScreenshotsPath; } }
        private int ActualIndentLevel { get { return MessageSettings.indentModel.CurrentLevel; } }
        private int PendingIndentLevel { get { return MessageSettings.GetPendingLevel(); } }
        
        private List<int> wdlist = new List<int>();
        private IWebDriver driver;
        //public _Config Config = new _Config();
        public XmlConfigurationClass Config = XmlConfigurationClass.Instance();

        private SeleniumLog(IWebDriver webdriver = null, bool overwrite = false, bool debug = false)
        {

            //string name = webdriver.GetType().FullName;

            //Console.WriteLine("output_file_path : " + Config.OutputFilePath);
            //Console.ReadLine();

            if (webdriver != null)
            {
                driver = webdriver;
            }
            else
            {
                driver = null;
            }

            //ParseXML();

            //if ((LogFilePath == null) || (LogFilePath == ""))  // Prevent log redirected to a new file if filepath has already been declared by previous instance.
            {

            }
            //else if (((LogFilePath != null) || (LogFilePath != "")) && (_FileCreated == false))
            if (overwrite == true)
            {
                //_LogFilePath = LogFilePath;
                _LogFilePath = Config.LogFilePath;
                //MessageSettings = new _MessageSettings();
                NewFile();
                Thread.Sleep(500);
                if (overwrite)
                    Clear();
                
                _ScreenshotsPath = Config.ScreenshotsFolder;
                MessageSettings.TimestampFormat = Config.TimestampFormat;
                MessageSettings.EnableLogging = Config.EnableSeleniumLog;
            }
            else
            {
            }

            if (Config.AutoLaunchSeleniumLogDesktop)
            {
                Process logger = new Process();
                logger.StartInfo.FileName = Config.SeleniumLogAppInstallationFolder +  @"\SeleniumLog Desktop.exe";
                logger.StartInfo.Arguments = Config.LogFilePath;
                logger.Start();
            }

        }

        private static SeleniumLog instance = null;      

        public static SeleniumLog Instance(IWebDriver webdriver = null, bool overwrite = true, bool debug = false)
        {
            //get 
            {
                //lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SeleniumLog(webdriver: webdriver, overwrite: overwrite, debug: debug);
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
            }
        }

        public void SaveIndent(string Name)
        {
            _SavedIndents.Set(Name, PendingIndentLevel);
        }

        public void RestoreIndent(string Name)
        {
            MessageSettings.GetPendingLevel();

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
            if (Result == false)
                throw new AssertFailedException();
        }
        /// <summary>
        /// Create an empty file
        /// </summary>
        private void NewFile()
        {
            File.WriteAllText(_LogFilePath, "");
            _FileCreated = true;
        }

        /// <summary>
        /// Clear file
        /// </summary>
        private void Clear()
        {
            using (var stream = new FileStream(_LogFilePath, FileMode.Truncate))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write("");
                }
            }
        }

        /*
         * 
         * 
         * using (FileStream fs = File.OpenWrite(path)) 
        {
            Byte[] info = 
                new UTF8Encoding(true).GetBytes("This is to test the OpenWrite method.");

            // Add some information to the file.
            fs.Write(info, 0, info.Length);
        }
         * */

        private string FormPathString()
        {
            string ReturnString = "";
            foreach (Object obj in PathStack)
            {
                ReturnString = (obj.ToString() + "/" + ReturnString).TrimStart('/').TrimEnd('/');
            }
            return ReturnString;
        }

        /// <summary>
        /// Write msg string to file.
        /// </summary>
        /// <param name="msg"></param>
        public void WriteLine(string msg, bool take_screenshot = false)
        {
            /*
            FileLocker.Lock(@"c:\file",
                    (f) =>
                    {
                        try
                        {
                            f.Write(buf, 0, buf.Length);
                        }
                        catch (IOException ioe)
                        {
                            // handle IOException
                        }
                    });
             */ 
            //-------------------------
            var autoResetEvent = new AutoResetEvent(false);

            // Wait for file to be released by another process
            while (true)
            {
                try
                {
                    if (MessageSettings.EnableLogging)
                    {
                        if (Config.TakeScreenshotOnEveryWriteline)
                        {
                            Screenshot();
                        }
                        else
                        {
                            if (take_screenshot)
                                Screenshot();
                        }
                        MessageSettings.MessageStr = msg;
                        string StrToWrite = MessageSettings.FormMessageString();
                        File.AppendAllText(_LogFilePath, StrToWrite + "\n");
                        if (MessageSettings.indentModel.EmptyTree)
                            MessageSettings.indentModel.EmptyTree = false;
                    }
                    break;
                }
                catch (IOException f)
                {
                    Console.WriteLine("..... Exception: File locked. Message - " + f.Message);
                    var fileSystemWatcher =
                        new FileSystemWatcher(System.IO.Path.GetDirectoryName(_LogFilePath))
                        {
                            EnableRaisingEvents = true
                        };

                    fileSystemWatcher.Changed +=
                        (o, e) =>
                        {
                            if (System.IO.Path.GetFullPath(e.FullPath) == System.IO.Path.GetFullPath(_LogFilePath))
                            {
                                autoResetEvent.Set();
                            }
                        };
                }
            }
        }

        /// <summary>
        /// true - Display line numbers. false - turn off line numbers.
        /// </summary>
        /// <param name="AddLN"></param>
        /// <returns></returns>
        public SeleniumLog DisplayLineNumbers()
        {
            MessageSettings.ShowLineNumbers = true;
            return this;
        }

        public SeleniumLog RemoveLineNumbers()
        {
            MessageSettings.ShowLineNumbers = false;
            return this;
        }

        /// <summary>
        /// Indent by one level.
        /// </summary>
        /// <returns></returns>
        public SeleniumLog Indent()
        {
            MessageSettings.Indent = MessageSettings.Indent + 1;
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
            int Delta = SetLevel - MessageSettings.CurrentIndentLevel;
            if (Delta > 0)
            {
                MessageSettings.Indent = 0;
                MessageSettings.Unindent = 0;
                MessageSettings.Indent++;
            }
            else
            {
                MessageSettings.Indent = 0;
                MessageSettings.Unindent = 0;
                Unindent(Math.Abs(Delta));
            }
            return this;
        }

        /// <summary>
        /// Indent by one level then write message buffer to file.
        /// </summary>
        /// <param name="WriteNow">Set to true to write message buffer to the file now.</param>
        /// <returns></returns>
        public void Indent(bool WriteNow)
        {

            MessageSettings.Indent++;

            if (WriteNow)
            {
                string StrToWrite = MessageSettings.FormMessageString();
                WriteLine(StrToWrite);
            }
        }

        /// <summary>
        /// Unindent by one level. 
        /// </summary>
        /// <returns></returns>
        public SeleniumLog Unindent()
        {

            if ((MessageSettings.CurrentIndentLevel - 1) >= 0)
                MessageSettings.Unindent = MessageSettings.Unindent + 1;
            return this;
        }

        /// <summary>
        /// Unindent by multiple levels.
        /// </summary>
        /// <param name="Number">Number of times to unindent.</param>
        /// <returns></returns>
        public SeleniumLog Unindent(int Number)
        {
            for (int i = 0; i < Math.Abs(Number); i++ )
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
        public void Unindent(bool WriteNow)
        {

            MessageSettings.Unindent++;

            if (WriteNow)
            {
                string StrToWrite = MessageSettings.FormMessageString();
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
            //MessageSettings.CalculatePendingLevel();

            return this;
        }

        /// <summary>
        /// Reset indentation to root level.
        /// </summary>
        /// <returns></returns>
        public SeleniumLog Root()
        {
            MessageSettings.Root = true;
            //MessageSettings.CalculatePendingLevel();

            return this;
        }

        /// <summary>
        /// Reset indentation to root level.
        /// </summary>
        /// <param name="WriteNow">Set to true to write message buffer to the file now.</param>
        /// <returns></returns>
        public void Root(bool WriteNow)
        {
            MessageSettings.Root = true;

            if (WriteNow)
            {
                string StrToWrite = MessageSettings.FormMessageString();
                WriteLine(StrToWrite);
                //MessageSettings.CalculatePendingLevel();

            }
        }

        /// <summary>
        /// Initiate watchdog feature.
        /// </summary>
        /// <returns></returns>
        private SeleniumLog WatchdogStart()
        {
            MessageSettings.WatchdogStart = true;
            bool containsItem = wdlist.Any(item => item == MessageSettings.CurrentIndentLevel);
            if (containsItem)
            {
                WriteLine(">>>>>>>>>> wdlist already contains this indentation level!");
            }
            else
            {
                wdlist.Add(MessageSettings.CurrentIndentLevel);
            }
            return this;
        }

        /// <summary>
        /// Initiate watchdog feature.
        /// </summary>
        /// <param name="WriteNow">Set to true to write message buffer to the file now.</param>
        /// <returns></returns>
        private SeleniumLog WatchdogStart(bool WriteNow)
        {
            MessageSettings.WatchdogStart = true;

            if (WriteNow)
            {
                string StrToWrite = MessageSettings.FormMessageString();
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
        /// Terminate current watchdog which was previously initiated by WatchdogStart.
        /// </summary>
        /// <returns></returns>
        private SeleniumLog WatchdogEnd()
        {
            MessageSettings.WatchdogEnd = true;
            bool containsItem = wdlist.Any(item => item == MessageSettings.CurrentIndentLevel);
            if (containsItem)
            {
                MessageSettings.WatchdogEnd = true;
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
        private SeleniumLog WatchdogEnd(bool WriteNow)
        {
            MessageSettings.WatchdogEnd = true;

            if (WriteNow)
            {
                string StrToWrite = MessageSettings.FormMessageString();
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
            MessageSettings.Pass = true;
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
            MessageSettings.Fail = true;
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
            MessageSettings.Warning = true;
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
            MessageSettings.Error = true;
            return this;
        }

        /// <summary>
        /// Set custom font color. Each component range is 0 to 255.
        /// </summary>
        /// <param name="red">between 0 - 255</param>
        /// <param name="green">between 0 - 255</param>
        /// <param name="blue">between 0 - 255</param>
        /// <returns></returns>
        public SeleniumLog RGB(int red, int green, int blue)
        {
            Color rgb = new Color();
            rgb.red = red;
            rgb.green = green;
            rgb.blue = blue;
            MessageSettings.RGB = rgb;
            return this;
        }


        /// <summary>
        /// Attach a picture file to a step. 
        /// </summary>
        /// <param name="PicturePath"></param>
        /// <returns></returns>
        public SeleniumLog AttachPicture(string PicturePath)
        {
            MessageSettings.Image = PicturePath;
            return this;
        }

        /// <summary>
        /// Attach a file of any format to the step. Can also use for pictures.
        /// </summary>
        /// <param name="_FilePath"></param>
        /// <returns></returns>
        private SeleniumLog AttachFile(string FilePath)
        {
            MessageSettings.File = FilePath;
            return this;
        }

        /// <summary>
        /// Change the tree path on the left panel. New lines will be added to this path.
        /// </summary>
        /// <param name="PathStr">The tree path separated by / character. Eg., LoginFeature/TestCase1/Setup. If path already exist, then switch to this path.</param>
        /// <param name="WriteNow">Optional. If true, create the path now without having to call Message(str) after it.</param>
        /// <returns></returns>
        public SeleniumLog Path(string PathStr, bool WriteNow = false)
        {
            MessageSettings.Path = PathStr;

            if (WriteNow)
            {
                string StrToWrite = MessageSettings.FormMessageString();
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
        public SeleniumLog Tab(string TabStr, bool WriteNow = false)
        {
            MessageSettings.Tab = TabStr;

            if (WriteNow)
            {
                string StrToWrite = MessageSettings.FormMessageString();
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
        /// Set timestamp format.
        /// </summary>
        /// <param name="Format"></param>
        /// <returns></returns>
        private SeleniumLog TimestampFormat(string Format)
        {
            MessageSettings.TimestampFormat = Format;
            return this;
        }

        /// <summary>
        /// Generates a unique filename based on current date/time. This can be used by end-user for screenshots.
        /// </summary>
        /// <returns></returns>
        public string GetUniqueFilename(string Extension = "JPG")
        {
            string Filename = "";
            DateTime Now = DateTime.Now;

            string Extension2 = Regex.Replace(input: Extension, pattern: @"^ *\.* *", replacement: "");
            Filename = "scn_" + Now.Year.ToString()
                    + "_" + Now.Month.ToString()
                    + "_" + Now.Day.ToString()
                    + "_" + Now.Hour.ToString()
                    + "_" + Now.Minute.ToString()
                    + "_" + Now.Second.ToString()
                    + "_" + Now.Millisecond.ToString()
                    + "." + Extension2.ToLower();

            return Filename;
        }

        public void Test(string msg)
        {
            Thread.Sleep(50);
            WriteLine(msg);
        }

        /// <summary>
        /// Pass the step.
        /// </summary>
        /// <returns></returns>
        public SeleniumLog Screenshot(IWebDriver _driver = null)
        {
            try
            {
                IWebDriver scrndriver = null;
                if (_driver != null)
                    scrndriver = _driver;
                else
                    scrndriver = driver;

                // only take a screenshot if scrndriver is not null. This is so that no exceptions are raised if
                // user sets the config to true in the SeleniumLog.config, but has not set the Selenium Webdriver pointer.
                try
                {
                    string PICTURE_PATH = Config.ScreenshotsFolder + "/" + GetUniqueFilename("jpg");
                    Screenshot ss = ((ITakesScreenshot)scrndriver).GetScreenshot();
                    ss.SaveAsFile(PICTURE_PATH, ImageFormat.Jpeg);
                    AttachPicture(PICTURE_PATH);
                    MessageSettings.Image = PICTURE_PATH;
                }
                catch (Exception e)
                {
                }
                return this;
            }
            catch (NullReferenceException e)
            {
                Error().WriteLine("NULL REFERENCE EXCEPTION");
                throw e;
                return null;
            }
        }

        /*
        static void Screenshot(IWebDriver driver)
        {
            SeleniumLog log = SeleniumLog.Instance();
            string folder = @"C:\Tmp\";
            string file = log.GetUniqueFilename();

            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile(folder + file, ImageFormat.Jpeg); //use any of the built in image formating
            //ss.ToString();
            log.AttachPicture(folder + file).Message("screenshot");
        }
        */

        public SeleniumLog Red()
        {
            if (Config.ScreenshotOnEveryMessage)
                Screenshot().RGB(255, 0, 0);
            else
                RGB(255, 0, 0);
            return this;
        }

        public void Red(string msg)
        {
            Red().WriteLine(msg);
        }

        public SeleniumLog Green()
        {
            RGB(0, 150, 0);
            return this;
        }

        public void Green(string msg)
        {
            Green().WriteLine(msg);
        }

        public SeleniumLog Blue()
        {
            RGB(0, 0, 250);
            return this;
        }

        public void Blue(string msg)
        {
            Blue().WriteLine(msg);
        }

        public SeleniumLog Pink()
        {
            RGB(247, 91, 208);
            return this;
        }

        public void Pink(string msg)
        {
            Pink().WriteLine(msg);
        }

        public SeleniumLog Magenta()
        {
            RGB(233, 12, 177);
            return this;
        }

        public void Magenta(string msg)
        {
            Magenta().WriteLine(msg);
        }

        public SeleniumLog Orange()
        {
            RGB(250,97, 5);
            return this;
        }

        public void Orange(string msg)
        {
            Orange().WriteLine(msg);
        }

        public SeleniumLog Purple()
        {
            RGB(97, 0, 193);
            return this;
        }

        public void Purple(string msg)
        {
            Purple().WriteLine(msg);
        }

        public SeleniumLog Gray()
        {
            RGB(135, 135, 135);
            return this;
        }

        public void Gray(string msg)
        {
            Gray().WriteLine(msg);
        }

        public SeleniumLog BlueGreen()
        {
            RGB(13, 115, 71);
            return this;
        }

        public void BlueGreen(string msg)
        {
            BlueGreen().WriteLine(msg);
        }

        public SeleniumLog Brown()
        {
            RGB(113, 0, 0);
            return this;
        }

        public void Brown(string msg)
        {
            Brown().WriteLine(msg);
        }

        public SeleniumLog Olive()
        {
            RGB(128, 128, 0);
            return this;
        }

        public void Olive(string msg)
        {
            Olive().WriteLine(msg);
        }

    }
}
