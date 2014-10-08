using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modelo
{
    public class Acuerdo:Generica
    {
        public int SesionID { get; set; }
        public string Notas { get; set; }
        public string NumAcuerdo { get; set; }
        public virtual Sesion Sesion { get; set; }
    }
}
