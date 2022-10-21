using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace B1
{
    internal class File_Output
    {
        public DateTime Date;
        public string Latin;
        public string Russian;
        public uint Int_Number;
        public double Double_Number;


        private readonly Random gen = new Random();

        public File_Output()
        {
            SetRandomAll();
        }



        private void SetRandomDate()
        {
            DateTime start = new DateTime(2017, 10, 13);
            int range = (DateTime.Today - start).Days;
            this.Date = start.AddDays(gen.Next(range));
        }

        private string GenRandomString(string Alphabet, int Length)
        {
            StringBuilder sb = new StringBuilder(Length - 1);
            int Position = 0;
            for (int i = 0; i < Length; i++)
            {
                Position = gen.Next(0, Alphabet.Length - 1);

                sb.Append(Alphabet[Position]);
            }
            return sb.ToString();
        }

        private void SetRandomInt_Number()
        {
            this.Int_Number = (uint)gen.Next(0, 100000000);
        }

        private void SetRandomDouble_Number()
        {
            this.Double_Number = (double)gen.Next(100000000, 2000000000) / 100000000;
        }

        private void SetRandomAll()
        {
            SetRandomDate();
            this.Latin = GenRandomString("QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm", 10);
            this.Russian = GenRandomString("ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮЁйцукенгшщзхъфывапролджэячсмитьбюё", 10);
            SetRandomInt_Number();
            SetRandomDouble_Number();
        }

        public string ConvertToString()
        {
            return $"{Date.ToShortDateString()}||{Latin}||{Russian}||{Int_Number}||{Double_Number}";
        }

    }

}
