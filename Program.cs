using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using B1;
using System.Data;
using System.Text;
using System.Threading;
using System.Reflection.Emit;

class Program
{
    static void Main(string[] args)
    {
        // переделать через делегаты


        Console.WriteLine("Write Path to save files:");
        string PATH = Console.ReadLine();
        Console.Clear();

        PATH = $@"D:\MYPROJ\B1\OUTPUT\";// по умолчанию (чтоб у меня все работало)

        File_Builder builder = new File_Builder();

        builder.Build_files(PATH);

        File.Delete($@"{PATH}\result.txt");// на всякий случай удалить прошлый общий файл (если такой есть)
        
        builder.Merge_files();// слияние файлов в один

        Console.WriteLine("Exiting...");

        Console.Clear();
        Console.Write("Enter the name for database(if this database exists data will be appended to databse): ");

        string name = Console.ReadLine();

        //builder.To_Database(name);
        builder.CountInt(name);
        //builder.CountInt(name);



    }
}
