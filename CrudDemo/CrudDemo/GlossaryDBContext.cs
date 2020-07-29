using CrudDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudDemo
{
    public class GlossaryDBContext : DbContext
    {
        public GlossaryDBContext(DbContextOptions<GlossaryDBContext> options) : base(options)
        { 
        }

        public DbSet<GlossaryItem> GlossaryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            builder.Entity<GlossaryItem>()
                .HasIndex(u => u.Term)
                .IsUnique();
        }
    }
}
