
using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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
        private IBusinessLogic<UsuarioFideicomiso> usuarioFideicomisoBusinessLogic;
        private IBusinessLogic<Usuario> usuarioBusinessLogic;
        private IBusinessLogic<TipoUsuario> tipoUsuarioBusinessLogic;

        private IBusinessLogic<Actas> actasBusinessLogic;
        private IBusinessLogic<Notas> notasBusinessLogic;
        private IBusinessLogic<FichaTecnica> fichaTecnicaBusinessLogic;
        private IBusinessLogic<StatusAcuerdo> statusAcuerdoBusinessLogic;
        private IBusinessLogic<Imagenes> imagenesBusinessLogic;
        private IBusinessLogic<SesionHistorico> sesionHistoricoBusinessLogic;
        private IBusinessLogic<FichaTecnicaHistorico> fichaTecnicaHistoricoBusinessLogic;
        private IBusinessLogic<rptSintesisInformativa> rptSintesisInformativaBusinessLogic;
        private IBusinessLogic<rptSesiones> rptSesionesBusinessLogic;
        private IBusinessLogic<rptSesionesHistorico> rptSesionesHistoricoBusinessLogic;
        private IBusinessLogic<rptSintesisInformativaHistorico> rptSintesisInformativaHistoricoBusinessLogic;



        public UnitOfWork()
        {
            this.contexto = new SISEF();
        }


        public IBusinessLogic<rptSintesisInformativaHistorico> RptSintesisInformativaHistoricoBusinessLogic
        {
            get
            {
                if (this.rptSintesisInformativaHistoricoBusinessLogic == null)
                {
                    this.rptSintesisInformativaHistoricoBusinessLogic = new GenericBusinessLogic<rptSintesisInformativaHistorico>(contexto);
                }

                return rptSintesisInformativaHistoricoBusinessLogic;
            }
        }

        public IBusinessLogic<rptSesionesHistorico> RptSesionesHistoricoBusinessLogic
        {
            get
            {
                if (this.rptSesionesHistoricoBusinessLogic == null)
                {
                    this.rptSesionesHistoricoBusinessLogic = new GenericBusinessLogic<rptSesionesHistorico>(contexto);
                }

                return rptSesionesHistoricoBusinessLogic;
            }
        }

        public IBusinessLogic<rptSesiones> RptSesionesBusinessLogic
        {
            get
            {
                if (this.rptSesionesBusinessLogic == null)
                {
                    this.rptSesionesBusinessLogic = new GenericBusinessLogic<rptSesiones>(contexto);
                }

                return rptSesionesBusinessLogic;
            }
        }


        public IBusinessLogic<rptSintesisInformativa> RptSintesisInformativaBusinessLogic
        {
            get
            {
                if (this.rptSintesisInformativaBusinessLogic == null)
                {
                    this.rptSintesisInformativaBusinessLogic = new GenericBusinessLogic<rptSintesisInformativa>(contexto);
                }

                return rptSintesisInformativaBusinessLogic;
            }
        }

        public IBusinessLogic<FichaTecnicaHistorico> FichaTecnicaHistoricoBusinessLogic
        {
            get
            {
                if (this.fichaTecnicaHistoricoBusinessLogic == null)
                {
                    this.fichaTecnicaHistoricoBusinessLogic = new GenericBusinessLogic<FichaTecnicaHistorico>(contexto);
                }

                return fichaTecnicaHistoricoBusinessLogic;
            }
        }


        public IBusinessLogic<SesionHistorico> SesionHistoricoBusinessLogic
        {
            get
            {
                if (this.sesionHistoricoBusinessLogic == null)
                {
                    this.sesionHistoricoBusinessLogic = new GenericBusinessLogic<SesionHistorico>(contexto);
                }

                return sesionHistoricoBusinessLogic;
            }
        }


        public IBusinessLogic<Imagenes> ImagenesBusinessLogic
        {
            get
            {
                if (this.imagenesBusinessLogic == null)
                {
                    this.imagenesBusinessLogic = new GenericBusinessLogic<Imagenes>(contexto);
                }

                return imagenesBusinessLogic;
            }
        }

        public IBusinessLogic<StatusAcuerdo> StatusAcuerdoBusinessLogic
        {
            get
            {
                if (this.statusAcuerdoBusinessLogic == null)
                {
                    this.statusAcuerdoBusinessLogic = new GenericBusinessLogic<StatusAcuerdo>(contexto);
                }

                return statusAcuerdoBusinessLogic;
            }
        }

        public IBusinessLogic<FichaTecnica> FichaTecnicaBusinessLogic
        {
            get
            {
                if (this.fichaTecnicaBusinessLogic == null)
                {
                    this.fichaTecnicaBusinessLogic = new GenericBusinessLogic<FichaTecnica>(contexto);
                }

                return fichaTecnicaBusinessLogic;
            }
        }
        public IBusinessLogic<Notas> NotasBusinessLogic
        {
            get
            {
                if (this.notasBusinessLogic == null)
                {
                    this.notasBusinessLogic = new GenericBusinessLogic<Notas>(contexto);
                }

                return notasBusinessLogic;
            }
        }

        public IBusinessLogic<Actas> ActasBusinessLogic
        {
            get
            {
                if (this.actasBusinessLogic == null)
                {
                    this.actasBusinessLogic = new GenericBusinessLogic<Actas>(contexto);
                }

                return actasBusinessLogic;
            }
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


        public IBusinessLogic<UsuarioFideicomiso> UsuarioFideicomisoBusinessLogic
        {
            get
            {
                if (this.usuarioFideicomisoBusinessLogic == null)
                {
                    this.usuarioFideicomisoBusinessLogic = new GenericBusinessLogic<UsuarioFideicomiso>(contexto);
                }

                return usuarioFideicomisoBusinessLogic;
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


                #region GUARDAR HISTORICO PARA SESION

                var changedEntries = contexto.ChangeTracker.Entries<Sesion>().Where(e => e.State == EntityState.Modified);
                if (changedEntries.Count() > 0)
                {
                    foreach (DbEntityEntry sesion in changedEntries)
                    {

                        if (sesion.Entity != null && sesion.Entity is Sesion)
                        {
                            bool copiar = false;

                            List<string> propiedades = sesion.CurrentValues.PropertyNames.ToList(); ;
                            var valoresAnteriores = sesion.GetDatabaseValues();

                            foreach (string propiedad in propiedades)
                            {
                                if (propiedad.Equals("FechaModificacion") || propiedad.Equals("UsuarioModifica"))
                                    continue;

                                string valAnterior = valoresAnteriores.GetValue<object>(propiedad) != null ? valoresAnteriores.GetValue<object>(propiedad).ToString() : string.Empty;
                                string valActual = sesion.CurrentValues.GetValue<object>(propiedad) != null ? sesion.CurrentValues.GetValue<object>(propiedad).ToString() : string.Empty;
                                
                                if (!valActual.Equals(valAnterior))
                                {
                                    copiar = true;
                                    break;
                                }


                            }

                            if (copiar)
                            {
                                SesionHistorico sesionHistorico;
                                string fechaModificacionCorta = sesion.CurrentValues.GetValue<DateTime>("FechaModificacion").ToShortDateString();
                                bool nuevo = false;
                                sesionHistorico = this.SesionHistoricoBusinessLogic.Get(e => e.FechaCapturaCorta == fechaModificacionCorta).FirstOrDefault();


                                if (sesionHistorico == null)
                                {
                                    nuevo = true;
                                    sesionHistorico = new SesionHistorico();
                                    sesionHistorico.FechaCaptura = DateTime.Now;
                                    sesionHistorico.FechaCapturaCorta = DateTime.Now.ToShortDateString();
                                    
                                }

                                sesionHistorico.UsuarioCaptura=valoresAnteriores.GetValue<string>("UsuarioModifica");
                                sesionHistorico.Mes = valoresAnteriores.GetValue<object>("Mes") != null ? valoresAnteriores.GetValue<int>("Mes") : 0;
                                sesionHistorico.Descripcion = valoresAnteriores.GetValue<object>("Descripcion") != null ? valoresAnteriores.GetValue<string>("Descripcion") : null;
                                sesionHistorico.FechaProgramada = valoresAnteriores.GetValue<object>("FechaProgramada") != null ? valoresAnteriores.GetValue<DateTime>("FechaProgramada") : Utilerias.StrToDate("null");
                                sesionHistorico.FechaCelebrada = valoresAnteriores.GetValue<object>("FechaCelebrada") != null ? valoresAnteriores.GetValue<DateTime>("FechaCelebrada") : Utilerias.StrToDate("null");
                                sesionHistorico.FechaOficio = valoresAnteriores.GetValue<object>("FechaOficio") != null ? valoresAnteriores.GetValue<DateTime>("FechaOficio") : Utilerias.StrToDate("null");
                                sesionHistorico.FechaReprogramada = valoresAnteriores.GetValue<object>("FechaReprogramada") != null ? valoresAnteriores.GetValue<DateTime>("FechaReprogramada") : Utilerias.StrToDate("null");
                                sesionHistorico.HoraCelebrada = valoresAnteriores.GetValue<object>("HoraCelebrada") != null ? valoresAnteriores.GetValue<string>("HoraProgramada") : null;
                                sesionHistorico.HoraProgramada = valoresAnteriores.GetValue<object>("HoraProgramada") != null ? valoresAnteriores.GetValue<string>("HoraCelebrada") : null;
                                sesionHistorico.HoraReprogramada = valoresAnteriores.GetValue<object>("HoraReprogramada") != null ? valoresAnteriores.GetValue<string>("HoraReprogramada") : null;
                                sesionHistorico.LugarReunion = valoresAnteriores.GetValue<object>("LugarReunion") != null ? valoresAnteriores.GetValue<string>("LugarReunion") : null;
                                sesionHistorico.NumOficio = valoresAnteriores.GetValue<object>("NumOficio") != null ? valoresAnteriores.GetValue<string>("NumOficio") : null;
                                sesionHistorico.NumSesion = valoresAnteriores.GetValue<object>("NumSesion") != null ? valoresAnteriores.GetValue<string>("NumSesion") : null;
                                sesionHistorico.Observaciones = valoresAnteriores.GetValue<object>("Observaciones") != null ? valoresAnteriores.GetValue<string>("Observaciones") : null;
                                sesionHistorico.SesionID = valoresAnteriores.GetValue<object>("ID") != null ? valoresAnteriores.GetValue<int>("ID") : 0;
                                sesionHistorico.TipoSesionID = valoresAnteriores.GetValue<object>("TipoSesionID") != null ? valoresAnteriores.GetValue<int>("TipoSesionID") : 0;
                                sesionHistorico.StatusSesionID = valoresAnteriores.GetValue<object>("StatusSesionID") != null ? valoresAnteriores.GetValue<int>("StatusSesionID") : 0;
                                sesionHistorico.UsuarioCaptura = valoresAnteriores.GetValue<object>("UsuarioCaptura") != null ? valoresAnteriores.GetValue<string>("UsuarioCaptura") : null;

                                if (nuevo)
                                    this.SesionHistoricoBusinessLogic.Insert(sesionHistorico);
                                else
                                    this.SesionHistoricoBusinessLogic.Update(sesionHistorico);

                            }

                        }


                    }
                }


                #endregion

                #region GUARDAR HISTORICO PARA FICHA TECNICA

                var changedEntities = contexto.ChangeTracker.Entries<FichaTecnica>().Where(e => e.State == EntityState.Modified);
                if (changedEntities.Count() > 0)
                {
                    foreach (DbEntityEntry ficha in changedEntities)
                    {
                        
                        if (ficha.Entity != null && ficha.Entity is FichaTecnica)
                        {
                            bool copiar = false;

                            List<string> propiedades = ficha.CurrentValues.PropertyNames.ToList(); ;
                            var valoresAnteriores = ficha.GetDatabaseValues();

                            foreach (string propiedad in propiedades)
                            {
                                if (propiedad.Equals("FechaModificacion") || propiedad.Equals("UsuarioModifica"))
                                    continue;

                                string valAnterior = valoresAnteriores.GetValue<object>(propiedad) != null ? valoresAnteriores.GetValue<object>(propiedad).ToString() : string.Empty;
                                string valActual = ficha.CurrentValues.GetValue<object>(propiedad) != null ? ficha.CurrentValues.GetValue<object>(propiedad).ToString() : string.Empty;
                                
                                if (!valActual.Equals(valAnterior))
                                {
                                    copiar = true;
                                    break;
                                }


                            }

                            if (copiar)
                            {
                                FichaTecnicaHistorico fichaHistorico;
                                string fechaModificacionCorta = ficha.CurrentValues.GetValue<DateTime>("FechaModificacion").ToShortDateString();
                                bool nuevo = false;
                                fichaHistorico = this.FichaTecnicaHistoricoBusinessLogic.Get(e => e.FechaCapturaCorta == fechaModificacionCorta).FirstOrDefault();


                                if (fichaHistorico == null)
                                {
                                    nuevo = true;
                                    fichaHistorico = new FichaTecnicaHistorico();
                                    fichaHistorico.FechaCaptura = DateTime.Now;
                                    fichaHistorico.FechaCapturaCorta = DateTime.Now.ToShortDateString();
                                    
                                }

                                fichaHistorico.UsuarioCaptura = valoresAnteriores.GetValue<string>("UsuarioModifica");
                                fichaHistorico.Descripcion = valoresAnteriores.GetValue<object>("Descripcion") != null ? valoresAnteriores.GetValue<string>("Descripcion") : null;
                                fichaHistorico.NombreArchivo = valoresAnteriores.GetValue<object>("NombreArchivo") != null ? valoresAnteriores.GetValue<string>("NombreArchivo") : null;
                                fichaHistorico.TipoArchivo = valoresAnteriores.GetValue<object>("TipoArchivo") != null ? valoresAnteriores.GetValue<string>("TipoArchivo") : null;
                                fichaHistorico.ResponsableOperativo = valoresAnteriores.GetValue<object>("ResponsableOperativo") != null ? valoresAnteriores.GetValue<string>("ResponsableOperativo") : null;
                                fichaHistorico.Finalidad = valoresAnteriores.GetValue<object>("Finalidad") != null ? valoresAnteriores.GetValue<string>("Finalidad") : null;
                                fichaHistorico.Creacion = valoresAnteriores.GetValue<object>("Creacion") != null ? valoresAnteriores.GetValue<string>("Creacion") : null;
                                fichaHistorico.Formalizacion = valoresAnteriores.GetValue<object>("Formalizacion") != null ? valoresAnteriores.GetValue<string>("Formalizacion") : null;
                                fichaHistorico.Partes = valoresAnteriores.GetValue<object>("Partes") != null ? valoresAnteriores.GetValue<string>("Partes") : null;
                                fichaHistorico.Modificaciones = valoresAnteriores.GetValue<object>("Modificaciones") != null ? valoresAnteriores.GetValue<string>("Modificaciones") : null;
                                fichaHistorico.ComiteTecnico = valoresAnteriores.GetValue<object>("ComiteTecnico") != null ? valoresAnteriores.GetValue<string>("ComiteTecnico") : null;
                                fichaHistorico.FichaTecnicaID = valoresAnteriores.GetValue<object>("ID") != null ? valoresAnteriores.GetValue<int>("ID") : 0;
                                fichaHistorico.UsuarioCaptura = valoresAnteriores.GetValue<object>("UsuarioCaptura") != null ? valoresAnteriores.GetValue<string>("UsuarioCaptura") : null;
                                fichaHistorico.ReglasOperacion = valoresAnteriores.GetValue<object>("ReglasOperacion") != null ? valoresAnteriores.GetValue<string>("ReglasOperacion") : null;
                                fichaHistorico.EstructuraAdministrativa = valoresAnteriores.GetValue<object>("EstructuraAdministrativa") != null ? valoresAnteriores.GetValue<string>("EstructuraAdministrativa") : null;
                                fichaHistorico.Calendario = valoresAnteriores.GetValue<object>("Calendario") != null ? valoresAnteriores.GetValue<string>("Calendario") : null;
                                fichaHistorico.PresupuestoAnual = valoresAnteriores.GetValue<object>("PresupuestoAnual") != null ? valoresAnteriores.GetValue<string>("PresupuestoAnual") : null;
                                fichaHistorico.SituacionPatrimonial = valoresAnteriores.GetValue<object>("SituacionPatrimonial") != null ? valoresAnteriores.GetValue<string>("SituacionPatrimonial") : null;

                                if (nuevo)
                                    this.FichaTecnicaHistoricoBusinessLogic.Insert(fichaHistorico);
                                else
                                    this.FichaTecnicaHistoricoBusinessLogic.Update(fichaHistorico);

                            }

                        }


                    }
                }

                #endregion


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
