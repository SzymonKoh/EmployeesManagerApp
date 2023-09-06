namespace EmployeesManagerApp.Data.Entities
{
    public class Employee : EntityBase
    {
        public string? Imie { get; set; }
        public string? Nazwisko { get; set; }
        public string? Stanowisko { get; set; }
        public DateTime DataUrodzenia { get; set; }
        public override string ToString() => $"Id: {Id}, Imie: {Imie}, Nazwisko: {Nazwisko}, Stanowisko: {Stanowisko}, DataUrodzenia: {DataUrodzenia.ToShortDateString()}";
    }
}