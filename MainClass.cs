using System;

namespace Payroll
{
    class MainClass
    {
        static void Main()
        {
            bool flagExit = false;
            Authorization authorization = new Authorization();
            while (!flagExit)
            {
                Console.Write("Представьтесь, пожалуйста: ");
                string user = Console.ReadLine();
                string position = authorization.LogIn(user);
                switch (position)
                {
                    case "Руководитель": { Leader leader = new Leader(); leader.Start(user); flagExit = true; break; }
                    case "Сотрудник": { Employee employee = new Employee(); employee.Start(user); flagExit = true; break; }
                    case "Фрилансер": { Freelancer freelancer = new Freelancer(); freelancer.Start(user); break; }
                    case "Ошибка": { Console.WriteLine("Такой сотрудник не найден, хотите попробывать еще раз ввести?(да)");
                                     if (Console.ReadLine() == "да") break;
                                         flagExit = true; break; }
                }
                
            }
        }
    }
}
