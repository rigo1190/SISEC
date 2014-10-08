using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Modelo
{
    public class Contexto:DbContext
    {
        public Contexto():base("SISEF")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<UsuarioUnidadPresupuestal>()
            //   .HasRequired(c => c.Usuario)
            //   .WithMany(d => d.DetalleUnidadesPresupuestales)
            //   .HasForeignKey(c => c.UsuarioId);

            //modelBuilder.Entity<PlantillaDetalle>()
            //  .HasRequired(u => u.Plantilla)
            //  .WithMany(u => u.DetallePreguntas)
            //  .HasForeignKey(u => u.PlantillaId)
            //  .WillCascadeOnDelete(true);

            //modelBuilder.Entity<ObraPlantillaDetalle>()
            //  .HasRequired(u => u.ObraPlantilla)
            //  .WithMany(u => u.Detalles)
            //  .HasForeignKey(u => u.ObraPlantillaId)
            //  .WillCascadeOnDelete(true);

        }

        public override int SaveChanges()
        {

            var creados = this.ChangeTracker.Entries()
                            .Where(e => e.State == System.Data.Entity.EntityState.Added)
                            .Select(e => e.Entity).OfType<Generica>().ToList();

            foreach (var item in creados)
            {
                item.FechaCaptura = DateTime.Now;
                item.UsuarioCaptura = null;
            }

            var modificados = this.ChangeTracker.Entries()
                            .Where(e => e.State == System.Data.Entity.EntityState.Modified)
                            .Select(e => e.Entity).OfType<Generica>().ToList();

            foreach (var item in modificados)
            {
                item.FechaModificacion = DateTime.Now;
                item.UsuarioModifica = null;
            }

            return base.SaveChanges();


        }

        public virtual DbSet<Fideicomiso> Fideicomisos { get; set; }
        public virtual DbSet<Dependencia> Dependencias { get; set; }
        public virtual DbSet<Ejercicio> Ejercicios { get; set; }
        public virtual DbSet<DependenciaFideicomisoEjercicio> DependenciasFideicomisosEjercicio { get; set; }
        public virtual DbSet<StatusSesion> StatusSesiones { get; set; }
        public virtual DbSet<Sesion> Sesiones { get; set; }
        public virtual DbSet<Acuerdo> Acuerdos { get; set; }
        public virtual DbSet<Seguimiento> Seguimientos { get; set; }
        public virtual DbSet<Calendario> Calendarios { get; set; }
        public virtual DbSet<TipoCalendarizacion> TiposCalendarizacion { get; set; }
        public virtual DbSet<TipoSesion> TiposSesion { get; set; }
        public virtual DbSet<Normatividad> Normatividad { get; set; }
        public virtual DbSet<UsuarioDependencia> UsuariosDependencia { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<TipoUsuario> TiposUsuario { get; set; }


    }
}
