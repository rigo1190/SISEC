using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modelo
{
    public class Fideicomiso : Generica
    {
        public Fideicomiso()
        {
            this.DetalleSubFideicomisos = new HashSet<Fideicomiso>();
            this.DetalleDependencias = new HashSet<DependenciaFideicomisoEjercicio>();
        }

        [Index(IsUnique = true)]
        [StringLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Clave { get; set; }

        [StringLength(1000, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Descripcion { get; set; }
        public int Orden { get; set; }
        public int? DependeDeID { get; set; }
        public virtual Fideicomiso DependeDe { get; set; }
        public virtual ICollection<Fideicomiso> DetalleSubFideicomisos { get; set; }
        public virtual ICollection<DependenciaFideicomisoEjercicio> DetalleDependencias { get; set; }

    }
}
