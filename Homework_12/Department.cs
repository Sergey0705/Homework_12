using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Homework_12
{
    public class Department : INotifyPropertyChanged
    {
        [XmlIgnore]
        static Random r;
        [XmlIgnore]
        static int departmentId;
        [XmlIgnore]
        static int countDepartments;
        static Department()
        {
            r = new Random();
            departmentId = 0;
            countDepartments = 0;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Id департамента
        /// </summary>
        public int DepartmentId { get; set; }
        /// <summary>
        /// Имя департамента
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// Список работников в департаменте
        /// </summary>
        public List<Worker> WorkersDb { get; set; }
        //List<Department> sDepartmentsDb;
        /// <summary>
        /// Список департаментов в департаменте
        /// </summary>
        public List<Department> DepartmentsDb { get; set; }
        //{ 
        //    get { return sDepartmentsDb; }
        //    set
        //    {
        //        sDepartmentsDb = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DepartmentsDb))); 
        //    } 
        //}
        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        public Department()
        {
            //DepartmentsDb = new List<Department>();
        }

        public Department(double CountEmployee, int CountDepartment)
        {
            countDepartments = CountDepartment;
            departmentId = ++departmentId;
            DepartmentId = departmentId;
            DepartmentName = "Департамент_" + departmentId;
            WorkersDb = new List<Worker>();
            DepartmentsDb = new List<Department>();

            int countSpecialists = Convert.ToInt32(Math.Round(CountEmployee / 2) + 1);
            int countInterns = Convert.ToInt32(CountEmployee) - countSpecialists;

            for (int i = 0; i < countSpecialists; i++)
            {
                Specialist Specialist = new Specialist("Специалист_", r.Next(22, 50), DepartmentId);
                WorkersDb.Add(Specialist);
            }

            for (int i = 0; i < countInterns; i++)
            {
                Intern Intern = new Intern("Интерн_", r.Next(18, 22), DepartmentId);
                WorkersDb.Add(Intern);
            }

            int salary = default;
            for (int i = 0; i < WorkersDb.Count; i++)
            {
                salary += WorkersDb[i].Salary;
            }

            salary = Convert.ToInt32(salary * 0.15);

            if (salary < 1300)
            {
                salary = 1300;
            }

            HeadOfDepartment HeadOfDepartment = new HeadOfDepartment("Начальник департамента_", r.Next(30, 60), DepartmentId, salary);
            WorkersDb.Add(HeadOfDepartment);

            int countEmployee = r.Next(8, 17);

            if (countDepartments > 0)
            {
                --countDepartments;
                DepartmentsDb.Add(new Department(countEmployee, countDepartments));
            }
        }
        /// <summary>
        /// Создает часть дерева департаментов
        /// </summary>
        /// <param name="CountEmployee">Количество работников</param>
        /// <param name="CountDepartment">Глубина вложенности</param>
        /// <returns></returns>
        public static Department CreatePartOfTreeDepartments(int CountEmployee, int CountDepartment)
        {
            return new Department(CountEmployee, CountDepartment);
        }
        public class SortBySalary : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                Worker X = (Worker)x;
                Worker Y = (Worker)y;

                if (X.Salary == Y.Salary) return 0;
                else if (X.Salary > Y.Salary) return 0;
                else return -1;
            }
        }

        public class SortByName : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                Worker X = (Worker)x;
                Worker Y = (Worker)y;

                return String.Compare(X.Name, Y.Name);
            }
        }

    }
}
