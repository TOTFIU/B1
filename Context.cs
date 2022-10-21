using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace B1
{
    public class THING /*: IDisposable*/
    {
        [Key]
        public int Id { get; set; }
        public string Date { get; set; }
        public string Latin { get; set; }
        public string Russian { get; set; }
        public Int64 Int_Number { get; set; }
        public double Double_Number { get; set; }

       
    }

    public class Context : Microsoft.EntityFrameworkCore.DbContext
    {
        private string _name;

        public Context(string name) : base()
        {
            this._name = name;

        }
        //public bool CheckCreated()
        //{
        //    this.THINGS.
        //    return null;
        //}
        public Microsoft.EntityFrameworkCore.DbSet<THING> THINGS { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@$"Server=(localdb)\mssqllocaldb;Database={_name};Trusted_Connection=True;");
        }


    }

}
