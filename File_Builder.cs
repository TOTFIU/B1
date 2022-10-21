using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace B1
{
    internal class File_Builder
    {
        private string PATH_DIRECTORY;

        //private void SaveData(string line, string name)
        //{

        //   string[] separated = line.Split(new string[] { "||" }, StringSplitOptions.None);

        //    THING thing = new THING
        //    {
        //        Date = separated[0],
        //        Latin = separated[1],
        //        Russian = separated[2],
        //        Int_Number = Convert.ToInt32(separated[3]),
        //        Double_Number = Convert.ToDouble(separated[4])// формат известен, так что можно себе позволить сделать так
        //    };

        //    using (Context db = new Context(name))//передаем название базы данных
        //    {

        //        db.THINGS.Add(thing);
        //        db.SaveChanges();
        //    }

        //}
        
        public void To_Database(string name)
        {
          
            for (int file_num = 1; file_num <= 100; file_num++)
            {
                Console.Clear();
                string PATH = $@"{this.PATH_DIRECTORY}\{file_num}.txt";

                Console.WriteLine($"Writing to database {file_num}.txt...");
                string[] things = System.IO.File.ReadAllLines(PATH);
                int count = things.Length;

                int string_counter = 1;

                try
                {
                    foreach (string line in things)
                    {
                        string[] separated = line.Split(new string[] { "||" }, StringSplitOptions.None);

                        THING thing = new THING
                        {
                            Date = separated[0],
                            Latin = separated[1],
                            Russian = separated[2],
                            Int_Number = Convert.ToInt32(separated[3]),
                            Double_Number = Convert.ToDouble(separated[4]) // формат известен, так что можно себе позволить сделать так
                        };


                        using (Context db = new Context(name)) // (здесь мне кажется основная проблема медленной записи и есть, но в чем она понять не могу)
                        {
                            db.THINGS.Add(thing); // данные будут перезаписываться если такой id уже есть (id 
                                                  //генерится от 1 до 10 000 000 в зависимости какая строка какого файла.
                                                  //Медленно, если использовать просто Add будет быстрее, но и данные не перезапишутся а просто уйдут в конец
                            string_counter++;
                            db.SaveChanges();
                        }
                        Console.WriteLine($"{string_counter}/{count}");
                        Console.SetCursorPosition(0, 1);



                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }

            }



        }

        //это максимальная скорость записи, которую я смог выжать (1000 в секунду или 1,5) или я глуnпый или это максимум при чтении такого объема строк

        public void CountInt(string name)
        {
            string sqlExpression = "SELECT SUM(Int_Number) FROM dbo.THINGs";//проблема скорее всего тут

            using (Context db = new Context(name))
            {
                
                var comps = db.THINGS.ExecuteSqlRaw("GET_INT_SUM");//как?
                Console.WriteLine($"{comps.ToString()}");
            }


            using (SqlConnection connection = new SqlConnection(@$"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = {name}; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                //SqlParameter intParam = new SqlParameter
                //{
                   
                //    SqlDbType = SqlDbType.BigInt,
                //    Direction = ParameterDirection.Output // параметр выходной
                //};

                //command.Parameters.Add(intParam);
                command.ExecuteNonQuery();

                //Console.WriteLine("{0}",intParam.Value.ToString());
            }
            // по идее считает, но не выводит
        }
        // в чем ошибка?
        public void Build_files(string PATH_DIRECTORY)
        {
            this.PATH_DIRECTORY = PATH_DIRECTORY;
            int i = 1;
            for (int file_num = 1; file_num <= 100; file_num++)
            {

                Console.WriteLine($"Writing file {file_num}.txt \n");
                string PATH = $@"{this.PATH_DIRECTORY}\{file_num}.txt";
                int count = 0;
                if (System.IO.File.Exists(PATH))
                {
                    count = System.IO.File.ReadAllLines(PATH).Length;
                }


                for (int file_string = count + 1; file_string <= 100000; file_string++)
                {

                    File_Output file_Output = new File_Output();
                    //file_Output.SetRandomAll();

                    using (StreamWriter writer = new StreamWriter(PATH, true))
                    {
                        writer.WriteLine(file_Output.ConvertToString());

                    }

                    Console.SetCursorPosition(1, i);
                    Console.WriteLine($"{file_string} string out of 100000 ");

                }

                count = System.IO.File.ReadAllLines(PATH).Length;
                Console.WriteLine($"{file_num}.txt Done! {count} strings in file");
                i += 3;
            }
        }


        public void Delete_files(string combination)
        {
            Console.WriteLine("Merging and deleting...");

            int deleted_strings = 0;

            for (int file_num = 1; file_num <= 100; file_num++)
            {
                string PATH = $@"{this.PATH_DIRECTORY}\{file_num}.txt";
                string[] file_mas = System.IO.File.ReadAllLines(PATH);
                List<string> file = file_mas.ToList();
                bool is_modified = false;

                foreach (string line in file_mas)
                {

                    if (combination != null && line.Contains(combination))
                    {
                        file.Remove(line);
                        deleted_strings++;
                        is_modified = true;
                    }


                }
                if (is_modified)
                {
                    System.IO.File.WriteAllLines(PATH, file);
                }

                System.IO.File.AppendAllLines($@"{this.PATH_DIRECTORY}\result.txt", file);
            }
            Console.WriteLine($"{deleted_strings} strings deleted");

        }

        public void Merge_files()
        {
            Console.WriteLine("Merge files? Y/N");

            string confirm = Console.ReadLine();

            do
            {
                if (confirm == "Y" || confirm == "y")
                {

                    Console.WriteLine("Want to delete anything? Y/N");
                    string confirm_delete = Console.ReadLine();

                    do
                    {
                        if (confirm_delete == "Y" || confirm_delete == "y")
                        {
                            Console.WriteLine("Write the combination you want to delete:");
                            Delete_files(Console.ReadLine());
                            break;
                        }

                        else if (confirm_delete == "N" || confirm_delete == "n")
                        {
                            Console.WriteLine("Merging...");
                            for (int file_num = 1; file_num <= 100; file_num++)
                            {

                                string PATH = $@"{this.PATH_DIRECTORY}\{file_num}.txt";
                                string[] file_mas = System.IO.File.ReadAllLines(PATH);
                                List<string> file = file_mas.ToList();
                                System.IO.File.AppendAllLines($@"{this.PATH_DIRECTORY}\result.txt", file);

                            }


                            break;
                        }
                        else
                        {
                            Console.WriteLine("Want to delete anything? Y/N");
                            confirm_delete = Console.ReadLine();
                        }
                    }
                    while (true);


                    Console.WriteLine($"------------------- files are merged {System.IO.File.ReadAllLines($@"{this.PATH_DIRECTORY}\result.txt").Length} strings in file. Press any key to exit");
                    Console.ReadKey();
                    break;
                }

                else if (confirm == "N" || confirm == "n")
                {

                    break;
                }
                else
                {
                    Console.WriteLine("Merge files? Y/N");
                    confirm = Console.ReadLine();
                }

            } while (true);

        }
    }

}