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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bibliotecario>().ToTable("bibliotecarios");
        modelBuilder.Entity<Lector>().ToTable("lector");
        modelBuilder.Entity<Libro>().ToTable("libros");
        modelBuilder.Entity<Prestamo>().ToTable("prestamos");
    }
}