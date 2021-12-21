using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace Homework_12
{
    [XmlInclude(typeof(HeadOfDepartment))]
    public class HeadOfDepartment : Worker
    {
        public HeadOfDepartment(string name, int age, int departmentId, int salary)
            : base(name, age, departmentId)
        {
            Salary = salary;
        }
        public HeadOfDepartment() { }
    }
}
