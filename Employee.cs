using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Payroll
{
    class Employee
    {

        public void Start(string input)
        {
            bool flagExit = false;
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Здравствуйте, {input}!\nВаша должность: сотрудник.");
            Console.WriteLine("-------------------------------------------------");
            Menu();
            while (!flagExit)
            {
                byte marker = 0;
                try
                {
                    Console.Write("Выберите желаемое действие: ");
                    marker = Convert.ToByte(Console.ReadLine());
              
                switch (marker)
                {
                    case 1: { Work work = new Work(); work.AddHourWork(input, "Сотрудник"); break; }
                    case 2: { Work work = new Work(); work.ReportWork(input, "Сотрудник"); break; }
                    case 3: { flagExit = true; break; }
                    default: { Console.WriteLine("Неверно выбрано действие"); Menu(); break; }
                }
                }
                catch { Console.WriteLine("!!!!! Неверно выбрано действие !!!!!"); Menu(); }
            }
        }

        void Menu()
        {
            Console.WriteLine("Что вы можете сделать в этой программе:\n(1). Добавить свои часы работы\n(2). " +
                              "Просмотреть свои отработанные часы и зарплату за период\n(3). Выход из программы");
        }

    }  
}
