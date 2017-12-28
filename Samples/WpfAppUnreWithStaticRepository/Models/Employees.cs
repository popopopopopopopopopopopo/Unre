using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppUnreWithStaticRepository.Models
{
    public class Employee
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        public int UserId { get; set; } = -1;

        public string Name { get; set; } = "hogemoge";
        public string Position { get; set; } = "hoge position";
        public string Department { get; set; } = "moge department";
        public override string ToString()
        {
            return Name;
        }

        public static ObservableCollection<Employee> GetSamples()
        {
            ObservableCollection<Employee> stuff = new ObservableCollection<Employee>();
            stuff.Add(new Employee() { Name = "Gregory S. Price", Department = "", Position = "President" });
            stuff.Add(new Employee() { Name = "Irma R. Marshall", Department = "Marketing", Position = "Vice President" });
            stuff.Add(new Employee() { Name = "John C. Powell", Department = "Operations", Position = "Vice President" });
            stuff.Add(new Employee() { Name = "Christian P. Laclair", Department = "Production", Position = "Vice President" });
            stuff.Add(new Employee() { Name = "Karen J. Kelly", Department = "Finance", Position = "Vice President" });

            stuff.Add(new Employee() { Name = "Brian C. Cowling", Department = "Marketing", Position = "Manager" });
            stuff.Add(new Employee() { Name = "Thomas C. Dawson", Department = "Marketing", Position = "Manager" });
            stuff.Add(new Employee() { Name = "Angel M. Wilson", Department = "Marketing", Position = "Manager" });
            stuff.Add(new Employee() { Name = "Bryan R. Henderson", Department = "Marketing", Position = "Manager" });

            stuff.Add(new Employee() { Name = "Harold S. Brandes", Department = "Operations", Position = "Manager" });
            stuff.Add(new Employee() { Name = "Michael S. Blevins", Department = "Operations", Position = "Manager" });
            stuff.Add(new Employee() { Name = "Jan K. Sisk", Department = "Operations", Position = "Manager" });
            stuff.Add(new Employee() { Name = "Sidney L. Holder", Department = "Operations", Position = "Manager" });

            stuff.Add(new Employee() { Name = "James L. Kelsey", Department = "Production", Position = "Manager" });
            stuff.Add(new Employee() { Name = "Howard M. Carpenter", Department = "Production", Position = "Manager" });
            stuff.Add(new Employee() { Name = "Jennifer T. Tapia", Department = "Production", Position = "Manager" });

            stuff.Add(new Employee() { Name = "Judith P. Underhill", Department = "Finance", Position = "Manager" });
            stuff.Add(new Employee() { Name = "Russell E. Belton", Department = "Finance", Position = "Manager" });
            return stuff;
        }
    }
}
