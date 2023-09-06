using EmployeesManagerApp.Data.Entities;
using EmployeesManagerApp.Data.Repositories;
using EmployeesManagerApp.Components;

namespace EmployeesManagerApp.Tests
{
    public class Tests
    {
        public class EmployeesManagerTests
        {
            private string tempXmlFileName;
            private List<Employee> testEmployees;

            [SetUp]
            public void Setup()
            {
                // Inicjalizacja testowych danych i pliku XML
                tempXmlFileName = Path.GetTempFileName();
                testEmployees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Imie = "Jacek",
                    Nazwisko = "Dorn",
                    Stanowisko = "Programista",
                    DataUrodzenia = new DateTime(1990, 1, 1)
                }
            };
            }

            [Test]
            public void DodajPracownika_DodajePracownika()
            {
                // Arrange
                var manager = new EmployeesManager<Employee>( );
                var employee = new Employee { Imie = "Jacek", Nazwisko = "Dor", Stanowisko = "Programista", DataUrodzenia = new DateTime(1990, 4, 1) };

                // Act
                manager.DodajPracownika(employee);

                // Assert
                Assert.AreEqual(1, manager.GetAll().Count());
                Assert.AreEqual(0, employee.Id);
            }

            [Test]
            public void DodajPracownika_PoprawnieDodanoPracownika_Wywo³ujeZdarzeniePracownikDodany()
            {
                // Arrange
                var manager = new EmployeesManager<Employee>();
                var employee = new Employee { Imie = "Jacek", Nazwisko = "Dor", Stanowisko = "Programista", DataUrodzenia = new DateTime(1990, 4, 1) };

                bool zdarzenieWywolane = false;
                manager.PracownikDodany += (sender, e) =>
                {
                    zdarzenieWywolane = true;
                };

                // Act
                manager.DodajPracownika(employee);

                // Assert
                Assert.IsTrue(zdarzenieWywolane, "Zdarzenie 'PracownikDodany' zosta³o wywo³ane.");
                CollectionAssert.Contains(manager.GetAll(), employee, "Pracownik zosta³ dodany do kolekcji.");
            }

            [Test]
            public void PobierzPracownikaPoId_PobieraPracownika()
            {
                // Arrange
                var manager = new EmployeesManager<Employee>();
                var employee = new Employee { Imie = "Jacek", Nazwisko = "Dor", Stanowisko = "Programista", DataUrodzenia = new DateTime(1990, 4, 1) };
                manager.DodajPracownika(employee);

                // Act
                var retrievedEmployee = manager.PobierzPracownikaPoId(0);

                // Assert
                Assert.AreEqual(employee, retrievedEmployee);
            }

            [Test]
            public void UsunPracownika_UsuwaPracownika()
            {
                // Arrange
                var manager = new EmployeesManager<Employee>();
                var employee = new Employee { Imie = "Jacek", Nazwisko = "Dor", Stanowisko = "Programista", DataUrodzenia = new DateTime(1990, 4, 1) };
                manager.DodajPracownika(employee);

                // Act
                manager.UsunPracownika(employee);

                // Assert
                Assert.AreEqual(0, manager.GetAll().Count());
            }

            [Test]
            public void UsunPracownika_PoprawnieUsunietoPracownika_Wywo³ujeZdarzeniePracownikUsuniety()
            {
                // Arrange
                var manager = new EmployeesManager<Employee>();
                var employee = new Employee { Imie = "Jacek", Nazwisko = "Dor", Stanowisko = "Programista", DataUrodzenia = new DateTime(1990, 4, 1) };
                manager.DodajPracownika(employee);

                bool zdarzenieWywolane = false;
                manager.PracownikUsuniety += (sender, e) =>
                {
                    zdarzenieWywolane = true;
                };

                // Act
                manager.UsunPracownika(employee);

                // Assert
                Assert.IsTrue(zdarzenieWywolane, "Zdarzenie PracownikUsuniety zosta³o wywo³ane.");
                CollectionAssert.DoesNotContain(manager.GetAll(), employee, "Pracownik zosta³ usuniêty z kolekcji.");
            }

            [Test]
            public void ZapiszDoPlikuXml_PoprawneZapisywanieDoPliku()
            {
                // Arrange
                var manager = new EmployeesManager<Employee>();
                manager.DodajKolekcje(testEmployees);
                // Act
                manager.ZapiszDoPlikuXml(tempXmlFileName);

                // Assert
                Assert.IsTrue(File.Exists(tempXmlFileName));

            }

            [Test]
            public void WczytajZPlikuXml_PoprawneWczytywaniePliku()
            {
                // Arrange
                var manager = new EmployeesManager<Employee>();
                manager.ZapiszDoPlikuXml(tempXmlFileName); 

                // Act
                manager.WczytajZPlikuXml(tempXmlFileName);

                // Assert
                Assert.DoesNotThrow(() =>
                {
                    var loadedEmployees = manager.GetAll();
                });

                // Clean up 
                File.Delete(tempXmlFileName);

            }

            [Test]
            public void WczytajZPlikuXml_PlikNieIstnieje_PowinienZwrocicException()
            {
                // Arrange
                var manager = new EmployeesManager<Employee>();
                string nieistniejacyPlik = "nieistniejacy.xml";

                // Act and Assert
                Assert.Throws<Exception>(() =>
                {
                    manager.WczytajZPlikuXml(nieistniejacyPlik);
                });
            }

            [Test]
            public void DodajKolekcje_DodajeKolekcjePracownikow()
            {
                // Arrange
                var manager = new EmployeesManager<Employee>();
                var employeesToAdd = new List<Employee>
            {
                new Employee { Imie = "Jacek", Nazwisko = "Dor", Stanowisko = "Programista", DataUrodzenia = new DateTime(1990, 4, 1) },
                new Employee { Imie = "Jan", Nazwisko = "Smok", Stanowisko = "Programista", DataUrodzenia = new DateTime(1990, 4, 2) }
            };

                // Act
                manager.DodajKolekcje(employeesToAdd);

                // Assert
                Assert.AreEqual(2, manager.GetAll().Count());
            }

            [Test]
            public void WyczyœæListe_WczytujePustaListe()
            {
                // Arrange
                var manager = new EmployeesManager<Employee>();
                var employee = new Employee { Imie = "Jacek", Nazwisko = "Dor", Stanowisko = "Programista", DataUrodzenia = new DateTime(1990, 4, 1) };
                manager.DodajPracownika(employee);

                // Act
                manager.WyczyœæListe();

                // Assert
                Assert.AreEqual(0, manager.GetAll().Count());
            }
        }
    }
}