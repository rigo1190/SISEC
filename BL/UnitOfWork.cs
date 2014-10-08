
using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace BL
{
    public class UnitOfWork : IDisposable
    {
        internal SISEF contexto;

        private List<String> errors = new List<String>();
        private IBusinessLogic<Fideicomiso> fideicomisoBusinessLogic;
        private IBusinessLogic<Dependencia> dependenciaBusinessLogic;
        private IBusinessLogic<Ejercicio> ejercicioBusinessLogic;
        private IBusinessLogic<DependenciaFideicomisoEjercicio> dependenciaFideicomisoEjercicioBusinessLogic;
        private IBusinessLogic<StatusSesion> statusSesionBusinessLogic;
        private IBusinessLogic<Sesion> sesionBusinessLogic;
        private IBusinessLogic<Acuerdo> acuerdoBusinessLogic;
        private IBusinessLogic<Seguimiento> seguimientoBusinessLogic;
        private IBusinessLogic<Calendario> calendarioBusinessLogic;
        private IBusinessLogic<TipoCalendarizacion> tipoCalendarizacionBusinessLogic;
        private IBusinessLogic<TipoSesion> tipoSesionBusinessLogic;
        private IBusinessLogic<Normatividad> normatividadBusinessLogic;
        private IBusinessLogic<UsuarioDependencia> usuarioDependenciaBusinessLogic;
        private IBusinessLogic<Usuario> usuarioBusinessLogic;
        private IBusinessLogic<TipoUsuario> tipoUsuarioBusinessLogic;

        public UnitOfWork()
        {
            this.contexto = new SISEF();
        }

        public IBusinessLogic<Usuario> UsuarioBusinessLogic
        {
            get
            {
                if (this.usuarioBusinessLogic == null)
                {
                    this.usuarioBusinessLogic = new GenericBusinessLogic<Usuario>(contexto);
                }

                return usuarioBusinessLogic;
            }
        }

        public IBusinessLogic<Fideicomiso> FideicomisoBusinessLogic
        {
            get
            {
                if (this.fideicomisoBusinessLogic == null)
                {
                    this.fideicomisoBusinessLogic = new GenericBusinessLogic<Fideicomiso>(contexto);
                }

                return fideicomisoBusinessLogic;
            }
        }

        public IBusinessLogic<Dependencia> DependenciaBusinessLogic
        {
            get
            {
                if (this.dependenciaBusinessLogic == null)
                {
                    this.dependenciaBusinessLogic = new GenericBusinessLogic<Dependencia>(contexto);
                }

                return dependenciaBusinessLogic;
            }
        }

        public IBusinessLogic<Ejercicio> EjercicioBusinessLogic
        {
            get
            {
                if (this.ejercicioBusinessLogic == null)
                {
                    this.ejercicioBusinessLogic = new GenericBusinessLogic<Ejercicio>(contexto);
                }

                return ejercicioBusinessLogic;
            }
        }

        public IBusinessLogic<DependenciaFideicomisoEjercicio> DependenciaFideicomisoEjercicioBusinessLogic
        {
            get
            {
                if (this.dependenciaFideicomisoEjercicioBusinessLogic == null)
                {
                    this.dependenciaFideicomisoEjercicioBusinessLogic = new GenericBusinessLogic<DependenciaFideicomisoEjercicio>(contexto);
                }

                return dependenciaFideicomisoEjercicioBusinessLogic;
            }
        }

        public IBusinessLogic<StatusSesion> StatusSesionBusinessLogic
        {
            get
            {
                if (this.statusSesionBusinessLogic == null)
                {
                    this.statusSesionBusinessLogic = new GenericBusinessLogic<StatusSesion>(contexto);
                }

                return statusSesionBusinessLogic;
            }
        }

        public IBusinessLogic<Sesion> SesionBusinessLogic
        {
            get
            {
                if (this.sesionBusinessLogic == null)
                {
                    this.sesionBusinessLogic = new GenericBusinessLogic<Sesion>(contexto);
                }

                return sesionBusinessLogic;
            }
        }

        public IBusinessLogic<Acuerdo> AcuerdoBusinessLogic
        {
            get
            {
                if (this.acuerdoBusinessLogic == null)
                {
                    this.acuerdoBusinessLogic = new GenericBusinessLogic<Acuerdo>(contexto);
                }

                return acuerdoBusinessLogic;
            }
        }

        public IBusinessLogic<Seguimiento> SeguimientoBusinessLogic
        {
            get
            {
                if (this.seguimientoBusinessLogic == null)
                {
                    this.seguimientoBusinessLogic = new GenericBusinessLogic<Seguimiento>(contexto);
                }

                return seguimientoBusinessLogic;
            }
        }

        public IBusinessLogic<Calendario> CalendarioBusinessLogic
        {
            get
            {
                if (this.calendarioBusinessLogic == null)
                {
                    this.calendarioBusinessLogic = new GenericBusinessLogic<Calendario>(contexto);
                }

                return calendarioBusinessLogic;
            }
        }

        public IBusinessLogic<TipoCalendarizacion> TipoCalendarizacionBusinessLogic
        {
            get
            {
                if (this.tipoCalendarizacionBusinessLogic == null)
                {
                    this.tipoCalendarizacionBusinessLogic = new GenericBusinessLogic<TipoCalendarizacion>(contexto);
                }

                return tipoCalendarizacionBusinessLogic;
            }
        }


        public IBusinessLogic<TipoSesion> TipoSesionBusinessLogic
        {
            get
            {
                if (this.tipoSesionBusinessLogic == null)
                {
                    this.tipoSesionBusinessLogic = new GenericBusinessLogic<TipoSesion>(contexto);
                }

                return tipoSesionBusinessLogic;
            }
        }

        public IBusinessLogic<Normatividad> NormatividadBusinessLogic
        {
            get
            {
                if (this.normatividadBusinessLogic == null)
                {
                    this.normatividadBusinessLogic = new GenericBusinessLogic<Normatividad>(contexto);
                }

                return normatividadBusinessLogic;
            }
        }


        public IBusinessLogic<UsuarioDependencia> UsuarioDependenciaBusinessLogic
        {
            get
            {
                if (this.usuarioDependenciaBusinessLogic == null)
                {
                    this.usuarioDependenciaBusinessLogic = new GenericBusinessLogic<UsuarioDependencia>(contexto);
                }

                return usuarioDependenciaBusinessLogic;
            }
        }

        public IBusinessLogic<TipoUsuario> TipoUsuarioBusinessLogic
        {
            get
            {
                if (this.tipoUsuarioBusinessLogic == null)
                {
                    this.tipoUsuarioBusinessLogic = new GenericBusinessLogic<TipoUsuario>(contexto);
                }

                return tipoUsuarioBusinessLogic;
            }
        }


        public void SaveChanges()
        {
            try
            {
                errors.Clear();
                contexto.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {

                this.RollBack();

                foreach (var item in ex.EntityValidationErrors)
                {

                    errors.Add(String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors", item.Entry.Entity.GetType().Name, item.Entry.State));

                    foreach (var error in item.ValidationErrors)
                    {
                        errors.Add(String.Format("Propiedad: \"{0}\", Error: \"{1}\"", error.PropertyName, error.ErrorMessage));
                    }


                }

            }
            catch (DbUpdateException ex)
            {
                this.RollBack();
                errors.Add(String.Format("{0}", ex.InnerException.InnerException.Message));
            }
            catch (System.InvalidOperationException ex)
            {
                this.RollBack();
                errors.Add(String.Format("{0}", ex.Message));
            }
            catch (Exception ex)
            {
                this.RollBack();
                errors.Add(String.Format("{0}\n{1}", ex.Message, ex.InnerException.Message));
            }

        }

        public List<String> Errors
        {
            get
            {
                return errors;
            }
        }


        public void RollBack()
        {

            var changedEntries = contexto.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged);

            #region < Pendiente revisar, esto podria cancelar toda una sesión de trabajo >

            //foreach (var entry in changedEntries.Where(x => x.State == EntityState.Modified))
            //{
            //    entry.CurrentValues.SetValues(entry.OriginalValues);
            //    entry.State = EntityState.Unchanged;
            //}

            //foreach (var entry in changedEntries.Where(x => x.State == EntityState.Added))
            //{
            //    entry.State = EntityState.Detached;
            //} 

            #endregion

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Deleted))
            {
                entry.State = EntityState.Unchanged;
            }

        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    contexto.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
