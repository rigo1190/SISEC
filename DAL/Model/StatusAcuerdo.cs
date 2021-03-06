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
    
    public partial class StatusAcuerdo
    {
        public StatusAcuerdo()
        {
            this.DetalleAcuerdos = new HashSet<Acuerdo>();
        }
    
        public int ID { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> Orden { get; set; }
        public Nullable<System.DateTime> FechaCaptura { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string UsuarioCaptura { get; set; }
        public string UsuarioModifica { get; set; }
        public Nullable<bool> Inicial { get; set; }
    
        public virtual ICollection<Acuerdo> DetalleAcuerdos { get; set; }
    }
}
