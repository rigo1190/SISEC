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
    
    public partial class Sesion
    {
        public Sesion()
        {
            this.DetalleAcuerdos = new HashSet<Acuerdo>();
            this.DetalleNotas = new HashSet<Notas>();
            this.DetalleActas = new HashSet<Actas>();
            this.DetalleSesionesHistorico = new HashSet<SesionHistorico>();
        }
    
        public int ID { get; set; }
        public int CalendarioID { get; set; }
        public int Mes { get; set; }
        public string Descripcion { get; set; }
        public Nullable<System.DateTime> FechaProgramada { get; set; }
        public Nullable<System.DateTime> FechaCelebrada { get; set; }
        public Nullable<System.DateTime> FechaReprogramada { get; set; }
        public int TipoSesionID { get; set; }
        public int StatusSesionID { get; set; }
        public string Observaciones { get; set; }
        public Nullable<System.DateTime> FechaCaptura { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string UsuarioCaptura { get; set; }
        public string UsuarioModifica { get; set; }
        public Nullable<System.DateTime> FechaOficio { get; set; }
        public string NumOficio { get; set; }
        public string LugarReunion { get; set; }
        public string NumSesion { get; set; }
        public string HoraProgramada { get; set; }
        public string HoraReprogramada { get; set; }
        public string HoraCelebrada { get; set; }
    
        public virtual ICollection<Acuerdo> DetalleAcuerdos { get; set; }
        public virtual Calendario Calendario { get; set; }
        public virtual StatusSesion StatusSesion { get; set; }
        public virtual TipoSesion TipoSesion { get; set; }
        public virtual ICollection<Notas> DetalleNotas { get; set; }
        public virtual ICollection<Actas> DetalleActas { get; set; }
        public virtual ICollection<SesionHistorico> DetalleSesionesHistorico { get; set; }
    }
}
