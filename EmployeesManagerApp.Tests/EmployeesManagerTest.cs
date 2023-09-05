using EmployeesManagerApp.Data.Entities;
using EmployeesManagerApp.Data.Repositories;
using System.Text;

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
                var manager = new EmployeesManager<Employee>();
                var employee = new Employee { Imie = "Jacek", Nazwisko = "Dor", Stanowisko = "Programista", DataUrodzenia = new DateTime(1990, 4, 1) };

                // Act
                manager.DodajPracownika(employee);

                // Assert
                Assert.AreEqual(1, manager.GetAll().Count());
                Assert.AreEqual(1, employee.Id);
            }

            [Test]
            public void PobierzPracownikaPoId_PobieraPracownika()
            {
                // Arrange
                var manager = new EmployeesManager<Employee>();
                var employee = new Employee { Imie = "Jacek", Nazwisko = "Dor", Stanowisko = "Programista", DataUrodzenia = new DateTime(1990, 4, 1) };
                manager.DodajPracownika(employee);

                // Act
                var retrievedEmployee = manager.PobierzPracownikaPoId(1);

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
                manager.ZapiszDoPlikuXml(tempXmlFileName); // Zapisujemy dane do pliku

                // Act
                manager.WczytajZPlikuXml(tempXmlFileName);

                // Assert
                Assert.DoesNotThrow(() =>
                {
                    var loadedEmployees = manager.GetAll();
                });

                // Clean up - usuñ plik po zakoñczeniu testu
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
            public void WyswietlInformacjeOPracownikach_ListaNieJestPusta_WyswietlaInformacje()
            {
                // Arrange
                var manager = new EmployeesManager<Employee>();
                var employee = new Employee
                {
                    Id = 1,
                    Imie = "Jan",
                    Nazwisko = "Dor",
                    Stanowisko = "Programista",
                    DataUrodzenia = new DateTime(1990, 1, 1)
                };
                manager.DodajPracownika(employee);

                var expectedOutput = $"Lista pracowników:{Environment.NewLine}Id: 1, Imie: Jan, Nazwisko: Dor, Stanowisko: Programista, DataUrodzenia: 01/01/1990{Environment.NewLine}";
                var consoleOutput = new StringBuilder();
                Console.SetOut(new StringWriter(consoleOutput));

                // Act
                manager.WyswietlInformacjeOPracownikach();

                // Assert
                Assert.AreEqual(expectedOutput, consoleOutput.ToString());
            }

            [Test]
            public void WyswietlInformacjeOPracownikach_ListaJestPusta_WyswietlaKomunikat()
            {
                // Arrange
                var manager = new EmployeesManager<Employee>();
                var expectedOutput = "Lista pracowników jest pusta." + Environment.NewLine;
                var consoleOutput = new StringBuilder();
                Console.SetOut(new StringWriter(consoleOutput));

                // Act
                manager.WyswietlInformacjeOPracownikach();

                // Assert
                Assert.AreEqual(expectedOutput, consoleOutput.ToString());
            }

            [Test]
            public void EdytujDanePracownika_EdytujeDane()
            {
                // Arrange
                var manager = new EmployeesManager<Employee>();
                var employee = new Employee { Imie = "Jacek", Nazwisko = "Dor", Stanowisko = "Programista", DataUrodzenia = new DateTime(1990, 4, 1) };
                manager.DodajPracownika(employee);

                // Act
                manager.EdytujDanePracownika(employee, "Jan", "Smok", "Architekt", new DateTime(1990, 1, 1));

                // Assert
                var editedEmployee = manager.PobierzPracownikaPoId(1);
                Assert.AreEqual("Jan", editedEmployee.Imie);
                Assert.AreEqual("Smok", editedEmployee.Nazwisko);
                Assert.AreEqual("Architekt", editedEmployee.Stanowisko);
                Assert.AreEqual(new DateTime(1990, 1, 1), editedEmployee.DataUrodzenia);
            }

            [Test]
            public void EdytujDanePracownika_ZleDanePracownika_PowinienZwrocicException()
            {
                // Arrange
                var manager = new EmployeesManager<Employee>();
                var employee = new Employee { Imie = "Jacek", Nazwisko = "Dor", Stanowisko = "Programista", DataUrodzenia = new DateTime(1990, 4, 1) };
                manager.DodajPracownika(employee);

                // Act and Assert - u¿ywamy Assert.Throws
                Assert.Throws<Exception>(() =>
                {
                    manager.EdytujDanePracownika(null, "Jan", "Smok", "Architekt", new DateTime(1990, 1, 1));
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