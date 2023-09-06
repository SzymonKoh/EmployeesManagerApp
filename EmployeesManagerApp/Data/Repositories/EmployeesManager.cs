using EmployeesManagerApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;

namespace EmployeesManagerApp.Data.Repositories
{
    public class EmployeesManager<T> : IEmployeesManager<T> where T : class, IEntity
    {
        public List<T> _employees = new();

        public event EventHandler<T>? PracownikDodany;
        public event EventHandler<T>? PracownikUsuniety;

        public void DodajPracownika(T employee)
        {
            _employees.Add(employee);
            PracownikDodany?.Invoke(this, employee);
        }

        public T? PobierzPracownikaPoId(int id) => _employees.Find(e => e.Id == id);

        public void UsunPracownika(T employee)
        {
            _employees.Remove(employee);
            PracownikUsuniety?.Invoke(this, employee);
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
