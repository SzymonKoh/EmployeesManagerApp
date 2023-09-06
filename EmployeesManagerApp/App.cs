using EmployeesManagerApp.Components;
using EmployeesManagerApp.Data.Entities;
using EmployeesManagerApp.Data.Repositories;
using System.Runtime.CompilerServices;

namespace EmployeesManagerApp
{
    public class App : IApp
    {
        private readonly IEmployeesManager<Employee> _employeesRepository;
        private readonly IEmployeeProvider _employeeProvider;

        public App(IEmployeesManager<Employee> employeesRepository,
                   IEmployeeProvider employeeProvider)
        {
            _employeesRepository = employeesRepository;
            _employeeProvider = employeeProvider;
        }

        public void Run()
        {
            _employeesRepository.PracownikDodany += PracownikDodanyPomyślnie;
            _employeesRepository.PracownikUsuniety += PracownikUsunietyPomyślnie;

            static void PracownikDodanyPomyślnie(object? sender, Employee e)
            {
                Console.WriteLine($"\nPracownik dodany pomyślnie:");
                Console.WriteLine($"\n{e.ToString()}");
            }

            static void PracownikUsunietyPomyślnie(object? sender, Employee e)
            {
                Console.WriteLine($"\nPracownik usunięty pomyślnie:");
                Console.WriteLine($"\nPracownik {e.Imie} {e.Nazwisko} usunięty.");
            }

            int GenerujId()
            {
                var existingIds = _employeesRepository.GetAll().Select(e => e.Id).ToList();
                int newId = 1;
                while (existingIds.Contains(newId))
                {
                    newId++;
                }
                return newId;
            }

            bool WalidujDane(string imie, string nazwisko, string stanowisko, DateTime dataUrodzenia)
            {
                if (string.IsNullOrWhiteSpace(imie) || string.IsNullOrWhiteSpace(nazwisko) || string.IsNullOrWhiteSpace(stanowisko))
                {
                    Console.WriteLine("Pola 'Imienia', 'Nazwiska' oraz 'Stanowiska' nie mogą być puste");
                    return false;
                }

                if (!char.IsUpper(imie[0]) || !char.IsUpper(nazwisko[0]) || !char.IsUpper(stanowisko[0]))
                {
                    Console.WriteLine("Nie umieszczamy cyfr, znaków interpunkcyjnych i specjalnych w 'Imieniu', 'Nazwisku' oraz 'Stanowisku'.");
                    Console.WriteLine("'Imię', 'Nazwisko' oraz 'Stanowisko' zapisujemy od wielkiej litery.");
                    return false;
                }

                if (imie.Any(char.IsDigit) || nazwisko.Any(char.IsDigit) || stanowisko.Any(char.IsDigit))
                {
                    Console.WriteLine("Nie umieszczamy cyfr, znaków interpunkcyjnych i specjalnych w 'Imieniu', 'Nazwisku' oraz 'Stanowisku'.");
                    Console.WriteLine("'Imię', 'Nazwisko' oraz 'Stanowisko' zapisujemy od wielkiej litery.");
                    return false;
                }

                if (dataUrodzenia > DateTime.Now || dataUrodzenia <= DateTime.Now.AddYears(-100))
                {
                    Console.WriteLine("Data urodzenia jest błędna.");
                    return false;
                }

                return true;
            }

            Console.WriteLine("Witaj w aplikacji do zarządzania pracownikami!");
            while (true)
            {
                Console.WriteLine("\nWybierz opcję:");
                Console.WriteLine("1. Dodaj pracownika");
                Console.WriteLine("2. Edytuj pracownika");
                Console.WriteLine("3. Wyświetl listę pracowników");
                Console.WriteLine("4. Usuń pracownika");
                Console.WriteLine("5. Sortuj pracowników");
                Console.WriteLine("6. Zapisz do pliku XML");
                Console.WriteLine("7. Wczytaj plik XML");
                Console.WriteLine("0. Wyjdź z aplikacji, pmiętaj o zapisie danych do pliku XML");
                Console.WriteLine();

                int wybor;
                if (int.TryParse(Console.ReadLine(), out wybor))
                {
                    switch (wybor)
                    {
                        case 1:
                            Console.WriteLine("\nPodaj imię: ");
                            string imie = Console.ReadLine();
                            Console.WriteLine("Podaj nazwisko: ");
                            string nazwisko = Console.ReadLine();
                            Console.WriteLine("Podaj stanowisko: ");
                            string stanowisko = Console.ReadLine();
                            Console.WriteLine("\nPodająć datę urodzenia pamiętaj o odstępach(RRRR MM DD)");
                            Console.WriteLine("Podająć datę urodzenia pamiętaj o odstępach, użyj 'SPACJI' aby to zrobić.");
                            Console.WriteLine("\nPodaj datę urodzenia (RRRR MM DD): ");
                            DateTime.TryParse(Console.ReadLine(), out DateTime dataUrodzenia);

                            if (WalidujDane(imie, nazwisko, stanowisko, dataUrodzenia))
                            {
                                _employeesRepository.DodajPracownika(new Employee
                                {
                                    Id = GenerujId(),
                                    Imie = imie,
                                    Nazwisko = nazwisko,
                                    Stanowisko = stanowisko,
                                    DataUrodzenia = dataUrodzenia
                                });
                            }
                            else
                            {
                                Console.Write($"\nPracownik nie został dodany.");
                            }
                            break;

                        case 2:
                            Console.Write("\nPodaj 'ID' pracownika do edycji: ");
                            if (int.TryParse(Console.ReadLine(), out int id1))
                            {
                                var pracownikDoEdycji = _employeesRepository.PobierzPracownikaPoId(id1);
                                if (pracownikDoEdycji != null)
                                {
                                    Console.WriteLine("\n'Imię' pracownika musi być zawszę ustawiane na nowo.");
                                    Console.WriteLine("Podaj nowe imię: ");
                                    string noweImie = Console.ReadLine();
                                    Console.WriteLine("\n'Nazwisko' pracownika musi być zawszę ustawiane na nowo.");
                                    Console.WriteLine("Podaj nowe nazwisko: ");
                                    string noweNazwisko = Console.ReadLine();
                                    Console.WriteLine("\n'Stanowisko' pracownika musi być zawszę ustawiane na nowo.");
                                    Console.WriteLine("Podaj nowe stanowisko: ");
                                    string noweStanowisko = Console.ReadLine();
                                    Console.WriteLine("\n'Data urodzenia' pracownika musi być zawszę ustawiane na nowo.");
                                    Console.WriteLine("Przy wprowadzaniu 'daty urdodzenia' używamy tylko liczb całkowitych.");
                                    Console.WriteLine("\nPodaj nową date urodzenia (RRRR-MM-DD): ");
                                    string nowaDataUrodzeniaStr = Console.ReadLine();
                                    DateTime.TryParse(nowaDataUrodzeniaStr, out DateTime nowaDataUrodzenia);
                                    try
                                    {
                                        if (WalidujDane(noweImie, noweNazwisko, noweStanowisko, nowaDataUrodzenia))
                                        {
                                            if (pracownikDoEdycji != null)
                                            {
                                                pracownikDoEdycji.Id = id1;
                                                pracownikDoEdycji.Imie = noweImie;
                                                pracownikDoEdycji.Nazwisko = noweNazwisko;
                                                pracownikDoEdycji.Stanowisko = noweStanowisko;
                                                pracownikDoEdycji.DataUrodzenia = nowaDataUrodzenia;
                                            }
                                            else
                                            {
                                                throw new Exception("\nNieprawidłowe dane pracownika.");
                                            }
                                        }
                                        Console.WriteLine($"\nDane pracownika {pracownikDoEdycji.Imie} {pracownikDoEdycji.Nazwisko} zostały zaktualizowane.");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"\n{ex.Message}");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nNieprawidłowy format 'ID'.");
                            }
                            break;

                        case 3:
                            if (_employeesRepository != null)
                            {
                                bool isNotEmpty = false;
                                foreach (var employee in _employeesRepository.GetAll())
                                {
                                    isNotEmpty = true;
                                }

                                if (!isNotEmpty)
                                {
                                    Console.WriteLine("\nLista pracowników jest pusta.");
                                }
                                else
                                {
                                    Console.WriteLine("\nLista pracowników:");
                                    foreach (var employee in _employeesRepository.GetAll())
                                    {
                                        Console.WriteLine(employee.ToString());
                                    }
                                }
                            }
                            break;

                        case 4:
                            Console.Write("\nPodaj 'ID' pracownika do usunięcia: ");
                            if (int.TryParse(Console.ReadLine(), out int id2))
                            {
                                var pracownikDoUsuniecia = _employeesRepository.PobierzPracownikaPoId(id2);
                                if (pracownikDoUsuniecia != null)
                                {
                                    _employeesRepository.UsunPracownika(pracownikDoUsuniecia);
                                }
                                else if (pracownikDoUsuniecia == null)
                                {
                                    Console.WriteLine("\nPodany pracownik nie istnieje.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nNieprawidłowy format 'ID'.");
                            }
                            break;

                        case 5:
                            Console.WriteLine("\nWybierz rodzaj sortowania:");
                            Console.WriteLine("1. Po imieniu");
                            Console.WriteLine("2. Po nazwisku");
                            Console.WriteLine("3. Po stanowisku");
                            Console.WriteLine("4. Po Id");
                            Console.WriteLine("---------------------------");
                            Console.WriteLine("Pamiętaj że po każdym sortowaniu zmienia się kolejność pracowników.");
                            Console.WriteLine("Przed zapisem listy prcowników do pliku XML wybierz odpowiednią kolejność sortowania.");
                            Console.WriteLine("\nWybierz opcję: ");
                            string choice = Console.ReadLine();

                            switch (choice)
                            {
                                case "1":
                                    if (_employeesRepository != null)
                                    {
                                        bool isNotEmpty = false;
                                        foreach (var employee in _employeesRepository.GetAll())
                                        {
                                            isNotEmpty = true;
                                        }

                                        if (!isNotEmpty)
                                        {
                                            Console.WriteLine("\nLista pracowników jest pusta.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nPracownicy posortowani po imieniu:");
                                            foreach (var employee in _employeesRepository.GetAll()) { }
                                            foreach (var employee in _employeeProvider.SortujPracownikowWedlugImie())
                                            {
                                                Console.WriteLine(employee.ToString());
                                            }

                                            var sortedEmp = _employeeProvider.SortujPracownikowWedlugImie();
                                            _employeesRepository.WyczyśćListe();
                                            _employeesRepository.DodajKolekcje(sortedEmp);
                                        }
                                    }
                                    break;
                                case "2":
                                    if (_employeesRepository != null)
                                    {
                                        bool isNotEmpty = false;
                                        foreach (var employee in _employeesRepository.GetAll())
                                        {
                                            isNotEmpty = true;
                                        }

                                        if (!isNotEmpty)
                                        {
                                            Console.WriteLine("\nLista pracowników jest pusta.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nPracownicy posortowani po nazwisku:");
                                            foreach (var employee in _employeesRepository.GetAll()) { }
                                            foreach (var employee in _employeeProvider.SortujPracownikowWedlugNazwiska())
                                            {
                                                Console.WriteLine(employee.ToString());
                                            }

                                            var sortedEmp = _employeeProvider.SortujPracownikowWedlugNazwiska();
                                            _employeesRepository.WyczyśćListe();
                                            _employeesRepository.DodajKolekcje(sortedEmp);
                                        }
                                    }
                                    break;
                                case "3":
                                    if (_employeesRepository != null)
                                    {
                                        bool isNotEmpty = false;
                                        foreach (var employee in _employeesRepository.GetAll())
                                        {
                                            isNotEmpty = true;
                                        }

                                        if (!isNotEmpty)
                                        {
                                            Console.WriteLine("\nLista pracowników jest pusta.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nPracownicy posortowani po stanowisku:");
                                            foreach (var employee in _employeesRepository.GetAll()) { }
                                            foreach (var employee in _employeeProvider.SortujPracownikowWedlugStanowiska())
                                            {
                                                Console.WriteLine(employee.ToString());
                                            }

                                            var sortedEmp = _employeeProvider.SortujPracownikowWedlugStanowiska();
                                            _employeesRepository.WyczyśćListe();
                                            _employeesRepository.DodajKolekcje(sortedEmp);
                                        }
                                    }
                                    break;
                                case "4":
                                    if (_employeesRepository != null)
                                    {
                                        bool isNotEmpty = false;
                                        foreach (var employee in _employeesRepository.GetAll())
                                        {
                                            isNotEmpty = true;
                                        }

                                        if (!isNotEmpty)
                                        {
                                            Console.WriteLine("\nLista pracowników jest pusta.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nPracownicy posortowani po Id:");
                                            foreach (var employee in _employeesRepository.GetAll()) { }
                                            foreach (var employee in _employeeProvider.SortujPracownikowWedlugId())
                                            {
                                                Console.WriteLine(employee.ToString());
                                            }

                                            var sortedEmp = _employeeProvider.SortujPracownikowWedlugId();
                                            _employeesRepository.WyczyśćListe();
                                            _employeesRepository.DodajKolekcje(sortedEmp);
                                        }
                                    }
                                    break;
                                default:
                                    Console.WriteLine("\nNieprawidłowa opcja sortowania.");
                                    break;
                            }
                            break;

                        case 6:
                            Console.Write("\nPodaj nazwę pliku do zapisu: ");
                            string nazwaPlikuZapisu = Console.ReadLine();
                            try
                            {
                                _employeesRepository.ZapiszDoPlikuXml(nazwaPlikuZapisu);
                                Console.Write($"\nPlik {nazwaPlikuZapisu} stworzony.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;

                        case 7:
                            Console.Write("\nPodaj nazwę pliku do odczytu: ");
                            string nazwaPlikuOdczytu = Console.ReadLine();
                            try
                            {
                                _employeesRepository.WczytajZPlikuXml(nazwaPlikuOdczytu);
                                Console.Write($"\nPlik {nazwaPlikuOdczytu} odczytany.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"\nBłąd podczas wczytywania danych z pliku XML: {ex.Message}");
                            }
                            break;

                        case 0:
                            Console.WriteLine("\nZakończono aplikację.");
                            return;

                        default:
                            Console.WriteLine("\nNieznana opcja. Wybierz ponownie.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\nBłędna wartość. Wybierz ponownie.");
                }
            }
        }
    }
}