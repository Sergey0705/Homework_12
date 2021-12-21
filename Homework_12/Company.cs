using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Homework_12
{
    [Serializable]
    [XmlRoot]
    public class Company : INotifyPropertyChanged
    {
        static Random r;

        static Company()
        {
            r = new Random();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    
        //public List<Department> sDepartmentsCompanyDb;
        /// <summary>
        /// Список департаментов в компании
        /// </summary>
        public List<Department> DepartmentsCompanyDb { get; set; }
        //{ 
        //    get { return sDepartmentsCompanyDb; }
        //    set
        //    {
        //        sDepartmentsCompanyDb = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DepartmentsCompanyDb))); 
        //    } 
        //}
        /// <summary>
        /// Количество головных департаментов
        /// </summary>
        public int CountHeadDepartments { get; set; }

        public Company(int countHeadDepartments)
        {

            //sDepartmentsCompanyDb = new List<Department>();
            DepartmentsCompanyDb = new List<Department>();
            CountHeadDepartments = countHeadDepartments;
            int countEmployee;
            int countDepartment;
            for (int i = 0; i < countHeadDepartments; i++)
            {
                countEmployee = r.Next(8, 17);
                countDepartment = r.Next(0, 3);

                Department departments = Department.CreatePartOfTreeDepartments(countEmployee, countDepartment);

                DepartmentsCompanyDb.Add(departments);
                //this.DepartmentsCompanyDb.AddRange(departments.DepartmentsDb);
            }
        }

        public Company() { }
        /// <summary>
        /// Создает компанию
        /// </summary>
        /// <param name="countHeadDepartments"></param>
        /// <returns>Возвращает созданную компанию</returns>
        public static Company CreateCompany(int countHeadDepartments)
        {
            return new Company(countHeadDepartments);
        }

        //public List<Department> AddDepartment(Department department)
        //{
        //    int countEmployee = r.Next(8, 17);
        //    int countDepartment = 0;
        //    int counter = 1;
            //List<Department> departments = new List<Department>();
            //if (this.DepartmentsCompanyDb.Contains(department))
            //{
            //    this.DepartmentsCompanyDb.Add(new Department(countEmployee, countDepartment));   
            //    --counter;
            //}
            //departments.AddRange(companyDepartments);
            //if (counter > 0)
            //{
            //    for (int i = 0; i < this.DepartmentsCompanyDb.Count; i++)
            //    {
            //        if (this.DepartmentsCompanyDb[i].DepartmentsDb.Contains(department))
            //        {
            //            this.DepartmentsCompanyDb[i].DepartmentsDb.Add(new Department(countEmployee, countDepartment));
            //            --counter;
            //        }
            //        for (int j = 0; j < this.DepartmentsCompanyDb[i].DepartmentsDb.Count; j++)
            //        {
            //            if (this.DepartmentsCompanyDb[i].DepartmentsDb[j].DepartmentsDb.Contains(department))
            //            {
            //                this.DepartmentsCompanyDb[i].DepartmentsDb[j].DepartmentsDb.Add(new Department(countEmployee, countDepartment));
            //            }
            //        }
            //    }
             
                //if (counter > 0)
                //{
                //    for (int i = 0; i < companyDepartments.Count; i++)
                //    {
                //        departments.AddRange(companyDepartments[i].DepartmentsDb);
                //    }
                //}
            //}
            //return this.DepartmentsCompanyDb;
            //if (counter > 0)
            //{
            //    AddDepartment(departments, department);
            //}
            //return departments;
        //}
    }
}
