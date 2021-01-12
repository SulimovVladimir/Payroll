using System;

namespace Payroll
{
    class MainClass
    {
        static void Main()
        {
            Authorization authorization = new Authorization();
            Console.Write("Представьтесь, пожалуйста:");
            string user = Console.ReadLine();
            string position = authorization.LogIn(user);
            switch (position)
            {
                case "Руководитель": { break; }
                case "Сотрудник": { break; }
                case "Фрилансер": { break; }
                case "Ошибка": { break; }

            }
        }
    }
}
