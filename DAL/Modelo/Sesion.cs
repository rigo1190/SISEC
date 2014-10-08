using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modelo
{
    public class Sesion:Generica
    {
        public Sesion() 
        {
            DetalleAcuerdos = new HashSet<Acuerdo>();
            DetalleSeguimientos = new HashSet<Seguimiento>();
        }
        public int CalendarioID { get; set; }
        public int Mes { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaProgramada { get; set; }
        public DateTime? FechaCelebrada { get; set; }
        public DateTime? FechaReprogramada { get; set; }
        public int TipoSesionID { get; set; }
        public int StatusSesionID { get; set; }
        public int TipoCalendarizacionID { get; set; }
        public string Observaciones { get; set; }
        public Calendario Calendario { get; set; } 
        public virtual TipoCalendarizacion TipoCalendarizacion { get; set; }
        public virtual StatusSesion StatusSesion { get; set; }
        public virtual TipoSesion TipoSesion { get; set; }
        public virtual ICollection<Acuerdo> DetalleAcuerdos { get; set; }
        public virtual ICollection<Seguimiento> DetalleSeguimientos { get; set; }
         
        
    }
}
