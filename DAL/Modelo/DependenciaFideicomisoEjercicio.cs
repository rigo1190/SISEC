using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modelo
{
    public class DependenciaFideicomisoEjercicio:Generica
    {
        public DependenciaFideicomisoEjercicio()
        {
            this.DetalleCalendarios = new HashSet<Calendario>();
        }
        public int DependenciaID { get; set; }
        public int FideicomisoID { get; set; }
        public int EjercicioID { get; set; }
        public bool Activo { get; set; }
        public virtual Dependencia Dependencia { get; set; }
        public virtual Fideicomiso Fideicomiso { get; set; }
        public virtual Ejercicio Ejercicio { get; set;} 
        public ICollection<Calendario> DetalleCalendarios { get; set; }
        public ICollection<Normatividad> DetalleNormatividad { get; set; }


    }
}
