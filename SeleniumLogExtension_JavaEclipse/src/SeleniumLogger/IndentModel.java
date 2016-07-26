/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package SeleniumLogger;

/// <summary>

//import java.util.regex.Pattern;

/// The following class models the indentation on SeleniumLog.exe as closely as possible.
/// This VS Plugin has no access to the actual state of the indentation in SeleniumLog.exe,
/// hence this function is updated just when we are about to write to the output file.
/// </summary>
public class IndentModel
{
    public int getPendingDelta() { return _pending_delta; } 
    public int getCurrentLevel() { return _level; }
    public void setCurrentLevel(int value) { _level = (value >= 0) ? value : 0; }
    public boolean getEmptyTree() { return _empty_tree; }
    public void setEmptyTree(boolean value) { _empty_tree = value; }

    private int _level;  //root level = 0
    private int _pending_delta;
    private boolean _empty_tree;

    public IndentModel() {
        _empty_tree = true;
        _level = 0;
        _pending_delta = 0;
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
                _pending_delta++;  // SeleniumLog.exe can only increase level by 1.
            }
        }
    }

    public void pUnindent(int delta)
    {
        if (!_empty_tree)
        {
            if ((_level - delta) <= 0)
            {
                //_pending_delta = 0;
                _pending_delta = 0 - _level;
            }
            else
            {
                //_pending_delta = _level - delta;
                _pending_delta = 0 - delta;
            }
        }
    }

    public void SimulateIndentations(String FORMATTING_STRING)
    {
        String input = "<;; ;PASS; INDENT; INDENT; PICTIRE; FILE; INDENT; INDENT; UNINDENT;UNINDENT;;>message > 100";

        String res0 = FORMATTING_STRING.replaceAll("\\>..*$", "");
        String res1 = res0.replaceAll("\\<", "");
        //remove all whitespaces
        String res2 = res1.replaceAll(" +", "");
        //remove leading and trailing colons
        String res3 = res2.replaceAll("^\\;*", "");
        String res4 = res3.replaceAll("\\;*$", "");

        String[] formatters = res4.split(";");
        int indent_count = 0;
        int unindent_count = 0;
        for (String formatter : formatters)
        {
            if (formatter.equals("INDENT"))
            {
                if (unindent_count > 0)
                {
                    //log.Pink("Unindent " + unindent_count + " times");
                    Unindent(unindent_count);
                    unindent_count = 0;
                }
                indent_count++;
            }
            else if (formatter.equals("UNINDENT"))
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


    public void CalculatePendingLevel(String FORMATTING_STRING)
    {

        String res0 = FORMATTING_STRING.replaceAll("\\>..*$", "");
        String res1 = res0.replaceAll("\\<", "");
        String res2 = res1.replaceAll(" +", "");
        String res3 = res2.replaceAll("^\\;*", "");
        String res4 = res3.replaceAll("\\;*$", "");

        _pending_delta = 0;

        String[] formatters = res4.split(";");
        int indent_count = 0;
        int unindent_count = 0;
        for (String formatter : formatters)
        {
            if (formatter.equals("INDENT"))
            {
                if (unindent_count > 0)
                {
                    //log.Pink("Unindent " + unindent_count + " times");
                    pUnindent(unindent_count);
                    unindent_count = 0;
                }
                indent_count++;
            }
            else if (formatter.equals("UNINDENT"))
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