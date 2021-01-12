﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Payroll
{
    class Leader
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список сотрудников.csv");

        public void Start(string input)
        {
            bool flagExit=false;
            Console.WriteLine($"Здравствуйте, {input}!\nВаша должность: руководитель.");
            Menu();
            while (!flagExit)
            {
                Console.Write("Выберите желаемое действие: ");
                byte marker = Convert.ToByte(Console.ReadKey());
                switch (marker)
                {
                    case 1: { if (AddPeople()) Console.WriteLine("Сотрудник добавлен\n"); break; }
                    case 2: { break; }
                    case 3: { break; }
                    case 4: { break; }
                    case 5: { flagExit = true; break; }
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
            Console.Write("Введите фамилию и имя нового сотрудника: ");
            string name = Console.ReadLine();
            if (FindPeople(name)) { Console.WriteLine("Данный сотрудник уже есть в списке\n"); return false; }
            Console.Write("Доступные должности:\n(1). Руководитель\n(2). Сотрудник\n (3). Фрилансер\nВыберите должность нового сотрудника: ");
            byte role = Convert.ToByte(Console.ReadLine());
            if(role<1 && role>3) { Console.WriteLine("Неверно выбрана должность\n"); return false; }
            StreamWriter sw = new StreamWriter(path, true, Encoding.Default);
            switch (role)
            {
                case 1: { sw.WriteLine($"{name},Руководитель"); break; }
                case 2: { sw.WriteLine($"{name},Сотрудник"); break; }
                case 3: { sw.WriteLine($"{name},Фрилансер"); break; }
            }
            return true;
        }

        bool FindPeople(string input)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                byte pozition = (byte)line.IndexOf(',');
                line = line.Substring(0, pozition);
                if (line == input) return true;
            }
            return false;
        }
    }
}