using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPersistence
{
    public class DataContext : DbContext
    {
        public DataContext() : base("PIMDB")
        { }
        public DbSet<User> Users { get; set;}
    }
}