using UI.Data;
using UI.Models;

namespace UI.Services
{
    public class MockPersonRepository : IPersonRepository
    {
        private readonly AppDbContext _appDbContext;

        public MockPersonRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<Person> GetAllPersons()
        {
            var personList = _appDbContext.Persons.ToList();
            return personList;
        }

        public Person GetPerson(int Id)
        {
            return _appDbContext.Persons.Where(p => p.PersonId == Id).FirstOrDefault();

        }

        public Person PersonDel(int id)
        {
            Person personToDelete = GetPerson(id);
            if (personToDelete != null)
            {
                _appDbContext.Persons.Remove(personToDelete);
                _appDbContext.SaveChanges();
            }
            return personToDelete;
        }

        public Person PersonIns(Person person)
        {
            _appDbContext.Persons.Add(person);
            _appDbContext.SaveChanges();
            return person;
        }

        public Person PersonUpd(Person person)
        {
            return person;
            //Person personToUpdate = GetPerson(person.PersonId);
            //if (personToUpdate != null)
            //{
            //    listItemCategoryToUpdate.ListItemCategoryName = listItemCategory.ListItemCategoryName;
            //    _appDbContext.ListItemCategories.Update(listItemCategoryToUpdate);
            //    _appDbContext.SaveChanges();
            //}
            //return listItemCategoryToUpdate;
        }
    }
}
