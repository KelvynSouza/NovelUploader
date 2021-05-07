using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace NovelUploader.Models
{
    public class NovelContext:DbContext
    {

        public NovelContext(DbContextOptions<NovelContext> options) : base(options)
        {  }
        public DbSet<NovelModel> Novels { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<NovelModel>().HasKey(m => m.Id);
            base.OnModelCreating(builder);
        }
    }
}
