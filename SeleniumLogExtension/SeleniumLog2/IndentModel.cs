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
        /// <summary>
        /// The following class models the indentation on SeleniumLog.exe as closely as possible.
        /// This VS Plugin has no access to the actual state of the indentation in SeleniumLog.exe,
        /// hence this function is updated just when we are about to write to the output file.
        /// </summary>
        public class IndentModel
        {
            public int PendingLevel { get { return _pending_level; } }
            public int CurrentLevel { get { return _level; } }
            public bool EmptyTree { get { return _empty_tree; } set {_empty_tree = value; }}
            private int _level { get; set; }  //root level = 0
            private int _pending_level { get; set; }
            private bool _empty_tree;

            public IndentModel() {
                _empty_tree = true;
                _level = 0;
                _pending_level = 0;
            }


            public void Indent(int delta)
            {
                if (!_empty_tree) {
                    if (delta > 0)
                    {
                        _level++;  // SeleniumLog.exe can only increase level by 1.
                    }
                }
            }

            public void Unindent(int delta)
            {
                if (!_empty_tree) {
                    if ((_level - delta) <= 0)
                    {
                        _level = 0;
                    }
                    else
                    {
                        _level = _level - delta;
                    }
                }
            }

            public void pIndent(int delta)
            {
                if (!_empty_tree)
                {
                    if (delta > 0)
                    {
                        _pending_level++;  // SeleniumLog.exe can only increase level by 1.
                    }
                }
            }

            public void pUnindent(int delta)
            {
                if (!_empty_tree)
                {
                    if ((_level - delta) <= 0)
                    {
                        _pending_level = 0;
                    }
                    else
                    {
                        //_pending_level = _level - delta;
                        _pending_level = 0 - delta;
                    }
                }
            }

            public void SimulateIndentations(string FORMATTING_STRING)
            {
                string input = "<;; ;PASS; INDENT; INDENT; PICTIRE; FILE; INDENT; INDENT; UNINDENT;UNINDENT;;>message > 100";

                string res0 = Regex.Replace(input: FORMATTING_STRING, pattern: @"\>..*$", replacement: "");
                string res1 = Regex.Replace(input: res0, pattern: @"\<", replacement: "");
                //remove all whitespaces
                string res2 = Regex.Replace(input: res1, pattern: @" +", replacement: "");
                //remove leading and trailing colons
                string res3 = Regex.Replace(input: res2, pattern: @"^\;*", replacement: "");
                string res4 = Regex.Replace(input: res3, pattern: @"\;*$", replacement: "");

                string[] formatters = res4.Split(';');
                int indent_count = 0;
                int unindent_count = 0;
                foreach (string formatter in formatters)
                {
                    if (formatter == "INDENT")
                    {
                        if (unindent_count > 0)
                        {
                            //log.Pink("Unindent " + unindent_count + " times");
                            Unindent(unindent_count);
                            unindent_count = 0;
                        }
                        indent_count++;
                    }
                    else if (formatter == "UNINDENT")
                    {
                        if (indent_count > 0)
                        {
                            //log.Pink("Indent " + indent_count + " times");
                            Indent(indent_count);
                            indent_count = 0;
                        }
                        unindent_count++;
                    }
                    else
                    {
                        // Apply indents or unindents, depending which one has accumulated value
                        if (indent_count > 0)
                        {
                            //log.Blue("Indent " + indent_count + " times");
                            Indent(indent_count);
                            indent_count = 0;
                        }
                        else if (unindent_count > 0)
                        {
                            //log.Blue("Unindent " + unindent_count + " times");
                            Unindent(unindent_count);
                            unindent_count = 0;
                        }
                    }
                }

                // Apply remaining indents or unindents, depending which one has accumulated value
                if (indent_count > 0)
                {
                    //log.WriteLine("Indent " + indent_count + " times");
                    Indent(indent_count);
                    indent_count = 0;
                }
                else if (unindent_count > 0)
                {
                    //log.WriteLine("Unindent " + unindent_count + " times");
                    Unindent(unindent_count);
                    unindent_count = 0;
                }

            } // end function


            public void CalculatePendingLevel(string FORMATTING_STRING)
            {

                string res0 = Regex.Replace(input: FORMATTING_STRING, pattern: @"\>..*$", replacement: "");
                string res1 = Regex.Replace(input: res0, pattern: @"\<", replacement: "");
                //remove all whitespaces
                string res2 = Regex.Replace(input: res1, pattern: @" +", replacement: "");
                //remove leading and trailing colons
                string res3 = Regex.Replace(input: res2, pattern: @"^\;*", replacement: "");
                string res4 = Regex.Replace(input: res3, pattern: @"\;*$", replacement: "");

                _pending_level = 0;

                string[] formatters = res4.Split(';');
                int indent_count = 0;
                int unindent_count = 0;
                foreach (string formatter in formatters)
                {
                    if (formatter == "INDENT")
                    {
                        if (unindent_count > 0)
                        {
                            //log.Pink("Unindent " + unindent_count + " times");
                            pUnindent(unindent_count);
                            unindent_count = 0;
                        }
                        indent_count++;
                    }
                    else if (formatter == "UNINDENT")
                    {
                        if (indent_count > 0)
                        {
                            //log.Pink("Indent " + indent_count + " times");
                            pIndent(indent_count);
                            indent_count = 0;
                        }
                        unindent_count++;
                    }
                    else
                    {
                        // Apply indents or unindents, depending which one has accumulated value
                        if (indent_count > 0)
                        {
                            //log.Blue("Indent " + indent_count + " times");
                            pIndent(indent_count);
                            indent_count = 0;
                        }
                        else if (unindent_count > 0)
                        {
                            //log.Blue("Unindent " + unindent_count + " times");
                            pUnindent(unindent_count);
                            unindent_count = 0;
                        }
                    }
                }

                // Apply remaining indents or unindents, depending which one has accumulated value
                if (indent_count > 0)
                {
                    //log.WriteLine("Indent " + indent_count + " times");
                    pIndent(indent_count);
                    indent_count = 0;
                }
                else if (unindent_count > 0)
                {
                    //log.WriteLine("Unindent " + unindent_count + " times");
                    pUnindent(unindent_count);
                    unindent_count = 0;
                }

            } // end function

        }

    }
}
