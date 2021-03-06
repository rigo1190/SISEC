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
    
    public partial class DependenciaFideicomisoEjercicio
    {
        public DependenciaFideicomisoEjercicio()
        {
            this.DetalleCalendarios = new HashSet<Calendario>();
            this.DetallesNormatividad = new HashSet<Normatividad>();
            this.DetalleUsuarios = new HashSet<UsuarioFideicomiso>();
        }
    
        public int ID { get; set; }
        public int FideicomisoID { get; set; }
        public int EjercicioID { get; set; }
        public bool Activo { get; set; }
        public Nullable<System.DateTime> FechaCaptura { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string UsuarioCaptura { get; set; }
        public string UsuarioModifica { get; set; }
    
        public virtual ICollection<Calendario> DetalleCalendarios { get; set; }
        public virtual Ejercicio Ejercicio { get; set; }
        public virtual Fideicomiso Fideicomiso { get; set; }
        public virtual ICollection<Normatividad> DetallesNormatividad { get; set; }
        public virtual ICollection<UsuarioFideicomiso> DetalleUsuarios { get; set; }
    }
}
