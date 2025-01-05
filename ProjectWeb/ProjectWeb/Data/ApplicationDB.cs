using Microsoft.EntityFrameworkCore;
using ProjectWeb.Models.Entities;

namespace ProjectWeb.Data
{
    //ApplicationDB - bridge between aplication and server
    public class ApplicationDB: DbContext
    {
        
        public ApplicationDB(DbContextOptions<ApplicationDB> options) : base(options) {  }


        //DbSet - colectie de tipul Utilizator, iar numele tabelului este Utilizator {get;get;}
        //ne folosim de acesta pentru a accesa utilizatorii
        public DbSet<Utilizator> Utilizator { get; set; }  
        public DbSet<Zbor> Zbor {  get; set; }
        public DbSet<Checkin> Checkin { get; set; }     

        
    }
}
