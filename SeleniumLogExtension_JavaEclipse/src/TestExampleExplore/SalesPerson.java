
package TestExampleExplore;

import java.util.ArrayList;
import java.util.Date;

public class SalesPerson
{
    public String Name;
    public Date DOB;
    public ArrayList<Sales> SalesList = new ArrayList<>();
    public SalesPerson(String name, Date birthdate)
    {
        Name = name;
        DOB = birthdate;
    }
}
