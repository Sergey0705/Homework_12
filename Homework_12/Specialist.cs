using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Homework_12
{
    [XmlInclude(typeof(Specialist))]
    public class Specialist : Worker
    {
        [XmlIgnore]
        static Random r;
        static Specialist() { r = new Random(); }

        public Specialist(string name, int age, int departmentId)
            : base(name, age, departmentId)
        {
            Salary = 12 * r.Next(70, 100);
        }

        public Specialist() { }
    }
}
