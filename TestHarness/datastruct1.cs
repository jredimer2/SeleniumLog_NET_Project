using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHarness
{
    public class datastruct1
    {
        public DateTime DTIME1 = Convert.ToDateTime("11/10/1980");
        public DateTime DTIME2 = Convert.ToDateTime("5/10/1980");
        public List<int> integers = new List<int>();
        public List<string> strings = new List<string>();
        public List<elem> elems = new List<elem>();

        public datastruct1()
        {

            integers.Add(100);
            integers.Add(101);
            integers.Add(102);
            strings.Add("str1");
            strings.Add("str2");
            strings.Add("str3");

            elems.Add(new elem(5, 6, "fivesix"));
            elems.Add(new elem(7, 8, null));
            elems.Add(null);
            elems.Add(new elem(9, 10, "nineten"));
            elems.Add(null);
        }
    }
}
