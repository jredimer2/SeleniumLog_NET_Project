using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using XMLConfig;

namespace SeleniumLogger
{
    #region Base Class to be inherited

    [InterceptMethod]
    public abstract class BaseClass : ContextBoundObject
    {

    }

    #endregion

    #region Selenium Log Trace Attribute

    public interface ISeleniumLogTraceHandler
    {
        void PreMethodExecutionProcess(ref IMethodCallMessage msg);
        void PostMethodExecutionProcess(IMethodCallMessage callMsg, ref IMethodReturnMessage retMsg);
    }

    public class SeleniumLogTraceHandler : ISeleniumLogTraceHandler
    {
        private bool _enabled;
        private Stopwatch _stopwatch = new Stopwatch();
        private int id1 = -1;
        private int id2 = -1;

        public SeleniumLogTraceHandler(bool enabled)
        {
            XmlConfigurationClass Config = XmlConfigurationClass.Instance();
            _enabled = Convert.ToBoolean(Config.Enable_Generic_Function_Trace);
        }

        public void PreMethodExecutionProcess(ref IMethodCallMessage msg)
        {
            SeleniumLog log = SeleniumLog.Instance();
            try
            {
                if (_enabled)
                {
                    _stopwatch.Start();
                    log.Blue().WriteLine("FUNCTION CALL: " + msg.MethodBase);
                    log.SaveIndent("__PRE_METHOD_EXECUTION_PROCESS_1_");
                    log.Indent();
                    log.SaveIndent("__PRE_METHOD_EXECUTION_PROCESS_2_");
                    PrintParameters printParameters = new PrintParameters();
                    printParameters.PrintValues(msg);
                    log.RestoreIndent("__PRE_METHOD_EXECUTION_PROCESS_2_");
                }
            }
            catch (Exception e)
            {
                log.Warning().WriteLine("SeleniumLog Exception: 01-01 - " + e.Message);
            }
        }

        public void PostMethodExecutionProcess(IMethodCallMessage callMsg, ref IMethodReturnMessage retMsg)
        {
            SeleniumLog log = SeleniumLog.Instance();
            try
            {
                if (_enabled)
                {
                    _stopwatch.Stop();
                    log.Blue().WriteLine(string.Format("Return value: {0}", retMsg.ReturnValue));
                    log.Blue().WriteLine(string.Format("Duration: {0} msec", _stopwatch.ElapsedMilliseconds));
                    //log.RestoreIndent("__SELENIUMLOG_INDENT__");
                    log.RestoreIndent("__PRE_METHOD_EXECUTION_PROCESS_1_");
                }
            }
            catch (Exception e)
            {
                log.Warning().WriteLine("SeleniumLog Exception: 01-02 - " + e.Message);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class SeleniumLogTraceAttribute : Attribute
    {
        private readonly ISeleniumLogTraceHandler _methodCallHandler;

        public SeleniumLogTraceAttribute(bool enabled = true)
        {
            try
            {
                this._methodCallHandler = new SeleniumLogTraceHandler(enabled);
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Warning().WriteLine("SeleniumLog Exception: 01-03 - " + e.Message);
            }
        }

        public ISeleniumLogTraceHandler MethodCallHandler
        {
            get { return _methodCallHandler; }
        }
    }

    #endregion

    #region Pre-Method Execution Handler

    public interface IPreMethodExecutionHandler
    {
        void Process(ref IMethodCallMessage msg);
    }

    public class PreMethodExecutionHandler : IPreMethodExecutionHandler
    {
        private readonly bool _enabled;

        public PreMethodExecutionHandler(bool enabled)
        {
            _enabled = enabled;
        }

        public void Process(ref IMethodCallMessage msg)
        {
            SeleniumLog log = SeleniumLog.Instance();
            try
            {
                log.Blue().WriteLine("FUNCTION CALL: " + msg.MethodBase);
                log.SaveIndent("__SELENIUMLOG_INDENT__");
                log.Indent();

                PrintParameters printParameters = new PrintParameters();
                printParameters.PrintValues(msg);
            }
            catch (Exception e)
            {
                log.Warning().WriteLine("SeleniumLog Exception: 01-04 - " + e.Message);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class PreMethodExecutionAttribute : Attribute
    {
        private readonly IPreMethodExecutionHandler _methodCallHandler;
        public PreMethodExecutionAttribute(bool enabled = true)
        {
            this._methodCallHandler = new PreMethodExecutionHandler(enabled);
        }

        public IPreMethodExecutionHandler MethodCallHandler
        {
            get { return _methodCallHandler; }
        }
    }

    #endregion


    #region Post-Method Execution Handler

    public interface IPostMethodExecutionHandler
    {
        void Process(IMethodCallMessage callMsg, ref IMethodReturnMessage retMsg);
    }

    public class PostMethodExecutionHandler : IPostMethodExecutionHandler
    {
        private bool _enabled = true;

        public PostMethodExecutionHandler(bool enabled)
        {
            _enabled = enabled;
        }

        public void Process(IMethodCallMessage callMsg, ref IMethodReturnMessage retMsg)
        {
            if (_enabled)
            {
                SeleniumLog log = SeleniumLog.Instance();
                try
                {
                    //_stopwatch.Stop();
                    log.Blue().WriteLine(string.Format("Return value: {0}", retMsg.ReturnValue));
                    log.RestoreIndent("__SELENIUMLOG_INDENT__");
                }
                catch (Exception e)
                {
                    log.Warning().WriteLine("SeleniumLog Exception: 01-05 - " + e.Message);
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class PostMethodExecutionAttribute : Attribute
    {
        private readonly IPostMethodExecutionHandler _methodCallHandler;
        public PostMethodExecutionAttribute(bool enabled = true)
        {
            this._methodCallHandler = new PostMethodExecutionHandler(enabled);
        }

        public IPostMethodExecutionHandler MethodCallHandler
        {
            get { return _methodCallHandler; }
        }
    }

    #endregion

    #region Intercept Property

    public class InterceptProperty : IContextProperty, IContributeObjectSink
    {
        public InterceptProperty() : base()
        {
        }
        #region IContextProperty Members

        public string Name
        {
            get
            {
                return "Intercept";
            }
        }

        public bool IsNewContextOK(Context newCtx)
        {
            try
            {
                InterceptProperty p = newCtx.GetProperty("Intercept") as InterceptProperty;
                if (p == null)
                    return false;
                return true;
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Warning().WriteLine("SeleniumLog Exception: 01-06 - " + e.Message);
                return false;
            }
        }

        public void Freeze(Context newContext)
        {
        }

        #endregion

        #region IContributeObjectSink Members

        public System.Runtime.Remoting.Messaging.IMessageSink GetObjectSink(MarshalByRefObject obj, System.Runtime.Remoting.Messaging.IMessageSink nextSink)
        {
            return new InterceptMethodCalls(nextSink);
        }

        #endregion
    }

    #endregion

    #region Intercept Attribute Class

    [AttributeUsage(AttributeTargets.Class)]
    public class InterceptMethodAttribute : ContextAttribute
    {

        public InterceptMethodAttribute() : base("InterceptMethod")
        {
        }

        public override void Freeze(Context newContext)
        {
        }

        public override void GetPropertiesForNewContext(
            System.Runtime.Remoting.Activation.IConstructionCallMessage ctorMsg)
        {
            try
            {
                ctorMsg.ContextProperties.Add(new InterceptProperty());
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Warning().WriteLine("SeleniumLog Exception: 01-07 - " + e.Message);
            }
        }

        public override bool IsContextOK(Context ctx,
            System.Runtime.Remoting.Activation.IConstructionCallMessage ctorMsg)
        {
            try
            {
                InterceptProperty p = ctx.GetProperty("Intercept") as InterceptProperty;
                if (p == null)
                    return false;
                return true;
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Warning().WriteLine("SeleniumLog Exception: 01-08 - " + e.Message);
                return false;
            }
        }

        public override bool IsNewContextOK(Context newCtx)
        {
            try
            {
                InterceptProperty p = newCtx.GetProperty("Intercept") as InterceptProperty;
                if (p == null)
                    return false;
                return true;
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Warning().WriteLine("SeleniumLog Exception: 01-09 - " + e.Message);
                return false;
            }
        }
    }

    #endregion

    #region Class for intercepting Method Calls

    public class InterceptMethodCalls : IMessageSink
    {
        private IMessageSink _nextSink;

        public InterceptMethodCalls(IMessageSink nextSink)
        {
            this._nextSink = nextSink;
        }

        #region IMessageSink Members

        public IMessage SyncProcessMessage(IMessage msg)
        {
            try
            {
                IMethodCallMessage mcm = (msg as IMethodCallMessage);

                // Check if Selenium Log Trace attribute is applied
                SeleniumLogTraceAttribute[] attrs = (SeleniumLogTraceAttribute[])mcm.MethodBase.GetCustomAttributes(typeof(SeleniumLogTraceAttribute), true);

                // Execute the Pre Method Processing
                for (int i = 0; i < attrs.Length; i++)
                    attrs[i].MethodCallHandler.PreMethodExecutionProcess(ref mcm);

                this.PreMethodExecution(ref mcm);
                IMessage rtnMsg = _nextSink.SyncProcessMessage(msg);
                IMethodReturnMessage mrm = (rtnMsg as IMethodReturnMessage);

                // Execute the Pre Method Processing for Selenium Log Attribute
                for (int i = 0; i < attrs.Length; i++)
                    attrs[i].MethodCallHandler.PostMethodExecutionProcess(mcm, ref mrm);

                this.PostMethodExecution(msg as IMethodCallMessage, ref mrm);
                return mrm;
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Warning().WriteLine("SeleniumLog Exception: 01-10 - " + e.Message);
                return null;
            }
        }

        public IMessageSink NextSink
        {
            get { return this._nextSink; }
        }

        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            try
            {
                IMessageCtrl rtnMsgCtrl = _nextSink.AsyncProcessMessage(msg, replySink);
                return rtnMsgCtrl;
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Warning().WriteLine("SeleniumLog Exception: 01-11 - " + e.Message);
                return null;
            }
        }

        #endregion

        private void PreMethodExecution(ref IMethodCallMessage msg)
        {
            try
            {
                PreMethodExecutionAttribute[] attrs =
                    (PreMethodExecutionAttribute[])
                        msg.MethodBase.GetCustomAttributes(typeof(PreMethodExecutionAttribute), true);
                for (int i = 0; i < attrs.Length; i++)
                    attrs[i].MethodCallHandler.Process(ref msg);
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Warning().WriteLine("SeleniumLog Exception: 01-12 - " + e.Message);
            }
        }

        private void PostMethodExecution(IMethodCallMessage callMsg, ref IMethodReturnMessage rtnMsg)
        {
            try {
            PostMethodExecutionAttribute[] attrs =
                (PostMethodExecutionAttribute[])
                    callMsg.MethodBase.GetCustomAttributes(typeof (PostMethodExecutionAttribute), true);
            for (int i = 0; i < attrs.Length; i++)
                attrs[i].MethodCallHandler.Process(callMsg, ref rtnMsg);
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Warning().WriteLine("SeleniumLog Exception: 01-13 - " + e.Message);
            }
        }

    }

    #endregion

    #region Verify if Parameter can be iterated through

    internal static class ParameterProcessor
    {
        internal static bool CheckIfEnumerable(object variable)
        {
            try
            {
                if (variable is string)
                {
                    return false;
                }
                //if (typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
                if (variable.GetType().GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Warning().WriteLine("SeleniumLog Exception: 01-14 - " + e.Message);
                return false;
            }
        }

        internal static bool CheckIfCustomType(Type type)
        {
            try
            {
                if (type.Namespace != null && !type.Namespace.ToLowerInvariant().Contains("system"))
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Warning().WriteLine("SeleniumLog Exception: 01-15 - " + e.Message);
                return false;
            }
        }

        internal static PropertyInfo[] GetPublicProperties(object arg)
        {
            try
            {
                Type type = arg.GetType();
                BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                return type.GetProperties(flags);
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Warning().WriteLine("SeleniumLog Exception: 01-16 - " + e.Message);
                return null;
            }
        }

        internal static FieldInfo[] GetPublicFields(object arg)
        {
            try
            {
                Type type = arg.GetType();
                BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                return type.GetFields(flags);
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Warning().WriteLine("SeleniumLog Exception: 01-17 - " + e.Message);
                return null;
            }
        }
    }

    #endregion

    #region Print Parameter Values

    internal class PrintParameters0
    {
        private string CURRENT_ARGUMENT_NAME = "";

        internal void PrintValues(IMethodCallMessage msg)
        {
            try {
                SeleniumLog log = SeleniumLog.Instance();
                log.Blue().WriteLine("Expand to view input parameters");
                log.SaveIndent("__SELENIUMLOG_PRINT_INPUTS__");
                log.Indent();

                int argumentIndex = 0;
                foreach (var arg in msg.Args)
                {
                    //  Display Values for custom attribute
                    CURRENT_ARGUMENT_NAME = msg.GetInArgName(argumentIndex);
                    DispalyInformation(CURRENT_ARGUMENT_NAME, arg);

                    argumentIndex++;
                }
                log.RestoreIndent("__SELENIUMLOG_PRINT_INPUTS__");
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Warning().WriteLine("SeleniumLog Exception: 01-18 - " + e.Message);
            }
        }

        private void DispalyInformation(string argumentName, object arg)
        {
            try
            {
                SeleniumLog log = SeleniumLog.Instance();
                // Display data structure       
                if (arg == null)
                {
                    if (log.Config.FunctionTrace_DisplayNullInputs == true)
                        log.Pink().WriteLine(argumentName + " [NULL]");
                }
                else
                {
                    if (ParameterProcessor.CheckIfEnumerable(arg))
                    {
                        //log.Red().WriteLine("Property: " + argumentName);
                        log.Blue().WriteLine(argumentName + " :");
                        log.SaveIndent("__DISPLAY_ARRAY__");
                        log.Indent();
                        int i = 0;
                        foreach (var iterateValue in (IEnumerable)arg)
                        {
                            ManageCustomDataStructure(argumentName, iterateValue, 1, i);
                            i++;
                        }
                        log.RestoreIndent("__DISPLAY_ARRAY__");
                    }
                    else
                    {
                        // Display property (simple structure)
                        ManageCustomDataStructure(argumentName, arg, 0);
                    }
                }
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                if (log.Config.Enable_Generic_Function_Trace == true)
                    log.Warning().WriteLine("SeleniumLog Exception: 01-19 - " + e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argumentName"></param>
        /// <param name="arg"></param>
        /// <param name="type">0 for Input parameter. 1 for data structure property.</param>
        /// <param name="elem_counter">The nth element if type is a data structure array.</param>
        /// <returns></returns>
        private bool ManageCustomDataStructure(string argumentName, object arg, int type, int elem_counter = -1) 
        {
            try {
                SeleniumLog log = SeleniumLog.Instance();

                if (arg == null)
                {
                    if (log.Config.FunctionTrace_DisplayNullInputs == true)
                        log.Pink().WriteLine(string.Format("{0} {1} : [NULL]", argumentName, elem_counter));
                }
                else
                {
                    if (ParameterProcessor.CheckIfCustomType(arg.GetType()))
                    {
                        var properties = ParameterProcessor.GetPublicProperties(arg);

                        //log.Blue().WriteLine(">>> Input data structure:" + argumentName + ":");
                        if (elem_counter > -1)
                        {
                            log.Blue().WriteLine(string.Format("{0} {1} :", argumentName, elem_counter));
                        }
                        else
                        {
                            log.Blue().WriteLine(string.Format("{0} :", argumentName));
                        }
                        log.SaveIndent("__MANAGE_CUSTOM_DATA_STRUCTURE__");
                        log.Indent();

                        if (properties.Any())
                        {
                            foreach (PropertyInfo property in properties)
                            {
                                DispalyInformation(property.Name, property.GetValue(arg, null));
                            }
                        }

                        var fields = ParameterProcessor.GetPublicFields(arg);

                        if (fields.Any())
                        {
                            foreach (FieldInfo field in fields)
                            {
                                DispalyInformation(field.Name, field.GetValue(arg));
                            }
                        }

                        log.RestoreIndent("__MANAGE_CUSTOM_DATA_STRUCTURE__");
                        return true;
                    }

                    if (type == 0)
                    {
                        log.Blue().WriteLine(string.Format("{0} [{1}]", argumentName, arg));
                    }
                    else
                    {
                        log.Blue().WriteLine(string.Format("[{0}]", arg));
                    }
                    //log.Blue().WriteLine(string.Format("+++ Input {0} [{1}]", argumentName, arg));
                    //log.RestoreIndent("__SELENIUMLOG_PRINT_INPUTS__");

                    //log.RestoreIndent("__MANAGE_CUSTOM_DATA_STRUCTURE__");
                    return false;
                }
                return false;
            }
            catch (Exception e)
            {
                SeleniumLog log = SeleniumLog.Instance();
                log.Warning().WriteLine("SeleniumLog Exception: 01-20 - " + e.Message);
                return false;
            }
        }
    }

    #endregion
}


