
package SeleniumLogger;

import java.lang.reflect.Field;
import java.util.HashMap;
import java.util.logging.Level;
import java.util.logging.Logger;

class PrintParameters {
    
    private boolean debug = false;
    
    public void PrintValues(Object arg) {
        PrintValues(arg, "");
    }

    public void PrintValues(Object arg, String comment)
    {
        //  Display Values for custom attribute
        DispalyInformation(arg.toString(), arg, comment);
    }
    
    public void DispalyInformation(String argumentName, Object arg) {
        DispalyInformation(argumentName, arg, -1, -1, "");
    }
    
    public void DispalyInformation(String argumentName, Object arg, String comment) {
        DispalyInformation(argumentName, arg, -1, -1, comment);
    }
    
    public void DispalyInformation(String argumentName, Object arg, int elem_counter, int type, String comment) {
        SeleniumLog log = SeleniumLog.Instance();
        log.SaveIndent("DispalyInformation");

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
            int count;
            // Display Dictionary and Sorted List values
            if (CheckParameterType.CheckIfDictionary(arg) || CheckParameterType.CheckIfSortedList(arg))
            {
                //Console.WriteLine("TP-1.S");
                //var data = arg as IDictionary;
                HashMap data = (HashMap)arg;

                if (debug) 
                    log.Blue().WriteLine("45 " + argumentName + ": ");
                else 
                    log.Purple().WriteLine(argumentName + ": ");

                log.SaveIndent("______DisplayDictionaryValues_____");
                log.Indent();
                count = data.size();

                Object[] keysArray = data.keySet().toArray();
                Object[] valuesArray = data.values().toArray();

                for (int i = 0; i < count; i++)
                {
                    ManageCustomDataStructure("" + keysArray[i], valuesArray[i], i, 1, comment);
                }
                log.RestoreIndent("______DisplayDictionaryValues_____");
            }
            // Display HashTable values
            else if (CheckParameterType.CheckIfHashTable(arg))
            {
                HashMap hashtable = (HashMap)arg;
                count = hashtable.size();

                Object[] keysArray = hashtable.keySet().toArray();
                Object[] valuesArray = hashtable.values().toArray();

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
                    ManageCustomDataStructure("" + keysArray[i], valuesArray[i], i, 2, comment);
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

                for (Object iterateValue : (Iterable)arg)
                {
                    log.SaveIndent("______DisplayEenumerableValues2_____");
                    ManageCustomDataStructure(argumentName, iterateValue, j, 3, comment);
                    j++;
                    log.RestoreIndent("______DisplayEenumerableValues2_____");
                }
                log.RestoreIndent("______DisplayEenumerableValues_____");
            }
            else
            {
                ManageCustomDataStructure(argumentName, arg, 4, comment);
            }
        }
        log.RestoreIndent("DispalyInformation");
    }
      
    private boolean ManageCustomDataStructure(String argumentName, Object arg, int type, String comment) {
        return ManageCustomDataStructure(argumentName, arg, -1, type, comment);
    }
    
    //[SeleniumLogTrace]
    private boolean ManageCustomDataStructure(String argumentName, Object arg, int elem_counter, int type, String comment)
    {
        SeleniumLog log = SeleniumLog.Instance();
        log.SaveIndent("ManageCustomDataStructure");
        //log.Purple().WriteLine("ManageCustomDataStructure()");
        //log.Indent();

        // Display Dictionary and Sorted List values
        if (arg == null)
        {
            if (log.Config.FunctionTrace_DisplayNullInputs == true)
                log.Gray().WriteLine(String.format("%s %d : [NULL]", argumentName, elem_counter));
            log.RestoreIndent("ManageCustomDataStructure");
            return false;
        }

        if (CheckParameterType.CheckIfDictionary(arg) || CheckParameterType.CheckIfSortedList(arg))
        {
            DispalyInformation(argumentName, arg, elem_counter, -1, "");
            log.RestoreIndent("ManageCustomDataStructure");
            return true;
        }
        // Display HashTable values
        else if (CheckParameterType.CheckIfHashTable(arg))
        {
            DispalyInformation(argumentName, arg, elem_counter, -1, "");
            log.RestoreIndent("ManageCustomDataStructure");
            return true;
        }
        // Display Enumerable values
        else if (CheckParameterType.CheckIfEnumerable(arg))
        {
            DispalyInformation(argumentName, arg, elem_counter, -1, "");
            log.RestoreIndent("ManageCustomDataStructure");
            return true;
        }
        else if (CheckParameterType.CheckIfCustomType(arg.getClass()))
        {
            //Field[] properties = CheckParameterType.GetPublicProperties(arg);

            if (type == 4)
            {
                //log.Red().WriteLine("30 type 4 : " + comment);
                log.Purple().WriteLine(comment);
                log.Indent();
            }

            Field[] fields = CheckParameterType.GetPublicFields(arg);

            if (fields.length > 0)
            {
                if (debug)
                    log.Green("37 type " + type + "  fields.Any(): " + argumentName + " " + elem_counter + " :");
                else
                    log.Purple(argumentName + " " + elem_counter + " :");

                log.SaveIndent("_______FieldsAny______");
                log.Indent();
                int k = 0;
                for (Field field : fields)
                {
                    log.SaveIndent("_______CustomType2________");
                    //log.Indent();

                    Object fieldValue;
                    try {
                        fieldValue = field.get(arg);
                    } catch (IllegalArgumentException | IllegalAccessException ex) {
                        fieldValue = "";
                    }
                    //DispalyInformation(field.Name, fieldValue);
                    DispalyInformation(field.getName(), fieldValue, k, -1, "");
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
                log.Purple().WriteLine(String.format("100 type" + type + " %s [%s]", argumentName, arg.toString()));
            else
                log.Purple().WriteLine(String.format("%s [%s]", argumentName, arg.toString()));
        }
        else if (type == 2)
        {
            if (debug)
                log.Purple().WriteLine(String.format("101 type %d: key [%s] value [%s]", type, argumentName, arg.toString()));
            else
                log.Purple().WriteLine(String.format("key [%s] value [%s]", argumentName, arg.toString()));
        }
        else if (type == 3)
        {
            if (debug)
                log.Purple().WriteLine(String.format("102 type" + type + " [%s]", arg.toString()));
            else
                log.Purple().WriteLine(String.format("[%s]", arg.toString()));
        }
        else if (type == 4)
        {
            // not a data structure, so don't parse
            if (debug)
                log.Purple().WriteLine(String.format("103 type" + type + " %s [%s]", argumentName, arg.toString()));
            else
                log.Purple().WriteLine(String.format("%s [%s]", argumentName, arg.toString()));
            //return false;
        }
        log.RestoreIndent("ManageCustomDataStructure");
        return false;
    }
    
}
