using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modelo
{
    public class Normatividad: Generica
    {
        [StringLength(250)]
        public string Descripcion { get; set; }

        [StringLength(250)]
        public string RutaArchivo { get; set; }

        [StringLength(10)]
        public string TipoArchivo { get; set; }
        public TipoNormatividad TipoNormatividad { get; set; }
        public int? DependenciaFideicomisoEjercicioID { get; set; }
        public virtual DependenciaFideicomisoEjercicio DependenciaFideicomisoEjercicio { get; set; } 

    }

    public enum TipoNormatividad
    {
        General=1,
        Especifica=2
    }
}
