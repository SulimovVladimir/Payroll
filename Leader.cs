using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Payroll
{
    class Leader
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список сотрудников.csv");
        string pathLeader = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список отработанных часов руководителей.csv");
        string pathEmployee = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список отработанных часов сотрудник.csv");
        string pathFreelancer = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список отработанных часов фрилансер.csv");
        List<string> AllEmloyee = new List<string>();

        public void Start(string input)
        {
            bool flagExit=false;
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Здравствуйте, {input}!\nВаша должность: руководитель.");
            Console.WriteLine("-------------------------------------------------");
            Menu();
            while (!flagExit)
            {
                try
                {
                    Console.Write("Выберите желаемое действие: ");
                    byte marker = Convert.ToByte(Console.ReadLine());
                    switch (marker)
                    {
                        case 1: { if (AddPeople()) Console.WriteLine("----- Сотрудник добавлен -----\n"); break; }
                        case 2: { ReportAllWork(); break; }
                        case 3: { ReportWork(); break; }
                        case 4: { AddHourWork(); break; }
                        case 5: { flagExit = true; break; }
                        default: { Console.WriteLine("!!!!! Такой операции не существует !!!!!"); Menu(); break; }
                    }
                }
                catch 
                {
                    Console.WriteLine("!!!!! Неверно выбрано действие !!!!!"); Menu();
                }
            }
        }

        void Menu ()
        {
            Console.WriteLine("Что вы можете сделать в этой программе:\n(1). Добавить сотрудника\n(2). Просмотреть отчет по всем сотрудникам\n" +
                              "(3). Просмотреть  отчет по конкретному сотруднику\n(4). Добавить часы работы\n(5). Выход из программы");
        }

        bool AddPeople()
        {
            try
            {
                Console.WriteLine("-------------------------------------------------");
                Console.Write("Введите фамилию и имя нового сотрудника: ");
                string name = Console.ReadLine();
                if (FindPeople(name)) { Console.WriteLine("Данный сотрудник уже есть в списке\n"); return false; }
                Console.Write("Доступные должности:\n(1). Руководитель\n(2). Сотрудник\n(3). Фрилансер\nВыберите должность нового сотрудника: ");
                byte role = Convert.ToByte(Console.ReadLine());
                if (role < 1 && role > 3) { Console.WriteLine("Неверно выбрана должность\n"); return false; }
                StreamWriter sw = new StreamWriter(path, true, Encoding.Default);
                switch (role)
                {
                    case 1: { sw.WriteLine($"{name},Руководитель"); break; }
                    case 2: { 
                            sw.WriteLine($"{name},Сотрудник"); 
                            break; }
                    case 3: { sw.WriteLine($"{name},Фрилансер"); break; }
                }
                sw.Close();
                return true;
            }
            catch 
            {
                Console.WriteLine("Не верно введенные данные"); return false;
            }
        }

        bool FindPeople(string input)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            try
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    byte pozition = (byte)line.IndexOf(',');
                    line = line.Substring(0, pozition);
                    if (line==input) return true;
                    string[] masLine = line.Split(' ');
                    foreach (string str in masLine) if (str == input) return true;
                }
                return false;
            }
            finally { sr.Close(); }
        }

        void AddHourWork()
        {
            Console.Write("Ведите имя сотруднику, которому нужно добавить часы: ");
            string name = Console.ReadLine();
            if (FindPeople(name))
            {
                Employee employee = new Employee();
                employee.AddHourWork(name);
            }
            else Console.WriteLine("Сотрудника с таким именем нет в нашей организации");
        }

        void ReportWork()
        {
            Console.Write("Ведите имя сотруднику, по которомузапрашивается отчет: ");
            string name = Console.ReadLine();
            if (FindPeople(name))
            {
                Employee employee = new Employee();
                employee.ReportWork(name);
            }
            else Console.WriteLine("Сотрудника с таким именем нет в нашей организации");
        }

        void ReportAllWork()
        {
            ListEmployee();
            DateTime dateEnd = DateTime.Now;
            DateTime dateStart = dateEnd.AddDays(-dateEnd.Day + 1);
            int totalHour = 0;
            int totalSalary = 0;
            try
            {
                Console.Write("Введите дату начала (в формате DD.MM.YYYY): ");
                dateStart = Convert.ToDateTime(Console.ReadLine());
                Console.Write("Введите дату завершения (в формате DD.MM.YYYY): ");
                dateEnd = Convert.ToDateTime(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Не верный ввод даты");
            }
            Console.WriteLine($"Отчет за период c {dateStart} по {dateEnd}:") ;
            Console.WriteLine("----- Руководители ------");
            Report(ref totalHour, ref totalSalary, pathLeader, "Руководитель");
            Console.WriteLine("-----  Сотрудники  ------");
            Report(ref totalHour, ref totalSalary, pathEmployee, "Сотрудник");
            Console.WriteLine("-----  Фрилансеры  ------");
            Report(ref totalHour, ref totalSalary, pathFreelancer, "Фрилансер");

            Console.WriteLine($"Всего в период отработано {totalHour} часов, сумма к выплате {totalSalary} руб.");
        }

        void Report(ref int totalHour, ref int totalSalary, string specialpath, string role)
        {
            StreamReader sr = new StreamReader(specialpath, Encoding.Default);
            foreach (string name in AllEmloyee)
            {
                var hourEployee = 0;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] mas = line.Split(',');
                    if (name == mas[1])
                    {
                        hourEployee += Convert.ToInt32(mas[2]);
                    }
                }
                if (hourEployee != 0)
                {
                    switch (role)
                    {
                        case "Руководитель":
                            {
                                Console.WriteLine($"{name} отработал {hourEployee} часов и зароботал {hourEployee * 1250} руб.");
                                totalHour += hourEployee;
                                totalSalary += hourEployee * 1250;
                                break;
                            }
                        case "Сотрудник":
                            {
                                Console.WriteLine($"{name} отработал {hourEployee} часов и зароботал {hourEployee * 750} руб.");
                                totalHour += hourEployee;
                                totalSalary += hourEployee * 750;
                                break;
                            }
                        case "Фрилансер":
                            {
                                Console.WriteLine($"{name} отработал {hourEployee} часов и зароботал {hourEployee * 1000} руб.");
                                totalHour += hourEployee;
                                totalSalary += hourEployee * 1000;
                                break;
                            }
                    }   
                }
            }
            sr.Close();
        }

        void ListEmployee()
        {
            AllEmloyee.Clear();
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] mas = line.Split(' ',',');
                AllEmloyee.Add(mas[1]);
            }
        }
    }
}
