using EmployeesManagerApp.Data.Entities;

namespace EmployeesManagerApp.Components
{
    public interface IEmployeeProvider
    {
        List<Employee> SortujPracownikowWedlugId();
        List<Employee> SortujPracownikowWedlugImie();
        List<Employee> SortujPracownikowWedlugNazwiska();
        List<Employee> SortujPracownikowWedlugStanowiska();
    }
}
