using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchivosBaseDeDatos.Areas.Identity.Data;
using ArchivosBaseDeDatos.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArchivosBaseDeDatos.Models
{
    public class GestorContext : IdentityDbContext<GestorUser>
    {
        public GestorContext(DbContextOptions<GestorContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Documento> Documento { get; set; }
        public virtual DbSet<DocumentoRegistro> DocumentoRegistro { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Documento>(entity =>
            {
                entity.Property(e => e.Archivo64).IsRequired();

                entity.Property(e => e.ArchivoNombre)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Departamento)
                    .IsRequired();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<DocumentoRegistro>(entity =>
            {
                entity.Property(e => e.Departamento)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.DocumentoNavigation)
                    .WithMany(p => p.DocumentoRegistro)
                    .HasForeignKey(d => d.Documento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Documento_Registro_Documento");
            });
        }
    }
}
