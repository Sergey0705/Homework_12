using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Homework_12
{
    public class Worker
    {
        /// <summary>
        /// Имя работника
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Возраст работника
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// Зарплата работника
        /// </summary>
        public int Salary { get; set; }
        /// <summary>
        /// Id работника
        /// </summary>
        public static int id;
        /// <summary>
        /// Id департамента к которому сотрудник принадлежит
        /// </summary>
        public int DepartamentId { get; set; }
        /// <summary>
        /// Определение статичесткого конструктора
        /// </summary>
        static Worker()
        {
            id = 0;
        }
        /// <summary>
        /// Определение специального конструктора
        /// </summary>
        /// <param name="name"></param>
        /// <param name="age"></param>
        public Worker(string name, int age, int departmentId, int salary = 0)
        {
            id = ++id;
            Name = name + id;
            DepartamentId = departmentId;
            Age = age;
        }

        public Worker() { }

 
    }


}
