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
using System.Drawing;
using System.Windows.Forms;

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
        private Stack _CurrentIndentLevelStack = new Stack();
        private SaveIndents _SavedIndents = new SaveIndents();
        private _MessageSettings MessageSettings1 = new _MessageSettings();
        private _MessageSettings MessageSettings2 = new _MessageSettings();
        private _MessageSettings MessageSettings3 = new _MessageSettings();


        private bool Result = true;

        public string OutputFilePath { get { return _LogFilePath; } }
        public string ScreenshotsPath { get { return _ScreenshotsPath; } }
        private int ActualIndentLevel { get { return MessageSettings1.indentModel.CurrentLevel; } }
        private int PendingIndentLevel { get { return MessageSettings1.GetPendingLevel(); } }

        private List<int> wdlist = new List<int>();
        public IWebDriver driver;
        //public _Config Config = new _Config();
        public XmlConfigurationClass Config = XmlConfigurationClass.Instance();

        private SeleniumLog(IWebDriver webdriver = null, bool overwrite = false, bool debug = false)
        {
            if (webdriver != null)
            {
                driver = webdriver;
            }
            else
            {
                driver = null;
            }

            if (overwrite == true)
            {
                if (Config.OutputFormatText)
                {
                    NewFile(Config.OutputFormatText_Filepath);
                    Thread.Sleep(500);
                    if (overwrite)
                        Clear(Config.OutputFormatText_Filepath);
                }

                if (Config.OutputFormatSeleniumLogViewer)
                {
                    NewFile(Config.OutputFormatSeleniumLogViewer_Filepath);
                    Thread.Sleep(500);
                    if (overwrite)
                        Clear(Config.OutputFormatSeleniumLogViewer_Filepath);
                    _ScreenshotsPath = Config.OutputFormatSeleniumLogViewer_Screenshots;
                }


                MessageSettings1.TimestampFormat = Config.TimestampFormat;
                MessageSettings1.EnableLogging = Config.EnableSeleniumLog;
                MessageSettings2.TimestampFormat = Config.TimestampFormat;
                MessageSettings2.EnableLogging = Config.EnableSeleniumLog; 
                MessageSettings3.TimestampFormat = Config.TimestampFormat;
                MessageSettings3.EnableLogging = Config.EnableSeleniumLog;
            }
            else
            {
            }

            if (Config.AutoLaunchSeleniumLogDesktop)
            {
                Process logger = new Process();
                logger.StartInfo.FileName = Config.SeleniumLogAppInstallationFolder + @"\SeleniumLog Viewer.exe";
                logger.StartInfo.Arguments = Config.OutputFormatSeleniumLogViewer_Filepath;
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

        public void RestoreIndent(string Name, bool debug = false)
        {

            MessageSettings1.GetPendingLevel();
            //if (debug) Purple().Info("RestoreIndent :: tp-1 :: indent = " + MessageSettings1.Indent + "   unindent = " + MessageSettings1.Unindent);
            MessageSettings2.GetPendingLevel();
            //if (debug) Purple().Info("RestoreIndent :: tp-2 :: indent = " + MessageSettings1.Indent + "   unindent = " + MessageSettings1.Unindent);
            MessageSettings3.GetPendingLevel();
            //if (debug) Purple().Info("RestoreIndent :: tp-3 :: indent = " + MessageSettings1.Indent + "   unindent = " + MessageSettings1.Unindent);

            int[] irestore = _SavedIndents.Get(Name);
            //if (debug) Purple().Info("RestoreIndent :: tp-4 :: indent = " + MessageSettings1.Indent + "   unindent = " + MessageSettings1.Unindent);
            if (irestore[1] < 0)
                IndentTo(0);
            //Error().Red().WriteLine("ERROR: Cannot restore unknown indent name [" + Name + "]");
            //if (debug) Purple().Info("RestoreIndent :: tp-5 :: indent = " + MessageSettings1.Indent + "   unindent = " + MessageSettings1.Unindent);
            IndentTo(irestore[1]);
            //if (debug) Purple().Info("RestoreIndent :: tp-6 :: indent = " + MessageSettings1.Indent + "   unindent = " + MessageSettings1.Unindent);
            _SavedIndents.DeleteKey(Name, irestore[0]);
            //if (debug) Purple().Info("RestoreIndent :: tp-7 :: indent = " + MessageSettings1.Indent + "   unindent = " + MessageSettings1.Unindent);
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
        private void NewFile(string Filepath)
        {
            File.WriteAllText(Filepath, "");
            _FileCreated = true;
        }

        /// <summary>
        /// Clear file
        /// </summary>
        private void Clear(string Filepath)
        {
            using (var stream = new FileStream(Filepath, FileMode.Truncate))
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
        private void WriteLine(bool TakeScreenshots, string msg, bool take_screenshot = false )
        {

            var autoResetEvent = new AutoResetEvent(false);

            // Wait for file to be released by another process
            while (true)
            {
                try
                {
                    if (MessageSettings1.EnableLogging)
                    {
                        
                        if (Config.OutputFormatSeleniumLogViewer)
                        {
                            if (TakeScreenshots || take_screenshot)
                            {
                                Screenshot();
                            }

                            MessageSettings1.MessageStr = msg;
                            string StrToWrite = MessageSettings1.FormMessageString(true);
                            File.AppendAllText(Config.OutputFormatSeleniumLogViewer_Filepath, StrToWrite + "\n");
                            if (MessageSettings1.indentModel.EmptyTree)
                                MessageSettings1.indentModel.EmptyTree = false;
                        }

                        if (Config.OutputFormatText)
                        {
                            MessageSettings2.MessageStr = msg;
                            string StrToWrite = MessageSettings2.FormMessageString(false);
                            File.AppendAllText(Config.OutputFormatText_Filepath, StrToWrite + "\n");
                            if (MessageSettings2.indentModel.EmptyTree)
                                MessageSettings2.indentModel.EmptyTree = false;

                        }

                        if (Config.OutputFormatConsole)
                        {
                            MessageSettings3.MessageStr = msg;
                            string StrToWrite = MessageSettings3.FormMessageString(false);
                            Console.WriteLine(StrToWrite);
                            if (MessageSettings3.indentModel.EmptyTree)
                                MessageSettings3.indentModel.EmptyTree = false;
                        }

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
        /// Write msg string to file.
        /// </summary>
        /// <param name="msg"></param>
        public void WriteLine(string msg, bool take_screenshot = false)
        {
            WriteLine(msg: msg, take_screenshot: take_screenshot, TakeScreenshots: Config.TakeScreenshotOnEveryWriteline);
        }

        /// <summary>
        /// Write msg string to file.
        /// </summary>
        /// <param name="msg"></param>
        public void Info(string msg, bool take_screenshot = false)
        {
            WriteLine(msg: msg, take_screenshot: take_screenshot, TakeScreenshots: Config.TakeScreenshotOnEveryInfo);
        }

        /// <summary>
        /// Write msg string to file.
        /// </summary>
        /// <param name="msg"></param>
        public void Info2(string msg, bool take_screenshot = false)
        {
            WriteLine(msg: msg, take_screenshot: take_screenshot, TakeScreenshots: Config.TakeScreenshotOnEveryInfo2);
        }

        /// <summary>
        /// Write msg string to file.
        /// </summary>
        /// <param name="msg"></param>
        public void Debug(string msg, bool take_screenshot = false)
        {
            Debug();
            WriteLine(msg: msg, take_screenshot: take_screenshot, TakeScreenshots: Config.TakeScreenshotOnEveryDebug);
        }

        /// <summary>
        /// Write msg string to file.
        /// </summary>
        /// <param name="msg"></param>
        public void Debug2(string msg, bool take_screenshot = false)
        {
            Debug();
            WriteLine(msg: msg, take_screenshot: take_screenshot, TakeScreenshots: Config.TakeScreenshotOnEveryDebug2);
        }
        /// <summary>
        /// Write msg string to file.
        /// </summary>
        /// <param name="msg"></param>
        public void Pass(string msg, bool take_screenshot = false)
        {
            Pass();
            WriteLine(msg: msg, take_screenshot: take_screenshot, TakeScreenshots: Config.TakeScreenshotOnEveryPass);
        }
        /// <summary>
        /// Write msg string to file with a PASS icon.
        /// </summary>
        /// <param name="msg"></param>
        public void Pass2(string msg, bool take_screenshot = false)
        {
            Pass();
            WriteLine(msg: msg, take_screenshot: take_screenshot, TakeScreenshots: Config.TakeScreenshotOnEveryPass2);
        }

        /// <summary>
        /// Write msg string to file with a FAIL icon.
        /// </summary>
        /// <param name="msg"></param>
        public void Fail(string msg, bool take_screenshot = false)
        {
            Fail();
            WriteLine(msg: msg, take_screenshot: take_screenshot, TakeScreenshots: Config.TakeScreenshotOnEveryFail);
        }

        /// <summary>
        /// Write msg string to file with a FAIL icon.
        /// </summary>
        /// <param name="msg"></param>
        public void Fail2(string msg, bool take_screenshot = false)
        {
            Fail();
            WriteLine(msg: msg, take_screenshot: take_screenshot, TakeScreenshots: Config.TakeScreenshotOnEveryFail2);
        }

        /// <summary>
        /// Write msg string to filewith an ERROR icon.
        /// </summary>
        /// <param name="msg"></param>
        public void Error(string msg, bool take_screenshot = false)
        {
            Error();
            WriteLine(msg: msg, take_screenshot: take_screenshot, TakeScreenshots: Config.TakeScreenshotOnEveryError);
        }

        /// <summary>
        /// Write msg string to filewith an ERROR icon.
        /// </summary>
        /// <param name="msg"></param>
        public void Error2(string msg, bool take_screenshot = false)
        {
            Error();
            WriteLine(msg: msg, take_screenshot: take_screenshot, TakeScreenshots: Config.TakeScreenshotOnEveryError2);
        }

        /// <summary>
        /// Write msg string to filewith an ERROR icon.
        /// </summary>
        /// <param name="msg"></param>
        public void Fatal(string msg, bool take_screenshot = false)
        {
            Red().Fatal();
            if (Config.OutputFormatSeleniumLogViewer)
                msg = "FATAL - " + msg;
            WriteLine(msg: msg, take_screenshot: take_screenshot, TakeScreenshots: Config.TakeScreenshotOnEveryFatal);
        }

        /// <summary>
        /// Write msg string to filewith an ERROR icon.
        /// </summary>
        /// <param name="msg"></param>
        public void Fatal2(string msg, bool take_screenshot = false)
        {
            Red().Fatal();
            WriteLine(msg: msg, take_screenshot: take_screenshot, TakeScreenshots: Config.TakeScreenshotOnEveryFatal2);
        }

        /// <summary>
        /// Write msg string to filewith a WARNING icon.
        /// </summary>
        /// <param name="msg"></param>
        public void Warning(string msg, bool take_screenshot = false)
        {
            Warning();
            WriteLine(msg: msg, take_screenshot: take_screenshot, TakeScreenshots: Config.TakeScreenshotOnEveryWarning);
        }

        /// <summary>
        /// Write msg string to filewith a WARNING icon.
        /// </summary>
        /// <param name="msg"></param>
        public void Warning2(string msg, bool take_screenshot = false)
        {
            Warning();
            WriteLine(msg: msg, take_screenshot: take_screenshot, TakeScreenshots: Config.TakeScreenshotOnEveryWarning2);
        }

        /// <summary>
        /// true - Display line numbers. false - turn off line numbers.
        /// </summary>
        /// <param name="AddLN"></param>
        /// <returns></returns>
        public SeleniumLog DisplayLineNumbers()
        {
            MessageSettings1.ShowLineNumbers = true;
            MessageSettings2.ShowLineNumbers = true;
            MessageSettings3.ShowLineNumbers = true;
            return this;
        }

        public SeleniumLog RemoveLineNumbers()
        {
            MessageSettings1.ShowLineNumbers = false;
            MessageSettings2.ShowLineNumbers = false;
            MessageSettings3.ShowLineNumbers = false;
            return this;
        }

        /// <summary>
        /// Indent by one level.
        /// </summary>
        /// <returns></returns>
        public SeleniumLog Indent()
        {
            MessageSettings1.Indent = MessageSettings1.Indent + 1;
            MessageSettings2.Indent = MessageSettings2.Indent + 1;
            MessageSettings3.Indent = MessageSettings3.Indent + 1;
            return this;
        }

        /// <summary>
        /// Set indent to any level specified by SetLevel. Note that SetLevel value has to be an existing level.
        /// </summary>
        /// <param name="SetLevel">Set the level. It will calculate the number of unindents required to set the indent level to this. Base level is 0.</param>
        /// <returns></returns>
        public SeleniumLog IndentTo(int SetLevel)
        {
            int Delta = SetLevel - MessageSettings1.CurrentIndentLevel;
            if (Delta > 0)
            {
                MessageSettings1.Indent = 0;   MessageSettings1.Unindent = 0;   MessageSettings1.Indent++;
                MessageSettings2.Indent = 0;   MessageSettings2.Unindent = 0;   MessageSettings2.Indent++;
                MessageSettings3.Indent = 0;   MessageSettings3.Unindent = 0;   MessageSettings3.Indent++;
            }
            else
            {
                MessageSettings1.Indent = 0; MessageSettings1.Unindent = 0; 
                MessageSettings2.Indent = 0; MessageSettings2.Unindent = 0; 
                MessageSettings3.Indent = 0; MessageSettings3.Unindent = 0; 
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

            MessageSettings1.Indent++;
            MessageSettings2.Indent++;
            MessageSettings3.Indent++;

            if (WriteNow)
            {
                string StrToWrite = MessageSettings1.FormMessageString();
                WriteLine(StrToWrite);
            }
        }

        /// <summary>
        /// Unindent by one level. 
        /// </summary>
        /// <returns></returns>
        public SeleniumLog Unindent()
        {

            if ((MessageSettings1.CurrentIndentLevel - 1) >= 0)
            {
                MessageSettings1.Unindent = MessageSettings1.Unindent + 1;
                MessageSettings2.Unindent = MessageSettings2.Unindent + 1;
                MessageSettings3.Unindent = MessageSettings3.Unindent + 1;
            }
            return this;
        }

        /// <summary>
        /// Unindent by multiple levels.
        /// </summary>
        /// <param name="Number">Number of times to unindent.</param>
        /// <returns></returns>
        public SeleniumLog Unindent(int Number)
        {
            for (int i = 0; i < Math.Abs(Number); i++)
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

            MessageSettings1.Unindent++;
            MessageSettings2.Unindent++;
            MessageSettings3.Unindent++;

            if (WriteNow)
            {
                string StrToWrite = MessageSettings1.FormMessageString();
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
            MessageSettings1.Root = true;
            MessageSettings2.Root = true;
            MessageSettings3.Root = true;

            return this;
        }

        /// <summary>
        /// Reset indentation to root level.
        /// </summary>
        /// <param name="WriteNow">Set to true to write message buffer to the file now.</param>
        /// <returns></returns>
        public void Root(bool WriteNow)
        {
            MessageSettings1.Root = true;
            MessageSettings2.Root = true;
            MessageSettings3.Root = true;

            if (WriteNow)
            {
                string StrToWrite = MessageSettings1.FormMessageString();
                WriteLine(StrToWrite);
                //MessageSettings.CalculatePendingLevel();

            }
        }

        /// <summary>
        /// Initiate watchdog feature.
        /// </summary>
        /// <returns></returns>
        public SeleniumLog WatchdogStart()
        {
            MessageSettings1.WatchdogStart = true;
            MessageSettings2.WatchdogStart = true;
            MessageSettings3.WatchdogStart = true;

            bool containsItem = wdlist.Any(item => item == MessageSettings1.CurrentIndentLevel);
            if (containsItem)
            {
                WriteLine(">>>>>>>>>> wdlist already contains this indentation level!");
            }
            else
            {
                wdlist.Add(MessageSettings1.CurrentIndentLevel);
            }
            return this;
        }

        /// <summary>
        /// Initiate watchdog feature.
        /// </summary>
        /// <param name="WriteNow">Set to true to write message buffer to the file now.</param>
        /// <returns></returns>
        public SeleniumLog WatchdogStart(bool WriteNow)
        {
            MessageSettings1.WatchdogStart = true;
            MessageSettings2.WatchdogStart = true;
            MessageSettings3.WatchdogStart = true;

            if (WriteNow)
            {
                string StrToWrite = MessageSettings1.FormMessageString();
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
        public SeleniumLog WatchdogEnd()
        {
            MessageSettings1.WatchdogEnd = true;
            MessageSettings2.WatchdogEnd = true;
            MessageSettings3.WatchdogEnd = true;

            bool containsItem = wdlist.Any(item => item == MessageSettings1.CurrentIndentLevel);
            if (containsItem)
            {
                MessageSettings1.WatchdogEnd = true;
                MessageSettings2.WatchdogEnd = true;
                MessageSettings3.WatchdogEnd = true;
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
        public SeleniumLog WatchdogEnd(bool WriteNow)
        {
            MessageSettings1.WatchdogEnd = true;
            MessageSettings2.WatchdogEnd = true;
            MessageSettings3.WatchdogEnd = true;

            if (WriteNow)
            {
                string StrToWrite = MessageSettings1.FormMessageString();
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
        public SeleniumLog Debug()
        {
            Gray();
            MessageSettings1.Debug = true;
            MessageSettings2.Debug = true;
            MessageSettings3.Debug = true;
            return this;
        }

        /// <summary>
        /// Pass the step.
        /// </summary>
        /// <returns></returns>
        public SeleniumLog Pass()
        {
            Green();
            MessageSettings1.Pass = true;
            MessageSettings2.Pass = true;
            MessageSettings3.Pass = true;
            return this;
        }

        /// <summary>
        /// Fail the step.
        /// </summary>
        /// <returns></returns>
        public SeleniumLog Fail()
        {
            Red();
            MessageSettings1.Fail = true;
            MessageSettings2.Fail = true;
            MessageSettings3.Fail = true;
            return this;
        }


        /// <summary>
        /// Put a warning icon on the step.
        /// </summary>
        /// <returns></returns>
        public SeleniumLog Warning()
        {
            MessageSettings1.Warning = true;
            MessageSettings2.Warning = true;
            MessageSettings3.Warning = true;
            return this;
        }


        /// <summary>
        /// Put an error icon on the step.
        /// </summary>
        /// <returns></returns>
        public SeleniumLog Error()
        {
            MessageSettings1.Error = true;
            MessageSettings2.Error = true;
            MessageSettings3.Error = true;
            return this;
        }

        /// <summary>
        /// Put an error icon on the step in red fonts.
        /// </summary>
        /// <returns></returns>
        public SeleniumLog Fatal()
        {
            MessageSettings1.Fatal = true;
            MessageSettings2.Fatal = true;
            MessageSettings3.Fatal = true;
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
            MessageSettings1.RGB = rgb;
            //MessageSettings2.RGB = rgb;
            //MessageSettings3.RGB = rgb;
            return this;
        }


        /// <summary>
        /// Attach a picture file to a step. 
        /// </summary>
        /// <param name="PicturePath"></param>
        /// <returns></returns>
        public SeleniumLog AttachPicture(string PicturePath)
        {
            MessageSettings1.Image = PicturePath;
            //MessageSettings2.Image = PicturePath;
            //MessageSettings3.Image = PicturePath;
            return this;
        }

        /// <summary>
        /// Attach a file of any format to the step. Can also use for pictures.
        /// </summary>
        /// <param name="_FilePath"></param>
        /// <returns></returns>
        private SeleniumLog AttachFile(string FilePath)
        {
            MessageSettings1.File = FilePath;
            //MessageSettings2.File = FilePath;
            //MessageSettings3.File = FilePath;
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
            MessageSettings1.Path = PathStr;
            MessageSettings2.Path = PathStr;
            MessageSettings3.Path = PathStr;

            if (WriteNow)
            {
                string StrToWrite = MessageSettings1.FormMessageString();
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
            MessageSettings1.Tab = TabStr;
            MessageSettings2.Tab = TabStr;
            MessageSettings3.Tab = TabStr;

            if (WriteNow)
            {
                string StrToWrite = MessageSettings1.FormMessageString();
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
            MessageSettings1.TimestampFormat = Format;
            MessageSettings2.TimestampFormat = Format;
            MessageSettings3.TimestampFormat = Format;
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
                    string PICTURE_PATH = Config.OutputFormatSeleniumLogViewer_Screenshots + "/" + GetUniqueFilename("jpg");

                    if (Config.UseFastScreenshot)
                    {
                        TakeScreenshot(scrndriver, PICTURE_PATH);
                    }
                    else
                    {
                        Screenshot ss = ((ITakesScreenshot)scrndriver).GetScreenshot();
                        ss.SaveAsFile(PICTURE_PATH, ImageFormat.Jpeg);
                    }
                    MessageSettings1.Image = PICTURE_PATH;
                    //MessageSettings2.Image = PICTURE_PATH;
                    //MessageSettings3.Image = PICTURE_PATH;
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

        private void TakeScreenshot(IWebDriver drv, string path)
        {
            Rectangle bounds = new Rectangle(drv.Manage().Window.Position, drv.Manage().Window.Size);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                bitmap.Save(path, ImageFormat.Jpeg);
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
            //if (Config.ScreenshotOnEveryMessage)
            //    Screenshot().RGB(255, 0, 0);
            //else
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
            RGB(0, 0, 255);
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
            RGB(250, 97, 5);
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