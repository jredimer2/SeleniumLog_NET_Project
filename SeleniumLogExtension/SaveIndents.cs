using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SeleniumLogger
{
    public sealed partial class SeleniumLog
    {
        

        public class SaveIndents
        {
            private string Separator = @"_______/\/\/\/\/\/\/|||\/\/\/________seleniumlog_iNdEx_____:::::_____";
            private Dictionary<string, int> Indents = new Dictionary<string, int>();
            public SaveIndents() { }

            public int[] Get(string key)
            {
                int[] return_arr = new int[2];
                int highest_index = GetGreatestIndex(key);
                string new_key = key + Separator + highest_index.ToString();
                //string new_key = key;
                return_arr[0] = highest_index;
                if (Indents.ContainsKey(new_key))
                {
                    return_arr[1] = Indents[new_key];
                }
                else
                {
                    return_arr[1] = -1;
                }
                return return_arr;
            }

            public void Set(string key, int value)
            {
                int index = 0;
                string new_key = "";
                int highest_index = GetGreatestIndex(key);

                if (highest_index > -1)
                {
                    index = GetNewIndex(key);
                    //Indents[key] = value;
                    new_key = key + Separator + index.ToString();
                    Indents.Add(new_key, value);
                }
                else
                {
                    new_key = key + Separator + "0";
                    Indents.Add(new_key, value);
                }
                 
                /*
                int index = GetNewIndex(key);
                string new_key = key + Separator + index.ToString();
                Indents.Add(new_key, value);
                return index;
                 */ 
            }

            /// <summary>
            /// Returns the next and highest index available for a given key string. This is done by incrementing the highest index of a key.
            /// </summary>
            /// <param name="partial_key"></param>
            /// <returns></returns>
            private int GetNewIndex(string partial_key)
            {
                try
                {
                    int highest_index = GetGreatestIndex(partial_key);
                    return highest_index + 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine("SavedIndexes.GetNewIndex() Exception - " + e.Message);
                    return 0;
                }
            }

            /// <summary>
            /// Returns the highest index for a given key string.
            /// </summary>
            /// <param name="partial_key"></param>
            /// <returns></returns>
            private int GetGreatestIndex(string partial_key)
            {
                try
                {
                    var filtered = Indents.Where(d => d.Key.Contains(partial_key + Separator))
                                   .ToDictionary(d => d.Key, d => d.Value);
                    int index = -1;
                    int highest_index = -1;
                    foreach (var pair in filtered)
                    {
                        //Console.WriteLine("{0} => {1}", pair.Key, pair.Value);
                        index = Convert.ToInt32(Regex.Replace(input: pair.Key, pattern: "^" + partial_key + Regex.Escape(Separator), replacement: ""));
                        if (index > highest_index)
                            highest_index = index;
                    }
                    return highest_index;
                }
                catch (Exception e)
                {
                    Console.WriteLine("SavedIndexes.GetGreatestIndex() Exception - " + e.Message);
                    return 0;
                }
            }

            public void DeleteKey(string key, int index)
            {
                string new_key = key + Separator + index.ToString();
                Indents.Remove(new_key);
            }
        }
    }

}