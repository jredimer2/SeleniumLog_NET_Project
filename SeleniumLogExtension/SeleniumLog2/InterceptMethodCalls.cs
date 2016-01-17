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
        private Stopwatch _stopwatch = new Stopwatch();

        public void PreMethodExecutionProcess(ref IMethodCallMessage msg)
        {
            _stopwatch.Start();
            //Console.WriteLine("Method details: " + msg.MethodBase);

            PrintParameters printParameters = new PrintParameters();
            printParameters.PrintValues(msg);
        }

        public void PostMethodExecutionProcess(IMethodCallMessage callMsg, ref IMethodReturnMessage retMsg)
        {
            _stopwatch.Stop();

            //Console.WriteLine("Return value: {0}", retMsg.ReturnValue);
            //Console.WriteLine("Ticks consumed: {0}", _stopwatch.ElapsedTicks);
        }
    }

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class SeleniumLogTraceAttribute : Attribute
    {
        private readonly ISeleniumLogTraceHandler _methodCallHandler;

        public SeleniumLogTraceAttribute()
        {
            this._methodCallHandler = new SeleniumLogTraceHandler();
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
        public void Process(ref IMethodCallMessage msg)
        {
            //Console.WriteLine("Method details: " + msg.MethodBase);

            PrintParameters printParameters = new PrintParameters();
            printParameters.PrintValues(msg);
        }
    }

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class PreMethodExecutionAttribute : Attribute
    {
        private readonly IPreMethodExecutionHandler _methodCallHandler;
        public PreMethodExecutionAttribute()
        {
            this._methodCallHandler = new PreMethodExecutionHandler();
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
        public void Process(IMethodCallMessage callMsg, ref IMethodReturnMessage retMsg)
        {
            //Console.WriteLine("Return value: {0}", retMsg.ReturnValue);
        }
    }

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class PostMethodExecutionAttribute : Attribute
    {
        private readonly IPostMethodExecutionHandler _methodCallHandler;
        public PostMethodExecutionAttribute()
        {
            this._methodCallHandler = new PostMethodExecutionHandler();
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
        public InterceptProperty()
            : base()
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
            InterceptProperty p = newCtx.GetProperty("Intercept") as InterceptProperty;
            if (p == null)
                return false;
            return true;
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
            ctorMsg.ContextProperties.Add(new InterceptProperty());
        }

        public override bool IsContextOK(Context ctx,
            System.Runtime.Remoting.Activation.IConstructionCallMessage ctorMsg)
        {
            InterceptProperty p = ctx.GetProperty("Intercept") as InterceptProperty;
            if (p == null)
                return false;
            return true;
        }

        public override bool IsNewContextOK(Context newCtx)
        {
            InterceptProperty p = newCtx.GetProperty("Intercept") as InterceptProperty;
            if (p == null)
                return false;
            return true;
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

        public IMessageSink NextSink
        {
            get { return this._nextSink; }
        }

        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            IMessageCtrl rtnMsgCtrl = _nextSink.AsyncProcessMessage(msg, replySink);
            return rtnMsgCtrl;
        }

        #endregion

        private void PreMethodExecution(ref IMethodCallMessage msg)
        {
            PreMethodExecutionAttribute[] attrs =
                (PreMethodExecutionAttribute[])
                    msg.MethodBase.GetCustomAttributes(typeof (PreMethodExecutionAttribute), true);
            for (int i = 0; i < attrs.Length; i++)
                attrs[i].MethodCallHandler.Process(ref msg);
        }

        private void PostMethodExecution(IMethodCallMessage callMsg, ref IMethodReturnMessage rtnMsg)
        {
            PostMethodExecutionAttribute[] attrs =
                (PostMethodExecutionAttribute[])
                    callMsg.MethodBase.GetCustomAttributes(typeof (PostMethodExecutionAttribute), true);
            for (int i = 0; i < attrs.Length; i++)
                attrs[i].MethodCallHandler.Process(callMsg, ref rtnMsg);
        }

    }

    #endregion

    #region Verify if Parameter can be iterated through

    internal static class ParameterProcessor
    {
        internal static bool CheckIfEnumerable(object variable)
        {
            if (variable is string)
            {
                return false;
            }
            //if (typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
            if (variable.GetType().GetInterfaces().Any( i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof (IEnumerable<>)))
            {
                return true;
            }

            return false;
        }

        internal static bool CheckIfCustomType(Type type)
        {
            if (type.Namespace != null && !type.Namespace.ToLowerInvariant().Contains("system"))
            {
                return true;
            }

            return false;
        }

        internal static PropertyInfo[] GetPublicProperties(object arg)
        {
            Type type = arg.GetType();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            return type.GetProperties(flags);
        }

        internal static FieldInfo[] GetPublicFields(object arg)
        {
            Type type = arg.GetType();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            return type.GetFields(flags);
        }
    }

    #endregion

    #region Print Parameter Values

    internal class PrintParameters
    {
        internal void PrintValues(IMethodCallMessage msg)
        {
            int argumentIndex = 0;
            foreach (var arg in msg.Args)
            {
                //Console.WriteLine(string.Format("Argument Name: {0}", msg.GetInArgName(argumentIndex)));

                //  Display Values for custom attribute
                DispalyInformation(msg.GetInArgName(argumentIndex), arg);

                argumentIndex++;
            }
        }

        private void DispalyInformation(string argumentName, object arg)
        {
            // Display Enumerable values
            if (ParameterProcessor.CheckIfEnumerable(arg))
            {
                foreach (var iterateValue in (IEnumerable)arg)
                {
                    ManageCustomDataStructure(argumentName, iterateValue);
                }
            }
            else
            {
                ManageCustomDataStructure(argumentName, arg);
            }
        }

        private bool ManageCustomDataStructure(string argumentName, object arg) 
        {
            if (ParameterProcessor.CheckIfCustomType(arg.GetType()))
            {
                var properties = ParameterProcessor.GetPublicProperties(arg);

                //Console.WriteLine(argumentName + ":");

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

                return true;
            }

            //Console.WriteLine(string.Format("{0}:{1}", argumentName, arg));
            return false;
        }
    }

    #endregion
}


