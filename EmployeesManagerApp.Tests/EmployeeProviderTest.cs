using EmployeesManagerApp.Components;
using EmployeesManagerApp.Data.Entities;
using EmployeesManagerApp.Data.Repositories;
using Moq;

namespace EmployeesManagerApp.Tests
{
    public class EmployeeProviderTest
    {
        [Test]
        public void SortujPracownikowWedlugId_SortowaniePoId_ZwrociPosortowanaListe()
        {
            // Arrange
            var mockEmployeesProvider = new Mock<IEmployeesManager<Employee>>();
            var employeeProvider = new EmployeeProvider(mockEmployeesProvider.Object);
            var employees = new List<Employee>
            {
                new Employee { Id = 3 },
                new Employee { Id = 1 },
                new Employee { Id = 2 }
            };
            mockEmployeesProvider.Setup(m => m.GetAll()).Returns(employees);

            // Act
            var sortedEmployees = employeeProvider.SortujPracownikowWedlugId();

            // Assert
            CollectionAssert.AreEqual(new List<int> { 1, 2, 3 }, sortedEmployees.Select(e => e.Id).ToList());
        }

        [Test]
        public void SortujPracownikowWedlugImie_SortowaniePoImieniu_ZwrociPosortowanaListe()
        {
            // Arrange
            var mockEmployeesProvider = new Mock<IEmployeesManager<Employee>>();
            var employeeProvider = new EmployeeProvider(mockEmployeesProvider.Object);
            var employees = new List<Employee>
            {
                new Employee { Imie = "Jan" },
                new Employee { Imie = "Aleksander" },
                new Employee { Imie = "Bartek" }
            };
            mockEmployeesProvider.Setup(m => m.GetAll()).Returns(employees);

            // Act
            var sortedEmployees = employeeProvider.SortujPracownikowWedlugImie();

            // Assert
            CollectionAssert.AreEqual(new List<string> { "Aleksander", "Bartek", "Jan" }, sortedEmployees.Select(e => e.Imie).ToList());
        }

        [Test]
        public void SortujPracownikowWedlugNazwisk_SortowaniePoNazwiskach_ZwrociPosortowanaListe()
        {
            // Arrange
            var mockEmployeesProvider = new Mock<IEmployeesManager<Employee>>();
            var employeeProvider = new EmployeeProvider(mockEmployeesProvider.Object);
            var employees = new List<Employee>
            {
                new Employee { Nazwisko = "Dorn" },
                new Employee { Nazwisko = "Kowalski" },
                new Employee { Nazwisko = "Murawski" }
            };
            mockEmployeesProvider.Setup(m => m.GetAll()).Returns(employees);

            // Act
            var sortedEmployees = employeeProvider.SortujPracownikowWedlugNazwiska();

            // Assert
            CollectionAssert.AreEqual(new List<string> { "Dorn", "Kowalski", "Murawski" }, sortedEmployees.Select(e => e.Nazwisko).ToList());
        }

        [Test]
        public void SortujPracownikowWedlugStanowiska_SortowaniePoStanowisku_ZwrociPosortowanaListe()
        {
            // Arrange
            var mockEmployeesProvider = new Mock<IEmployeesManager<Employee>>();
            var employeeProvider = new EmployeeProvider(mockEmployeesProvider.Object);
            var employees = new List<Employee>
            {
                new Employee { Stanowisko = "Programista" },
                new Employee { Stanowisko = "Architekt" },
                new Employee { Stanowisko = "Manager" }
            };
            mockEmployeesProvider.Setup(m => m.GetAll()).Returns(employees);

            // Act
            var sortedEmployees = employeeProvider.SortujPracownikowWedlugStanowiska();

            // Assert
            CollectionAssert.AreEqual(new List<string> { "Architekt", "Manager", "Programista" }, sortedEmployees.Select(e => e.Stanowisko).ToList());
        }
    }
}
