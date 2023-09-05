using EmployeesManagerApp.Data.Entities;

namespace EmployeesManagerApp.Data.Repositories
{
    public interface IWriteRepository<in T> where T : class, IEntity
    {
        void DodajPracownika(T employee);
        void UsunPracownika(T employee);
        void DodajKolekcje(IEnumerable<T> collection);
        void WyczyśćListe();
        void ZapiszDoPlikuXml(string? nazwaPliku);
        void WczytajZPlikuXml(string? nazwaPliku);
    }
}