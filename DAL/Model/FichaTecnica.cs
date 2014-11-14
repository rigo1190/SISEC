//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class FichaTecnica
    {
        public FichaTecnica()
        {
            this.DetalleFichasTecnicaHistorico = new HashSet<FichaTecnicaHistorico>();
        }
    
        public int ID { get; set; }
        public int DependenciaFideicomisoEjercicioID { get; set; }
        public string Descripcion { get; set; }
        public string NombreArchivo { get; set; }
        public string TipoArchivo { get; set; }
        public Nullable<System.DateTime> FechaCaptura { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string UsuarioCaptura { get; set; }
        public string UsuarioModifica { get; set; }
        public string ResponsableOperativo { get; set; }
        public string Finalidad { get; set; }
        public string Creacion { get; set; }
        public string Formalizacion { get; set; }
        public string Partes { get; set; }
        public string Modificaciones { get; set; }
        public string ComiteTecnico { get; set; }
        public string ReglasOperacion { get; set; }
        public string EstructuraAdministrativa { get; set; }
        public string Calendario { get; set; }
        public string PresupuestoAnual { get; set; }
        public string SituacionPatrimonial { get; set; }
    
        public virtual DependenciaFideicomisoEjercicio DependenciaFideicomisoEjercicio { get; set; }
        public virtual ICollection<FichaTecnicaHistorico> DetalleFichasTecnicaHistorico { get; set; }
    }
}
