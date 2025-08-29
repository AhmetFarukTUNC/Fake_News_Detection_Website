using FakeNewsMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using YourProject.Models;

namespace FakeNewsMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { }

        public DbSet<PredictionResult> Predictions { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ContactMessage> ContactMessages { get; set; }
    }
}
