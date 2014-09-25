using System;
using System.Collections.Generic;
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
        public string Login { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public virtual ICollection<UsuarioDependencia> DetalleDependencias { get; set; }
    }
}
