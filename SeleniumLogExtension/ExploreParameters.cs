using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumLogger
{
    //public class ExploreParameters
    public sealed partial class SeleniumLog
    {
        /// <summary>
        /// Explores any data structure and display the entire tree of elements and values in the log. Works with Lists, Stacks, Queues, etc., or any combination thereof.
        /// </summary>
        /// <param name="arg">The data structure to be explored.</param>
        /// <param name="comment">Optional string to be displayed in the log.</param>
        public void Explore(object arg, string comment = "")
        {
            SeleniumLog log = SeleniumLog.Instance();
            log.SaveIndent("ExploreParams");
            PrintParameters printParameters = new PrintParameters();
            printParameters.PrintValues(arg, comment);
            log.RestoreIndent("ExploreParams");
            //log.MessageSettings.indentModel.SimulateIndentations(MessageSettings.MessageStr);
            log.MessageSettings.GetPendingLevel();
        }
    }

    #region Print Parameter Values

    internal class PrintParameters
    {
        private bool debug = false;
 
        internal void PrintValues(object arg, string comment = "")
        {
            //  Display Values for custom attribute
            DispalyInformation(arg.ToString(), arg, comment: comment);
        }


        public void DispalyInformation(string argumentName, object arg, int elem_counter = -1, int type = -1, string comment = "")
        {
            SeleniumLog log = SeleniumLog.Instance();
            log.SaveIndent("DispalyInformation");
            //log.Purple().WriteLine("DisplayInformation()");
            //log.Indent();

            if (arg == null)
            {
                //Console.WriteLine("TP-0.1.S");
                if (log.Config.FunctionTrace_DisplayNullInputs == true)
                    log.Gray().WriteLine(argumentName + " [NULL]");
                //Console.WriteLine("TP-0.1.E");
            }
            else
            {
                int j = 0;
                // Display Dictionary and Sorted List values
                if (CheckParameterType.CheckIfDictionary(arg) || CheckParameterType.CheckIfSortedList(arg))
                {
                    //Console.WriteLine("TP-1.S");
                    var data = arg as IDictionary;

                    if (data == null) return;

                    if (debug) 
                        log.Blue().WriteLine("45 " + argumentName + ": ");
                    else 
                        log.Purple().WriteLine(argumentName + ": ");

                    log.SaveIndent("______DisplayDictionaryValues_____");
                    log.Indent();
                    int count = data.Count;

                    object[] keysArray = new object[count];
                    object[] valuesArray = new object[count];

                    data.Keys.CopyTo(keysArray, 0);
                    data.Values.CopyTo(valuesArray, 0);

                    for (int i = 0; i < count; i++)
                    {
                        ManageCustomDataStructure("" + keysArray[i], valuesArray[i], i, type: 1, comment: comment);
                    }
                    log.RestoreIndent("______DisplayDictionaryValues_____");
                }
                // Display HashTable values
                else if (CheckParameterType.CheckIfHashTable(arg))
                {
                    var hashtable = arg as Hashtable;

                    if (hashtable == null) return;

                    int count = hashtable.Count;

                    object[] keysArray = new object[count];
                    object[] valuesArray = new object[count];

                    hashtable.Keys.CopyTo(keysArray, 0);
                    hashtable.Values.CopyTo(valuesArray, 0);

                    //Console.WriteLine(argumentName + ":");
                    if (elem_counter > -1)
                        if (debug)
                            log.Purple().WriteLine("60" + argumentName + " " + elem_counter + " : ");
                        else
                            log.Purple().WriteLine(argumentName + " " + elem_counter + " : ");
                    
                    else
                        if (debug)
                            log.Purple().WriteLine("60" + argumentName + " " + "COUNT?? : ");
                        else
                            log.Purple().WriteLine(argumentName + " " + elem_counter + " : ");

                    log.SaveIndent("______DisplayHashTableValues_____");
                    log.Indent();
                    for (int i = 0; i < count; i++)
                    {
                        ManageCustomDataStructure("" + keysArray[i], valuesArray[i], elem_counter: i, type: 2, comment: comment);
                    }
                    log.RestoreIndent("______DisplayHashTableValues_____");
                }
                // Display Enumerable values
                else if (CheckParameterType.CheckIfEnumerable(arg))
                {
                    if (debug)
                        log.Red().WriteLine("50 type " + type + "  " + argumentName + ": ");
                    else
                        log.Purple().WriteLine(argumentName + ": ");
                    
                    log.SaveIndent("______DisplayEenumerableValues_____");
                    log.Indent();

                    foreach (var iterateValue in (IEnumerable)arg)
                    {
                        log.SaveIndent("______DisplayEenumerableValues2_____");
                        ManageCustomDataStructure(argumentName, iterateValue, j, type: 3, comment: comment);
                        j++;
                        log.RestoreIndent("______DisplayEenumerableValues2_____");
                    }
                    log.RestoreIndent("______DisplayEenumerableValues_____");
                }
                else
                {
                    ManageCustomDataStructure(argumentName, arg, type: 4, comment: comment);
                }
            }
            log.RestoreIndent("DispalyInformation");
        }

        [SeleniumLogTrace]
        private bool ManageCustomDataStructure(string argumentName, object arg, int elem_counter = -1, int type = -1, string comment = "")
        {
            SeleniumLog log = SeleniumLog.Instance();
            log.SaveIndent("ManageCustomDataStructure");
            //log.Purple().WriteLine("ManageCustomDataStructure()");
            //log.Indent();

            // Display Dictionary and Sorted List values
            if (arg == null)
            {
                if (log.Config.FunctionTrace_DisplayNullInputs == true)
                    log.Gray().WriteLine(string.Format("{0} {1} : [NULL]", argumentName, elem_counter));
                log.RestoreIndent("ManageCustomDataStructure");
                return false;
            }

            if (CheckParameterType.CheckIfDictionary(arg) || CheckParameterType.CheckIfSortedList(arg))
            {
                DispalyInformation(argumentName: argumentName, elem_counter: elem_counter, arg: arg);
                log.RestoreIndent("ManageCustomDataStructure");
                return true;
            }
            // Display HashTable values
            else if (CheckParameterType.CheckIfHashTable(arg))
            {
                DispalyInformation(argumentName: argumentName, elem_counter: elem_counter, arg: arg);
                log.RestoreIndent("ManageCustomDataStructure");
                return true;
            }
            // Display Enumerable values
            else if (CheckParameterType.CheckIfEnumerable(arg))
            {
                DispalyInformation(argumentName: argumentName, elem_counter: elem_counter, arg: arg);
                log.RestoreIndent("ManageCustomDataStructure");
                return true;
            }
            else if (CheckParameterType.CheckIfCustomType(arg.GetType()))
            {
                var properties = CheckParameterType.GetPublicProperties(arg);

                //Console.WriteLine(argumentName + ":");

                //log.SaveIndent("_______CustomType1________");
                //log.Red().WriteLine("type: " + type + " argumentName - " + argumentName + " : count = " + elem_counter);
                if (type == 4)
                {
                    //log.Red().WriteLine("30 type 4 : " + comment);
                    log.Purple().WriteLine(comment);
                    log.Indent();
                }

                //log.Indent();
                if (properties.Any())
                {
                    log.SaveIndent("_______CustomType1________");

                    //log.Green("type " + type + "  properties.Any(): " + argumentName + " " + elem_counter + " :");
                    log.Green(argumentName + " " + elem_counter + " :");
                    int m = 0;
                    foreach (PropertyInfo property in properties)
                    {
                        //log.SaveIndent("_______CustomType1________");
                        //log.Blue().WriteLine(argumentName + ":");
                        //log.Indent();

                        var propertyValue = property.GetValue(arg, null) ?? "null";
                        //DispalyInformation(property.Name, propertyValue);
                        DispalyInformation(argumentName: property.Name, elem_counter: m, arg: propertyValue);
                        m++;

                        //log.RestoreIndent("_______CustomType1________");
                    }
                    log.RestoreIndent("_______CustomType1________");
                }

                var fields = CheckParameterType.GetPublicFields(arg);

                if (fields.Any())
                {
                    if (debug)
                        log.Green("37 type " + type + "  fields.Any(): " + argumentName + " " + elem_counter + " :");
                    else
                        log.Purple(argumentName + " " + elem_counter + " :");
                    
                    log.SaveIndent("_______FieldsAny______");
                    log.Indent();
                    int k = 0;
                    foreach (FieldInfo field in fields)
                    {
                        log.SaveIndent("_______CustomType2________");
                        //log.Indent();

                        var fieldValue = field.GetValue(arg) ?? "null";
                        //DispalyInformation(field.Name, fieldValue);
                        DispalyInformation(argumentName: field.Name, elem_counter: k, arg: fieldValue);
                        k++;
                        log.RestoreIndent("_______CustomType2________");
                    }
                    //log.Red().WriteLine("loop 2 done");
                    log.RestoreIndent("_______FieldsAny______");
                }
                log.RestoreIndent("ManageCustomDataStructure");
                return true;
            }
            
            //Console.WriteLine(string.Format("{0}:{1}", argumentName, arg));
            if (type == 1)
            {
                if (debug)
                    log.Purple().WriteLine(string.Format("100 type" + type + " {0} [{1}]", argumentName, arg));
                else
                    log.Purple().WriteLine(string.Format("{0} [{1}]", argumentName, arg));
            }
            else if (type == 2)
            {
                if (debug)
                    log.Purple().WriteLine(string.Format("101 type {0}: key [{1}] value [{2}]", type, argumentName, arg));
                else
                    log.Purple().WriteLine(string.Format("key [{1}] value [{2}]", type, argumentName, arg));
            }
            else if (type == 3)
            {
                if (debug)
                    log.Purple().WriteLine(string.Format("102 type" + type + " [{0}]", arg));
                else
                    log.Purple().WriteLine(string.Format("[{0}]", arg));
            }
            else if (type == 4)
            {
                // not a data structure, so don't parse
                if (debug)
                    log.Purple().WriteLine(string.Format("103 type" + type + " {0} [{1}]", argumentName, arg));
                else
                    log.Purple().WriteLine(string.Format("{0} [{1}]", argumentName, arg));
                //return false;
            }
            log.RestoreIndent("ManageCustomDataStructure");
            return false;
        }
    }

    #endregion

    #region Verify if Parameter can be iterated through

    internal static class CheckParameterType
    {
        internal static bool CheckIfEnumerable(object variable)
        {
            if (variable != null)
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
            }
            return false;
        }

        internal static bool CheckIfHashTable(object variable)
        {
            if (variable != null)
            {
                if (variable.GetType() == typeof(Hashtable))
                {
                    return true;
                }
            }
            return false;
        }

        internal static bool CheckIfDictionary(object variable)
        {
            if (variable != null)
            {
                if (variable.GetType().Name.ToLower().Contains("dictionary"))
                {
                    return true;
                }
            }
            return false;
        }

        internal static bool CheckIfSortedList(object variable)
        {
            if (variable != null)
            {
                if (variable.GetType().Name.ToLower().Contains("sortedlist"))
                {
                    return true;
                }
            }
            return false;
        }

        internal static bool CheckIfCustomType(Type type)
        {
            if (type != null)
            {
                if (type.Namespace != null && !type.Namespace.ToLowerInvariant().Contains("system"))
                {
                    return true;
                }
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
}
