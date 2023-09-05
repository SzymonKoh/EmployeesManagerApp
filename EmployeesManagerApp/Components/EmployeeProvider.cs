using EmployeesManagerApp.Data.Entities;
using EmployeesManagerApp.Data.Repositories;

namespace EmployeesManagerApp.Components
{
    public class EmployeeProvider : IEmployeeProvider
    {
        public IEmployeesManager<Employee> _employeeProvider;

        public EmployeeProvider(IEmployeesManager<Employee> employeesProvider)
        {
            _employeeProvider = employeesProvider;
        }

        public List<Employee> SortujPracownikowWedlugId()
        {
            var employees = _employeeProvider.GetAll();
            return employees.OrderBy(e => e.Id).ToList();
        }

        public List<Employee> SortujPracownikowWedlugImie()
        {
            var employees = _employeeProvider.GetAll();
            return employees
             .OrderBy(e => e.Imie)
             .ThenBy(e => e.Id)
             .ToList();
        }

        public List<Employee> SortujPracownikowWedlugNazwiska()
        {
            var employees = _employeeProvider.GetAll();
            return employees
            .OrderBy(e => e.Nazwisko)
            .ThenBy(e => e.Id)
            .ToList();
        }

        public List<Employee> SortujPracownikowWedlugStanowiska()
        {
            var employees = _employeeProvider.GetAll();
            return employees
            .OrderBy(e => e.Stanowisko)
            .ThenBy(e => e.Id)
            .ToList();
        }
    }
}
