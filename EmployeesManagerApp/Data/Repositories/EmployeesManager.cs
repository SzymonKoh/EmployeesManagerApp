using EmployeesManagerApp.Data.Entities;
using System.Xml.Serialization;

namespace EmployeesManagerApp.Data.Repositories
{
    public class EmployeesManager<T> : IEmployeesManager<T> where T : class, IEntity
    {
        public List<T> _employees = new();

        public void DodajPracownika(T employee)
        {
            employee.Id = _employees.Count + 1;
            _employees.Add(employee);
        }

        public T PobierzPracownikaPoId(int id) => _employees.FirstOrDefault(e => e.Id == id);

        public void UsunPracownika(T employee)
        {
            if (_employees.Contains(employee))
            {
                _employees.Remove(employee);
            }
        }

        public IEnumerable<T> GetAll() => _employees.ToList();

        public void ZapiszDoPlikuXml(string nazwaPliku)
        {
            try
            {
                using (FileStream fileStream = new FileStream(nazwaPliku, FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>));
                    serializer.Serialize(fileStream, _employees);
                }
            }
            catch (Exception)
            {
                throw new Exception($"\nBłąd podczas zapisywania danych do pliku XML");
            }
        }

        public void WczytajZPlikuXml(string nazwaPliku)
        {
            if (File.Exists(nazwaPliku))
            {
                using (FileStream fileStream = new FileStream(nazwaPliku, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>));
                    _employees = (List<T>)serializer.Deserialize(fileStream);
                }
            }
            else
            {
                throw new Exception("Plik XML nie istnieje.");
            }
        }

        public void WyswietlInformacjeOPracownikach()
        {
            if (_employees.Count == 0)
            {
                Console.WriteLine("\nLista pracowników jest pusta.");
            }
            else
            {
                Console.WriteLine("\nLista pracowników:");
                foreach (var employee in _employees)
                {
                    Console.WriteLine(employee.ToString());
                }
            }
        }

        public void EdytujDanePracownika(Employee employee, string noweImie, string noweNazwisko, string noweStanowisko, DateTime nowaDataUrodzenia)
        {
            if (employee != null)
            {
                employee.Imie = noweImie;
                employee.Nazwisko = noweNazwisko;
                employee.Stanowisko = noweStanowisko;
                employee.DataUrodzenia = nowaDataUrodzenia;
            }
            else
            {
                throw new Exception("\nNieprawidłowe dane pracownika.");
            }
        }

        public void DodajKolekcje(IEnumerable<T> collection)
        {
            _employees.AddRange(collection);
        }

        public void WyczyśćListe()
        {
            _employees.Clear();
        }
    }
}
