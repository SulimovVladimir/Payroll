using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Payroll       //класс работы с руководителями
{
    class Leader
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список сотрудников.csv");
        string pathLeader = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список отработанных часов руководителей.csv");
        string pathEmployee = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список отработанных часов сотрудник.csv");
        string pathFreelancer = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список отработанных часов фрилансер.csv");
        List<string> AllEmloyee = new List<string>();

        public void Start(string input)     //интерфейс для руководителей
        {
            bool flagExit=false;
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Здравствуйте, {input}!\nВаша должность: руководитель.");
            
            Menu();
            while (!flagExit)
            {
                try
                {
                    Console.WriteLine("-------------------------------------------------");
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

        void Menu ()    //возможные действия для руководителей
        {
            Console.WriteLine("Что вы можете сделать в этой программе:\n(1). Добавить сотрудника\n(2). Просмотреть отчет по всем сотрудникам\n" +
                              "(3). Просмотреть  отчет по конкретному сотруднику\n(4). Добавить часы работы\n(5). Выход из программы");
        }

        bool AddPeople()    // добавления сотрудника
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
                    case 2: { sw.WriteLine($"{name},Сотрудник"); break; }
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

        bool FindPeople(string input)       //поиск сотрудника в списке сотрудников
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

        void AddHourWork()      //добавление часов сотруднику
        {
            Console.Write("Ведите имя сотруднику, которому нужно добавить часы: ");
            string name = Console.ReadLine();
            if (FindPeople(name))
            {

                Work work = new Work();
                work.AddHourWork(name, ReturnRole(name));
            }
            else Console.WriteLine("Сотрудника с таким именем нет в нашей организации");
        }

        void ReportWork()       //отчет по работе по конкретному сотруднику
        {
            Console.Write("Ведите имя сотруднику, по которому запрашивается отчет: ");
            string name = Console.ReadLine();
            if (FindPeople(name))
            {
                Work work = new Work();
                work.ReportWork(name, ReturnRole(name));
            }
            else Console.WriteLine("Сотрудника с таким именем нет в нашей организации");
        }

        void ReportAllWork()        //отчет по работе по всем сотрудникам
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
                Console.WriteLine("Не верный ввод даты, для вывода будет использован текущий месяц");
            }
            Console.WriteLine($"Отчет за период c {dateStart.ToShortDateString()} по {dateEnd.ToShortDateString()}:") ;
            Console.WriteLine("----- Руководители ------");
            Report(ref totalHour, ref totalSalary, pathLeader, "Руководитель");
            Console.WriteLine("-----  Сотрудники  ------");
            Report(ref totalHour, ref totalSalary, pathEmployee, "Сотрудник");
            Console.WriteLine("-----  Фрилансеры  ------");
            Report(ref totalHour, ref totalSalary, pathFreelancer, "Фрилансер");

            Console.WriteLine($"Всего в период отработано {totalHour} часов, сумма к выплате {totalSalary} руб.");
        }

        void Report(ref int totalHour, ref int totalSalary, string specialpath, string role)        //вспомогательный метод для формирования отчета по всем сотрудникам
        {
            foreach (string name in AllEmloyee)
            {
                StreamReader sr = new StreamReader(specialpath, Encoding.Default);
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
                sr.Close();
            }
        }

        void ListEmployee()     //формирование списка сотрудников
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

        string ReturnRole(string name)      //метод определения должности по имени сотрудника
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] mas = line.Split(' ',',');
                if (name == mas[1]) return mas[2];
            }
            return null;
        }       
    }
}
