/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package SeleniumLogger;

import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class SaveIndents
{
    private String Separator = "_______///////|||///________seleniumlog_iNdEx_____:::::_____";
    private HashMap<String, Integer> Indents = new HashMap<String, Integer>();
    public SaveIndents() { }

    public int[] Get(String key)
    {
        int[] return_arr = new int[2];
        int highest_index = GetGreatestIndex(key);
        String new_key = key + Separator + Integer.toString(highest_index);
        //string new_key = key;
        return_arr[0] = highest_index;
        if (Indents.containsKey(new_key))
        {
            return_arr[1] = Indents.get(new_key);
        }
        else
        {
            return_arr[1] = -1;
        }
        return return_arr;
    }

    public void Set(String key, int value)
    {
        int index = 0;
        String new_key = "";
        int highest_index = GetGreatestIndex(key);

        if (highest_index > -1)
        {
            index = GetNewIndex(key);
            //Indents[key] = value;
            new_key = key + Separator + Integer.toString(index);
            Indents.put(new_key, value);
        }
        else
        {
            new_key = key + Separator + "0";
            Indents.put(new_key, value);
        }
    }

    /// <summary>
    /// Returns the next and highest index available for a given key string. This is done by incrementing the highest index of a key.
    /// </summary>
    /// <param name="partial_key"></param>
    /// <returns></returns>
    private int GetNewIndex(String partial_key)
    {
        try
        {
            int highest_index = GetGreatestIndex(partial_key);
            return highest_index + 1;
        }
        catch (Exception e)
        {
            System.out.println("SavedIndexes.GetNewIndex() Exception - " + e.getMessage());
            return 0;
        }
    }

    /// <summary>
    /// Returns the highest index for a given key string.
    /// </summary>
    /// <param name="partial_key"></param>
    /// <returns></returns>
    private int GetGreatestIndex(String partial_key)
    {
        try
        {
            int index = -1;
            int highest_index = -1;
            String filterString = partial_key + Separator;
            for (Map.Entry<String, Integer> pair : Indents.entrySet()) {
                if (pair.getKey().contains(filterString)) {
                    index = Integer.parseInt(pair.getKey().replaceAll(Pattern.quote(partial_key + Separator), ""));
                    if (index > highest_index)
                        highest_index = index;
                }
            }
            return highest_index;
        }
        catch (Exception e)
        {
            System.out.println("SavedIndexes.GetGreatestIndex() Exception - " + e.getMessage());
            return 0;
        }
    }

    public void DeleteKey(String key, int index)
    {
        String new_key = key + Separator + Integer.toString(index);
        Indents.remove(new_key);
    }
}