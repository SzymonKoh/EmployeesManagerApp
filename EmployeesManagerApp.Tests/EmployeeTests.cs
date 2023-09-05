using EmployeesManagerApp.Data.Entities;
  
namespace EmployeesManagerApp.Tests
{
    public class EmployeeTests
    {
        [Test]
        public void ToString_PrawidloweFormatowanie_ZwrociPoprawnyNapis()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                Imie = "Jan",
                Nazwisko = "Dor",
                Stanowisko = "Programista",
                DataUrodzenia = new DateTime(1990, 1, 1)
            };

            // Act
            string result = employee.ToString();

            // Assert
            string expected = "Id: 1, Imie: Jan, Nazwisko: Dor, Stanowisko: Programista, DataUrodzenia: 01/01/1990";
            Assert.AreEqual(expected, result);
        }
    }
}
