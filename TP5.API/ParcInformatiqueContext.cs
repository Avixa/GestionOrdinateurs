using TP5.Shared;
using Microsoft.EntityFrameworkCore;

namespace TP5.API;

public class ParcInformatiqueContext : DbContext
{
    // table des ordi
    public DbSet<Ordinateur> Ordinateurs { get; set; }

    // table des reparations
    public DbSet<Reparation> Reparations { get; set; }

    // table des utilisateurs
    public DbSet<User> Users { get; set; }

    //constructeur pour la config de la bdd
    public ParcInformatiqueContext(DbContextOptions<ParcInformatiqueContext> options) : base(options)
    {
    }

    // config bdd
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=parcinfo.db");
        }
    }

    // config des relations entre les tables
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 3
        modelBuilder.Entity<Ordinateur>()
            .OwnsOne(o => o.Caracteristiques);

        // un ordinateur peut avoir plusieurs reparations mais une reparation un ordinateur
        modelBuilder.Entity<Ordinateur>()
            .HasMany(o => o.Reparations)
            .WithOne(r => r.UnOrdinateur);

        // login unique
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();
    }
}