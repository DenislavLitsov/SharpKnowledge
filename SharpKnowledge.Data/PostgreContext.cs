using Microsoft.EntityFrameworkCore;
using SharpKnowledge.Common;
using SharpKnowledge.Data.Models;
using System.Reflection.Metadata;

namespace SharpKnowledge.Data
{
    public class PostgreContext : DbContext
    {
        private static PostgreContext _context;
        public DbSet<BrainModel> BrainModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(EnvironmentManager.GetDBConnectionString());

        public static PostgreContext GetContext()
        {
            if (_context == null)
            {
                _context = new PostgreContext();
                _context.Database.EnsureCreated();
            }
            
            return _context;
        }
    }
}
