using BibliotecaAPI.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Datos;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
        : base(options)
    {
    }
    public DbSet<Bibliotecario> Bibliotecarios { get; set; }
    public DbSet<Lector> Lectores { get; set; }

    public DbSet<Libro> Libros { get; set; }

    public DbSet<Prestamo> Prestamos { get; set; }

    public DbSet<Ejemplar> Ejemplares { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bibliotecario>().ToTable("bibliotecarios");
        modelBuilder.Entity<Lector>().ToTable("lector");
        modelBuilder.Entity<Libro>().ToTable("libros");
        modelBuilder.Entity<Ejemplar>().ToTable("ejemplares");
        modelBuilder.Entity<Prestamo>().ToTable("prestamos");


        modelBuilder.Entity<Lector>()
    .HasIndex(l => l.DNI)
    .IsUnique();
      
        modelBuilder.Entity<Prestamo>()
    .HasOne(p => p.Ejemplar)
    .WithMany(e => e.Prestamos)
    .HasForeignKey(p => p.EjemplarId)
    .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Prestamo>()
       .HasOne(p => p.Lector)
       .WithMany(l => l.Prestamos)
       .HasForeignKey(p => p.LectorId)
       .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Prestamo>()
            .Property(p => p.Multa)
            .HasPrecision(18, 2);
    }

}