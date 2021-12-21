using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Homework_12
{
    [XmlInclude(typeof(Intern))]
    public class Intern : Worker
    {
        public Intern(string name, int age, int departmentId)
            : base(name, age, departmentId)
        {
            Salary = 500;
        }

        public Intern() { }
    }
}
