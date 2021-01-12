using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Payroll
{
    class Authorization
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список сотрудников.csv");

        public string LogIn(string input)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string sourse, line;
            while ((line = sr.ReadLine()) != null)
            {
                sourse = line;
                string temp = Parse(ref line);
                if (temp == input) return ParseTwo(sourse);
                if (line != null)
                {
                    if (line == input) return ParseTwo(sourse);
                }
            }
            return "Ошибка";
        }

        string Parse(ref string input)
        {
            byte position = (byte)input.IndexOf(",");
            string temp = input.Substring(0, position);
            if (input.IndexOf(" ") == -1) { input = null; return temp; }
            else
            {
                position = (byte)temp.IndexOf(" ");
                input = temp.Substring(0, position);
                temp = temp.Substring(position, temp.Length - 1);
                return temp;
            }
        }

        string ParseTwo(string input)
        {
            byte position = (byte)input.IndexOf(",");
            input = input.Substring(position, input.Length);
            return input;
        }
    }
}
