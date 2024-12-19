using CALE.Models;
using Microsoft.EntityFrameworkCore;

namespace CALE.Data;

public partial class AppDataContext : DbContext
{
    public AppDataContext()
    {
    }

    public AppDataContext(DbContextOptions<AppDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Adopcion> Adopciones { get; set; }

    public virtual DbSet<Animal> Animales { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adopcion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ADOPCION__3214EC27A55834B1");

            entity.ToTable("ADOPCION");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.AnimalId).HasColumnName("ANIMAL_ID");
            entity.Property(e => e.FechaAdopcion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_ADOPCION");
            entity.Property(e => e.UsuarioId).HasColumnName("USUARIO_ID");

            entity.HasOne(d => d.Animal).WithMany(p => p.Adopcions)
                .HasForeignKey(d => d.AnimalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ADOPCION__ANIMAL__44FF419A");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Adopcions)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ADOPCION__USUARI__440B1D61");
        });

        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ANIMAL__3214EC2796C94B37");

            entity.ToTable("ANIMAL");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.DuenoId).HasColumnName("DUENO_ID");
            entity.Property(e => e.Edad).HasColumnName("EDAD");
            entity.Property(e => e.Especie)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ESPECIE");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("DISPONIBLE")
                .HasColumnName("ESTADO");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMAGE_URL");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Raza)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RAZA");
            entity.Property(e => e.Sexo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SEXO");

            entity.HasOne(d => d.Dueno).WithMany(p => p.Animals)
                .HasForeignKey(d => d.DuenoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ANIMAL__DUENO_ID__3F466844");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USUARIO__3214EC27E2658C26");

            entity.ToTable("USUARIO");

            entity.HasIndex(e => e.Email, "UQ__USUARIO__161CF724FA10DC02").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.Contrasenia)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CONTRASENIA");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DIRECCION");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Telefono)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("TELEFONO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
