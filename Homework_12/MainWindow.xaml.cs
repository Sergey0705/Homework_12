using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;

namespace Homework_12
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Company company;
        Company tempCompany;

        int counterCreateCompany;
        int counterDeserialisation;

        static Random r;
        public MainWindow()
        {
            InitializeComponent();
            r = new Random();
            counterCreateCompany = 1;
            counterDeserialisation = 1;
        }
        /// <summary>
        /// Создает компанию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnCreatCompany(object sender, RoutedEventArgs e)
        {
            if (counterCreateCompany > 0 && counterDeserialisation > 0)
            {
                int countHeadDepartments = r.Next(5, 11);
                company = Company.CreateCompany(countHeadDepartments);
                treeView.ItemsSource = company.DepartmentsCompanyDb;
                --counterCreateCompany;
            }
     
            btnSalaryDirectorAndAssistant(sender, e);
        }
        /// <summary>
        /// Кнопрка сериализации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSerialization(object sender, RoutedEventArgs e)
        {
            if (counterDeserialisation > 0)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Company), new Type[] { typeof(Specialist), typeof(Intern), typeof(HeadOfDepartment) });

                string pathToXml = "data.xml";

                using (FileStream ss = new FileStream(pathToXml, FileMode.Create))
                {
                    xmlSerializer.Serialize(ss, company);
                }
            }
            else
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Company), new Type[] { typeof(Specialist), typeof(Intern), typeof(HeadOfDepartment) });

                string pathToXml = "data.xml";

                using (FileStream ss = new FileStream(pathToXml, FileMode.Create))
                {
                    xmlSerializer.Serialize(ss, tempCompany);
                }
            }
        }
        /// <summary>
        /// Диссериализует файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeserialization(object sender, RoutedEventArgs e)
        {
            if (counterDeserialisation > 0 && counterCreateCompany > 0)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Company), new Type[] { typeof(Specialist), typeof(Intern), typeof(HeadOfDepartment) });

                string pathToXml = "data.xml";

                using (FileStream fs = new FileStream(pathToXml, FileMode.Open, FileAccess.Read))
                {
                    tempCompany = xmlSerializer.Deserialize(fs) as Company;
                }

                treeView.ItemsSource = tempCompany.DepartmentsCompanyDb;
                if (counterCreateCompany > 0)
                {
                    btnSalaryDirectorAndAssistant(sender, e);
                }
                --counterDeserialisation;
                --counterCreateCompany;
            }
        }
        /// <summary>
        /// Кнопка обновления treeView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefDepartment(object sender, RoutedEventArgs e)
        {
            treeView.Items.Refresh();       
        }
        /// <summary>
        /// Кнопка обновления listView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefEmployee(object sender, RoutedEventArgs e)
        {
            listView.Items.Refresh();
        }
        /// <summary>
        /// Вызывается после выбора департамента в treeView, определяет источник данных для listView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_SelectedDepartmentChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (counterCreateCompany == 0 && counterDeserialisation == 0)
            {
                SelectedDepartmentChanged(tempCompany.DepartmentsCompanyDb);
            }
            else
            {
                SelectedDepartmentChanged(company.DepartmentsCompanyDb);
            }
        }
        /// <summary>
        /// Вспомогательный метод для treeView_SelectedDepartmentChanged(для избегания дублирования кода)
        /// </summary>
        /// <param name="companyDepartments"></param>
        private void SelectedDepartmentChanged(List<Department> companyDepartments)
        {
            List<Worker> WorkDb = new List<Worker>();
            List<Department> DepDb = new List<Department>();

            for (int i = 0; i < companyDepartments.Count; i++)
            {
                WorkDb.AddRange(companyDepartments[i].WorkersDb);
                DepDb.AddRange(companyDepartments[i].DepartmentsDb);
            }
            listView.ItemsSource = WorkDb.Where(find);

            for (int i = 0; i < DepDb.Count; i++)
            {
                WorkDb.AddRange(DepDb[i].WorkersDb);
                DepDb.AddRange(DepDb[i].DepartmentsDb);
            }
            listView.ItemsSource = WorkDb.Where(find);
        }
        /// <summary>
        /// Метод критерия поиска по DepartmentId 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool find(Worker arg)
        {
            return arg.DepartamentId == (treeView.SelectedItem as Department).DepartmentId;
        }
        /// <summary>
        /// Кпопка зарплат директора и заместителя директора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalaryDirectorAndAssistant(object sender, RoutedEventArgs e)
        {
            if (counterDeserialisation == 0 && counterCreateCompany == 0)
            {
                SalaryDirectorAndAssistant(tempCompany.DepartmentsCompanyDb);
            }
            else if (counterCreateCompany == 0)
            {
                SalaryDirectorAndAssistant(company.DepartmentsCompanyDb);
            }
        }
        /// <summary>
        /// Кнопка удаления сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteEmployee(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                if (counterDeserialisation > 0)
                {
                    Worker worker = listView.SelectedItem as Worker;
                    List<Worker> WorkDb = DeleteEmployee(company.DepartmentsCompanyDb, worker);
                    listView.ItemsSource = WorkDb.Where(find);
                }
                else
                {
                    Worker worker = listView.SelectedItem as Worker;
                    List<Worker> WorkDb = DeleteEmployee(tempCompany.DepartmentsCompanyDb, worker);
                    listView.ItemsSource = WorkDb.Where(find);
                }
            }
        }
        /// <summary>
        /// Кнопка удаления департамента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteDepartment(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem != null)
            {
                if (counterDeserialisation > 0)
                {
                    Department department = treeView.SelectedItem as Department;
                    company.DepartmentsCompanyDb = DeleteDepartment(company.DepartmentsCompanyDb, department);
                    treeView.ItemsSource = company.DepartmentsCompanyDb;
                }
                else
                {
                    Department department = treeView.SelectedItem as Department;
                    tempCompany.DepartmentsCompanyDb = DeleteDepartment(tempCompany.DepartmentsCompanyDb, department);
                    treeView.ItemsSource = tempCompany.DepartmentsCompanyDb;
                }
            }
        }
        /// <summary>
        /// Кнопка добавления работника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddEmployee(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                if (counterDeserialisation > 0)
                {
                    Worker worker = listView.SelectedItem as Worker;
                    List<Worker> WorkDb = AddEmployee(company.DepartmentsCompanyDb, worker);
                    listView.ItemsSource = WorkDb.Where(find);
                }
                else
                {
                    Worker worker = listView.SelectedItem as Worker;
                    List<Worker> WorkDb = AddEmployee(tempCompany.DepartmentsCompanyDb, worker);
                    listView.ItemsSource = WorkDb.Where(find);
                }
            }
        }
        /// <summary>
        /// Кнопка добавления департамента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDepartment(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem != null)
            {
                if (counterDeserialisation > 0)
                {
                    Department department = treeView.SelectedItem as Department;
                    company.DepartmentsCompanyDb = AddDepartment(company.DepartmentsCompanyDb, department);
                    treeView.ItemsSource = company.DepartmentsCompanyDb;
                }
                else
                {
                    Department department = treeView.SelectedItem as Department;
                    //List<Department> DepDb = new List<Department>();
                    tempCompany.DepartmentsCompanyDb = AddDepartment(tempCompany.DepartmentsCompanyDb, department);
                    treeView.ItemsSource = tempCompany.DepartmentsCompanyDb;
                }
            }
        }
        /// <summary>
        /// Удаляет работника из компании
        /// </summary>
        /// <param name="companyDepartments">Департаменты компании</param>
        /// <param name="worker">Выбранный в listView работник</param>
        /// <returns>Список работников без удаленного работника</returns>
        private List<Worker> DeleteEmployee(List<Department> companyDepartments, Worker worker)
        {
            int counter = 1;
            List<Worker> workers = new List<Worker>();
            List<Department> departments = new List<Department>();
            for (int i = 0; i < companyDepartments.Count; i++)
            {
                if (companyDepartments[i].WorkersDb.Contains(worker))
                {
                    companyDepartments[i].WorkersDb.Remove(worker);
                    --counter;
                }
                workers.AddRange(companyDepartments[i].WorkersDb);
                departments.AddRange(companyDepartments[i].DepartmentsDb);
            }
            if (counter > 0)
            {
                workers.AddRange(DeleteEmployee(departments, worker));
            }
            return workers;
        }
        /// <summary>
        /// Удаляет департамент из компании
        /// </summary>
        /// <param name="companyDepartments">Департаменты в компании</param>
        /// <param name="department">Выбранный департамент в treeView</param>
        private List<Department> DeleteDepartment(List<Department> companyDepartments, Department department)
        {
            int counter = 1;
            List<Department> departments = new List<Department>();
            if (companyDepartments.Contains(department))
            {
                companyDepartments.Remove(department);

                --counter;
            }
            departments.AddRange(companyDepartments);
            if (counter > 0)
            {
                for (int i = 0; i < companyDepartments.Count; i++)
                {
                    if (companyDepartments[i].DepartmentsDb.Contains(department))
                    {
                        companyDepartments[i].DepartmentsDb.Remove(department);
                    }
                    for (int j = 0; j < companyDepartments[i].DepartmentsDb.Count; j++)
                    {
                        if (companyDepartments[i].DepartmentsDb[j].DepartmentsDb.Contains(department))
                        {
                            companyDepartments[i].DepartmentsDb[j].DepartmentsDb.Remove(department);
                        }
                    }
                }
            }
            return departments;
        }
        /// <summary>
        /// Добавлет работника в компанию
        /// </summary>
        /// <param name="companyDepartments">Департаменты компании</param>
        /// <param name="worker">Выбранный в listView работник</param>
        /// <returns></returns>
        private List<Worker> AddEmployee(List<Department> companyDepartments, Worker worker)
        {
            int counter = 1;
            List<Worker> workers = new List<Worker>();
            List<Department> departments = new List<Department>();
            for (int i = 0; i < companyDepartments.Count; i++)
            {
                if (companyDepartments[i].WorkersDb.Contains(worker))
                {
                    companyDepartments[i].WorkersDb.Add(worker);
                    --counter;
                }
                workers.AddRange(companyDepartments[i].WorkersDb);
                departments.AddRange(companyDepartments[i].DepartmentsDb);
            }
            if (counter > 0)
            {
                workers.AddRange(AddEmployee(departments, worker));
            }
            return workers;
        }
        /// <summary>
        /// Добавляет департамент в компанию
        /// </summary>
        /// <param name="companyDepartments">Департаменты в компании</param>
        /// <param name="department">Выбранный департамент в treeView</param>
        private List<Department> AddDepartment(List<Department> companyDepartments, Department department)
        {
            int countEmployee = r.Next(8, 17);
            int countDepartment = 0;
            int counter = 1;
            List<Department> departments = new List<Department>();
            //for (int i = 0; i < companyDepartments.Count; i++)
            //{
            //    companyDepartments[i].WorkersDb.Clear();
            //}
            if (companyDepartments.Contains(department))
            {
                companyDepartments.Add(new Department(countEmployee, countDepartment));
                
                --counter;
            }

            departments.AddRange(companyDepartments);
            if (counter > 0)
            {
                for (int i = 0; i < companyDepartments.Count; i++)
                {     
                    if (companyDepartments[i].DepartmentsDb.Contains(department))
                    {                      
                        companyDepartments[i].DepartmentsDb.Add(new Department(countEmployee, countDepartment));
                        --counter;
                    }
                    for (int j = 0; j < companyDepartments[i].DepartmentsDb.Count; j++)
                    { 
                        //if (counter > 0)
                        //{
                        //    companyDepartments[i].DepartmentsDb[j].WorkersDb.Clear();
                        //}
                                          
                        if (companyDepartments[i].DepartmentsDb[j].DepartmentsDb.Contains(department))
                        {
                            companyDepartments[i].DepartmentsDb[j].DepartmentsDb.Add(new Department(countEmployee, countDepartment));
                        }
                    }
                }
            }
            return departments;
        }
        /// <summary>
        /// Вычисляет зарплаты директора и заместителя директора
        /// </summary>
        /// <param name="companyDepartments">Департаменты в компании</param>
        private void SalaryDirectorAndAssistant(List<Department> companyDepartments)
        {
            List<Worker> WorkDb = new List<Worker>();
            List<Department> DepDb = new List<Department>();
            int sumSalaries = 0;
            for (int i = 0; i < companyDepartments.Count; i++)
            {
                WorkDb.AddRange(companyDepartments[i].WorkersDb);
                DepDb.AddRange(companyDepartments[i].DepartmentsDb);
            }
            for (int i = 0; i < WorkDb.Count; i++)
            {
                sumSalaries += WorkDb[i].Salary;
            }
            WorkDb.Clear();

            for (int i = 0; i < DepDb.Count; i++)
            {
                WorkDb.AddRange(DepDb[i].WorkersDb);
            }
            for (int i = 0; i < WorkDb.Count; i++)
            {
                sumSalaries += WorkDb[i].Salary;
            }

            int assistantDirectorSalary = Convert.ToInt32(sumSalaries * 0.15);
            textBox.Text = assistantDirectorSalary.ToString();

            int directorSalary = assistantDirectorSalary + sumSalaries;
            textBox1.Text = directorSalary.ToString();
        }
        /// <summary>
        /// Кнопка сортировки сотрудников по зарплате
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSortEmployeesBySalary(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                if (counterDeserialisation > 0)
                {
                    Worker worker = listView.SelectedItem as Worker;
                    List<Worker> WorkDb = SortEmployeesBySalary(company.DepartmentsCompanyDb, worker);
                    listView.ItemsSource = WorkDb.Where(find);
                }
            }
        }
        /// <summary>
        /// Сортирует сотрудников по зарплате
        /// </summary>
        /// <param name="companyDepartments">Департаменты в компании</param>
        /// <param name="worker">Сотрудник</param>
        /// <returns></returns>
        private List<Worker> SortEmployeesBySalary(List<Department> companyDepartments, Worker worker)
        {
            int counter = 1;
            List<Worker> workers = new List<Worker>();
            List<Department> departments = new List<Department>();
            for (int i = 0; i < companyDepartments.Count; i++)
            {
                if (companyDepartments[i].WorkersDb.Contains(worker))
                {
                    companyDepartments[i].WorkersDb.Sort(new Department.SortBySalary());
                    --counter;
                }
                workers.AddRange(companyDepartments[i].WorkersDb);
                departments.AddRange(companyDepartments[i].DepartmentsDb);
            }
            if (counter > 0)
            {
                workers.AddRange(SortEmployeesBySalary(departments, worker));
            }
            return workers;
        }
        /// <summary>
        /// Кнопка сортировки сотрудников по имени
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSortEmployeesByName(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                if (counterDeserialisation > 0)
                {
                    Worker worker = listView.SelectedItem as Worker;
                    List<Worker> WorkDb = SortEmployeesByName(company.DepartmentsCompanyDb, worker);
                    listView.ItemsSource = WorkDb.Where(find);
                }
            }
        }
        /// <summary>
        /// Сортирует сотрудников по имени
        /// </summary>
        /// <param name="companyDepartments">Департаменты в компании</param>
        /// <param name="worker">Сотрудник</param>
        /// <returns></returns>
        private List<Worker> SortEmployeesByName(List<Department> companyDepartments, Worker worker)
        {
            int counter = 1;
            List<Worker> workers = new List<Worker>();
            List<Department> departments = new List<Department>();
            for (int i = 0; i < companyDepartments.Count; i++)
            {
                if (companyDepartments[i].WorkersDb.Contains(worker))
                {
                    companyDepartments[i].WorkersDb.Sort(new Department.SortByName());
                    --counter;
                }
                workers.AddRange(companyDepartments[i].WorkersDb);
                departments.AddRange(companyDepartments[i].DepartmentsDb);
            }
            if (counter > 0)
            {
                workers.AddRange(SortEmployeesByName(departments, worker));
            }
            return workers;
        }
    }
}
