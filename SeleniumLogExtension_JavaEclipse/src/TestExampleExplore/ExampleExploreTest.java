
package TestExampleExplore;

import SeleniumLogger.SeleniumLog;
import java.text.ParseException;
import java.text.SimpleDateFormat;
//import java.util.ArrayList;

public class ExampleExploreTest {
    
    public static void main(String[] args) throws ParseException {
        int count;
        //ArrayList<SalesGroup> SalesDepartment = new ArrayList<>();

        SalesGroup UsedCarSalesGroup = new SalesGroup("Used Cars Group");

        SimpleDateFormat format = new SimpleDateFormat("dd/MM/yyyy");
        UsedCarSalesGroup.SalesPersons.add(new SalesPerson("Peter Anderson", format.parse("10/05/1980")));
        count = UsedCarSalesGroup.SalesPersons.size();
        UsedCarSalesGroup.SalesPersons.get(count - 1).SalesList.add(new Sales("Toyota Camry 2005", 8000));
        UsedCarSalesGroup.SalesPersons.get(count - 1).SalesList.add(new Sales("Toyota Camry 2006", 8500));
        UsedCarSalesGroup.SalesPersons.get(count - 1).SalesList.add(new Sales("Toyota Corolla 2007", 8000));
        UsedCarSalesGroup.SalesPersons.get(count - 1).SalesList.add(new Sales("Toyota Camry 2008", 9000));

        UsedCarSalesGroup.SalesPersons.add(new SalesPerson("Timothy Williams", format.parse("10/05/1980")));
        count = UsedCarSalesGroup.SalesPersons.size();
        UsedCarSalesGroup.SalesPersons.get(count - 1).SalesList.add(new Sales("VW Polo 2005", 8000));
        UsedCarSalesGroup.SalesPersons.get(count - 1).SalesList.add(new Sales("BMW 5 series 2006", 17000));
        UsedCarSalesGroup.SalesPersons.get(count - 1).SalesList.add(new Sales("Subaru Impreza 2005", 9000));

        SeleniumLog log = SeleniumLog.Instance();

        log.Explore(UsedCarSalesGroup);
    }
}
