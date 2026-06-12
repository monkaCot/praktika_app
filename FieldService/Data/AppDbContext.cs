using FieldService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Data;

public class AppDbContext : DbContext
{
    public DbSet<ServiceRequest> Requests { get; set; }
    public DbSet<DocumentTemplate> Templates { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "fieldservice.db");
        options.UseSqlite($"Data Source={dbPath}");
    }
}
