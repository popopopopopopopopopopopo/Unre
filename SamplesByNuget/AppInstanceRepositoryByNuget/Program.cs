using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unre;

namespace AppInstanceRepositoryByNuget
{
    class Program
    {
        static void Main(string[] args)
        {
            var person = new Person();
            var repo = new UnreRepository<Person>();

            Console.WriteLine(person.Id.ToString());
            repo.Do(person);
            repo.Do(new Person());
            var undoPerson = repo.Undo();
            Console.WriteLine(undoPerson.Id.ToString());

            var isEqual = undoPerson.Equals(person);
            Console.WriteLine(isEqual.ToString());

            Console.ReadKey();
        }
    }

    public class Person
    {
        public int? Age { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }
}
