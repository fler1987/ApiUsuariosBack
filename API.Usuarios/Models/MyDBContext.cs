using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Models
{
    public class MyDBContext : DbContext
    {
        
        public DbSet<Schedule> Schedules { get; set; } 
        public DbSet<Periodo> Periodos { get; set; } 
        public DbSet<Configuracion> Configuraciones { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Migracion> Migraciones { get; set; }
        public DbSet<UsuarioCursoOracle> UsuarioCursoOracles { get; set; } 
        public DbSet<UsuarioCursoMoodle> UsuarioCursoMoodles { get; set; } 
        public DbSet<Alumno> Alumnos { get; set; }


        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) {}


        //Creacion de tablas y mapeo de atributos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Map Entidades       
            modelBuilder.Entity<Schedule>().ToTable("Schedule");
            modelBuilder.Entity<Periodo>().ToTable("Periodo");
            modelBuilder.Entity<Configuracion>().ToTable("Configuracion");
            modelBuilder.Entity<Log>().ToTable("Log");
            modelBuilder.Entity<Migracion>().ToTable("Migracion");
            modelBuilder.Entity<UsuarioCursoOracle>().ToTable("UsuarioCursoOracle");
            modelBuilder.Entity<UsuarioCursoMoodle>().ToTable("UsuarioCursoMoodle");
            modelBuilder.Entity<Alumno>().ToTable("Alumno");

            // Configure Primary Keys   
            modelBuilder.Entity<Schedule>().HasKey(u => u.IdSchedule).HasName("PK_IdSchedule");
            modelBuilder.Entity<Periodo>().HasKey(u => u.IdPeriodo).HasName("PK_IdPeriodo");
            modelBuilder.Entity<Configuracion>().HasKey(u => u.IdConfiguracion).HasName("PK_IdConfiguracion");
            modelBuilder.Entity<Log>().HasKey(u => u.IdLog).HasName("PK_IdLog");
            modelBuilder.Entity<Migracion>().HasKey(u => u.IdMigracion).HasName("PK_IdMigracion");
            modelBuilder.Entity<UsuarioCursoOracle>().HasKey(u => u.IdUsuarioCursoOracle).HasName("PK_IdUsuarioCursoOracle");
            modelBuilder.Entity<UsuarioCursoMoodle>().HasKey(u => u.IdUsuarioCursoMoodle).HasName("PK_IdUsuarioCursoMoodle");
            modelBuilder.Entity<Alumno>().HasKey(u => u.IdAlumno).HasName("IdAlumno");

            // Configure indexes   
            //modelBuilder.Entity<Prueba>().HasIndex(u => u.FirstName).HasDatabaseName("Idx_FirstName");
            //modelBuilder.Entity<Prueba>().HasIndex(u => u.LastName).HasDatabaseName("Idx_LastName");

            modelBuilder.Entity<Schedule>().Property(u => u.IdSchedule).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Periodo>().Property(u => u.IdPeriodo).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Configuracion>().Property(u => u.IdConfiguracion).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Log>().Property(u => u.IdLog).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Migracion>().Property(u => u.IdMigracion).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<UsuarioCursoOracle>().Property(u => u.IdUsuarioCursoOracle).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<UsuarioCursoMoodle>().Property(u => u.IdUsuarioCursoMoodle).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Alumno>().Property(u => u.IdAlumno).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();


            //AGregar Tipo de datos a tablas
            // ----------- SCHEDULE- -------------            
            modelBuilder.Entity<Schedule>().Property(u => u.Fecha).HasColumnType("datetime").IsRequired(true);            
            //-----------------------------------
            
            //AGregar Tipo de datos a tablas
            // ----------- PERIODO- -------------
            modelBuilder.Entity<Periodo>().Property(u => u.IdPeriodo).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Periodo>().Property(u => u.NPeriodo).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<Periodo>().Property(u => u.Nombre).HasColumnType("varchar(200)").IsRequired(false);
            //-----------------------------------


            // ------------CONFIGURACION -----------
            //modelBuilder.Entity<Configuracion>().Property(u => u.IdConfiguracion).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Configuracion>().Property(u => u.Periodo).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<Configuracion>().Property(u => u.Nombre).HasColumnType("varchar(200)").IsRequired(false);
            //modelBuilder.Entity<Configuracion>().Property(u => u.Habilitar).HasColumnType("boolean");
            //---------------------------------------


            //------------LOG -----------------
            modelBuilder.Entity<Log>().Property(u => u.IdLog).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Log>().Property(u => u.Request).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<Log>().Property(u => u.Response).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<Log>().Property(u => u.Fecha).HasColumnType("datetime");
            modelBuilder.Entity<Log>().Property(u => u.Tipo).HasColumnType("varchar(200)").IsRequired(false);
            //--------------------


            // ----------MIGRACION ------
            modelBuilder.Entity<Migracion>().Property(u => u.IdMigracion).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Migracion>().Property(u => u.TipoMigracion).HasColumnType("varchar(200)").IsRequired(false); 
            modelBuilder.Entity<Migracion>().Property(u => u.Fecha).HasColumnType("datetime");
            //------------------------

            // ----------UsuarioCursoOracle ------
            modelBuilder.Entity<UsuarioCursoOracle>().Property(u => u.IdUsuarioCursoOracle).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<UsuarioCursoOracle>().Property(u => u.OpridSemilla).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<UsuarioCursoOracle>().Property(u => u.Unidneg).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<UsuarioCursoOracle>().Property(u => u.ShortName).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<UsuarioCursoOracle>().Property(u => u.UserName).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<UsuarioCursoOracle>().Property(u => u.AcadCareer).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<UsuarioCursoOracle>().Property(u => u.ClassSection).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<UsuarioCursoOracle>().Property(u => u.Strm).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<UsuarioCursoOracle>().Property(u => u.Sysdate).HasColumnType("datetime");
            //------------------------

            // ----------UsuarioCursoMoodle ------
            modelBuilder.Entity<UsuarioCursoMoodle>().Property(u => u.IdUsuarioCursoMoodle).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<UsuarioCursoMoodle>().Property(u => u.UserName).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<UsuarioCursoMoodle>().Property(u => u.Strm).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<UsuarioCursoMoodle>().Property(u => u.Sysdate).HasColumnType("datetime");
            //------------------------

            // ----------Alumno ------
            modelBuilder.Entity<Alumno>().Property(u => u.IdAlumno).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Alumno>().Property(u => u.Emplid).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Alumno>().Property(u => u.Nombre).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<Alumno>().Property(u => u.ApellidoPaterno).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<Alumno>().Property(u => u.ApellidoMaterno).HasColumnType("varchar(200)").IsRequired(false);
            modelBuilder.Entity<Alumno>().Property(u => u.Email).HasColumnType("varchar(200)").IsRequired(false);
            //------------------------
  
        }
    }
}
