using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modelo
{
    public class Usuario
    {
        public Usuario() 
        {
            this.DetalleDependencias = new HashSet<UsuarioDependencia>();
        }

        public int ID { get; set; }

        [Index(IsUnique = true)]
        [StringLength(20, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Login { get; set; }
        public string Password { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Nombre { get; set; }
        public virtual ICollection<UsuarioDependencia> DetalleDependencias { get; set; }
    }
}
