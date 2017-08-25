using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebApplicationOlimpiadas.Models
{
    public class TablaPrueba
    {
        public int ID { get; set; }
        public String Correlative { get; set; }
        public int goodAnswer { get; set; }
        public int wrongAnswer { get; set; }
        public int blankAnswer { get; set; }
        public int Course { get; set; }
        public int Level { get; set; }
    }

    public class TablaPruebaDBContext : DbContext
    {
        public DbSet<TablaPrueba> Movies { get; set; }
    }
}