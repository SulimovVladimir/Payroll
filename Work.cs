using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Payroll
{
    class Work
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список сотрудников.csv");
        string pathLeader = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список отработанных часов руководителей.csv");
        string pathEmployee = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список отработанных часов сотрудник.csv");
        string pathFreelancer = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список отработанных часов фрилансер.csv");

        const int salaryLeader = 200000;
        const int salaryEmployee = 120000;
        const int salaryFreelancer = 1000;

        public void AddHourWork(string input, string role)
        {
            DateTime date = DateTime.Now; ;
            byte hour = 0;
            string work = null;
            try
            {
                Console.Write("Введите дату (в формате DD.MM.YYYY): ");
                date = Convert.ToDateTime(Console.ReadLine());
                if (!GoodDate(date, role)) throw new Exception();
                Console.Write("Сколько часов работал {0}: ", input);
                hour = Convert.ToByte(Console.ReadLine());
                Console.Write("Что именно сделал {0}: ", input);
                work = Console.ReadLine();
            
            StreamWriter sw = new StreamWriter(DefinePath(role), true, Encoding.Default);
            sw.WriteLine($"{date.ToShortDateString()},{input},{hour},{work}");
            sw.Close();
            }
            catch { Console.WriteLine("Неверно введены данные"); }
        }

        bool GoodDate(DateTime date, string role)
        {
            DateTime dateReal = DateTime.Today;
            if (dateReal >= date && dateReal.AddDays(-15)<= date && (role == "Руководитель" || role == "Сотрудник")) return true;
            if (role == "Фрилансер" && dateReal >= date && dateReal.AddDays(-2) <= date) return true;
            return false;
        }

        public void ReportWork(string input, string role)
        {
            DateTime dateEnd = DateTime.Now;
            DateTime dateStart = dateEnd.AddDays(-dateEnd.Day + 1);
            var totalHour = 0;
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

            Console.WriteLine("-------------------------------------------------");
            if (role=="Сотрудник") 
                Console.WriteLine($"Отчет по сотруднику: {input} за период с {dateStart.ToShortDateString()} по {dateEnd.ToShortDateString()}");
            if (role == "Руководитель")
                Console.WriteLine($"Отчет по руководителю: {input} за период с {dateStart.ToShortDateString()} по {dateEnd.ToShortDateString()}");
            if (role == "Фрилансер")
                Console.WriteLine($"Отчет по фрилансеру: {input} за период с {dateStart.ToShortDateString()} по {dateEnd.ToShortDateString()}");
            StreamReader sr = new StreamReader(DefinePath(role), Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] mas = line.Split(',');
                if (dateStart <= Convert.ToDateTime(mas[0]) && dateEnd >= Convert.ToDateTime(mas[0]) && input == mas[1])
                {
                    Console.WriteLine($"{mas[0]}, {mas[2]} {GoodHour(mas[2])}, {mas[3]}");
                    totalHour += Convert.ToInt32(mas[2]);
                }
            }
            sr.Close();
            if (role == "Сотрудник")
                Console.WriteLine($"Итого: {totalHour} {GoodHour(totalHour.ToString())}, заработано {GoodSalary(totalHour, "Сотрудник")} руб.");
            if (role == "Руководитель")
                Console.WriteLine($"Итого: {totalHour} {GoodHour(totalHour.ToString())}, заработано {GoodSalary(totalHour, "Руководитель")} руб.");
            if (role == "Фрилансер")
                Console.WriteLine($"Итого: {totalHour} {GoodHour(totalHour.ToString())}, заработано {GoodSalary(totalHour, "Фрилансер")} руб.");

            Console.WriteLine("-------------------------------------------------");
        }

        string DefinePath(string role)
        {
            switch (role)
            {
                case "Руководитель":  return pathLeader;
                case "Сотрудник": return pathEmployee; 
                case "Фрилансер":  return pathFreelancer; 
            }
            return null;

        }

        string GoodHour(string input)
        {
            byte hour = Convert.ToByte(input);
            if (hour >= 5) return "часов";
            if (hour >= 2 && hour < 5) return "часа";
            if (hour == 1) return "час";
            return "";
        }

        int GoodSalary(int hour, string role)
        {
            int CashHour;
            if (role == "Сотрудник")
            {
                CashHour= salaryEmployee / 160;
                return hour * CashHour;
            }
            if (role == "Руководитель")
            {
                CashHour = salaryLeader/ 160;
                return hour * CashHour;
            }
            if (role == "Фрилансер") return hour * salaryFreelancer;

            return 0;
        }
    }
}

