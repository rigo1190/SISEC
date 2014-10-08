using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modelo
{
    public class Ejercicio:Generica
    {

        [Index(IsUnique = true)]
        public int Anio { get; set; }
        public int? Orden { get; set; }

        [StringLength(100)]
        public string Descripcion { get; set; }
        
       
    }
}
