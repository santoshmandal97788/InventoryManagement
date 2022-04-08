using UI.Models;

namespace UI.Services
{
    public interface IPersonRepository
    {
        Person GetPerson(int Id);
        IEnumerable<Person> GetAllPersons();
        Person PersonIns(Person person);
        Person PersonUpd(Person person);
        Person PersonDel(int id);
    }
}
