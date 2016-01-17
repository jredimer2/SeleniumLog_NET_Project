using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumLogger;

namespace ExampleExplore
{
    public class Sales {
        public string description {get;set;}
        public float amount {get;set;}
        public Sales(string _desc, float _amt) {
            description = _desc;
            amount = _amt;
        }
    }
    public class SalesPerson
    {
        public string Name {get;set;}
        public DateTime DOB {get;set;}
        public List<Sales> SalesList = new List<Sales>();
        public SalesPerson(string name, DateTime birthdate)
        {
            Name = name;
            DOB = birthdate;
        }
    }
    public class SalesGroup
    {
        public Collection<SalesPerson> SalesPersons = new Collection<SalesPerson>();
        public string name { get; set; }
        public SalesGroup(string _name) { name = _name; }
    }




    public class ExampleExploreTest
    {

        public void Test1()
        {
            int count = 0;
            List<SalesGroup> SalesDepartment = new List<SalesGroup>();

            SalesGroup UsedCarSalesGroup = new SalesGroup("Used Cars Group");

            UsedCarSalesGroup.SalesPersons.Add(new SalesPerson("Peter Anderson", Convert.ToDateTime("10/05/1980")));
            count = UsedCarSalesGroup.SalesPersons.Count;
            UsedCarSalesGroup.SalesPersons[count - 1].SalesList.Add(new Sales("Toyota Camry 2005", 8000));
            UsedCarSalesGroup.SalesPersons[count - 1].SalesList.Add(new Sales("Toyota Camry 2006", 8500));
            UsedCarSalesGroup.SalesPersons[count - 1].SalesList.Add(new Sales("Toyota Corolla 2007", 8000));
            UsedCarSalesGroup.SalesPersons[count - 1].SalesList.Add(new Sales("Toyota Camry 2008", 9000));

            UsedCarSalesGroup.SalesPersons.Add(new SalesPerson("Timothy Williams", Convert.ToDateTime("10/05/1980")));
            count = UsedCarSalesGroup.SalesPersons.Count;
            UsedCarSalesGroup.SalesPersons[count - 1].SalesList.Add(new Sales("VW Polo 2005", 8000));
            UsedCarSalesGroup.SalesPersons[count - 1].SalesList.Add(new Sales("BMW 5 series 2006", 17000));
            UsedCarSalesGroup.SalesPersons[count - 1].SalesList.Add(new Sales("Subaru Impreza 2005", 9000));

            SeleniumLog log = SeleniumLog.Instance();

            log.Explore(UsedCarSalesGroup);
        }
    }
}

