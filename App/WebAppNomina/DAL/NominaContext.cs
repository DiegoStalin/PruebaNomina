using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema; // Necesario para DatabaseGeneratedOption
using WebAppNomina.Models;

namespace WebAppNomina.DAL
{
    public class NominaContext : DbContext
    {
        public NominaContext() : base("name=conexion")
        {
            // Evita que Entity Framework intente crear o modificar la base de datos
            Database.SetInitializer<NominaContext>(null);
        }

        // --- TABLAS DEL SISTEMA ---
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<AsignacionDepto> Asignaciones { get; set; }
        public DbSet<GerenteDepto> Gerentes { get; set; }
        public DbSet<Titulo> Titulos { get; set; }

        // --- RF-07 y RF-08: SALARIOS Y AUDITORÍA ---
        public DbSet<Salario> Salarios { get; set; }
        public DbSet<LogAuditoriaSalario> Auditorias { get; set; } // Mapeo para el historial de cambios

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 1. CORRECCIÓN DE NOMBRES DE TABLA
            // Sincroniza el modelo con la tabla física 'Empleadoes' en SQL Server
            modelBuilder.Entity<Empleado>().ToTable("Empleadoes");

            // 2. CONFIGURACIÓN DE TABLA SALARIOS (RF-07)
            modelBuilder.Entity<Salario>()
                .HasKey(s => s.id);

            // Indica que el ID es autoincremental (IDENTITY) en la base de datos
            modelBuilder.Entity<Salario>()
                .Property(s => s.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Salario>()
                .Property(s => s.emp_no)
                .HasColumnName("emp_no");

            // Relación obligatoria: Un salario pertenece a un empleado
            modelBuilder.Entity<Salario>()
                .HasRequired(s => s.Empleado)
                .WithMany()
                .HasForeignKey(s => s.emp_no);

            // 3. CONFIGURACIÓN DE AUDITORÍA (RF-08)
            // Sincroniza con la tabla de Log donde el Trigger inserta los datos
            modelBuilder.Entity<LogAuditoriaSalario>()
                .ToTable("LogAuditoriaSalarios");

            modelBuilder.Entity<LogAuditoriaSalario>()
                .HasKey(a => a.id);

            // El ID de auditoría también lo genera SQL Server automáticamente
            modelBuilder.Entity<LogAuditoriaSalario>()
                .Property(a => a.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // 4. SOLUCIÓN PARA TABLA TÍTULOS (RF-06)
            modelBuilder.Entity<Titulo>()
                .HasRequired(t => t.Empleado)
                .WithMany()
                .HasForeignKey(t => t.emp_no);

            base.OnModelCreating(modelBuilder);
        }
    }
}