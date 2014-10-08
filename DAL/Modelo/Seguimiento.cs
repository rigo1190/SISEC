using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modelo
{
    public class Seguimiento:Generica
    {
        public int SesionID { get; set; }

        public string Descripcion { get; set; }

        public virtual Sesion Sesion { get; set; } 
    }
}
