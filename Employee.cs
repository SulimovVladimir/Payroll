using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Payroll
{
    class Employee
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список отработанных часов сотрудник.csv");
        const int salary = 120000;

        public void Start(string input)
        {
            bool flagExit = false;
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Здравствуйте, {input}!\nВаша должность: сотрудник.");
            Console.WriteLine("-------------------------------------------------");
            Menu();
            while (!flagExit)
            {
                Console.Write("Выберите желаемое действие: ");
                byte marker = Convert.ToByte(Console.ReadLine());
                switch (marker)
                {
                    case 1: { AddHourWork(input); break; }
                    case 2: { ReportWork(input); break; }
                    case 3: { flagExit = true; break; }
                    default: { Console.WriteLine("Неверно выбрано действие"); Menu(); break; }

                }
            }
        }

        void Menu()
        {
            Console.WriteLine("Что вы можете сделать в этой программе:\n(1). Добавить свои часы работы\n(2). " +
                              "Просмотреть свои отработанные часы и зарплату за период\n(3). Выход из программы");
        }

        public void AddHourWork(string input)
        {
            Console.Write("Введите дату (в формате DD.MM.YYYY): ");
            DateTime date = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Сколько часов Вы работали: ");
            byte hour = Convert.ToByte(Console.ReadLine());
            Console.Write("Что именно вы делали: ");
            string work = Console.ReadLine();

            StreamWriter sw = new StreamWriter(path, true, Encoding.Default);
            sw.WriteLine($"{date.ToShortDateString()},{input},{hour},{work}");
            sw.Close();

        }

        public void ReportWork(string input)
        {
            DateTime dateEnd = DateTime.Now;
            DateTime dateStart = dateEnd.AddDays(-dateEnd.Day+1);
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
                Console.WriteLine("Не верный ввод даты");
            }

            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Отчет по сотруднику: {input} за период с {dateStart.ToShortDateString()} по {dateEnd.ToShortDateString()}");
            StreamReader sr = new StreamReader(path, Encoding.Default);
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
            Console.WriteLine($"Итого: {totalHour} {GoodHour(totalHour.ToString())}, заработано {GoodSalary(totalHour)} руб.");
            Console.WriteLine("-------------------------------------------------");
        }

        string GoodHour(string input)
        {
            byte hour = Convert.ToByte(input);
            if (hour >= 5) return "часов";
            if (hour >= 2 && hour < 5) return "часа";
            if (hour == 1) return "час";
            return "";
        }

        int GoodSalary(int hour)
        {
            int CashHour = salary / 160;
            return hour * CashHour;
        }
    }
}
