
package SeleniumLogger;

import java.lang.reflect.Field;
import java.util.HashMap;
import java.util.SortedSet;

final class CheckParameterType
{
    private CheckParameterType() { }
    
    public static boolean CheckIfEnumerable(Object variable)
    {
        if (variable != null)
        {
            if (variable instanceof String)
            {
                return false;
            }
            //if (typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
            return (variable instanceof Iterable);
        }
        return false;
    }

    public static boolean CheckIfHashTable(Object variable)
    {
        if (variable != null)
        {
            if (variable instanceof HashMap)
            {
                return true;
            }
        }
        return false;
    }

    public static boolean CheckIfDictionary(Object variable)
    {
        if (variable != null)
        {
            if (variable instanceof HashMap)
            {
                return true;
            }
        }
        return false;
    }

    public static boolean CheckIfSortedList(Object variable)
    {
        if (variable != null)
        {
            if (variable instanceof SortedSet)
            {
                return true;
            }
        }
        return false;
    }

    public static boolean CheckIfCustomType(@SuppressWarnings("rawtypes") Class type)
    {
        if (type != null)
        {
            // It's not obvious what types should be considered as "custom" - A.V.
            if (!type.isPrimitive() && !type.getName().startsWith("java"))
            {
                return true;
            }
        }
        return false;
    }

    public static Field[] GetPublicProperties(Object arg)
    {
        return arg.getClass().getDeclaredFields();
    }

    public static Field[] GetPublicFields(Object arg)
    {
        return arg.getClass().getDeclaredFields();
    }
}