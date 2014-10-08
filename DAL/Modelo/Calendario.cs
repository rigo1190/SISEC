using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modelo
{
    public class Calendario:Generica
    {
        public Calendario()
        {
            this.DetalleSesiones = new HashSet<Sesion>();
        }
        public int DependenciaFideicomisoEjercicioID { get; set; }
        public int EjercicioID { get; set; }
        public virtual DependenciaFideicomisoEjercicio DependenciaFideicomisoEjercicio { get; set; }
        public virtual Ejercicio Ejercicio { get; set; }
        public virtual ICollection<Sesion> DetalleSesiones { get; set; }
        



    }
}
