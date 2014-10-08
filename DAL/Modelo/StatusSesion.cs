using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modelo
{
    public class StatusSesion:Generica
    {
        public StatusSesion()
        {
            
        }

        [Index(IsUnique = true)]
        [StringLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Clave { get; set; }

        [StringLength(100,ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Descripcion { get; set; }
        public int? Orden { get; set; }
        
        

    }
}
