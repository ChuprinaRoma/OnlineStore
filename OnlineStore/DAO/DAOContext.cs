
using Microsoft.EntityFrameworkCore;
using OnlineStore.DAO.Model;
using OnlineStore.model;
using System;
using System.Collections.Generic;

namespace OnlineStore.DAO
{
    public class DAOContext : DbContext
    {
        public DbSet<ModelAllShoppe> shope    { get; set; }
        public DbSet<ModelProductDAO> shopeTb { get; set; }
        public DbSet<ModelDatePrice> dataTime { get; set; }
        public DbSet<ModelPhoto> photo        { get; set; }

        public DAOContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=OnlineShope;Trusted_Connection=True;");
            }
        }
    }
}
