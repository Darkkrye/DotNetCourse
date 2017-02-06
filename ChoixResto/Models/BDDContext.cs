using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChoixResto.Models
{
    public class BDDContext : DbContext
    {
        public DbSet<Sondage> Sondages { get; set; }
        public DbSet<Resto> Restos { get; set; }
        public DbSet<User> Users { get; set; }
    }
}