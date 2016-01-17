using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumLogger;
using System.Collections;

namespace TestHarness
{
    public class elem
    {
        int x;
        int y;
        string name;
        public elem(int x, int y, string name)
        {
            this.x = x;
            this.y = y;
            this.name = name;
        }
    }

    public class TestDataStructure
    {
        public int ID = 0;
        public string StringProperty;
        public decimal DecimalProperty;
        public Int32 IntProperty;
    }

    public class Person
    {
        string Name;
        string DOB;
        public Person(string name, string birthdate)
        {
            Name = name;
            DOB = birthdate;
        }
    }

    public class TestDataStructureCollection : KeyedCollection<int, TestDataStructure>
    {
        protected override int GetKeyForItem(TestDataStructure item)
        {
            return item.ID;
        }
    }




    public class TestClass : BaseClass
    {
        public TestClass() { }

        [SeleniumLogTrace]
        public void Method1(int A, int B, object ds)
        {
            SeleniumLog log = SeleniumLog.Instance();
            log.WriteLine("Hello World");
        }
    }


    public class datastruct2
    {
        //public SortedList<int, Person> personsSortedList = new SortedList<int, Person>();

        public Dictionary<string, string> dictionary = new Dictionary<string, string>();
        public ConcurrentDictionary<string, string> concurrentDictionary = new ConcurrentDictionary<string, string>();
        public SortedList<int, string> sortedList = new SortedList<int, string>();
        public SortedList<int, Person> personsSortedList = new SortedList<int, Person>();
        public KeyedCollection<int, TestDataStructure> keyedCollection = new TestDataStructureCollection();
        public ConcurrentDictionary<string, object> concurrentDictionary2 = new ConcurrentDictionary<string, object>();
        public ConcurrentDictionary<string, object> concurrentDictionary3 = new ConcurrentDictionary<string, object>();
        public SortedList<int, ConcurrentDictionary<string, object>> sortedList2 = new SortedList<int, ConcurrentDictionary<string, object>>();
        public List<string> list1 = new List<string>();
        public List<object> list2 = new List<object>();
        public List<Hashtable> list3 = new List<System.Collections.Hashtable>();


        public datastruct2()
        {

            //personsSortedList.Add(1, new Person("James Scott", "10/10/1977"));
            //personsSortedList.Add(5, new Person("Andrew Simon", "10/08/1976"));



            dictionary.Add("1", "ONE");
            dictionary.Add("2", "TWO");
            dictionary.Add("3", "THREE");

            concurrentDictionary.TryAdd("4", "FOUR");
            concurrentDictionary.TryAdd("5", "FIVE");
            concurrentDictionary.TryAdd("6", "SIX");
            sortedList.Add(7, "SEVEN");
            sortedList.Add(8, "EIGHT");
            sortedList.Add(9, "NINE");
            personsSortedList.Add(1, new Person("James Scott", "10/10/1977"));
            personsSortedList.Add(5, new Person("Andrew Simon", "10/08/1976"));
            keyedCollection.Add(new TestDataStructure() { ID = 10 });
            keyedCollection.Add(new TestDataStructure() { ID = 11 });
            keyedCollection.Add(new TestDataStructure() { ID = 12 });

            ConcurrentDictionary<string, object> _concurrentDictionary = new ConcurrentDictionary<string, object>();
            _concurrentDictionary.TryAdd("20", "Twenty");
            _concurrentDictionary.TryAdd("21", "Twenty one");
            _concurrentDictionary.TryAdd("22", "Twenty two");
            List<string> _lista = new List<string>();
            List<object> _listb = new List<object>();
            _lista.Add("_hello1");
            _lista.Add("_hello2");
            _listb.Add(_concurrentDictionary);
            list2.Add(keyedCollection);


            concurrentDictionary2.TryAdd("7", _lista);
            concurrentDictionary2.TryAdd("8", _listb);
            concurrentDictionary2.TryAdd("9", "value100");


            concurrentDictionary3.TryAdd("10", "TEN");
            concurrentDictionary3.TryAdd("11", "ELEVEN");
            concurrentDictionary3.TryAdd("12", "TWELVE");

            sortedList2.Add(2, concurrentDictionary2);
            sortedList2.Add(3, concurrentDictionary3);

            Hashtable _hash1 = new Hashtable();
            _hash1.Add("key1", "val1");
            _hash1.Add("key2", "val2");
            _hash1.Add("key3", "val3");

            Hashtable _hash2 = new Hashtable();
            _hash2.Add("key4", 400);
            _hash2.Add("key5", 500);
            _hash2.Add("key6", 600);

            list3.Add(_hash1);
            list3.Add(_hash2);
        }
    }
}
