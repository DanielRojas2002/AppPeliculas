using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppPeliculas.Models;

public partial class DbpeliculasContext : DbContext
{
    public DbpeliculasContext()
    {
    }

    public DbpeliculasContext(DbContextOptions<DbpeliculasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Almacen> Almacens { get; set; }

    public virtual DbSet<Autor> Autors { get; set; }

    public virtual DbSet<Carrito> Carritos { get; set; }

    public virtual DbSet<CarritoDetalle> CarritoDetalles { get; set; }

    public virtual DbSet<CategoriaAutor> CategoriaAutors { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<EstatusPelicula> EstatusPeliculas { get; set; }

    public virtual DbSet<Pelicula> Peliculas { get; set; }

    public virtual DbSet<PeliculaAutor> PeliculaAutors { get; set; }

    public virtual DbSet<Tipoentradum> Tipoentrada { get; set; }

    public virtual DbSet<Tipousuario> Tipousuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DANIELROJAS; Database=DBPELICULAS; Trusted_Connection=True;  Encrypt=False ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Almacen>(entity =>
        {
            entity.HasKey(e => e.IdAlmacen).HasName("PK_Almacen");

            entity.ToTable("ALMACEN");

            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.Almacens)
                .HasForeignKey(d => d.IdPelicula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PK_Almacen_Pelicula");

            entity.HasOne(d => d.IdTipoEntradaNavigation).WithMany(p => p.Almacens)
                .HasForeignKey(d => d.IdTipoEntrada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PK_Almacen_TipoEntrada");
        });

        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(e => e.IdAutor).HasName("PK_Autor");

            entity.ToTable("AUTOR");

            entity.Property(e => e.AMaterno)
                .HasMaxLength(80)
                .HasColumnName("A_Materno");
            entity.Property(e => e.APaterno)
                .HasMaxLength(80)
                .HasColumnName("A_Paterno");
            entity.Property(e => e.Nombre).HasMaxLength(80);
        });

        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.HasKey(e => e.IdCarrito).HasName("PK_Carrito");

            entity.ToTable("CARRITO");

            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Carritos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carrito_Usuario");
        });

        modelBuilder.Entity<CarritoDetalle>(entity =>
        {
            entity.HasKey(e => e.IdCarritoDetalle).HasName("PK_CarritoDetalle");

            entity.ToTable("CARRITO_DETALLE");

            entity.HasOne(d => d.IdCarritoNavigation).WithMany(p => p.CarritoDetalles)
                .HasForeignKey(d => d.IdCarrito)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarritoDetalle_Carrito");
        });

        modelBuilder.Entity<CategoriaAutor>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaAutor).HasName("PK_CategoriaAutor");

            entity.ToTable("CATEGORIA_AUTOR");

            entity.HasIndex(e => new { e.IdCategoria, e.IdAutor }, "UC_IdCategoria_IdAutor").IsUnique();

            entity.HasOne(d => d.IdAutorNavigation).WithMany(p => p.CategoriaAutors)
                .HasForeignKey(d => d.IdAutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoriaAutor_Autor");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.CategoriaAutors)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoriaAutor_Categoria");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK_IdCategoria");

            entity.ToTable("CATEGORIA");

            entity.Property(e => e.Descripcion).HasMaxLength(80);
        });

        modelBuilder.Entity<EstatusPelicula>(entity =>
        {
            entity.HasKey(e => e.IdEstatusPelicula).HasName("PK_IdEstatusPelicula");

            entity.ToTable("ESTATUS_PELICULA");

            entity.Property(e => e.Descripcion).HasMaxLength(20);
        });

        modelBuilder.Entity<Pelicula>(entity =>
        {
            entity.HasKey(e => e.IdPelicula).HasName("PK_IdUsuario");

            entity.ToTable("PELICULA");

            entity.Property(e => e.Descripcion).HasMaxLength(280);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Imagen).HasMaxLength(280);
            entity.Property(e => e.Titulo).HasMaxLength(80);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Peliculas)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdCategoria_Pelicula");

            entity.HasOne(d => d.IdEstatusPeliculaNavigation).WithMany(p => p.Peliculas)
                .HasForeignKey(d => d.IdEstatusPelicula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdEstatusPelicula_Pelicula");
        });

        modelBuilder.Entity<PeliculaAutor>(entity =>
        {
            entity.HasKey(e => e.IdPeliculaAutor).HasName("PK_PeliculaAutor");

            entity.ToTable("PELICULA_AUTOR");

            entity.HasIndex(e => new { e.IdPelicula, e.IdCategoriaAutor }, "UC_IdPelicula_IdAutor").IsUnique();

            entity.HasOne(d => d.IdCategoriaAutorNavigation).WithMany(p => p.PeliculaAutors)
                .HasForeignKey(d => d.IdCategoriaAutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PeliculaAutor_CategoriaAutor");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.PeliculaAutors)
                .HasForeignKey(d => d.IdPelicula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PeliculaAutor_Pelicula");
        });

        modelBuilder.Entity<Tipoentradum>(entity =>
        {
            entity.HasKey(e => e.IdTipoEntrada).HasName("PK_IdTipoEntrada");

            entity.ToTable("TIPOENTRADA");

            entity.Property(e => e.Descripcion).HasMaxLength(20);
        });

        modelBuilder.Entity<Tipousuario>(entity =>
        {
            entity.HasKey(e => e.IdTipoUsuario).HasName("PK_TipoUsuario");

            entity.ToTable("TIPOUSUARIO");

            entity.Property(e => e.Descripcion).HasMaxLength(20);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK_Usuario");

            entity.ToTable("USUARIO");

            entity.Property(e => e.AMaterno)
                .HasMaxLength(80)
                .HasColumnName("A_Materno");
            entity.Property(e => e.APaterno)
                .HasMaxLength(80)
                .HasColumnName("A_Paterno");
            entity.Property(e => e.Contrasena).HasMaxLength(80);
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(80);

            entity.HasOne(d => d.IdTipoUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdTipoUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_TipoUsuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
